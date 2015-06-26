using System;
using System.Collections.Generic;
using dbqf.Configuration;

namespace dbqf.Criterion
{
    /// <summary>
    /// Represents a specialised list of fields that must be logical (i.e. can be traversed via IRelationFields).
    /// Any attempt to add/insert/remove a field that results in an illogical path will throw an exception.
    /// </summary>
    public interface IFieldPath : IList<IField>
    {
        /// <summary>
        /// Appends another path to this path.
        /// </summary>
        /// <param name="other"></param>
        void Add(IFieldPath other);

        /// <summary>
        /// Gets or sets a meaningful description for this path.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets the last field in the path.
        /// </summary>
        IField Last { get; }

        /// <summary>
        /// Gets the root subject for the path.
        /// </summary>
        ISubject Root { get; }

        /// <summary>
        /// Python-esque list slicing.  Can use negative 'to' to indicate number of items from the end.
        /// </summary>
        /// <param name="from">The index that slicing should start from (inclusive).</param>
        /// <param name="to">The index that slicing should end at (not inclusive, i.e. this[to] will no longer exist in the collection but this[to-1] will).  Use null to indicate end of list.</param>
        /// <returns></returns>
        IFieldPath this[int from, int? to] { get; }
    }
}
