using System;
using Standalone.Core;
using Standalone.Core.Data;

namespace Standalone.GtkSharp
{
	public class MainWindowAdapter : Standalone.Core.ApplicationBase
	{
		public MainWindowAdapter (Project project, DbServiceFactory serviceFactory)
			: base(project, serviceFactory)
		{
		}
	}
}

