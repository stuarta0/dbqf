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
using System.IO;
using Standalone.Core.Serialization.Assemblers;
using Standalone.Core;
using PropertyChanged;

namespace Standalone.Forms
{
    public class MainAdapter : Core.ApplicationBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ResultFactory ResultFactory { get; set; }
        public IFieldPathFactory PathFactory { get; private set; }
    
        public PresetView Preset { get; private set; }
        public StandardView Standard { get; private set; }
        public AdvancedView Advanced { get; private set; }
        public RetrieveFieldsView RetrieveFields { get; private set; }

        public BindingSource Result { get; private set; }

        public MainAdapter(
            Project project, IFieldPathFactory pathFactory, 
            PresetView preset, StandardView standard, AdvancedView advanced, 
            RetrieveFieldsView fields)
            : base(project)
        {
            PathFactory = pathFactory;

            Preset = preset;
            Standard = standard;
            Advanced = advanced;
            RetrieveFields = fields;

            Preset.Adapter.Search += Adapter_Search;
            Standard.Adapter.Search += Adapter_Search;
            Advanced.Adapter.Search += Adapter_Search;

            SelectedSubjectChanged += delegate { RefreshPaths(); };
            Project.CurrentConnectionChanged += delegate { RefreshPaths(); };
            Result = new BindingSource();
            RefreshPaths();
        }

        private void RefreshPaths()
        {
            // ask the factory twice as the individual views alter the path instances differently
            if (SelectedSubject != null)
            {
                Preset.Adapter.SetParts(PathFactory.GetFields(SelectedSubject));
                Standard.Adapter.SetPaths(PathFactory.GetFields(SelectedSubject));
            }
        }

        public void Reset()
        {
            SelectedSubject = SelectedSubject;
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
            var gen = ResultFactory.CreateSqlGenerator(Project.Configuration)
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

        protected override void Export(string filename, IExportService service)
        {
            service.Export(filename, (DataTable)Result.DataSource);
        }
    }
}
