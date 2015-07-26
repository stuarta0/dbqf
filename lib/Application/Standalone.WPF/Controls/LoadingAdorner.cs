using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Standalone.WPF.Controls
{
    public class LoadingAdorner : Adorner
    {
        private Control _content;
        VisualCollection _children;

        public LoadingAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            _children = new VisualCollection(this);
            _children.Add(_content = new LoadingControl());
            if (adornedElement is Control)
                _content.DataContext = ((Control)adornedElement).DataContext;
        }

        protected override int VisualChildrenCount { get { return _children.Count; } }
        protected override Visual GetVisualChild(int index) { return _children[index]; }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _content.Arrange(new Rect(this.DesiredSize));
            return finalSize;
        }
    }
}
