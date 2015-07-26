using dbqf.Display.Advanced;
using System.Diagnostics;
using System.Drawing;

namespace dbqf.WinForms.Advanced
{
    [DebuggerDisplay("WinForms {Description}")]
    public class WinFormsAdvancedPartNode : AdvancedPartNode
    {
        public Color NodeBackColor
        {
            get
            {
                if (IsSelected)
                    return Color.FromArgb(225, 240, 255);
                return Color.Transparent;
            }
        }

        protected override void OnIsSelectedChanged()
        {
            base.OnIsSelectedChanged();
            OnPropertyChanged("NodeBackColor");
        }

        public void Select()
        {
            this.IsSelected = !this.IsSelected;
        }

        public void Delete()
        {
            OnDeleteRequested();
        }
    }
}
