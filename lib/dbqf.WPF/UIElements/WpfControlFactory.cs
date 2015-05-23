using dbqf.Criterion;
using dbqf.Display;
using dbqf.Display.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace dbqf.WPF.UIElements
{
    public class WpfControlFactory: IControlFactory<UIElement>
    {
        public event ListRequestedEventHandler ListRequested;

        public UIElement<UIElement> Build(FieldPath path, ParameterBuilder builder)
        {
            UIElement<System.Windows.UIElement> c;

            // for now, everything is a textbox
            c = new TextBoxElement();

            return c;
        }
    }
}
