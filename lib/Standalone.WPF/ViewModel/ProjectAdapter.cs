using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dbqf.Configuration;
using PropertyChanged;
using Standalone.Core.Data;

namespace Standalone.WPF.ViewModel
{
    [ImplementPropertyChanged]
    public class ProjectAdapter
    {
        public Project Project { get; private set;  }
        public string Title { get { return Project.Title; } }
        public IConfiguration Configuration { get { return Project.Configuration; } }
        public ObservableCollection<ConnectionAdapter> Connections { get; private set; }

        public ConnectionAdapter CurrentConnection
        {
            get
            {
                return _current;
            }
            set
            {
                if (_current == value)
                    return;

                // if we have an old connection, set its IsChecked property to false
                if (_current != null)
                    _current.IsChecked = false;
                _current = value;
                if (_current != null)
                {
                    _current.IsChecked = true;
                    Project.CurrentConnection = _current.Connection;
                }
            }
        }
        private ConnectionAdapter _current;

        public ProjectAdapter(Project p)
        {
            Project = p;
            Project.CurrentConnectionChanged += Project_CurrentConnectionChanged;
            Connections = new ObservableCollection<ConnectionAdapter>(
                Project.Connections.ConvertAll((c) => new ConnectionAdapter(this, c)));
            UpdateConnectionFromProject();
        }

        void Project_CurrentConnectionChanged(object sender, EventArgs e)
        {
            UpdateConnectionFromProject();
        }

        private void UpdateConnectionFromProject()
        {
            CurrentConnection = Connections.Find(ca => ca.Connection.Equals(Project.CurrentConnection));
        }
    }
}
