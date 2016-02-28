using System;
using Gtk;
using Standalone.GtkSharp;

public partial class MainWindow: Gtk.Window
{
	public MainWindowAdapter Adapter { get; private set; }
	public MainWindow (MainWindowAdapter adapter) 
		: base (Gtk.WindowType.Toplevel)
	{
		Build ();
		Adapter = adapter;

		var presetPage = this.notebookParameters.GetNthPage (0) as Gtk.ScrolledWindow;
		if (presetPage != null) {
			presetPage.Add (Adapter.Preset);
			Adapter.Preset.Show ();
			this.notebookParameters.Page = 0;
		}

		this.notebookResults.Page = 0;
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
}
