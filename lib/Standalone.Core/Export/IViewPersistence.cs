using dbqf.Display;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standalone.Core.Export
{
    // TODO: don't work with IPartView, use IView (not yet created)
    public interface IViewPersistence
    {
        void Save(string filename, SearchDocument doc);

        SearchDocument Load(string filename);
    }
}
