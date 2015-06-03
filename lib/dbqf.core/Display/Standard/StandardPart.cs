using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Display.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using dbqf.Parsers;

namespace dbqf.Display.Standard
{
    /// <summary>
    /// Encapsulates logic relating to how to display a field in the standard search control.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StandardPart<T> : IPartView, INotifyPropertyChanged, IDisposable
    {
        protected IParameterBuilderFactory _builderFactory;
        protected IControlFactory<T> _controlFactory;

        /// <summary>
        /// Construct a new StandardPart with a builder and control factory for when the selected field changes.
        /// </summary>
        /// <param name="builderFactory"></param>
        public StandardPart(IParameterBuilderFactory builderFactory, IControlFactory<T> controlFactory)
        {
            _builderFactory = builderFactory;
            _controlFactory = controlFactory;
        }

        public virtual void Dispose()
        {
            if (UIElement != null)
                UIElement.Dispose();
        }

        /// <summary>
        /// Gets or sets the list of field paths to use within this control.
        /// </summary>
        public virtual BindingList<FieldPath> Paths
        {
            get { return _paths; }
            set
            {
                _paths = value;
                if (_paths == null || _paths.Count == 0)
                    throw new ArgumentException("StardardPart.Paths cannot be a null or empty list.");

                OnPropertyChanged("Paths");
                SelectedPath = _paths[0];
            }
        }
        private BindingList<FieldPath> _paths;

        /// <summary>
        /// Gets the list of builders that are relevant to the current SelectedPath.
        /// </summary>
        public virtual BindingList<ParameterBuilder> Builders
        {
            get { return _builders; }
            protected set
            {
                _builders = value;
                OnPropertyChanged("Builders");
            }
        }
        private BindingList<ParameterBuilder> _builders;

        /// <summary>
        /// Gets or sets the field used for this control.
        /// </summary>
        public virtual FieldPath SelectedPath 
        {
            get { return _path; }
            set
            {
                if (value == _path)
                    return;
                _path = value;
                OnPropertyChanged("SelectedPath");
                Builders = new BindingList<ParameterBuilder>(_builderFactory.Build(_path));
                SelectedBuilder = Builders[0];
            }
        }
        protected FieldPath _path;

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
                UIElement = _controlFactory.Build(SelectedPath, SelectedBuilder);
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
                if (value == _control)
                    return;

                object[] values = null;
                if (_control != null)
                {
                    _control.Search -= OnSearch;
                    try { values = _control.GetValues(); }
                    catch { }
                }

                _control = value;
                if (_control != null)
                {
                    _control.Search += OnSearch;

                    // try to set the values if we can, otherwise forget it
                    try { _control.SetValues(values); }
                    catch { }
                }

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

        public event EventHandler RemoveRequested;
        public virtual void Remove()
        {
            if (RemoveRequested != null)
                RemoveRequested(this, EventArgs.Empty);
        }

        public event EventHandler Search;
        private void OnSearch(object sender, EventArgs e)
        {
            if (Search != null)
                Search(this, EventArgs.Empty);
        }


        /// <summary>
        /// StandardPart will update all properties as long as the other SelectedPath and SelectedBuilder
        /// are part of this StandardPart's lists (SelectedBuilder checked from builders generated by SelectedPath).
        /// </summary>
        /// <param name="other"></param>
        public void CopyFrom(IPartView other)
        {
            if (other == null)
                return;

            if (Paths.Contains(other.SelectedPath))
            {
                var builders = _builderFactory.Build(other.SelectedPath);
                if (builders.Contains(other.SelectedBuilder))
                {
                    SelectedPath = other.SelectedPath; // this will call SelectedBuilder and UIElement setters
                    SelectedBuilder = other.SelectedBuilder; // this will call UIElement setter
                    Values = other.Values;
                    Parser = other.Parser;
                }
            }
        }

        /// <summary>
        /// StandardPart considered equal when the path, builder and parser are equal.
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
