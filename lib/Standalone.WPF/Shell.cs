using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using dbqf.Display;
using Standalone.Core.Data;
using Standalone.Core.Initialisers;
using Standalone.WPF.Properties;

namespace Standalone.WPF
{
    public class Shell
    {
        /// <summary>
        /// Gets the main UI instance.
        /// </summary>
        public MainWindow Main { get; set; }

        /// <summary>
        /// Gets or sets the control factory.  Best to initialise before Main form to ensure lists are resolved.
        /// </summary>
        public IControlFactory<UIElement> ControlFactory { get; private set; }
        private Dictionary<int, BindingList<object>> _listCache;

        /// <summary>
        /// Gets or sets the result retriever.
        /// </summary>
        public ResultFactory ResultFactory { get; private set; }

        /// <summary>
        /// Gets or sets the project in use.
        /// </summary>
        public Project Project
        {
            get { return _project; }
            set
            {
                if (_project != null)
                {
                    _project.CurrentConnectionChanged -= Project_CurrentConnectionChanged;
                }
                _project = value;
                if (_project != null)
                {
                    _project.CurrentConnectionChanged += Project_CurrentConnectionChanged;
                }
            }
        }
        private Project _project;

        public Shell(Project p, IControlFactory<UIElement> controls, ResultFactory results, IList<IInitialiser> initialisers)
        {
            Project = p;

            //// initialise last saved connection with this project
            //var connectionLookup = Settings.Default.SavedConnections;
            //if (connectionLookup.ContainsKey(Project.Id))
            //    Project.CurrentConnection = Project.Connections.Find(c => c.Identifier == connectionLookup[Project.Id]);

            _listCache = new Dictionary<int, BindingList<object>>();
            ControlFactory = controls;
            ControlFactory.ListRequested += ControlFactory_ListRequested;
            ResultFactory = results;

            foreach (var i in initialisers)
                i.Initialise();
        }

        public void Run()
        {
            if (Main == null)
                throw new ArgumentNullException("Main form not initialised.");
            Main.Show();
        }

        void Project_CurrentConnectionChanged(object sender, EventArgs e)
        {
            var project = (Project)sender;
            var key = project.Id;
            //if (!Settings.Default.SavedConnections.ContainsKey(key))
            //    Settings.Default.SavedConnections.Add(key, null);
            //Settings.Default.SavedConnections[key] = project.CurrentConnection.Identifier;
            Settings.Default.Save();
        }

        private void ControlFactory_ListRequested(object sender, ListRequestedArgs e)
        {
            // deals with the case where the list was set manually or some other event handler dealt with it,
            // or if we don't have the required instances to fulfill the request
            if (e.List != null || e.Path.Last.List == null || Project == null || ResultFactory == null)
                return;

            // initialise the type of list, pending data below
            e.Type = e.Path.Last.List.Type;

            // if we've cached this path before, use it
            if (_listCache.ContainsKey(e.Path.GetHashCode()))
            {
                e.List = _listCache[e.Path.GetHashCode()];
                return;
            }

            // return if the field has a pre-defined list of items
            if (e.Path.Last.List.Count > 0)
            {
                e.List = new BindingList<object>(e.Path.Last.List);
                e.List.Insert(0, string.Empty);
                _listCache.Add(e.Path.GetHashCode(), e.List);
                return;
            }

            var gen = ResultFactory.CreateSqlListGenerator(Project.CurrentConnection, Project.Configuration).Path(e.Path);
            if (Regex.IsMatch(e.Path.Last.List.Source, @"^select.*[`'\[\s]id", RegexOptions.IgnoreCase))
                gen.IdColumn("ID")
                    .ValueColumn("Value");

            // ensure we'll be able to generate a command
            try { gen.Validate(); }
            catch { return; }

            e.List = new BindingList<object>();
            _listCache.Add(e.Path.GetHashCode(), e.List);

            var bgw = new BackgroundWorker();
            bgw.DoWork += (s2, e2) => e2.Result = ResultFactory.CreateSqlResults(Project.CurrentConnection).GetList(gen);
            bgw.RunWorkerCompleted += (s2, e2) =>
            {
                e.List.RaiseListChangedEvents = false;
                e.List.Add(string.Empty);
                foreach (var i in (IList<object>)e2.Result)
                    e.List.Add(i);
                e.List.RaiseListChangedEvents = true;
                e.List.ResetBindings();
            };
            bgw.RunWorkerAsync();
        }
    }
}
