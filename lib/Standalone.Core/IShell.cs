using System;
namespace Standalone.Core
{
    public interface IShell
    {
        Project Project { get; set; }
        ListCacher Cacher { get; set; }

        /// <summary>
        /// Entry point.
        /// </summary>
        void Run();
    }
}
