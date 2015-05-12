using dbqf.Configuration;
using dbqf.Criterion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace dbqf.Display.Advanced
{
    public abstract class AdvancedAdapter<T> : INotifyPropertyChanged, IGetParameter
    {
        protected List<ISubject> _subjects;
        protected IFieldPathFactory _pathFactory;
        protected IControlFactory<T> _controlFactory;
        protected IParameterBuilderFactory _builderFactory;
        public AdvancedAdapter(IList<ISubject> subjects, IFieldPathFactory pathFactory, IControlFactory<T> controlFactory, IParameterBuilderFactory builderFactory)
        {
            _pathFactory = pathFactory;
            _controlFactory = controlFactory;
            _builderFactory = builderFactory;
            Part = new AdvancedPart<T>(_pathFactory, _builderFactory, _controlFactory);
            Part.Subjects = new BindingList<ISubject>(subjects);
        }

        /// <summary>
        /// Gets the control to display for creating a parameter.
        /// </summary>
        public virtual AdvancedPart<T> Part
        {
            get { return _part; }
            protected set
            {
                _part = value;
                OnPropertyChanged("Part");
            }
        }
        protected AdvancedPart<T> _part;

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
            throw new NotImplementedException();

            //var con = new Conjunction();
            //foreach (var c in Parts)
            //{
            //    var p = c.GetParameter();
            //    if (p != null)
            //        con.Add(p);
            //}

            //return con;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
