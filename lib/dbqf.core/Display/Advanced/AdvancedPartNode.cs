using System;
using System.ComponentModel;
using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Criterion.Builders;
using dbqf.Parsers;

namespace dbqf.Display.Advanced
{
    /// <summary>
    /// Encapsulates logic relating to how to display a field in the advanced search control.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AdvancedPartNode : IPartViewNode, INotifyPropertyChanged
    {
        /// <summary>
        /// Construct a new AdvancedPartNode that represents an IPartViewNode.
        /// </summary>
        /// <param name="builderFactory"></param>
        public AdvancedPartNode()
        {
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
    }
}
