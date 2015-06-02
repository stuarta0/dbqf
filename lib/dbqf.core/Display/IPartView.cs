using System;
using System.Collections.Generic;
using System.Text;
using dbqf.Criterion;
using dbqf.Display.Builders;
using dbqf.Display.Parsers;

namespace dbqf.Display
{
    /// <summary>
    /// Represents a part in a composed view that can be converted into an IParameter.
    /// </summary>
    public interface IPartView : IGetParameter
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
