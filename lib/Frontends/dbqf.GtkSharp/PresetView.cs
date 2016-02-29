using System;
using Gtk;
using dbqf.Display.Preset;
using System.Collections.Generic;
using System.ComponentModel;

namespace dbqf.GtkSharp
{
	[System.ComponentModel.ToolboxItem (false)]
	public partial class PresetView : Gtk.Bin
	{
		public PresetAdapter<Gtk.Widget> Adapter
		{
			get { return _adapter; }
		}

		private PresetAdapter<Gtk.Widget> _adapter;
		public PresetView(PresetAdapter<Gtk.Widget> adapter)
		{
			this.Build ();
			_adapter = adapter;
			_adapter.PropertyChanged += Adapter_PropertyChanged;
			RebuildLayout ();
		}

		void Adapter_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName.Equals("Parts"))
				RebuildLayout();
		}

		private void RebuildLayout()
		{
			if (this.Child is Table)
			{
				foreach (Widget w in ((Table)this.Child).Children)
					w.Dispose ();

				((Table)this.Child).Dispose ();
				this.Remove (this.Child);
			}

			Table table = new Table((uint)_adapter.Parts.Count + 1, 2, false);
			this.Add (table);
			table.RowSpacing = 0;
			table.ColumnSpacing = 6;
			table.BorderWidth = 3;

			for (uint i = 0; i < _adapter.Parts.Count; i++)
			{
				var part = _adapter.Parts[(int)i];

				var label = new Label(part.SelectedPath.Description.Replace("&", "&&"));
				label.Xalign = 0;
				table.Attach (label, 0, 1, i, i + 1,
					AttachOptions.Fill,
					AttachOptions.Fill,
					0, 0);
				label.Show ();

				var control = part.UIElement.Element;
				control.WidthRequest = 40;
				table.Attach (control, 1, 2, i, i + 1,
					AttachOptions.Expand | AttachOptions.Fill,
					AttachOptions.Fill,
					0, 0);
				control.Show ();
			}

			table.Show ();
			this.ShowAll ();
		}
	}
}

