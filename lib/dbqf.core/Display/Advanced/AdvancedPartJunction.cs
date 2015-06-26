using System;
using System.ComponentModel;
using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Criterion.Builders;
using dbqf.Parsers;

namespace dbqf.Display.Advanced
{
    /// <summary>
    /// Encapsulates logic relating to how to display a junction in the advanced search control.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AdvancedPartJunction<T> : PartViewJunction
    {
        /// <summary>
        /// Construct a new AdvancedPartJunction that represents an IPartViewJunction.
        /// </summary>
        /// <param name="builderFactory"></param>
        public AdvancedPartJunction()
        {
        }
    }
}
