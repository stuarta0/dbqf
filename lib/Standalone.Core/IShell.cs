using System;
namespace Standalone.Core
{
    public interface IShell
    {
        Standalone.Core.Data.Project Project { get; set; }
        Standalone.Core.Data.ResultFactory ResultFactory { get; }

        /// <summary>
        /// Entry point.
        /// </summary>
        void Run();

        /// <summary>
        /// Reset cache containing list data.
        /// </summary>
        void ResetCache();
    }
}
