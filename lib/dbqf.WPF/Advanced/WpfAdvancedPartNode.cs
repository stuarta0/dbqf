using dbqf.Criterion;
using dbqf.Criterion.Builders;
using dbqf.Display;
using dbqf.Parsers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace dbqf.WPF.Advanced
{
    [DebuggerDisplay("{Description}")]
    public class WpfAdvancedPartNode : WpfAdvancedPart
    {
        public WpfAdvancedPartNode()
        {
        }

        public string Description
        {
            get 
            {
                var sb = new StringBuilder();
                if (Values != null)
                    foreach (var obj in Values)
                        sb.Append(obj);
                return String.Concat(SelectedPath.Description, " ", SelectedBuilder.Label, " ", sb); 
            }
        }

        public virtual IFieldPath SelectedPath
        {
            get { return _field; }
            set
            {
                if (value == _field)
                    return;
                _field = value;
                OnPropertyChanged("SelectedPath");
                OnPropertyChanged("Description");
            }
        }
        protected IFieldPath _field;

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
                OnPropertyChanged("SelectedBuilder");
                OnPropertyChanged("Description");
            }
        }
        protected ParameterBuilder _builder;
        
        public virtual object[] Values
        {
            get
            {
                return _values;
            }
            set
            {
                _values = value;
                OnPropertyChanged("Values");
                OnPropertyChanged("Description");
            }
        }
        private object[] _values;

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
        public override IParameter GetParameter()
        {
            var values = Values;
            if (Parser != null && values != null)
                values = Parser.Parse(values);
            return SelectedBuilder.Build(SelectedPath, values);
        }
    }
}
