using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Display;
using dbqf.WPF;
using PropertyChanged;
using Standalone.Core.Data;
using Standalone.Core.Data.Processing;
using Standalone.Core.Export;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Standalone.WPF.ViewModel;
using Standalone.WPF.Controls;
using Standalone.Core;

namespace Standalone.WPF
{
    public class MainWindowAdapter : INotifyPropertyChanged, Core.IApplication
    {
        public ProjectAdapter ProjectAdapter { get; private set; }
        public ResultFactory ResultFactory { get; set; }
        public IFieldPathFactory PathFactory { get; private set; }
        public ExportServiceFactory ExportFactory { get; set; }

        public PresetView Preset { get; private set; }
        public StandardView Standard { get; private set; }
        public FieldPathCombo Advanced { get; private set; }
        public RetrieveFieldsView RetrieveFields { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        #region Application Appearance

        public string ApplicationTitle
        {
            get
            {
                return String.Concat("Database Query Framework",
                    String.IsNullOrWhiteSpace(ProjectAdapter.Title) ? "" : String.Concat(" - ", ProjectAdapter.Title),
                    " (", ProjectAdapter.CurrentConnection.DisplayName, ")");
            }
        }

        public GridLength ViewColumnSize
        {
            get { return _viewColumnSize; }
            set 
            {
                _viewColumnSize = value;
                Properties.Settings.Default.ViewColumnSize = _viewColumnSize.Value;
                Properties.Settings.Default.Save();
            }
        }
        private GridLength _viewColumnSize;

        private double _appWidth, _appHeight;
        public double AppWidth
        {
            get { return _appWidth; }
            set
            {
                _appWidth = value;

                if (AppWindowState == WindowState.Normal)
                {
                    Properties.Settings.Default.AppWidth = _appWidth;
                    Properties.Settings.Default.Save();
                }
            }
        }
        public double AppHeight
        {
            get { return _appHeight; }
            set
            {
                _appHeight = value;

                if (AppWindowState == WindowState.Normal)
                {
                    Properties.Settings.Default.AppHeight = _appHeight;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private WindowState _appWindowState;
        public WindowState AppWindowState
        {
            get { return _appWindowState; }
            set
            {
                _appWindowState = value;
                Properties.Settings.Default.AppWindowState = _appWindowState;
                Properties.Settings.Default.Save();
            }
        }

        #endregion

        public ObservableCollection<ISubject> SubjectSource { get; private set; }
        public ISubject SelectedSubject
        {
            get { return _subject; }
            set
            {
                _subject = value;
                RefreshPaths();
                if (SelectedSubjectChanged != null)
                    SelectedSubjectChanged(this, EventArgs.Empty);
            }
        }
        private ISubject _subject;
        public event EventHandler SelectedSubjectChanged;

        [AlsoNotifyFor("ResultHeader")]
        public DataTable Result { get; private set; }
        public string ResultSQL { get; set; }
        public string ResultHeader
        {
            get { return String.Concat("Results", Result == null ? string.Empty : String.Concat(" (", Result.Rows.Count, ")")); }
        }

        [AlsoNotifyFor("IsSearching")]
        private BackgroundWorker SearchWorker { get; set; }

        public bool IsSearching
        {
            get { return SearchWorker != null; }
        }


        public MainWindowAdapter(
            Project project, IFieldPathFactory pathFactory, 
            PresetView preset, StandardView standard, FieldPathCombo advanced, 
            RetrieveFieldsView fields)
        {
            _appWidth = Properties.Settings.Default.AppWidth;
            _appHeight = Properties.Settings.Default.AppHeight;
            _appWindowState = Properties.Settings.Default.AppWindowState;
            _viewColumnSize = new GridLength(Properties.Settings.Default.ViewColumnSize);

            Preset = preset;
            Standard = standard;
            Advanced = advanced;
            RetrieveFields = fields;

            Preset.Adapter.Search += Adapter_Search;
            Standard.Adapter.Search += Adapter_Search;
            //Advanced.Adapter.Search += Adapter_Search;

            ProjectAdapter = new ProjectAdapter(project);
            ProjectAdapter.Project.CurrentConnectionChanged += delegate 
            {
                RefreshPaths();
                PropertyChanged(this, new PropertyChangedEventArgs("ApplicationTitle")); 
            };
            PathFactory = pathFactory;
            
            SubjectSource = new ObservableCollection<ISubject>(ProjectAdapter.Configuration);
            SelectedSubject = SubjectSource[0];
        }

        private void RefreshPaths()
        {
            // ask the factory twice as the individual views alter the path instances differently
            if (_subject != null)
            {
                Preset.Adapter.SetParts(PathFactory.GetFields(SelectedSubject));
                Standard.Adapter.SetPaths(PathFactory.GetFields(SelectedSubject));
            }
        }

        void Adapter_Search(object sender, EventArgs e)
        {
            dbqf.Criterion.IParameter where;
            try { where = ((IGetParameter)sender).GetParameter(); }
            catch (Exception ex)
            {
                MessageBox.Show("There was something wrong with one or more of the parameters provided.\n\n" + ex.Message, "Search Failed", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            Search(where);
        }

        public void CancelSearch()
        {
            if (SearchWorker != null)
                SearchWorker.CancelAsync();
            SearchWorker = null;
        }

        public void Search(IParameter parameter)
        {
            if (IsSearching)
            {
                // if they don't cancel, do nothing
                if (MessageBox.Show("There is a search in progress.  Do you want to cancel the existing search?", "Search", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.No)
                    return;

                // if they said yes, cancel existing search and continue with new one
                CancelSearch();
            }

            var worker = (SearchWorker = new BackgroundWorker());
            worker.WorkerSupportsCancellation = true;

            var fields = RetrieveFields.Adapter.UseFields ? RetrieveFields.Adapter.Fields : PathFactory.GetFields(SelectedSubject);
            var gen = ResultFactory.CreateSqlGenerator(ProjectAdapter.Project.Configuration)
                .Target(SelectedSubject)
                .Column(fields)
                .Where(parameter);

            Result = null;
            ResultSQL = ((ExposedSqlGenerator)gen).GenerateSql();
            var repo = ResultFactory.CreateSqlResults(ProjectAdapter.Project.CurrentConnection);
            worker.DoWork += (s1, e1) =>
            {
                e1.Result = repo.GetResults(gen);
                e1.Cancel = worker.CancellationPending;
            };
            worker.RunWorkerCompleted += (s2, e2) =>
            {
                worker.Dispose();

                // cancellation assumes the SearchWorker property has been set null
                if (e2.Cancelled)
                    return;

                // should it ever occur that a worker that isn't cancelled is replaced?
                if (SearchWorker == worker)
                    SearchWorker = null;

                if (e2.Error != null)
                {
                    MessageBox.Show("There was an error when trying to perform the search.\n\n" + e2.Error.Message, "Search", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    // assign field references to columns returned
                    var data = (DataTable)e2.Result;
                    for (int i = 0; i < data.Columns.Count; i++)
                        data.Columns[i].ExtendedProperties.Add("FieldPath", fields[i]);
                    Result = data;
                }
            };
            worker.RunWorkerAsync();
        }

        public void Export(string filename)
        {
            throw new NotImplementedException();
        }


        public IView CurrentView
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Save(string filename)
        {
            throw new NotImplementedException();
        }

        public void Load(string filename)
        {
            throw new NotImplementedException();
        }
    }
}
