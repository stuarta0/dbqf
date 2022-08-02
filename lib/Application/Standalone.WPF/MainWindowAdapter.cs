using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Display;
using dbqf.WPF;
using PropertyChanged;
using Standalone.Core.Data;
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
    [AddINotifyPropertyChangedInterface]
    public class MainWindowAdapter : Core.ApplicationBase
    {
        public ProjectAdapter ProjectAdapter { get; private set; }
        public IFieldPathFactory PathFactory { get; private set; }
        public IDialogService DialogService { get; set; }

        public DbServiceFactory ServiceFactory { get; set; }
        private IDbServiceAsync _dbService;

        public PresetView Preset { get; private set; }
        public StandardView Standard { get; private set; }
        public AdvancedView Advanced { get; private set; }
        public RetrieveFieldsView RetrieveFields { get; private set; }

        public MainWindowAdapter(
            Project project, DbServiceFactory serviceFactory, IFieldPathFactory pathFactory,
            PresetView preset, StandardView standard, AdvancedView advanced,
            RetrieveFieldsView fields)
            : base(project)
        {
            _appWidth = Properties.Settings.Default.AppWidth;
            _appHeight = Properties.Settings.Default.AppHeight;
            _appWindowState = Properties.Settings.Default.AppWindowState;
            _viewColumnSize = new GridLength(Properties.Settings.Default.ViewColumnSize);

            Preset = preset;
            Standard = standard;
            Advanced = advanced;
            RetrieveFields = fields;
            _views.Add("Preset", preset.Adapter);
            _views.Add("Standard", standard.Adapter);
            _views.Add("Advanced", advanced.Adapter);

            Preset.Adapter.Search += Adapter_Search;
            Standard.Adapter.Search += Adapter_Search;
            Advanced.Adapter.Search += Adapter_Search;

            ProjectAdapter = new ProjectAdapter(project);
            PathFactory = pathFactory;
            ServiceFactory = serviceFactory;

            CurrentView = Preset.Adapter;
            SelectedSubjectChanged += delegate { RefreshPaths(); };
            var refresh = new EventHandler((s, e) =>
            {
                RefreshPaths();
                OnPropertyChanged("ApplicationTitle");
                _dbService = ServiceFactory.CreateAsync(Project.CurrentConnection);
            });
            ProjectAdapter.Project.CurrentConnectionChanged += refresh;
            refresh(this, EventArgs.Empty);
        }

        #region Application Appearance

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

        #region Commands

        public ICommand SaveAsCommand
        {
            get
            {
                if (_saveAsCommand == null)
                    _saveAsCommand = new RelayCommand(p => Save(DialogService.SaveFileDialog("dbqf Search (*.dbqf)|*.dbqf")));
                return _saveAsCommand;
            }
        }
        private ICommand _saveAsCommand;

        public ICommand OpenCommand
        {
            get
            {
                if (_openCommand == null)
                    _openCommand = new RelayCommand(p => Load(DialogService.OpenFileDialog("dbqf Search (*.dbqf)|*.dbqf")));
                return _openCommand;
            }
        }
        private ICommand _openCommand;

        public ICommand ExportCommand
        {
            get
            {
                if (_exportCommand == null)
                    _exportCommand = new RelayCommand(p =>
                        {
                            // TODO: handle exceptions in export
                            Export(DialogService.SaveFileDialog(
                                String.Join("|", ExportFactory.GetFilters().Convert<Filter, string>(p2 => String.Concat(p2.Name, "|", p2.FilePattern)))
                            ));
                        });
                return _exportCommand;
            }
        }
        private ICommand _exportCommand;

        public ICommand SearchCommand
        {
            get
            {
                if (_searchCommand == null)
                    _searchCommand = new RelayCommand(p => Search(CurrentView.GetParameter()));
                return _searchCommand;
            }
        }
        private ICommand _searchCommand;

        public ICommand ResetCommand
        {
            get
            {
                if (_resetCommand == null)
                    _resetCommand = new RelayCommand(p => CurrentView.Reset());
                return _resetCommand;
            }
        }
        private ICommand _resetCommand;

        public ICommand CancelSearchCommand
        {
            get
            {
                if (_cancelCommand == null)
                    _cancelCommand = new RelayCommand(p => CancelSearch());
                return _cancelCommand;
            }
        }
        private ICommand _cancelCommand;

        public ICommand RefineCommand
        {
            get
            {
                if (_refineCommand == null)
                    _refineCommand = new RelayCommand(p => Refine());
                return _refineCommand;
            }
        }
        private ICommand _refineCommand;



        #endregion

        #region View Handling

        public override IView CurrentView
        {
            get { return base.CurrentView; }
            set
            {
                base.CurrentView = value;
                OnPropertyChanged("TabIndex");
            }
        }

        /// <summary>
        /// HACK
        /// </summary>
        public int TabIndex
        {
            get 
            { 
                return new List<IView>() { Preset.Adapter, Standard.Adapter, Advanced.Adapter }.IndexOf(CurrentView);
            }
            set
            {
                var list = new List<IView>() { Preset.Adapter, Standard.Adapter, Advanced.Adapter };
                if (value >= 0 && value < list.Count)
                    CurrentView = list[value];
                OnPropertyChanged("TabIndex");
            }
        }

        #endregion

        protected override Action SearchCanceller
        {
            get
            {
                return base.SearchCanceller;
            }
            set
            {
                base.SearchCanceller = value;
                OnPropertyChanged("IsSearchingVisibility");
            }
        }
        public Visibility IsSearchingVisibility
        {
            get { return IsSearching ? Visibility.Visible : Visibility.Hidden; }
        }

        [AlsoNotifyFor("ResultHeader")]
        public DataTable Result { get; private set; }
        public string ResultHeader
        {
            get { return String.Concat("Results", Result == null ? string.Empty : String.Concat(" (", Result.Rows.Count, ")")); }
        }

        public override void Refine()
        {
            try { base.Refine(); }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Refine", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void RefreshPaths()
        {
            // HACK: ask the factory twice as the individual views alter the path instances differently
            if (SelectedSubject != null)
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

            // Get results asynchronously
            ResultSQL = null;
            var details = new SearchDetails()
            {
                Target = SelectedSubject,
                Columns = RetrieveFields.Adapter.UseFields ? RetrieveFields.Adapter.Fields : PathFactory.GetFields(SelectedSubject),
                Where = parameter
            };

            SearchCanceller = _dbService.GetResults(details, new ResultCallback(SearchComplete, details));
        }

        private void SearchComplete(IDbServiceAsyncCallback<DataTable> callback)
        {
            var data = (ResultCallback)callback;
            if (data.Exception != null)
                MessageBox.Show(data.Exception.Message, "Search", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            else
            {
                ResultSQL = ((SearchDetails)data.Details).Sql;
                Result = data.Results;
            }

            SearchCanceller = null;
        }

        public override bool Export(string filename)
        {
            if (String.IsNullOrWhiteSpace(filename))
                return false;
            return ExportFactory.Create(filename).Export(filename, Result);
        }

        protected override SearchDocument Load(string filename, bool reset)
        {
            if (String.IsNullOrWhiteSpace(filename))
                return null;

            SearchDocument doc = null;
            try { doc = base.Load(filename, true); }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Load", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return null;
            }

            var list = RetrieveFields.Adapter.Fields;
            list.Clear();
            if (doc.Outputs != null && doc.Outputs.Count > 0)
            {
                list.Clear();
                foreach (var path in doc.Outputs)
                    list.Add(path);
            }

            return doc;
        }

        protected override SearchDocument CreateSearchDocument()
        {
            var doc = base.CreateSearchDocument();
            var adapter = RetrieveFields.Adapter;
            if (adapter.UseFields && adapter.Fields.Count > 0)
                doc.Outputs = new List<IFieldPath>(adapter.Fields);
            else
                doc.Outputs = new List<IFieldPath>();
            return doc;
        }

        public override void Save(string filename)
        {
            try
            {
                base.Save(filename);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Save", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
    }
}
