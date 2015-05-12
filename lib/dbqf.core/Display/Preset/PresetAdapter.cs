using dbqf.Criterion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Collections.ObjectModel;

namespace dbqf.Display.Preset
{
    public abstract class PresetAdapter<T> : INotifyPropertyChanged, IGetParameter
    {
        // need list of fields/controls to display in preset
        // should this adapter be given a factory to generate controls?
        // needs call to trigger search (actually from each control)

        protected IControlFactory<T> _controlFactory;
        protected IParameterBuilderFactory _builderFactory;
        public PresetAdapter(IControlFactory<T> controlFactory, IParameterBuilderFactory builderFactory)
        {
            _controlFactory = controlFactory;
            _builderFactory = builderFactory;
            _parts = new List<PresetPart<T>>();
        }

        /// <summary>
        /// Gets the controls to display on the preset control. 
        /// </summary>
        public virtual ReadOnlyCollection<PresetPart<T>> Parts
        {
            get { return _parts.AsReadOnly(); }
        }
        protected List<PresetPart<T>> _parts;

        public event EventHandler Search;
        private void OnSearch(object sender, EventArgs e)
        {
            if (Search != null)
                Search(this, EventArgs.Empty);
        }

        /// <summary>
        /// Create preset parts to display based on a collection of FieldPaths.
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        public virtual void SetParts(IEnumerable<FieldPath> paths)
        {
            // if no paths given, 
            if (paths == null)
                paths = new List<FieldPath>();

            var newParts = new List<PresetPart<T>>();
            foreach (var path in paths)
            {
                path.Description += ":";
                newParts.Add(CreatePart(path));
            }

            if (_parts != null)
            {
                foreach (var p in _parts)
                {
                    p.UIElement.Search -= OnSearch;
                    p.Dispose();
                }
            }

            _parts = newParts;
            foreach (var p in _parts)
                p.UIElement.Search += OnSearch;
            OnPropertyChanged("Parts");
        }

        /// <summary>
        /// Factory method to create concrete part.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected virtual PresetPart<T> CreatePart(FieldPath path)
        {
            var part = new PresetPart<T>(path);
            part.Builder = _builderFactory.GetDefault(part.Path);
            part.UIElement = _controlFactory.Build(part.Path, part.Builder);
            return part;
        }

        /// <summary>
        /// Get parameter representing the current state of the Preset view.
        /// </summary>
        /// <returns></returns>
        public virtual IParameter GetParameter()
        {
            var con = new Conjunction();
            foreach (var c in Parts)
            {
                var p = c.GetParameter();
                if (p != null)
                    con.Add(p);
            }

            return con;
        }

        /// <summary>
        /// Try to set a parameter in the Preset view.  If it cannot be represented it will throw.
        /// </summary>
        /// <param name="p"></param>
        public virtual void SetParameter(IParameter p)
        {
            // valid conditions:
            // - p is a Conjunction, unwrap and determine if we can represent it for each PresetPart
            // - otherwise, see if p can be represented in any 1 PresetPart

            // considerations:
            // - once a combination of fieldpath/parameterbuilder/control is matched to a parameter, it can't be used again
            // - if we have a parser that manipulated the value from the control, it needs to be able to revert the incoming value
            // - if we used adapter.SetParameter(adapter.GetParameter()) it should always round-trip OK.

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
