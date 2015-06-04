using System;
using System.Collections.Generic;
using System.Windows.Forms;
using dbqf.Display;
using Standalone.Core;
using Standalone.Core.Initialisers;
using Standalone.Forms;
using Standalone.Properties;

namespace Standalone
{
    /// <summary>
    /// The root of the application, containing the service bus.
    /// </summary>
    public class Shell : Standalone.Core.ShellBase
    {
        /// <summary>
        /// Gets the main UI instance.
        /// </summary>
        public Main Main { get; set; }

        /// <summary>
        /// Gets or sets the control factory.  Best to initialise before Main form to ensure lists are resolved.
        /// </summary>
        public IControlFactory<Control> ControlFactory { get; private set; }

        public Shell(Project project, IControlFactory<Control> controlFactory, ListCacher cacher, IList<IInitialiser> initialisers)
            : base(project, cacher, initialisers)
        {
            // initialise last saved connection with this project
            var connectionLookup = Settings.Default.SavedConnections;
            if (connectionLookup.ContainsKey(Project.Id))
                Project.CurrentConnection = Project.Connections.Find(c => c.Identifier == connectionLookup[Project.Id]);

            ControlFactory = controlFactory;
            ControlFactory.ListRequested += ControlFactory_ListRequested;
        }

        public override void Run()
        {
            if (Main == null)
                throw new ApplicationException("Main form not initialised.");
            System.Windows.Forms.Application.Run(Main);
        }

        private void ControlFactory_ListRequested(object sender, ListRequestedArgs e)
        {
            Cacher.UpdateCache(e);
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
