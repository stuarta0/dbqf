using System;
using dbqf.Criterion;
using dbqf.Criterion.Builders;
using dbqf.Parsers;
using System.Collections.Generic;

namespace dbqf.Display
{
    /// <summary>
    /// Represents a part in a composed view that can be converted into an IParameter.
    /// </summary>
    public interface IPartView : IGetParameter, IEquatable<IPartView>
    {
        /// <summary>
        /// Configure this IPartView to match another IPartView.
        /// </summary>
        /// <param name="other"></param>
        void CopyFrom(IPartView other);
    }

    /// <summary>
    /// Represents a number of IPartView's to be used together.
    /// </summary>
    public interface IPartViewJunction : IPartView, IList<IPartView>
    {
        JunctionType Type { get; set; }
    }

    /// <summary>
    /// Represents a combination of field, parameter and value to be used when searching.
    /// </summary>
    public interface IPartViewNode : IPartView
    {
        /// <summary>
        /// Gets or sets the FieldPath that this path relates to.
        /// </summary>
        FieldPath SelectedPath { get; set; }

        /// <summary>
        /// Gets or sets the ParameterBuilder used to create an IParameter.
        /// </summary>
        ParameterBuilder SelectedBuilder { get; set; }

        /// <summary>
        /// Gets or sets the values from the UI.
        /// </summary>
        object[] Values { get; set; }

        /// <summary>
        /// Gets or sets a parser to parse Values before passing to the SelectedBuilder.
        /// </summary>
        Parser Parser { get; set; }
    }
}
