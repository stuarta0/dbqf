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

namespace Standalone.Core
{
    public abstract class ShellBase : Standalone.Core.IShell
    {
        public ListCacher Cacher { get; set; }
        public DbServiceFactory ServiceFactory { get; set; }

        /// <summary>
        /// Gets or sets the project in use.
        /// </summary>
        public Project Project
        {
            get { return _project; }
            set
            {
                if (_project != null)
                    _project.CurrentConnectionChanged -= Project_CurrentConnectionChanged;
                _project = value;
                if (_project != null)
                    _project.CurrentConnectionChanged += Project_CurrentConnectionChanged;
            }
        }
        private Project _project;

        public ShellBase(Project project, DbServiceFactory serviceFactory, ListCacher cacher, IList<IInitialiser> initialisers)
        {
            Project = project;
            ServiceFactory = serviceFactory;
            Cacher = cacher;
            OnConnectionChanged();

            foreach (var i in initialisers)
                i.Initialise();
        }

        void Project_CurrentConnectionChanged(object sender, EventArgs e)
        {
            OnConnectionChanged();
        }

        public abstract void Run();
        
        protected virtual void OnConnectionChanged()
        {
            Cacher.DbService = ServiceFactory.CreateAsync(Project.CurrentConnection);
        }
    }
}
