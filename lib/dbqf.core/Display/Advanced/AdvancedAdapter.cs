using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Criterion.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace dbqf.Display.Advanced
{
    /// <summary>
    /// Parts in the Advanced view represent the user-constructed tree of parameters.
    /// 
    /// Layout will be:
    /// - Subject (IList of ISubject)
    /// - Field (IFieldPathCombo)
    /// - Criterion (IParameterBuilderFactory)
    /// - Control (IControlFactory)
    /// - AND/OR buttons to add to IPartView
    /// - AdvancedPartViewJunction (hierarchy of IPartViews)
    /// 
    /// In addition, SelectedPart may be useful to know what to do when AND/OR clicked
    /// </summary>
    /// <typeparam name="T">The type of UI control in use.</typeparam>
    public abstract class AdvancedAdapter<T> : IView, INotifyPropertyChanged
    {
        protected IParameterBuilderFactory _builderFactory;
        protected IControlFactory<T> _controlFactory;
        protected IFieldPathComboBox _pathCombo;

        public AdvancedAdapter(
            IList<ISubject> subjects, 
            IFieldPathComboBox pathCombo, 
            IParameterBuilderFactory builderFactory, 
            IControlFactory<T> controlFactory)
        {
            // hook up utilities first so we can trickle down the initial ParameterBuilder/UIElement creation
            _builderFactory = builderFactory;
            _controlFactory = controlFactory;
            _pathCombo = pathCombo;
            _pathCombo.SelectedPathChanged += PathCombo_SelectedPathChanged;

            // now set subject source (begins process of creating initial set of options for user)
            SubjectSource = new BindingList<ISubject>(subjects);
        }

        private void PathCombo_SelectedPathChanged(object sender, EventArgs e)
        {
            BuilderSource = new BindingList<ParameterBuilder>(_builderFactory.Build(_pathCombo.SelectedPath));
        }

        #region Field/Value Selection

        public virtual BindingList<ISubject> SubjectSource
        {
            get { return _subjects; }
            set
            {
                if (object.Equals(_subjects, value))
                    return;

                _subjects = value;
                OnPropertyChanged("SubjectSource");
                SelectedSubject = _subjects[0];
            }
        }
        private BindingList<ISubject> _subjects;

        public virtual ISubject SelectedSubject
        {
            get { return _selectedSubject; }
            set
            {
                if (object.Equals(_selectedSubject, value))
                    return;

                _selectedSubject = value;
                OnPropertyChanged("SelectedSubject");
                _pathCombo.SelectedPath = FieldPath.FromDefault(_selectedSubject.DefaultField);
            }
        }
        private ISubject _selectedSubject;

        public virtual BindingList<ParameterBuilder> BuilderSource
        {
            get { return _builders; }
            set
            {
                if (object.Equals(_builders, value))
                    return;

                _builders = value;
                OnPropertyChanged("BuilderSource");
                SelectedBuilder = _builders[0];
            }
        }
        private BindingList<ParameterBuilder> _builders;

        public virtual ParameterBuilder SelectedBuilder
        {
            get { return _selectedBuilder; }
            set
            {
                if (object.Equals(_selectedBuilder, value))
                    return;

                _selectedBuilder = value;
                OnPropertyChanged("SelectedBuilder");
                UIElement = _controlFactory.Build(_pathCombo.SelectedPath, _selectedBuilder);
            }
        }
        private ParameterBuilder _selectedBuilder;

        public virtual UIElement<T> UIElement
        {
            get { return _uiElement; }
            set
            {
                _uiElement = value;
                OnPropertyChanged("UIElement");
            }
        }
        private UIElement<T> _uiElement;

        #endregion

        #region Advanced Search Specific

        public virtual AdvancedPart Part
        {
            get { return _parts; }
            set
            {
                _parts = value;
                OnPropertyChanged("Part");
            }
        }
        private AdvancedPart _parts;

        public virtual AdvancedPart SelectedPart
        {
            get { return _selectedPart; }
            private set
            {
                if (_selectedPart == value)
                    return;
                if (_selectedPart != null)
                    _selectedPart.IsSelected = false;

                _selectedPart = value;
                if (_selectedPart != null)
                    _selectedPart.IsSelected = true;

                OnPropertyChanged("SelectedPart");
            }
        }
        private AdvancedPart _selectedPart;

        /// <summary>
        /// Factory method to return a new node with properties set.
        /// </summary>
        /// <returns></returns>
        protected virtual AdvancedPartNode CreateNode()
        {
            return new AdvancedPartNode()
            {
                SelectedPath = _pathCombo.SelectedPath,
                SelectedBuilder = SelectedBuilder,
                Values = (UIElement != null ? UIElement.GetValues() : null)
            };
        }

        /// <summary>
        /// Creates a new node optionally from a template part.  If no template provided, logic ensures a 
        /// node can be added at this time by checking UIElement for values.
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        protected virtual AdvancedPartNode CreateNode(IPartView template)
        {
            if (template != null)
            {
                var node = CreateNode();
                node.CopyFrom(template);
                return node;
            }
            else
            {
                if (UIElement != null && UIElement.GetValues() == null)
                    return null;

                return CreateNode();
            }
        }

        protected virtual AdvancedPartJunction CreateJunction(JunctionType type)
        {
            return new AdvancedPartJunction(type);
        }

        /// <summary>
        /// Add a new part to the adapter.
        /// </summary>
        /// <param name="type"></param>
        public virtual void Add(JunctionType type)
        {
            var toAdd = CreateNode(null);
            if (toAdd == null)
                return;

            AddHandlers(toAdd);

            // if Parts null, just add node
            if (Part == null)
                Part = toAdd;
            else
            {
                // if we have a selected part, add as a sibling to this, otherwise add it as a sibling to the last item
                var sibling = SelectedPart;
                if (sibling == null)
                    sibling = Part;

                // if the sibling has a container and it's the same type of junction, add it to the same container
                AdvancedPartJunction container = null;
                if (sibling.Container != null && sibling.Container.Type == type)
                    container = sibling.Container;
                else if (sibling == Part && sibling is AdvancedPartJunction && ((AdvancedPartJunction)sibling).Type == type)
                    container = (AdvancedPartJunction)sibling;

                // no parent? (either no container, or the sibling's container is a different junction type)
                if (container == null)
                {
                    container = CreateJunction(type);
                    if (Part == sibling) // root case
                        Part = container;
                    else if (sibling.Container != null) // otherwise, replace index in original container
                    {
                        // it seems replacing the index directly messes with UI data templating
                        // so if we remove it and insert the container at the old items location, it works
                        var c = sibling.Container;
                        var index = c.IndexOf(sibling);
                        c.Remove(sibling);
                        c.Insert(index, container);
                    }
                    container.Add(sibling);
                    AddHandlers(container);
                }

                // and finally, add the new node to the junction
                container.Parts.Add(toAdd);
            }

            toAdd.IsSelected = true;
        }

        private void AddHandlers(AdvancedPart part)
        {
            part.DeleteRequested += Part_DeleteRequested;
            part.IsSelectedChanged += Part_IsSelectedChanged;
        }
        private void RemoveHandlers(AdvancedPart part)
        {
            part.DeleteRequested -= Part_DeleteRequested;
            part.IsSelectedChanged -= Part_IsSelectedChanged;

            // is the selected part in the hierarchy being removed?
            if (SelectedPart == part)
                SelectedPart = null;
            else if (SelectedPart != null)
            {
                var junction = SelectedPart.Container;
                while (junction != null)
                {
                    // yep, one of the parent junctions of the SelectedPart is the one being removed
                    if (junction == part)
                    {
                        SelectedPart = null;
                        break;
                    }
                    junction = junction.Container;
                }
            }
        }

        private void Part_IsSelectedChanged(object sender, EventArgs e)
        {
            var part = (AdvancedPart)sender;
            if (part.IsSelected)
                SelectedPart = part;
            else if (part == SelectedPart)
                SelectedPart = null;
        }

        private void Part_DeleteRequested(object sender, EventArgs e)
        {
            Remove(sender as AdvancedPart);
        }

        protected virtual void Remove(AdvancedPart part)
        {
            if (part == null)
                return;
            RemoveHandlers(part);

            if (Part == part)
                Part = null;
            else
            {
                var container = part.Container;
                container.Parts.Remove(part);

                // remove the container if it's only got 1 item left
                if (container.Parts.Count == 1)
                {
                    RemoveHandlers(container);
                    var parentContainer = container.Container;
                    if (parentContainer == null)
                    {
                        container.Parts[0].Container = null;
                        Part = container.Parts[0];
                    }
                    else
                    {
                        var index = parentContainer.Parts.IndexOf(container);
                        parentContainer.Parts.Remove(container);

                        // if the parent container junction type is the same as what we're inserting, merge the parts
                        var toMerge = container.Parts[0];
                        if (toMerge is AdvancedPartJunction && ((AdvancedPartJunction)toMerge).Type == parentContainer.Type)
                        {
                            RemoveHandlers(toMerge);
                            foreach (var p in (AdvancedPartJunction)toMerge)
                                parentContainer.Add(p);
                        }
                        else
                            parentContainer.Insert(index, container.Parts[0]);
                    }
                }
            }
        }

        #endregion

        #region IView

        public virtual IParameter GetParameter()
        {
            if (Part != null)
                return Part.GetParameter();
            return null;
        }

        public virtual IPartViewJunction GetParts()
        {
            if (Part is IPartViewNode)
            {
                var junction = new AdvancedPartJunction();
                junction.Add(Part);
                return junction;
            }

            return (IPartViewJunction)Part;
        }

        private class PartLoader
        {
            public IPartView Current { get; set; }
            public AdvancedPartJunction Junction { get; set; }
            public PartLoader(IPartView current)
            {
                Current = current;
                Junction = null;
            }
            public PartLoader(IPartView current, AdvancedPartJunction junction)
                : this(current)
            {
                Junction = junction;
            }
        }
        public virtual void SetParts(IPartViewJunction parts)
        {
            // convert all parts to AdvancedParts
            // TODO: ensure we don't have any single-item junctions
            var queue = new Queue<PartLoader>();
            queue.Enqueue(new PartLoader(parts));

            while (queue.Count > 0)
            {
                var load = queue.Dequeue();
                if (load.Current is IPartViewJunction)
                {
                    var junction = CreateJunction(((IPartViewJunction)load.Current).Type);
                    AddHandlers(junction);

                    foreach (var p in (IPartViewJunction)load.Current)
                        queue.Enqueue(new PartLoader(p, junction));

                    if (load.Junction == null)
                        Part = junction;
                    else
                        load.Junction.Add(junction);
                }
                else
                {
                    var node = CreateNode(load.Current);
                    AddHandlers(node);

                    if (load.Junction == null)
                        Part = node;
                    else
                        load.Junction.Add(node);
                }
            }
        }

        public virtual void Reset()
        {
            Part = null;
        }

        public event EventHandler Search;
        protected void OnSearch(object sender, EventArgs e)
        {
            if (Search != null)
                Search(this, EventArgs.Empty);
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
