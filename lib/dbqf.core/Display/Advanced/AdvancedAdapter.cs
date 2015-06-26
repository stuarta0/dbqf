using dbqf.Configuration;
using dbqf.Criterion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace dbqf.Display.Advanced
{
    /// <summary>
    /// Parts in the Advanced view represent the user-constructed tree of parameters.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AdvancedAdapter<T> : IView, INotifyPropertyChanged
    {
        protected List<ISubject> _subjects;
        protected IFieldPathFactory _pathFactory;
        protected IParameterBuilderFactory _builderFactory;
        protected IControlFactory<T> _controlFactory;

        public AdvancedAdapter(IList<ISubject> subjects, IFieldPathFactory pathFactory, IControlFactory<T> controlFactory, IParameterBuilderFactory builderFactory)
        {
            _pathFactory = pathFactory;
            _controlFactory = controlFactory;
            _builderFactory = builderFactory;
            //Part = new AdvancedPart<T>(_pathFactory, _builderFactory, _controlFactory);
            //Part.Subjects = new BindingList<ISubject>(subjects);
        }

        ///// <summary>
        ///// Gets the control to display for creating a parameter.
        ///// </summary>
        //public virtual AdvancedPart<T> Part
        //{
        //    get { return _part; }
        //    protected set
        //    {
        //        _part = value;
        //        OnPropertyChanged("Part");
        //    }
        //}
        //protected AdvancedPart<T> _part;

        public event EventHandler Search;
        private void OnSearch(object sender, EventArgs e)
        {
            if (Search != null)
                Search(this, EventArgs.Empty);
        }

        /// <summary>
        /// Factory method to create concrete part.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        //protected abstract AdvancedPart<T> CreatePart();

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

        public IPartViewJunction GetParts()
        {
            // return IPartViewJunction of root parts
            throw new NotImplementedException();
        }

        public void SetParts(IPartViewJunction parts)
        {
            // convert tree of data in IPartViewJunction into AdvancedPartJunction/Node.
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
