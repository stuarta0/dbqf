using dbqf.Configuration;
using dbqf.Display;
using dbqf.Display.Advanced;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace dbqf.WPF.Advanced
{
    public class WpfAdvancedAdapter : AdvancedAdapter<System.Windows.UIElement>
    {
        public WpfAdvancedAdapter(IList<ISubject> subjects, IFieldPathComboBox pathCombo, IParameterBuilderFactory builderFactory, IControlFactory<System.Windows.UIElement> controlFactory)
            : base(subjects, pathCombo, builderFactory, controlFactory)
        {
        }

        public FieldPathComboAdapter FieldPathComboAdapter
        {
            get { return (FieldPathComboAdapter)base._pathCombo; }
        }

        // override required to fire ValueVisibility changed
        public override UIElement<UIElement> UIElement
        {
            get { return base.UIElement; }
            set
            {
                base.UIElement = value;
                OnPropertyChanged("ValueVisibility");
            }
        }

        public Visibility ValueVisibility
        {
            get { return UIElement == null || UIElement.Element == null ? Visibility.Collapsed : Visibility.Visible; }
        }

        public ICommand AndCommand
        {
            get
            {
                if (_andCommand == null)
                    _andCommand = new RelayCommand(p => Add(JunctionType.Conjunction));
                return _andCommand;
            }
        }
        private ICommand _andCommand;

        public ICommand OrCommand
        {
            get
            {
                if (_orCommand == null)
                    _orCommand = new RelayCommand(p => Add(JunctionType.Disjunction));
                return _orCommand;
            }
        }
        private ICommand _orCommand;

        public WpfAdvancedPart Part
        {
            get { return _parts; }
            set
            {
                _parts = value;
                OnPropertyChanged("Part");
            }
        }
        private WpfAdvancedPart _parts;

        public WpfAdvancedPart SelectedPart
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
        private WpfAdvancedPart _selectedPart;

        /// <summary>
        /// Add a new part to the adapter.
        /// </summary>
        /// <param name="type"></param>
        private void Add(JunctionType type)
        {
            var toAdd = new WpfAdvancedPartNode()
                {
                    SelectedPath = _pathCombo.SelectedPath,
                    SelectedBuilder = SelectedBuilder,
                    Values = this.UIElement.GetValues()
                };
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
                WpfAdvancedPartJunction container = null;
                if (sibling.Container != null && sibling.Container.Type == type)
                    container = sibling.Container;
                else if (sibling == Part && sibling is WpfAdvancedPartJunction && ((WpfAdvancedPartJunction)sibling).Type == type)
                    container = (WpfAdvancedPartJunction)sibling;

                // no parent? (either no container, or the sibling's container is a different junction type)
                if (container == null)
                {
                    container = new WpfAdvancedPartJunction() { Type = type };
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

        private void AddHandlers(WpfAdvancedPart part)
        {
            part.DeleteRequested += Part_DeleteRequested;
            part.IsSelectedChanged += Part_IsSelectedChanged;
        }
        private void RemoveHandlers(WpfAdvancedPart part)
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

        void Part_IsSelectedChanged(object sender, EventArgs e)
        {
            var part = (WpfAdvancedPart)sender;
            if (part.IsSelected)
                SelectedPart = part;
            else if (part == SelectedPart)
                SelectedPart = null;
        }

        void Part_DeleteRequested(object sender, EventArgs e)
        {
            var part = sender as WpfAdvancedPart;
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
                        Part = container.Parts[0];
                    else
                    {
                        var index = parentContainer.Parts.IndexOf(container);
                        parentContainer.Parts.Remove(container);

                        // if the parent container junction type is the same as what we're inserting, merge the parts
                        var toMerge = container.Parts[0];
                        if (toMerge is WpfAdvancedPartJunction && ((WpfAdvancedPartJunction)toMerge).Type == parentContainer.Type)
                        {
                            RemoveHandlers(toMerge);
                            foreach (var p in (WpfAdvancedPartJunction)toMerge)
                                parentContainer.Add(p);
                        }
                        else
                            parentContainer.Insert(index, container.Parts[0]);
                    }
                }
            }
        }

        public override void Reset()
        {
            Part = null;
        }

        public override IPartViewJunction GetParts()
        {
            if (Part is IPartViewNode)
            {
                var junction = new WpfAdvancedPartJunction();
                junction.Add(Part);
                return junction;
            }
            
            return (IPartViewJunction)Part;
        }


        private class PartLoader
        {
            public IPartView Current { get; set; }
            public WpfAdvancedPartJunction Junction { get; set; }
            public PartLoader(IPartView current)
            {
                Current = current;
                Junction = null;
            }
            public PartLoader(IPartView current, WpfAdvancedPartJunction junction)
                : this(current)
            {
                Junction = junction;
            }
        }
        public override void SetParts(IPartViewJunction parts)
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
                    var junction = new WpfAdvancedPartJunction() { Type = ((IPartViewJunction)load.Current).Type };
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
                    var node = new WpfAdvancedPartNode();
                    AddHandlers(node);

                    node.CopyFrom(load.Current);
                    if (load.Junction == null)
                        Part = node;
                    else
                        load.Junction.Add(node);
                }
            }
        }
        
        public override Criterion.IParameter GetParameter()
        {
            if (Part != null)
                return Part.GetParameter();
            return null;
        }
    }
}
