using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standalone.WPF.ViewModel
{
    public interface IDialogService
    {
        string OpenFileDialog(string filter, string initialDirectory = null);
        string SaveFileDialog(string filter, string initialDirectory = null);
    }
}
