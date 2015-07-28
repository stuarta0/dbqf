
namespace dbqf.Criterion.Builders
{
    public abstract class ParameterBuilder
    {
        /// <summary>
        /// Gets or sets the label to display to the user.
        /// </summary>
        public virtual string Label
        {
            get { return _label; }
            set
            {
                _label = value;
                ComputeHash();
            }
        }
        private string _label;

        /// <summary>
        /// Builds a parameter for the given path with the list of values parsed from the UI.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public abstract IParameter Build(IFieldPath path, params object[] values);

        protected virtual bool Eq(object prop, object otherProp)
        {
            return (prop == null && otherProp == null)
                || (prop != null && otherProp != null && prop.Equals(otherProp));
        }

        protected int _hash;
        protected virtual void ComputeHash()
        {
            unchecked
            {
                _hash = 13;
                _hash = (_hash * 7) + GetType().GetHashCode();
                _hash = (_hash * 7) + Label.GetHashCode();
            }
        }
        public override int GetHashCode()
        {
            return _hash;
        }
    }
}
