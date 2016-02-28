using System;
using dbqf.Display;
using System.ComponentModel;
using Gtk;
using System.Runtime.InteropServices;

namespace dbqf.GtkSharp
{
	/// <summary>
	/// This entire class is currently untested.
	/// </summary>
	public class ComboBoxElement : UIElement<Gtk.Widget>
	{
		public ComboBoxElement(BindingList<object> list, bool isEntry)
		{
			Gtk.ComboBox combo = (isEntry ? new ComboBoxEntry() : new ComboBox());
			combo.Model = (Gtk.TreeModel) new IListStore (list);
			combo.Changed += (sender, e) => { OnChanged(); };

			// Detect Enter key and trigger search
			combo.KeyPressEvent += (sender, e) => 
			{ 
				//if (e.RetVal == Gdk.Key.KP_Enter)
				//	OnSearch(); 
			};
			Element = combo;
		}

		public override object[] GetValues()
		{
			var combo = (ComboBox)Element;
			object[] values = null;

			TreeIter iter;
			if (combo.GetActiveIter (out iter)) {
				values = new object[] { combo.Model.GetValue (iter, 0) };
			}
			return values;
		}

		public override void SetValues(params object[] values)
		{
			var combo = (ComboBox)Element;
			TreeIter iter = new TreeIter();

			// http://stackoverflow.com/a/17340419
			if (values == null || values.Length == 0 || values[0] == null)
				iter.UserData = IntPtr.Zero;
			else
				iter.UserData = (IntPtr) GCHandle.Alloc(values [0]);
			combo.SetActiveIter (iter);
		}
	}
}

