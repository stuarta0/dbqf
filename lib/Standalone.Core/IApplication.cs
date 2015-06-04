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
        IView<IPartView> CurrentView { get; set; }

        void Save(string filename);
        void Load(string filename);
        void Export(string filename);
    }
}
