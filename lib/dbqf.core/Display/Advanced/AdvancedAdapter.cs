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

        public virtual BindingList<ISubject> SubjectSource
        {
            get { return _subjects; }
            set
            {
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


        public event EventHandler Search;
        protected void OnSearch(object sender, EventArgs e)
        {
            if (Search != null)
                Search(this, EventArgs.Empty);
        }

        public virtual IParameter GetParameter()
        {
            // starting at the root IPartViewJunction, recurse over the tree and construct the IParameter
            throw new NotImplementedException();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual IPartViewJunction GetParts()
        {
            // return IPartViewJunction of root parts
            throw new NotImplementedException();
        }

        public virtual void SetParts(IPartViewJunction parts)
        {
            // convert tree of data in IPartViewJunction into AdvancedPartJunction/Node.
        }

        public virtual void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
