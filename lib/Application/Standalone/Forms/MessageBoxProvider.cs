using Standalone.Core.Display;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Standalone.Forms
{
    public class MessageBoxProvider : IMessageProvider
    {
        private Dictionary<MessageType, MessageBoxIcon> _icon;
        private Dictionary<MessageOption, MessageBoxButtons> _option;
        private Dictionary<DialogResult, MessageResult> _result;
        public MessageBoxProvider()
        {
            _icon = new Dictionary<MessageType, MessageBoxIcon>();
            _icon.Add(MessageType.Error, MessageBoxIcon.Error);
            _icon.Add(MessageType.Info, MessageBoxIcon.Information);
            _icon.Add(MessageType.Question, MessageBoxIcon.Question);
            _icon.Add(MessageType.Warning, MessageBoxIcon.Warning);

            _option = new Dictionary<MessageOption, MessageBoxButtons>();
            _option.Add(MessageOption.OK, MessageBoxButtons.OK);
            _option.Add(MessageOption.OKCancel, MessageBoxButtons.OKCancel);
            _option.Add(MessageOption.YesNo, MessageBoxButtons.YesNo);
            _option.Add(MessageOption.YesNoCancel, MessageBoxButtons.YesNoCancel);

            _result = new Dictionary<DialogResult, MessageResult>();
            _result.Add(DialogResult.OK, MessageResult.OK);
            _result.Add(DialogResult.Cancel, MessageResult.Cancel);
            _result.Add(DialogResult.Yes, MessageResult.Yes);
            _result.Add(DialogResult.No, MessageResult.No);
        }

        public MessageResult Show(string message, string title, MessageType type, MessageOption option)
        {
            return _result[MessageBox.Show(message, title, _option[option], _icon[type])];
        }
    }
}