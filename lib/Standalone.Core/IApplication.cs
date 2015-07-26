using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dbqf.Configuration;
using dbqf.Display;

namespace Standalone.Core
{
    public interface IApplication
    {
        ISubject SelectedSubject { get; set; }
        string ApplicationTitle { get; }
        string ResultSQL { get; set; }
        IView CurrentView { get; set; }

        void Save(string filename);
        void Load(string filename);
        bool Export(string filename);
    }
}
