using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using dbqf.WPF;
using PropertyChanged;
using Standalone.Core;
using Standalone.Core.Data;

namespace Standalone.WPF.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class ConnectionAdapter
    {
        private ProjectAdapter _project;
        public ProjectConnection Connection { get; private set; }
        public string DisplayName { get { return Connection.DisplayName; } }

        public bool IsChecked
        {
            get { return _isChecked; }
            set 
            {
                if (_isChecked == value)
                    return;
                _isChecked = value;
                if (_isChecked)
                    _project.CurrentConnection = this;
            } 
        }
        private bool _isChecked;

        public ConnectionAdapter(ProjectAdapter p, ProjectConnection c)
        {
            _project = p;
            Connection = c;
        }
    }
}
