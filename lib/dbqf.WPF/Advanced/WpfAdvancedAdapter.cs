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
            set
            {
                _selectedPart = value;
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
            toAdd.DeleteRequested += Part_DeleteRequested;

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
                        var index = c.Parts.IndexOf(sibling);
                        c.Parts.Remove(sibling);
                        c.Parts.Insert(index, container);
                    }
                    container.Parts.Add(sibling);
                    container.DeleteRequested += Part_DeleteRequested;
                }

                // and finally, add the new node to the junction
                container.Parts.Add(toAdd);
            }

            SelectedPart = toAdd;
        }

        void Part_DeleteRequested(object sender, EventArgs e)
        {
            
        }

        public override void Reset()
        {
            Part = null;
        }
    }
}
