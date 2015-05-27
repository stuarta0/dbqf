using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using dbqf.Criterion;
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
        private Dictionary<FieldPath, CacheData> _listCache;
        private class CacheData
        {
            public BackgroundWorker CurrentWorker;
            public BindingList<object> Data;
        }

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

            // initialise last saved connection with this project
            var connectionLookup = Settings.Default.SavedConnections;
            if (connectionLookup.ContainsKey(Project.Id))
                Project.CurrentConnection = Project.Connections.Find(c => c.Identifier == connectionLookup[Project.Id]);

            _listCache = new Dictionary<FieldPath, CacheData>();
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
            if (!Settings.Default.SavedConnections.ContainsKey(key))
                Settings.Default.SavedConnections.Add(key, null);
            Settings.Default.SavedConnections[key] = project.CurrentConnection.Identifier;
            Settings.Default.Save();

            // all lists will be out of date, we need to recreate them
            if (_listCache != null)
            {
                // only downside to this method is in a long running process with many changes 
                // to the displayed subject and connection, it'll take progressively more time 
                // to refresh the cache
                foreach (var item in _listCache)
                    UpdateCache(item.Key);
            }
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
            if (_listCache.ContainsKey(e.Path))
            {
                e.List = _listCache[e.Path].Data;
                return;
            }

            UpdateCache(e.Path);
            e.List = _listCache[e.Path].Data;
        }

        /// <summary>
        /// Adds or updates data for a FieldPath list in the cache.  If it already exists, it will query the data again.
        /// </summary>
        /// <param name="path"></param>
        private void UpdateCache(dbqf.Criterion.FieldPath path)
        {
            // if item exists in cache, clear and update
            // if item does not exist in cache, add it
            CacheData result;
            if (_listCache.ContainsKey(path))
                result = _listCache[path];
            else
            {
                result = new CacheData() { Data = new BindingList<object>() };
                _listCache.Add(path, result);

                // return if the field has a pre-defined list of items
                if (path.Last.List.Count > 0)
                {
                    result.Data.Add(string.Empty);
                    foreach (var x in path.Last.List)
                        result.Data.Add(x);
                    return;
                }
            }

            var gen = ResultFactory.CreateSqlListGenerator(Project.CurrentConnection, Project.Configuration).Path(path);
            if (Regex.IsMatch(path.Last.List.Source, @"^select.*[`'\[\s]id", RegexOptions.IgnoreCase))
                gen.IdColumn("ID")
                    .ValueColumn("Value");

            // ensure we'll be able to generate a command
            try { gen.Validate(); }
            catch { return; }

            // kill any existing worker
            if (result.CurrentWorker != null)
                result.CurrentWorker.CancelAsync();

            var bgw = new BackgroundWorker();
            bgw.DoWork += (s2, e2) =>
            {
                result.CurrentWorker = bgw;
                e2.Result = ResultFactory.CreateSqlResults(Project.CurrentConnection).GetList(gen);
            };
            bgw.RunWorkerCompleted += (s2, e2) =>
            {
                if (e2.Cancelled)
                    return;

                var list = result.Data;
                list.RaiseListChangedEvents = false;
                list.Clear();
                list.Add(string.Empty);
                foreach (var i in (IList<object>)e2.Result)
                    list.Add(i);
                list.RaiseListChangedEvents = true;
                list.ResetBindings();

                if (bgw == result.CurrentWorker)
                    result.CurrentWorker = null;
            };
            bgw.RunWorkerAsync();
        }
    }
}
