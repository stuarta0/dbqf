using System;
namespace Standalone.Core.Export
{
    public interface IExportServiceFactory
    {
        /// <summary>
        /// Gets an array of available filters (and subsequent IExportService) that this factory can create.
        /// </summary>
        /// <returns></returns>
        Filter[] GetFilters();

        /// <summary>
        /// Creates an export service based on a filter that this factory can create.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException">Occurs if a filter is given that this factory cannot create.</exception>
        IExportService Create(Filter filter);

        /// <summary>
        /// Creates an export service based on a filename assuming this IExportServiceFactory can handle it.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException">Occurs if the filename given does not match a filter that this factory can create.</exception>
        IExportService Create(string filename);
    }
}
