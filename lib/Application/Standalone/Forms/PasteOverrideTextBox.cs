using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Standalone.Forms
{
    //http://stackoverflow.com/questions/3446233/hook-on-default-paste-event-of-winforms-textbox-control

    class IDataObjectEventArgs : EventArgs
    {
        public IDataObject Payload { get; private set; }
        public IDataObjectEventArgs(IDataObject payload)
        {
            Payload = payload;
        }
    }

    class ClipboardTextEventArgs : IDataObjectEventArgs
    {
        /// <summary>
        /// Translates clipboard pasted text.
        /// </summary>
        public string Text { get; set; }

        public ClipboardTextEventArgs(IDataObject payload)
            : base(payload)
        {
            Text = payload.GetData(typeof(string)) as string;
        }
    }

    class PasteOverrideTextBox : TextBox
    {
        public event EventHandler<ClipboardTextEventArgs> Pasted;

        private const int WM_PASTE = 0x0302;
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_PASTE)
            {
                if (Pasted != null)
                {
                    if (Clipboard.ContainsText())
                    {
                        var e = new ClipboardTextEventArgs(Clipboard.GetDataObject());
                        Pasted(this, e);
                        Clipboard.SetText(e.Text);
                    }
                }
            }

            base.WndProc(ref m);
        }
    }
}
