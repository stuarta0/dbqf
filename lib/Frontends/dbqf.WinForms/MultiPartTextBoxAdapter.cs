using System;
using System.Collections.Generic;
using System.Text;
using dbqf.Criterion;
using dbqf.Display;
using System.ComponentModel;
using dbqf.Parsers;

namespace dbqf.WinForms
{
    public class MultiPartTextBoxAdapter : IGetParameter, INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets a parser to use to pre-process the text value.
        /// By setting this property, the resulting parameter will be a conjunction of part parameters.
        /// If null, the individual parts will each parse the entire text resulting in a disjunction of part parameters.
        /// </summary>
        public Parser Parser { get; set; }

        public MultiPartTextBoxAdapter()
        {
            Prefix = null;
            Parts = new BindingList<IPartViewNode>();
        }

        public BindingList<IPartViewNode> Parts
        {
            get { return _parts; }
            protected set
            {
                _parts = value;
                OnPropertyChanged("Parts");
            }
        }
        private BindingList<IPartViewNode> _parts;

        public string Prefix
        {
            get { return _prefix; }
            set
            {
                _prefix = value;
                OnPropertyChanged("Prefix");
            }
        }
        private string _prefix;

        public bool PrefixVisible
        {
            get { return !String.IsNullOrEmpty(Prefix); }
        }

        public string QueryText
        {
            get { return _queryText; }
            set
            {
                _queryText = value;
                OnPropertyChanged("QueryText");
            }
        }
        private string _queryText;

        public event EventHandler SearchRequested = delegate { };
        public void Search()
        {
            SearchRequested(this, EventArgs.Empty);
        }

        #region IGetParameter Members

        public IParameter GetParameter()
        {
            if (String.IsNullOrEmpty(QueryText))
                return null;

            Junction junction;
            if (Parser == null)
            {
                junction = new Disjunction();
                foreach (var part in Parts)
                {
                    // as we process each part, pass through the value to pull apart
                    part.Values = new object[] { QueryText };

                    // try getting a parameter, handling parsing failure by ignoring the part
                    IParameter p = null;
                    try { p = part.GetParameter(); }
                    catch { /* failure = null parameter, this is OK */ }

                    // if we generated something, add it to the junction
                    if (p != null)
                        junction.Add(p);
                }
            }
            else
            {
                junction = new Conjunction();
                // pre-process the query text using our own parser
                foreach (var value in Parser.Parse(QueryText))
                {
                    var disjunction = new Disjunction();
                    foreach (var part in Parts)
                    {
                        part.Values = new object[] { value };

                        // try getting a parameter, handling parsing failure by ignoring the part
                        IParameter p = null;
                        try { p = part.GetParameter(); }
                        catch { /* failure = null parameter, this is OK */ }

                        // if we generated something, add it to the junction
                        if (p != null)
                            disjunction.Add(p);
                    }

                    if (disjunction.Count > 0)
                        junction.Add(disjunction);
                }
            }

            if (junction.Count == 0)
                return null;
            else if (junction.Count == 1)
                return junction[0];
            return junction;
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
