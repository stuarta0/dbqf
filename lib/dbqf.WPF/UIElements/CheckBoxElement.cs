using dbqf.Criterion;
using dbqf.Display;
using dbqf.Display.Preset;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Controls;

namespace dbqf.WPF.UIElements
{
    public class CheckBoxElement : UIElement<System.Windows.UIElement>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _trueLabel, _falseLabel, _indeterminateLabel;
        public CheckBoxElement()
        {
            PropertyChanged += delegate { };

            var check = new CheckBox();
            check.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            Element = check;

            check.IsThreeState = true;
            check.IsChecked = null;
            check.DataContext = this;

            check.SetBinding(CheckBox.IsCheckedProperty,
                new System.Windows.Data.Binding("IsChecked")
                {
                    Source = this,
                    Mode = System.Windows.Data.BindingMode.TwoWay
                });

            check.SetBinding(CheckBox.ContentProperty, 
                new System.Windows.Data.Binding("Label") 
                { 
                    Source = this, 
                    Mode = System.Windows.Data.BindingMode.OneWay 
                });
            
            SetLabels("(true)", "(false)", "(either)");
        }

        public string Label
        {
            get
            {
                if (!IsChecked.HasValue)
                    return _indeterminateLabel;
                else if (IsChecked.Value)
                    return _trueLabel;
                return _falseLabel;
            }
        }

        [AlsoNotifyFor("Label")]
        public bool? IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                OnChanged();
            }
        }
        private bool? _isChecked;

        public virtual void SetLabels(string trueLabel, string falseLabel, string indeterminateLabel)
        {
            _trueLabel = trueLabel;
            _falseLabel = falseLabel;
            _indeterminateLabel = indeterminateLabel;
            PropertyChanged(this, new PropertyChangedEventArgs("Label"));
        }
        
        public override object[] GetValues()
        {
            var check = (CheckBox)Element;
            if (check.IsChecked.HasValue)
            {
                var value = check.IsChecked.Value;
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
                ((CheckBox)Element).IsChecked = Convert.ToBoolean(values[0]);
            }
            catch 
            {
                ((CheckBox)Element).IsChecked = null;
            }
        }
    }
}
