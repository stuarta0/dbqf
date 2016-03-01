using System;
using Gtk;
using Standalone.GtkSharp;
using System.Collections.Generic;

public partial class MainWindow: Gtk.Window
{
	public MainWindowAdapter Adapter { get; private set; }
	public MainWindow (MainWindowAdapter adapter) 
		: base (Gtk.WindowType.Toplevel)
	{
		Build ();
		Adapter = adapter;
		Adapter.PropertyChanged += Adapter_PropertyChanged;

		var presetPage = this.notebookParameters.GetNthPage (0) as Gtk.ScrolledWindow;
		if (presetPage != null) {
			presetPage.Add (Adapter.Preset);
			Adapter.Preset.Show ();
			this.notebookParameters.Page = 0;
		}

		this.notebookResults.Page = 0;
	}

	void Adapter_PropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if ("ResultStore".Equals (e.PropertyName)) {
			// Result data table will always be fired before ResultStore and will be in the same ordering
			var toRemove = new List<TreeViewColumn> (nodeviewResults.Columns);
			foreach (var col in toRemove)
				nodeviewResults.RemoveColumn (col);
			for (int i = 0; i < Adapter.Result.Columns.Count; i++) {
				nodeviewResults.AppendColumn (new TreeViewColumn (Adapter.Result.Columns [i].ColumnName,
					new CellRendererText (), "text", i));
			}
			nodeviewResults.Model = Adapter.ResultStore;
		}
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
}
