using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Display;
using dbqf.WinForms;
using Standalone.Core.Data;
using Standalone.Core.Data.Processing;
using Standalone.Core.Export;

namespace Standalone.Forms
{
    public class MainAdapter : INotifyPropertyChanged, Core.IApplication
    {
        public Project Project { get; private set; }
        public ResultFactory ResultFactory { get; set; }
        public IFieldPathFactory PathFactory { get; private set; }
        public ExportServiceFactory ExportFactory { get; set; }
    
        public PresetView Preset { get; private set; }
        public StandardView Standard { get; private set; }
        public AdvancedView Advanced { get; private set; }
        public RetrieveFieldsView RetrieveFields { get; private set; }

        public string ApplicationTitle
        {
            get
            {
                return String.Concat("Database Query Framework",
                    String.IsNullOrWhiteSpace(Project.Title) ? "" : String.Concat(" - ", Project.Title));
            }
        }

        public BindingList<ISubject> SubjectSource { get; private set; }
        public ISubject SelectedSubject
        {
            get { return _subject; }
            set
            {
                if (_subject == value)
                    return;
                _subject = value;

                // ask the factory twice as the individual views alter the path instances differently
                Preset.Adapter.SetParts(PathFactory.GetFields(SelectedSubject));
                Standard.Adapter.SetPaths(PathFactory.GetFields(SelectedSubject));

                OnPropertyChanged("SelectedSubject");
                if (SelectedSubjectChanged != null)
                    SelectedSubjectChanged(this, EventArgs.Empty);
            }
        }
        private ISubject _subject;
        public event EventHandler SelectedSubjectChanged;

        public string ResultSQL
        {
            get { return _resultSql; }
            set
            {
                if (_resultSql == value)
                    return;
                _resultSql = value;
                OnPropertyChanged("ResultSQL");
            }
        }
        private string _resultSql;

        public BindingSource Result
        {
            get { return _result; }
            private set
            {
                if (_result == value)
                    return;
                _result = value;
                OnPropertyChanged("Result");
            }
        }
        private BindingSource _result;

        public bool IsSearching
        {
            get { return _searchWorker != null; }
        }
        private BackgroundWorker SearchWorker
        {
            get { return _searchWorker; }
            set
            {
                if (_searchWorker == value)
                    return;
                _searchWorker = value;
                OnPropertyChanged("IsSearching");
            }
        }
        private BackgroundWorker _searchWorker;
        
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public MainAdapter(
            Project project, IFieldPathFactory pathFactory, 
            PresetView preset, StandardView standard, AdvancedView advanced, 
            RetrieveFieldsView fields)
        {
            Preset = preset;
            Standard = standard;
            Advanced = advanced;
            RetrieveFields = fields;

            Preset.Adapter.Search += Adapter_Search;
            Standard.Adapter.Search += Adapter_Search;
            Advanced.Adapter.Search += Adapter_Search;

            Project = project;
            PathFactory = pathFactory;
            
            SubjectSource = new BindingList<ISubject>(Project.Configuration);
            SelectedSubject = SubjectSource[0];

            Result = new BindingSource();
        }

        void Adapter_Search(object sender, EventArgs e)
        {
            dbqf.Criterion.IParameter where;
            try { where = ((IGetParameter)sender).GetParameter(); }
            catch (Exception ex)
            {
                MessageBox.Show("There was something wrong with one or more of the parameters provided.\n\n" + ex.Message, "Search Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                if (MessageBox.Show("There is a search in progress.  Do you want to cancel the existing search?", "Search", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    return;

                // if they said yes, cancel existing search and continue with new one
                CancelSearch();
            }

            var worker = (SearchWorker = new BackgroundWorker());
            worker.WorkerSupportsCancellation = true;

            var fields = RetrieveFields.Adapter.UseFields ? RetrieveFields.Adapter.Fields : PathFactory.GetFields(SelectedSubject);
            var gen = ResultFactory.CreateSqlGenerator(Project.CurrentConnection, Project.Configuration)
                .Target(SelectedSubject)
                .Column(fields)
                .Where(parameter);

            Result.DataSource = null;
            ResultSQL = ((ExposedSqlGenerator)gen).GenerateSql();
            var repo = ResultFactory.CreateSqlResults(Project.CurrentConnection);
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
                        MessageBox.Show("There was an error when trying to perform the search.\n\n" + e2.Error.Message, "Search", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        // assign field references to columns returned
                        var data = (DataTable)e2.Result;
                        for (int i = 0; i < data.Columns.Count; i++)
                            data.Columns[i].ExtendedProperties.Add("FieldPath", fields[i]);
                        Result.DataSource = data;
                    }
                };
            worker.RunWorkerAsync();
        }

        public void Reset()
        {

        }

        public void Export(string filename)
        {
            var ext = System.IO.Path.GetExtension(filename);
            ExportServiceFactory.ExportType etype;
            if (ext.Equals(".csv", StringComparison.OrdinalIgnoreCase))
                etype = ExportServiceFactory.ExportType.CommaSeparated;
            else if (ext.Equals(".txt", StringComparison.OrdinalIgnoreCase))
                etype = ExportServiceFactory.ExportType.TabDelimited;
            else
            {
                MessageBox.Show(String.Concat("Export to file with extension ", ext, " not implemented."), "Export", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            ExportFactory.Create(etype).Export(filename, (DataTable)Result.DataSource);
        }
    }
}
