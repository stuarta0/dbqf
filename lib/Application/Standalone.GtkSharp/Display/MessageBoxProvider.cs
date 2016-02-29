using Standalone.Core.Display;
using System.Collections.Generic;

namespace Standalone.GtkSharp.Display
{
    public class MessageDialogProvider : IMessageProvider
    {
        private Dictionary<MessageType, Gtk.MessageType> _icon;
        private Dictionary<MessageOption, Gtk.ButtonsType> _option;
        private Dictionary<Gtk.ResponseType, MessageResult> _result;
        public MessageDialogProvider()
        {
            _icon = new Dictionary<MessageType, Gtk.MessageType>();
            _icon.Add(MessageType.Error, Gtk.MessageType.Error);
            _icon.Add(MessageType.Info, Gtk.MessageType.Info);
            _icon.Add(MessageType.Question, Gtk.MessageType.Question);
            _icon.Add(MessageType.Warning, Gtk.MessageType.Warning);

            _option = new Dictionary<MessageOption, Gtk.ButtonsType>();
            _option.Add(MessageOption.OK, Gtk.ButtonsType.Ok);
            _option.Add(MessageOption.OKCancel, Gtk.ButtonsType.OkCancel);
            _option.Add(MessageOption.YesNo, Gtk.ButtonsType.YesNo);
            _option.Add(MessageOption.YesNoCancel, Gtk.ButtonsType.YesNo);

            _result = new Dictionary<Gtk.ResponseType, MessageResult>();
            _result.Add(Gtk.ResponseType.Ok, MessageResult.OK);
            _result.Add(Gtk.ResponseType.Cancel, MessageResult.Cancel);
            _result.Add(Gtk.ResponseType.Yes, MessageResult.Yes);
            _result.Add(Gtk.ResponseType.No, MessageResult.No);
        }

        public MessageResult Show(string message, string title, MessageType type, MessageOption option)
        {
            return _result[(Gtk.ResponseType)
                new Gtk.MessageDialog(null, Gtk.DialogFlags.Modal, Gtk.MessageType.Error, Gtk.ButtonsType.Ok, message).Run()];
        }
    }
}