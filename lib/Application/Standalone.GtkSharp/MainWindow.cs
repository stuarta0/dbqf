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
        //_lastAdded = new List<TreeViewColumn>();

		var presetPage = this.notebookParameters.GetNthPage (0) as Gtk.ScrolledWindow;
		if (presetPage != null) {
			presetPage.Add (Adapter.Preset);
			Adapter.Preset.Show ();
			this.notebookParameters.Page = 0;
		}

		this.notebookResults.Page = 0;
	}

    //private List<TreeViewColumn> _lastAdded;
	void Adapter_PropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if ("ResultStore".Equals (e.PropertyName)) {
			nodeviewResults.HideAll();
            // Result data table will always be fired before ResultStore and will be in the same ordering
             var toRemove = new List<TreeViewColumn>();
             foreach (var col in nodeviewResults.Columns)
                 toRemove.Add(col);
			foreach (var col in toRemove) {
				nodeviewResults.RemoveColumn (col);
				col.Dispose ();
			}

            //foreach (var col in _lastAdded)
            //    nodeviewResults.RemoveColumn(col);
            //_lastAdded.Clear();

			for (int i = 0; i < Adapter.Result.Columns.Count; i++) {
                var col = new TreeViewColumn(Adapter.Result.Columns[i].ColumnName,
                    new CellRendererText(), "text", i);

                //_lastAdded.Add(col);
                nodeviewResults.AppendColumn (col);
			}
			nodeviewResults.Model = Adapter.ResultStore;
			nodeviewResults.ShowAll();
		}
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
}
