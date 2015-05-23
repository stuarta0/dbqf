using dbqf.Configuration;
using dbqf.Display;
using dbqf.WPF;
using Standalone.Core.Data;
using Standalone.Core.Export;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standalone.WPF
{
    public class MainWindowAdapter
    {
        public Project Project { get; private set; }
        public ResultFactory ResultFactory { get; set; }
        public IFieldPathFactory PathFactory { get; private set; }
        public ExportServiceFactory ExportFactory { get; set; }

        public PresetView Preset { get; private set; }

        public string ApplicationTitle
        {
            get
            {
                return String.Concat("Database Query Framework",
                    String.IsNullOrWhiteSpace(Project.Title) ? "" : String.Concat(" - ", Project.Title));
            }
        }

        public ObservableCollection<ISubject> SubjectSource { get; private set; }
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
                //Standard.Adapter.SetPaths(PathFactory.GetFields(SelectedSubject));

                //OnPropertyChanged("SelectedSubject");
                if (SelectedSubjectChanged != null)
                    SelectedSubjectChanged(this, EventArgs.Empty);
            }
        }
        private ISubject _subject;
        public event EventHandler SelectedSubjectChanged;

        public MainWindowAdapter(
            Project project, IFieldPathFactory pathFactory, 
            PresetView preset) //, StandardView standard, AdvancedView advanced, 
            //RetrieveFieldsView fields)
        {
            Preset = preset;
            //Standard = standard;
            //Advanced = advanced;
            //RetrieveFields = fields;

            Preset.Adapter.Search += Adapter_Search;
            //Standard.Adapter.Search += Adapter_Search;
            //Advanced.Adapter.Search += Adapter_Search;

            Project = project;
            PathFactory = pathFactory;
            
            SubjectSource = new ObservableCollection<ISubject>(Project.Configuration);
            SelectedSubject = SubjectSource[0];

            //Result = new BindingSource();
        }

        void Adapter_Search(object sender, EventArgs e)
        {
            System.Windows.MessageBox.Show("Search");
        }
    }
}
