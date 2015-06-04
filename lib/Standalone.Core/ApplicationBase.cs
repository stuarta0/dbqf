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
    public class ApplicationBase : IApplication
    {
        public Project Project { get; protected set; }
        public ExportServiceFactory ExportFactory { get; set; }
        public IViewPersistence ViewPersistence { get; set; }
    
        public string ApplicationTitle
        {
            get
            {
                return String.Concat("Database Query Framework",
                    String.IsNullOrWhiteSpace(Project.Title) ? "" : String.Concat(" - ", Project.Title));
            }
        }

        public IView<IPartView> CurrentView { get; set; }
        public BindingList<ISubject> SubjectSource { get; private set; }
        public virtual ISubject SelectedSubject { get; set; }
        public event EventHandler SelectedSubjectChanged = delegate { };
        private void OnSelectedSubjectChanged()
        {
            SelectedSubjectChanged(this, EventArgs.Empty);
        }

        public string ResultSQL { get; set; }

        [AlsoNotifyFor("IsSearching")]
        protected BackgroundWorker SearchWorker { get; set; }
        public bool IsSearching
        {
            get { return SearchWorker != null; }
        }

        public ApplicationBase(Project project)
        {
            Project = project;
            SubjectSource = new BindingList<ISubject>(Project.Configuration);
            SelectedSubject = SubjectSource[0];
        }

        public virtual void CancelSearch()
        {
            if (SearchWorker != null)
                SearchWorker.CancelAsync();
            SearchWorker = null;
        }

        public virtual void Export(string filename)
        {
            if (ExportFactory == null)
                return; 

            var ext = System.IO.Path.GetExtension(filename);
            ExportServiceFactory.ExportType etype;
            if (ext.Equals(".csv", StringComparison.OrdinalIgnoreCase))
                etype = ExportServiceFactory.ExportType.CommaSeparated;
            else if (ext.Equals(".txt", StringComparison.OrdinalIgnoreCase))
                etype = ExportServiceFactory.ExportType.TabDelimited;
            else
                throw new NotImplementedException(String.Concat("Export to file with extension ", ext, " not implemented."));

            Export(filename, ExportFactory.Create(etype));
        }
        protected virtual void Export(string filename, IExportService service)
        {
        }

        public virtual void Load(string filename)
        {
            if (ViewPersistence == null)
                return;

            IList<IPartView> parts = ViewPersistence.Load(filename);
            if (parts == null)
                throw new ApplicationException("Failed to load search.");

            foreach (var p in CurrentView.Parts)
            {
                int index = parts.IndexOf(p);
                if (index >= 0)
                    p.CopyFrom(parts[index]);
            }
        }

        public virtual void Save(string filename)
        {
            if (ViewPersistence == null)
                return;

            List<IPartView> parts = new List<IPartView>();
            foreach (var p in CurrentView.Parts)
            {
                if (p.GetParameter() != null)
                    parts.Add(p);
            }

            if (parts.Count > 0)
                ViewPersistence.Save(filename, parts);
            else
                throw new ArgumentException("Select at least one parameter to save.");
        }
    }
}
