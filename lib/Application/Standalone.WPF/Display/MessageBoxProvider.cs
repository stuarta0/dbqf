using Standalone.Core.Display;
using System.Collections.Generic;
using System.Windows;

namespace Standalone.WPF.Display
{
    public class MessageBoxProvider : IMessageProvider
    {
        private Dictionary<MessageType, MessageBoxImage> _icon;
        private Dictionary<MessageOption, MessageBoxButton> _option;
        private Dictionary<MessageBoxResult, MessageResult> _result;
        public MessageBoxProvider()
        {
            _icon = new Dictionary<MessageType, MessageBoxImage>();
            _icon.Add(MessageType.Error, MessageBoxImage.Error);
            _icon.Add(MessageType.Info, MessageBoxImage.Information);
            _icon.Add(MessageType.Question, MessageBoxImage.Question);
            _icon.Add(MessageType.Warning, MessageBoxImage.Warning);

            _option = new Dictionary<MessageOption, MessageBoxButton>();
            _option.Add(MessageOption.OK, MessageBoxButton.OK);
            _option.Add(MessageOption.OKCancel, MessageBoxButton.OKCancel);
            _option.Add(MessageOption.YesNo, MessageBoxButton.YesNo);
            _option.Add(MessageOption.YesNoCancel, MessageBoxButton.YesNoCancel);

            _result = new Dictionary<MessageBoxResult, MessageResult>();
            _result.Add(MessageBoxResult.OK, MessageResult.OK);
            _result.Add(MessageBoxResult.Cancel, MessageResult.Cancel);
            _result.Add(MessageBoxResult.Yes, MessageResult.Yes);
            _result.Add(MessageBoxResult.No, MessageResult.No);
        }

        public MessageResult Show(string message, string title, MessageType type, MessageOption option)
        {
            return _result[MessageBox.Show(message, title, _option[option], _icon[type])];
        }
    }
}