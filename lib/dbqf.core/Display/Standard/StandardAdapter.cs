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

        /// <summary>
        /// Add as many parts as required that we can validly represent in the current setup.  This will leave other parts in place.
        /// </summary>
        /// <param name="parts"></param>
        /// <exception cref="System.ApplicationException">Thrown if some parts couldn't be represented in this view.</exception>
        public void SetParts(IEnumerable<IPartView> parts)
        {
            // only adding parts, so leave existing Parts alone
            // for each p in parts
            //   can we copy from p?  if so, do it, otherwise try the next part
            //   if we get to the end and the last part we added couldn't be populated, remove it

            int total = 0;
            var skipped = new List<IPartView>();
            StandardPart<T> part = null;
            foreach (var p in parts)
            {
                total++;
                if (part == null)
                    part = AddPart();
                if (part.CanCopyFrom(p))
                {
                    part.CopyFrom(p);
                    part = null;
                }
                else
                    skipped.Add(p);
            }

            if (part != null)
                this.RemovePart(part);

            // if skipped contains values, we should throw an exception here with the remaining parts
            if (skipped.Count > 0)
            {
                var message = new StringBuilder();
                message.AppendLine(String.Format("Could not load {0} of {1} parameters:", skipped.Count, total));
                foreach (var p in skipped)
                    message.AppendLine(String.Format("- {0} {1} {2}",
                        p.SelectedPath.Description,
                        p.SelectedBuilder.Label,
                        p.Values != null ? 
                            String.Join(", ", p.Values.Convert<object, string>(v => v != null ? v.ToString() : "").ToArray())
                            : string.Empty));
                
                throw new ApplicationException(message.ToString());
            }
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

        public StandardPart<T> AddPart()
        {
            return AddPart(CreatePart());
        }

        protected virtual StandardPart<T> AddPart(StandardPart<T> part)
        {
            Parts.Add(part);
            part.PropertyChanged += Part_PropertyChanged;
            part.RemoveRequested += OnRemoveRequested;
            part.Search += OnSearch;
            return part;
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

        /// <summary>
        /// Removes all parts.
        /// </summary>
        public virtual void Reset()
        {
            foreach (var p in new List<StandardPart<T>>(Parts))
                this.RemovePart(p);
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
