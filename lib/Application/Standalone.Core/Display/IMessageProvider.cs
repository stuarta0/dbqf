using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standalone.Core.Display
{
    // WinForms
    // MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    //public enum MessageBoxIcon
    //{
    //    None = 0,
    //    Hand = 16,
    //    Stop = 16,
    //    Error = 16,
    //    Question = 32,
    //    Exclamation = 48,
    //    Warning = 48,
    //    Asterisk = 64,
    //    Information = 64
    //}
    //public enum MessageBoxButtons
    //{
    //    OK = 0,
    //    OKCancel = 1,
    //    AbortRetryIgnore = 2,
    //    YesNoCancel = 3,
    //    YesNo = 4,
    //    RetryCancel = 5
    //}
    //public enum DialogResult
    //{
    //    None = 0,
    //    OK = 1,
    //    Cancel = 2,
    //    Abort = 3,
    //    Retry = 4,
    //    Ignore = 5,
    //    Yes = 6,
    //    No = 7
    //}


    // WPF
    // MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
    //public enum MessageBoxImage
    //{
    //    None = 0,
    //    Hand = 16,
    //    Stop = 16,
    //    Error = 16,
    //    Question = 32,
    //    Exclamation = 48,
    //    Warning = 48,
    //    Asterisk = 64,
    //    Information = 64
    //}
    public enum MessageOption
    {
        OK,
        OKCancel,
        YesNoCancel,
        YesNo
    }
    public enum MessageResult
    {
        None,
        OK,
        Cancel,
        Yes,
        No
    }


    // GtkSharp
    // new Gtk.MessageDialog(parent, Gtk.DialogFlags.Modal, Gtk.MessageType.Error, Gtk.ButtonsType.Ok, message).Run()
    public enum MessageType
    {
        Info,
        Warning,
        Question,
        Error
    }
    //public enum ButtonsType
    //{
    //    None = 0,
    //    Ok = 1,
    //    Close = 2,
    //    Cancel = 3,
    //    YesNo = 4,
    //    OkCancel = 5
    //}
    //public enum ResponseType
    //{
    //    Help = -11,
    //    Apply = -10,
    //    No = -9,
    //    Yes = -8,
    //    Close = -7,
    //    Cancel = -6,
    //    Ok = -5,
    //    DeleteEvent = -4,
    //    Accept = -3,
    //    Reject = -2,
    //    None = -1
    //}

    /// <summary>
    /// A message box interface to allow core behaviour to show a message without knowing the window system.
    /// </summary>
    public interface IMessageProvider
    {
        MessageResult Show(string message, string title, MessageType type, MessageOption option);
    }

    /// <summary>
    /// A default message provider that does nothing.
    /// </summary>
    public class NullMessageProvider : IMessageProvider
    {
        public MessageResult Show(string message, string title, MessageType type, MessageOption option)
        {
            return MessageResult.None;
        }
    }
}
