using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Display.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using dbqf.Parsers;

namespace dbqf.Display.Preset
{
    /// <summary>
    /// Encapsulates logic relating to how to display a field in the preset search control.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PresetPart<T> : IPartView, INotifyPropertyChanged, IDisposable
    {
        public PresetPart(FieldPath path)
        {
            SelectedPath = path;
            FullWidth = false;
        }

        public virtual void Dispose()
        {
            if (UIElement != null)
                UIElement.Dispose();
        }
        
        public virtual FieldPath SelectedPath 
        {
            get { return _field; }
            set
            {
                if (value == _field)
                    return;
                _field = value;
                OnPropertyChanged("Path");
            }
        }
        protected FieldPath _field;

        /// <summary>
        /// Gets or sets the parameter builder to use with this control.
        /// </summary>
        public virtual ParameterBuilder SelectedBuilder 
        {
            get { return _builder; } 
            set
            {
                if (value == _builder)
                    return;
                _builder = value;
                OnPropertyChanged("Builder");
            }
        }
        protected ParameterBuilder _builder;

        /// <summary>
        /// Gets the control to display on the preset control.  Type should be base control/widget type depending on the UI system in use.
        /// </summary>
        public virtual UIElement<T> UIElement 
        {
            get { return _control; }
            set
            {
                _control = value;
                OnPropertyChanged("UIElement");
            }
        }
        protected UIElement<T> _control;

        public virtual object[] Values
        {
            get
            {
                if (UIElement == null)
                    return null;
                return UIElement.GetValues();
            }
            set
            {
                if (UIElement != null)
                    UIElement.SetValues(value);
            }
        }

        public virtual Parser Parser
        {
            get { return _parser; }
            set
            {
                if (_parser == value)
                    return;
                _parser = value;
                OnPropertyChanged("Parser");
            }
        }
        private Parser _parser;

        /// <summary>
        /// In the preset control there are 2 columns, the first for the label and the second for the control.
        /// Setting FullWidth = True will use both columns for the control.
        /// </summary>
        public virtual bool FullWidth 
        {
            get { return _fullWidth; }
            set
            {
                if (value == _fullWidth)
                    return;
                _fullWidth = value;
                OnPropertyChanged("FullWidth");
            }
        }
        protected bool _fullWidth;

        /// <summary>
        /// Using the control and builder, constructs the final parameter based on what the user has selected.
        /// </summary>
        /// <returns>The parameter or null if no parameter can be provided.</returns>
        public virtual IParameter GetParameter()
        {
            var values = Values;
            if (Parser != null && values != null)
                values = Parser.Parse(values);
            return SelectedBuilder.Build(SelectedPath, values);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// PresetPart will only copy values from the other view.  
        /// The reasoning is that PresetPart has no way to update the UIElement if the SelectedBuilder changes.
        /// </summary>
        /// <param name="other"></param>
        public void CopyFrom(IPartView other)
        {
            if (other == null)
                return;

            this.Values = other.Values;
        }

        /// <summary>
        /// PresetPart considered equal when the path, builder and parser are equal.
        /// Note: value is ignored in equality test.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IPartView other)
        {
            if (other == null)
                return false;

            return SelectedPath.Equals(other.SelectedPath)
                && SelectedBuilder.Equals(other.SelectedBuilder)
                && Parser.Equals(Parser, other.Parser);
        }

        public override bool Equals(object obj)
        {
            if (obj is IPartView)
                return Equals((IPartView)obj);
            return base.Equals(obj);
        }
    }
}
