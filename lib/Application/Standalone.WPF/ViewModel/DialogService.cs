using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace Standalone.WPF.ViewModel
{
    public class DialogService : IDialogService
    {
        private OpenFileDialog _open;
        private SaveFileDialog _save;
        public DialogService()
        {
            _open = new OpenFileDialog();
            _save = new SaveFileDialog();
        }

        public string OpenFileDialog(string filter, string initialDirectory = null)
        {
            _open.Filter = filter;
            if (initialDirectory != null)
                _open.InitialDirectory = initialDirectory;

            var result = _open.ShowDialog();
            if (result == true)
                return _open.FileName;
            return null;
        }

        public string SaveFileDialog(string filter, string initialDirectory = null)
        {
            _save.Filter = filter;
            if (initialDirectory != null)
                _save.InitialDirectory = initialDirectory;

            var result = _save.ShowDialog();
            if (result == true)
                return _save.FileName;
            return null;
        }
    }
}
