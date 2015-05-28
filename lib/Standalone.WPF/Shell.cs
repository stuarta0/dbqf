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
    public class Shell : Standalone.Core.ShellBase
    {
        /// <summary>
        /// Gets the main UI instance.
        /// </summary>
        public MainWindow Main { get; set; }

        /// <summary>
        /// Gets or sets the control factory.  Best to initialise before Main form to ensure lists are resolved.
        /// </summary>
        public IControlFactory<UIElement> ControlFactory { get; private set; }

        public Shell(Project project, IControlFactory<UIElement> controlFactory, ResultFactory results, IList<IInitialiser> initialisers)
            : base(project, results, initialisers)
        {
            ControlFactory = controlFactory;
            ControlFactory.ListRequested += ControlFactory_ListRequested;

            // initialise last saved connection with this project
            var connectionLookup = Settings.Default.SavedConnections;
            if (connectionLookup.ContainsKey(Project.Id))
                Project.CurrentConnection = Project.Connections.Find(c => c.Identifier == connectionLookup[Project.Id]);
        }

        public override void Run()
        {
            if (Main == null)
                throw new ArgumentNullException("Main form not initialised.");
            Main.Show();
        }

        private void ControlFactory_ListRequested(object sender, ListRequestedArgs e)
        {
            base.UpdateCache(e);
        }

        protected override void OnConnectionChanged()
        {
            base.OnConnectionChanged();

            var key = Project.Id;
            if (!Settings.Default.SavedConnections.ContainsKey(key))
                Settings.Default.SavedConnections.Add(key, null);
            Settings.Default.SavedConnections[key] = Project.CurrentConnection.Identifier;
            Settings.Default.Save();
        }
    }
}
