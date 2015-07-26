using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Display;
using dbqf.Display.Advanced;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

namespace dbqf.WinForms.Advanced
{
    public class WinFormsAdvancedAdapter : AdvancedAdapter<Control>
    {
        public WinFormsAdvancedAdapter(IList<ISubject> subjects, IFieldPathComboBox pathCombo, IParameterBuilderFactory builderFactory, IControlFactory<Control> controlFactory)
            : base(subjects, pathCombo, builderFactory, controlFactory)
        {
        }

        /// <summary>
        /// Occurs when a UI rebuild is required due to changes to WinFormsAdvancedAdapter.Part.
        /// </summary>
        public event EventHandler RebuildRequired = delegate {};
        protected virtual void OnRebuildRequired()
        {
            RebuildRequired(this, EventArgs.Empty);
        }

        public FieldPathComboAdapter FieldPathComboAdapter
        {
            get { return (FieldPathComboAdapter)base._pathCombo; }
        }

        // override required to fire ValueVisibility changed
        public override UIElement<Control> UIElement
        {
            get { return base.UIElement; }
            set
            {
                base.UIElement = value;
                OnPropertyChanged("IsValueVisible");
            }
        }

        public bool IsValueVisible
        {
            get { return UIElement == null || UIElement.Element == null ? false : true; }
        }

        protected override AdvancedPartNode CreateNode()
        {
            if (UIElement != null && UIElement.GetValues() == null)
                return null;

            return new WinFormsAdvancedPartNode()
            {
                SelectedPath = _pathCombo.SelectedPath,
                SelectedBuilder = SelectedBuilder,
                Values = (UIElement != null ? UIElement.GetValues() : null)
            };
        }

        protected override AdvancedPartJunction CreateJunction(JunctionType type)
        {
            return new WinFormsAdvancedPartJunction(type);
        }

        public override void Add(JunctionType type)
        {
            base.Add(type);
            OnRebuildRequired();
        }

        protected override void Remove(AdvancedPart part)
        {
            base.Remove(part);
            OnRebuildRequired();
        }

        public override void Reset()
        {
            base.Reset();
            OnRebuildRequired();
        }

        public override void SetParts(IPartViewJunction parts)
        {
            base.SetParts(parts);
            OnRebuildRequired();
        }
    }
}
