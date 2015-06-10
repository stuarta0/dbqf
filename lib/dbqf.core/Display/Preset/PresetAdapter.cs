using dbqf.Criterion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Collections.ObjectModel;

namespace dbqf.Display.Preset
{
    public class PresetAdapter<T> : INotifyPropertyChanged, IView<PresetPart<T>>
    {
        protected IControlFactory<T> _controlFactory;
        protected IParameterBuilderFactory _builderFactory;
        public PresetAdapter(IControlFactory<T> controlFactory, IParameterBuilderFactory builderFactory)
        {
            _controlFactory = controlFactory;
            _builderFactory = builderFactory;
            _parts = new BindingList<PresetPart<T>>();
        }

        /// <summary>
        /// Gets the controls to display on the preset control. 
        /// </summary>
        public virtual BindingList<PresetPart<T>> Parts
        {
            get { return _parts; }
        }
        protected BindingList<PresetPart<T>> _parts;
        public IEnumerable<IPartView> GetParts()
        {
            foreach (var part in Parts)
                yield return part;
        }
        public void SetParts(IEnumerable<IPartView> parts)
        {
            var myParts = new List<IPartView>(GetParts());
            foreach (var p in parts)
            {
                int index = myParts.IndexOf(p);
                if (index >= 0)
                    myParts[index].CopyFrom(p);
            }
        }

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

            var newParts = new BindingList<PresetPart<T>>();
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
            part.SelectedBuilder = _builderFactory.GetDefault(part.SelectedPath);
            part.UIElement = _controlFactory.Build(part.SelectedPath, part.SelectedBuilder);
            return part;
        }

        /// <summary>
        /// Resets all user entered values.
        /// </summary>
        public virtual void Reset()
        {
            foreach (var p in Parts)
                p.Values = null;
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

            if (con.Count == 0)
                return null;
            else if (con.Count == 1)
                return con[0];
            return con;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
