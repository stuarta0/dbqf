using dbqf.Criterion;
using System;

namespace dbqf.Display
{
    public interface IFieldPathComboBox
    {
        IFieldPath SelectedPath { get; set; }
        event EventHandler SelectedPathChanged;
    }
}
