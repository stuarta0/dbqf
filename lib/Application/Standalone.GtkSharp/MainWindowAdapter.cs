using System;
using Standalone.Core;
using Standalone.Core.Data;
using dbqf.GtkSharp;
using dbqf.Display;
using dbqf.Criterion;
using System.Data;

namespace Standalone.GtkSharp
{
	public class MainWindowAdapter : Standalone.Core.ApplicationBase
	{
		public PresetView Preset { get; private set; }
		public IFieldPathFactory PathFactory { get; private set; }
		
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
			Preset.Adapter.SetParts(PathFactory.GetFields(project.Configuration[0]));
			CurrentView = Preset.Adapter;
			SelectedSubject = project.Configuration [0];
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
            base.Search(parameter, PathFactory.GetFields(Project.Configuration[0]));
		}
	}
}

