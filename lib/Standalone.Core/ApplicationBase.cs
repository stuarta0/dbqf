using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Display;
using Standalone.Core.Data;
using Standalone.Core.Data.Processing;
using Standalone.Core.Export;
using System.IO;
using Standalone.Core.Serialization.Assemblers;
using Standalone.Core;
using PropertyChanged;

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

        public ApplicationBase(Project project)
        {
            _views = new Dictionary<string, IView>();
            Project = project;
            SubjectSource = new BindingList<ISubject>(Project.Configuration);
            SelectedSubject = SubjectSource[0];
        }

        public Project Project { get; protected set; }
        public IExportServiceFactory ExportFactory { get; set; }
        public IViewPersistence ViewPersistence { get; set; }
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

        [AlsoNotifyFor("IsSearching")]
        protected virtual BackgroundWorker SearchWorker { get; set; }
        public virtual bool IsSearching
        {
            get { return SearchWorker != null; }
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
    
        public string ApplicationTitle
        {
            get
            {
                return String.Concat("Database Query Framework",
                    String.IsNullOrWhiteSpace(Project.Title) ? "" : String.Concat(" - ", Project.Title),
                    " (", Project.CurrentConnection.DisplayName, ")");
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
            return false;
        }

        /// <exception cref="System.ArgumentException">Thrown if the view SearchType can't be found in the application.</exception>
        /// <exception cref="System.ApplicationException">Thrown if some parts couldn't be represented in the view.</exception>
        public virtual void Load(string filename)
        {
            Load(filename, true);
        }
        protected virtual SearchDocument Load(string filename, bool reset)
        {
            if (ViewPersistence == null)
                return null;

            // may throw a load exception
            SearchDocument doc = ViewPersistence.Load(filename);

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
            if (ViewPersistence == null)
                return;

            var doc = CreateSearchDocument();
            if (doc.Parts.Count == 0 && doc.Outputs.Count == 0)
                throw new ArgumentException("Saving a search requires at least one parameter or output field.");
            else
                ViewPersistence.Save(filename, doc);
        }
    }
}
