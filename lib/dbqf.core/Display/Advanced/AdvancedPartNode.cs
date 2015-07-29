using System;
using System.ComponentModel;
using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Criterion.Builders;
using dbqf.Parsers;
using System.Text;
using System.Diagnostics;

namespace dbqf.Display.Advanced
{
    /// <summary>
    /// Encapsulates logic relating to how to display a field in the advanced search control.
    /// </summary>
    [DebuggerDisplay("{Description}")]
    public class AdvancedPartNode : AdvancedPart, IPartViewNode
    {
        /// <summary>
        /// Construct a new AdvancedPartNode that represents an IPartViewNode.
        /// </summary>
        /// <param name="builderFactory"></param>
        public AdvancedPartNode()
        {
        }

        public string Description
        {
            get
            {
                var sb = new StringBuilder();
                if (Values != null && Values.Length > 0)
                {
                    var junctionName = "or";
                    if (SelectedBuilder is JunctionBuilder && ((JunctionBuilder)SelectedBuilder).Type == JunctionType.Conjunction)
                        junctionName = "and";

                    sb.Append(Values[0]);
                    for (int i = 1; i < Values.Length; i++)
                    {
                        if (i == Values.Length - 1)
                            sb.AppendFormat(" {0} {1}", junctionName, Values[i]);
                        else
                            sb.AppendFormat(", {0}", Values[i]);
                    }
                }
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
                ComputeHash();
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
                ComputeHash();
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
                ComputeHash();
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

        public override void CopyFrom(IPartView other)
        {
            if (other is IPartViewNode)
            {
                var node = ((IPartViewNode)other);
                this.SelectedPath = node.SelectedPath;
                this.SelectedBuilder = node.SelectedBuilder;
                this.Parser = node.Parser;
                this.Values = node.Values;
            }
        }
        /// <summary>
        /// AdvancedPart considered equal when the path, builder and parser are equal.
        /// Note: value is ignored in equality test.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        bool IEquatable<IPartView>.Equals(IPartView other)
        {
            if (other == null || !(other is IPartViewNode))
                return false;

            var node = (IPartViewNode)other;
            return SelectedPath.Equals(node.SelectedPath)
                && SelectedBuilder.Equals(node.SelectedBuilder)
                && Parser.Equals(Parser, node.Parser)
                && (Values != null ? Values.Equals(node.Values) : true);
        }

        public override bool Equals(object obj)
        {
            if (obj is IPartView)
                return Equals((IPartView)obj);
            return base.Equals(obj);
        }

        private int _hash;
        private void ComputeHash()
        {
            unchecked
            {
                _hash = 13;
                if (SelectedPath != null) _hash = (_hash * 7) + SelectedPath.GetHashCode();
                if (SelectedBuilder != null) _hash = (_hash * 7) + SelectedBuilder.GetHashCode();
                if (Parser != null) _hash = (_hash * 7) + Parser.GetHashCode();
            }
        }
        public override int GetHashCode()
        {
            return _hash;
        }
    }
}
