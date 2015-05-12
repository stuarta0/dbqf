using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace dbqf.WinForms.Extensions
{
    internal class ComboBoxExtension
    {
        private static List<ComboBox> _calculated = new List<ComboBox>();

        public static void AdjustWidthOnDropDown(params ComboBox[] combo)
        {
            foreach (var c in combo)
            {
                c.DropDown += AdjustWidth_DropDown;
                c.Disposed += Combo_Disposed;
            }
        }

        private static void Combo_Disposed(object sender, EventArgs e)
        {
            // static hooks need to be released or we'll cause a memory leak
            ComboBox combo = (ComboBox)sender;
            combo.DropDown -= AdjustWidth_DropDown;
            combo.Disposed -= Combo_Disposed;
            _calculated.Remove(combo);
        }

        private static void AdjustWidth_DropDown(object sender, EventArgs e)
        {
            ComboBox combo = (ComboBox)sender;

            // do nothing if we've calculated the width previously
            if (_calculated.Contains(combo))
                return;

            int width = combo.DropDownWidth;
            Graphics g = combo.CreateGraphics();
            Font font = combo.Font;

            int vertScrollBarWidth =
                (combo.Items.Count > combo.MaxDropDownItems) ? SystemInformation.VerticalScrollBarWidth : 0;

            int newWidth;
            for (int i = 0; i < combo.Items.Count; i++)
            {
                newWidth = (int)g.MeasureString(combo.GetItemText(combo.Items[i]), font).Width + vertScrollBarWidth;
                if (width < newWidth)
                    width = newWidth;
            }

            combo.DropDownWidth = width;
            _calculated.Add(combo);
        }
    }
}
