using System;
using Standalone.Core;
using Standalone.Core.Data;
using dbqf.GtkSharp;
using dbqf.Display;
using dbqf.Criterion;
using System.Data;
using Gtk;
using System.Collections.Generic;
using PropertyChanged;

namespace Standalone.GtkSharp
{
	[ImplementPropertyChanged]
	public class MainWindowAdapter : Standalone.Core.ApplicationBase
	{
		public PresetView Preset { get; private set; }
		public IFieldPathFactory PathFactory { get; private set; }
		public ListStore ResultStore { get; private set; }
		
		public MainWindowAdapter (Project project, DbServiceFactory serviceFactory, IFieldPathFactory pathFactory, 
				PresetView preset)
			: base(project, serviceFactory)
		{
			PathFactory = pathFactory;
            MessageProvider = new Display.MessageDialogProvider();

			Preset = preset;
			_views.Add("Preset", preset.Adapter);
			Preset.Adapter.Search += Adapter_Search;

			// HACK
			SelectedSubject = project.Configuration [2];
			Preset.Adapter.SetParts(PathFactory.GetFields(SelectedSubject));
			CurrentView = Preset.Adapter;

			this.PropertyChanged += MainWindowAdapter_PropertyChanged;
		}

		void MainWindowAdapter_PropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if ("Result".Equals(e.PropertyName)) {
				var types = new List<Type> ();
				foreach (DataColumn column in Result.Columns) {
					types.Add (column.DataType);
				}

				// http://stackoverflow.com/a/7624978
				var store = new ListStore (types.ToArray());
				foreach (DataRow row in Result.Rows) {
					store.AppendValues (row.ItemArray);
				}

				ResultStore = store;
			}
		}

		void Adapter_Search(object sender, EventArgs e)
		{
			dbqf.Criterion.IParameter where;
			try { where = ((IGetParameter)sender).GetParameter(); }
			catch (Exception ex)
			{
                using (var message = new Gtk.MessageDialog(null, Gtk.DialogFlags.Modal, Gtk.MessageType.Error, Gtk.ButtonsType.Ok,
                    "There was something wrong with one or more of the parameters provided.\n\n" + ex.Message))
                    message.Run();
                
				return;
			}

			Search(where);
		}

		public void Search(IParameter parameter)
		{
            base.Search(parameter, PathFactory.GetFields(SelectedSubject));
		}
	}
}

