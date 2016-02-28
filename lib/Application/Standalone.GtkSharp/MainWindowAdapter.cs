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
				var message = new Gtk.MessageDialog(null, Gtk.DialogFlags.Modal, Gtk.MessageType.Error, Gtk.ButtonsType.Ok, 
					"There was something wrong with one or more of the parameters provided.\n\n" + ex.Message);
				message.ShowNow ();
				return;
			}

			Search(where);
		}

		public void Search()
		{
			Search(CurrentView.GetParameter());
		}

		public void Search(IParameter parameter)
		{
			if (IsSearching)
			{
				// if they don't cancel, do nothing
//				if (MessageBox.Show("There is a search in progress.  Do you want to cancel the existing search?", "Search", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
//					return;

				// if they said yes, cancel existing search and continue with new one
				CancelSearch();
			}

			// Get results asynchronously
			ResultSQL = null;
			var details = new SearchDetails()
			{
				Target = SelectedSubject,
				Columns = PathFactory.GetFields(SelectedSubject),
				Where = parameter
			};

			_dbService.GetResults(details, new ResultCallback(SearchComplete, details));
		}

		private void SearchComplete(IDbServiceAsyncCallback<DataTable> callback)
		{
			var data = (ResultCallback)callback;
			if (data.Exception != null)
				; //MessageBox.Show(data.Exception.Message, "Search", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			else
			{
				ResultSQL = ((SearchDetails)data.Details).Sql;
				// Result.DataSource = data.Results;
			}
		}
	}
}

