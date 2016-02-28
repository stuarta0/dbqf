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
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
}
