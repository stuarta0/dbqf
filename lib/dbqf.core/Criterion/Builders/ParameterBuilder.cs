
namespace dbqf.Criterion.Builders
{
    public abstract class ParameterBuilder
    {
        /// <summary>
        /// Gets or sets the label to display to the user.
        /// </summary>
        public virtual string Label { get; set; }

        /// <summary>
        /// Builds a parameter for the given path with the list of values parsed from the UI.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public abstract IParameter Build(FieldPath path, params object[] values);

        protected virtual bool Eq(object prop, object otherProp)
        {
            return (prop == null && otherProp == null)
                || (prop != null && otherProp != null && prop.Equals(otherProp));
        }
    }
}
