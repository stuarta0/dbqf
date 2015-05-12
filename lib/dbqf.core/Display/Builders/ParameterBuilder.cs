using dbqf.Criterion;
using System;
using System.Collections.Generic;
using System.Text;

namespace dbqf.Display.Builders
{
    public abstract class ParameterBuilder
    {
        /// <summary>
        /// Gets or sets the label to display to the user.
        /// </summary>
        public virtual string Label { get; set; }

        /// <summary>
        /// Gets or sets the junction object to use if multiple values need to be combined.
        /// </summary>
        public virtual Junction Junction { get; set; }

        /// <summary>
        /// Builds a parameter for the given path with the list of values parsed from the UI.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public abstract IParameter Build(FieldPath path, params object[] values);
    }
}
