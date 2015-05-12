using dbqf.Criterion;
using dbqf.Display;
using dbqf.Display.Preset;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace dbqf.WinForms.UIElements
{
    public class CheckBoxElement : ErrorProviderElement
    {
        private string _trueLabel, _falseLabel, _indeterminateLabel;
        public CheckBoxElement()
        {
            var check = new CheckBox();
            check.ThreeState = true;
            check.CheckState = CheckState.Indeterminate;
            check.CheckStateChanged += OnCheckStateChanged;
            Element = check;

            SetLabels("(true)", "(false)", "(either)");
        }

        public virtual void SetLabels(string trueLabel, string falseLabel, string indeterminateLabel)
        {
            _trueLabel = trueLabel;
            _falseLabel = falseLabel;
            _indeterminateLabel = indeterminateLabel;
            UpdateLabel();
        }

        protected virtual void OnCheckStateChanged(object sender, EventArgs e)
        {
            UpdateLabel();
            OnChanged();
        }

        private void UpdateLabel()
        {
            var check = (CheckBox)Element;
            if (check.CheckState == CheckState.Indeterminate)
                check.Text = _indeterminateLabel;
            else if (check.CheckState == CheckState.Checked)
                check.Text = _trueLabel;
            else if (check.CheckState == CheckState.Unchecked)
                check.Text = _falseLabel;
        }

        public override object[] GetValues()
        {
            var check = (CheckBox)Element;
            if (check.CheckState != CheckState.Indeterminate)
            {
                var value = check.CheckState == CheckState.Checked;
                if (Parser != null)
                    return Parser.Parse(value);
                return new object[] { value };
            }

            return null;
        }

        public override void SetValues(params object[] values)
        {
            if (Parser != null)
                values = Parser.Revert(values);

            try
            {
                ((CheckBox)Element).CheckState = Convert.ToBoolean(values[0]) ? CheckState.Checked : CheckState.Unchecked;
            }
            catch 
            {
                ((CheckBox)Element).CheckState = CheckState.Indeterminate;
            }
        }
    }
}
