using dbqf.Display;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace dbqf.WinForms.UIElements
{
    public abstract class ErrorProviderElement : UIElement<Control>
    {
        public virtual bool ShowError { get; set; }
        public virtual ErrorProvider Error { get; protected set; }

        public ErrorProviderElement()
        {
            Error = new ErrorProvider();
            Error.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            ShowError = false;
        }

        public override void Dispose()
        {
            if (Element != null)
                Element.Dispose();
            Error.Dispose();
            base.Dispose();
        }

        protected override void OnChanged()
        {
            if (ShowError)
            {
                try 
                {
                    GetValues();
                    Error.SetError(Element, string.Empty);
                }
                catch (Exception ex)
                {
                    Error.SetError(Element, ex.Message);
                }
            }

            base.OnChanged();
        }
    }
}
