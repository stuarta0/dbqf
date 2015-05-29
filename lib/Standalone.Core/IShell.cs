using System;
namespace Standalone.Core
{
    public interface IShell
    {
        Standalone.Core.Data.Project Project { get; set; }
        Standalone.Core.ListCacher Cacher { get; set; }

        /// <summary>
        /// Entry point.
        /// </summary>
        void Run();
    }
}
