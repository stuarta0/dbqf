using System;
using System.Collections.Generic;
using System.ComponentModel;
using dbqf.Configuration;
using dbqf.Display;
using Standalone.Core.Data;
using Standalone.Core.Export;
using PropertyChanged;
using Standalone.Core.Display;
using dbqf.Criterion;
using System.Data;

namespace Standalone.Core
{
    [ImplementPropertyChanged]
    public class ApplicationBase : IApplication, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public ApplicationBase(Project project, DbServiceFactory dbFactory)
        {
            _views = new Dictionary<string, IView>();
            Project = project;
            SubjectSource = new BindingList<ISubject>(Project.Configuration);
            SelectedSubject = SubjectSource[0];
            MessageProvider = new NullMessageProvider();

			ServiceFactory = dbFactory;
			var refresh = new EventHandler((s, e) => 
				{
					if (ServiceFactory != null)
					{
						try {
							_dbService = ServiceFactory.CreateAsync(Project.CurrentConnection);
						} catch {
							// result?
						}
					}
				});
			Project.CurrentConnectionChanged += refresh;
			refresh(this, EventArgs.Empty);
        }

        public Project Project { get; protected set; }
        public IExportServiceFactory ExportFactory { get; set; }
        public IViewPersistence ViewPersistence { get; set; }
        public IMessageProvider MessageProvider { get; set; }
        public string ResultSQL { get; set; }
        public BindingList<ISubject> SubjectSource { get; private set; }
        public virtual ISubject SelectedSubject { get; set; }
        public event EventHandler SelectedSubjectChanged = delegate { };
        protected void OnSelectedSubjectChanged()
        {
            SelectedSubjectChanged(this, EventArgs.Empty);
        }

        public virtual IView CurrentView { get; set; }
        public event EventHandler CurrentViewChanged = delegate { };
        private void OnCurrentViewChanged()
        {
            CurrentViewChanged(this, EventArgs.Empty);
        }

		public DbServiceFactory ServiceFactory { get; set; }
		protected IDbServiceAsync _dbService;

        [AlsoNotifyFor("IsSearching")]
        protected virtual BackgroundWorker SearchWorker { get; set; }
        public virtual bool IsSearching
        {
            get { return SearchWorker != null; }
        }

        [AlsoNotifyFor("ResultHeader")]
        public DataTable Result { get; private set; }
        public string ResultHeader
        {
            get { return String.Concat("Results", Result == null ? string.Empty : String.Concat(" (", Result.Rows.Count, ")")); }
        }

        /// <summary>
        /// Represents a string key mapping to an IView used for persistence and automatically changing view.
        /// </summary>
        protected Dictionary<string, IView> _views;
        protected string GetViewKey(IView view)
        {
            foreach (var pair in _views)
                if (pair.Value == view)
                    return pair.Key;
            return null;
        }

        /// <summary>
        /// Takes the current view parts and moves them to the next view in the _views collection.
        /// May throw exceptions if the target view can't display the incoming parts correctly.
        /// </summary>
        public virtual void Refine()
        {
            try
            {
                IView prev = null;
                foreach (var view in _views)
                {
                    if (prev != null)
                    {
                        view.Value.Reset();
                        CurrentView = view.Value;
                        view.Value.SetParts(prev.GetParts());
                        break;
                    }
                    else if (view.Value == CurrentView)
                        prev = view.Value;
                }
            }
            catch (Exception ex)
            {
                MessageProvider.Show(ex.Message, "Refine", MessageType.Warning, MessageOption.OK);
            }
        }
    
        public string ApplicationTitle
        {
            get
            {
                return String.Concat("Database Query Framework",
                    String.IsNullOrWhiteSpace(Project.Title) ? "" : String.Concat(" - ", Project.Title),
                    " (", Project.CurrentConnection.DisplayName, ")");
            }
        }

        public void Search(IParameter parameter, IList<IFieldPath> columns)
        {
            if (IsSearching)
            {
                // if they don't cancel, do nothing
                if (MessageProvider.Show("There is a search in progress.  Do you want to cancel the existing search?", "Search", MessageType.Question, MessageOption.YesNo) == MessageResult.No)
                    return;

                // if they said yes, cancel existing search and continue with new one
                CancelSearch();
            }

            // Get results asynchronously
            ResultSQL = null;
            var details = new SearchDetails()
            {
                Target = SelectedSubject,
                Columns = columns,
                Where = parameter
            };

            _dbService.GetResults(details, new ResultCallback(SearchComplete, details));
        }

        private void SearchComplete(IDbServiceAsyncCallback<DataTable> callback)
        {
            var data = (ResultCallback)callback;
            if (data.Exception != null)
                MessageProvider.Show(data.Exception.Message, "Search", MessageType.Error, MessageOption.OK);
            else
            {
                ResultSQL = ((SearchDetails)data.Details).Sql;
                Result = data.Results;
                //Result.DataSource = data.Results;
            }
        }

        public virtual void CancelSearch()
        {
            if (SearchWorker != null)
                SearchWorker.CancelAsync();
            SearchWorker = null;
        }

        public virtual bool Export(string filename)
        {
            if (String.IsNullOrWhiteSpace(filename))
                return false;

            try
            {
                return ExportFactory.Create(filename).Export(filename, Result);
            }
            catch (Exception ex)
            {
                MessageProvider.Show(ex.Message, "Export", MessageType.Error, MessageOption.OK);
                return false;
            }
        }

        /// <exception cref="System.ArgumentException">Thrown if the view SearchType can't be found in the application.</exception>
        /// <exception cref="System.ApplicationException">Thrown if some parts couldn't be represented in the view.</exception>
        public virtual void Load(string filename)
        {
            Load(filename, true);
        }
        protected virtual SearchDocument Load(string filename, bool reset)
        {
            if (ViewPersistence == null || String.IsNullOrWhiteSpace(filename))
                return null;

            // may throw a load exceptionSearchDocument doc = null;
            SearchDocument doc;
            try { doc = ViewPersistence.Load(filename); }
            catch (Exception ex)
            {
                MessageProvider.Show(ex.Message, "Load", MessageType.Error, MessageOption.OK);
                return null;
            }

            SelectedSubject = doc.Subject;
            if (!_views.ContainsKey(doc.SearchType))
                throw new ArgumentException(String.Format("Could not determine the appropriate view to display (View requested: {0}).", doc.SearchType));
            CurrentView = _views[doc.SearchType];
            if (reset)
                CurrentView.Reset();
            CurrentView.SetParts(doc.Parts);
            return doc;
        }

        protected virtual SearchDocument CreateSearchDocument()
        {
            return new SearchDocument(Project)
            {
                SearchType = GetViewKey(CurrentView),
                Subject = SelectedSubject,
                Parts = CurrentView.GetParts()
            };
        }

        public virtual void Save(string filename)
        {
            if (ViewPersistence == null || String.IsNullOrWhiteSpace(filename))
                return;

            try
            {
                var doc = CreateSearchDocument();
                if (doc.Parts.Count == 0 && doc.Outputs.Count == 0)
                    throw new ArgumentException("Saving a search requires at least one parameter or output field.");
                else
                    ViewPersistence.Save(filename, doc);
            }
            catch (ArgumentException ex)
            {
                MessageProvider.Show(ex.Message, "Save", MessageType.Error, MessageOption.OK);
            }
        }
    }
}
