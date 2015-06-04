using dbqf.Criterion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace dbqf.Display.Standard
{
    public abstract class StandardAdapter<T> : INotifyPropertyChanged, IView<StandardPart<T>>
    {
        protected List<FieldPath> _paths;
        protected IControlFactory<T> _controlFactory;
        protected IParameterBuilderFactory _builderFactory;
        public StandardAdapter(IControlFactory<T> controlFactory, IParameterBuilderFactory builderFactory)
        {
            _controlFactory = controlFactory;
            _builderFactory = builderFactory;
            Parts = new BindingList<StandardPart<T>>();
        }

        /// <summary>
        /// Gets the controls to display on the standard control.  
        /// Use StandardAdapter.AddPart and RemovePart to modify the collection.
        /// </summary>
        public virtual BindingList<StandardPart<T>> Parts
        {
            get { return _parts; }
            protected set
            {
                _parts = value;
                OnPropertyChanged("Parts");
            }
        }
        protected BindingList<StandardPart<T>> _parts;
        public IEnumerable<IPartView> GetParts()
        {
            foreach (var part in Parts)
                yield return part;
        }

        public event EventHandler Search;
        private void OnSearch(object sender, EventArgs e)
        {
            if (Search != null)
                Search(this, EventArgs.Empty);
        }

        /// <summary>
        /// Initialise valid paths to display.
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        public virtual void SetPaths(IEnumerable<FieldPath> paths)
        {
            // if no paths given, 
            if (paths == null)
                paths = new List<FieldPath>();

            _paths = new List<FieldPath>();
            foreach (var path in paths)
                _paths.Add(path);

            foreach (var part in new List<StandardPart<T>>(Parts))
                RemovePart(part);

            AddPart();
        }

        private void OnRemoveRequested(object sender, EventArgs e)
        {
            RemovePart((StandardPart<T>)sender);
        }

        public void AddPart()
        {
            AddPart(CreatePart());
        }

        protected virtual void AddPart(StandardPart<T> part)
        {
            Parts.Add(part);
            part.PropertyChanged += Part_PropertyChanged;
            part.RemoveRequested += OnRemoveRequested;
            part.Search += OnSearch;
        }

        private void Part_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPartChanged((StandardPart<T>)sender, e);
        }

        protected virtual void OnPartChanged(StandardPart<T> part, PropertyChangedEventArgs e)
        {
            // nothing to see here
        }

        public void RemovePart(StandardPart<T> part)
        {
            Parts.Remove(part);
            part.PropertyChanged -= Part_PropertyChanged;
            part.RemoveRequested -= OnRemoveRequested;
            part.Search -= OnSearch;
            part.Dispose();
        }

        /// <summary>
        /// Factory method to create concrete part.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected virtual StandardPart<T> CreatePart()
        {
            var part = new StandardPart<T>(_builderFactory, _controlFactory);
            part.Paths = new BindingList<FieldPath>(_paths);
            return part;
        }

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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
