﻿using System;
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
using Standalone.Core.Export;
using System.IO;
using Standalone.Core.Serialization.Assemblers;
using Standalone.Core;
using PropertyChanged;

namespace Standalone.Forms
{
    [AddINotifyPropertyChangedInterface]
    public class MainAdapter : Core.ApplicationBase
    {
        public DbServiceFactory ServiceFactory { get; set; }
        private IDbServiceAsync _dbService;

        public PresetView Preset { get; private set; }
        public StandardView Standard { get; private set; }
        public AdvancedView Advanced { get; private set; }
        public RetrieveFieldsView RetrieveFields { get; private set; }

        public IFieldPathFactory PathFactory { get; private set; }
        public BindingSource Result { get; private set; }

        public MainAdapter(
            Project project, DbServiceFactory serviceFactory, IFieldPathFactory pathFactory, 
            PresetView preset, StandardView standard, AdvancedView advanced, 
            RetrieveFieldsView fields)
            : base(project)
        {
            ServiceFactory = serviceFactory;
            PathFactory = pathFactory;

            Preset = preset;
            Standard = standard;
            Advanced = advanced;
            _views.Add("Preset", preset.Adapter);
            _views.Add("Standard", standard.Adapter);
            _views.Add("Advanced", advanced.Adapter);

            RetrieveFields = fields;

            Preset.Adapter.Search += Adapter_Search;
            Standard.Adapter.Search += Adapter_Search;
            Advanced.Adapter.Search += Adapter_Search;

            SelectedSubjectChanged += delegate { RefreshPaths(); };

            var refresh = new EventHandler((s, e) =>
            {
                RefreshPaths();
                _dbService = ServiceFactory.CreateAsync(Project.CurrentConnection);
            });
            Project.CurrentConnectionChanged += refresh;
            Result = new BindingSource();
            refresh(this, EventArgs.Empty);
        }

        public override void Refine()
        {
            try { base.Refine(); }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Refine", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
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
            CurrentView.Reset();
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

        public void Search()
        {
            Search(CurrentView.GetParameter());
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
                MessageBox.Show(data.Exception.Message, "Search", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                ResultSQL = ((SearchDetails)data.Details).Sql;
                Result.DataSource = data.Results;
            }

            SearchCanceller = null;
        }

        public override bool Export(string filename)
        {
            return ExportFactory.Create(filename).Export(filename, (DataTable)Result.DataSource);
        }

        protected override SearchDocument Load(string filename, bool reset)
        {
            SearchDocument doc = null;
            try { doc = base.Load(filename, true); }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Load", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
            }

            var list = RetrieveFields.Adapter.Fields;
            if (doc.Outputs != null && doc.Outputs.Count > 0)
            {
                list.RaiseListChangedEvents = false;
                list.Clear();
                foreach (var path in doc.Outputs)
                    list.Add(path);
                list.RaiseListChangedEvents = true;
                list.ResetBindings();
            }
            else
                list.Clear();

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
                MessageBox.Show(ex.Message, "Save", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
