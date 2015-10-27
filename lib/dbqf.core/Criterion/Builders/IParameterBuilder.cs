namespace dbqf.Criterion.Builders
{
    public interface IParameterBuilder
    {
        /// <summary>
        /// Gets or sets the label to display to the user.
        /// </summary>
        string Label { get; set; }

        /// <summary>
        /// Builds a parameter for the given path with the list of values parsed from the UI.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        IParameter Build(IFieldPath path, params object[] values);
    }
}
