using System;
using System.Collections.Generic;
using System.Text;

namespace dbqf.Display
{
    public abstract class UIElement<T> : IDisposable
    {
        /// <summary>
        /// Gets the control to use in the UI.
        /// </summary>
        public T Element { get; protected set; }

        /// <summary>
        /// Gets values from the control using specific logic based on the control being used.
        /// </summary>
        /// <returns></returns>
        public abstract object[] GetValues();

        /// <summary>
        /// Sets values for the control using specific logic based on the control being used.
        /// </summary>
        /// <param name="values"></param>
        public abstract void SetValues(params object[] values);


        /// <summary>
        /// Occurs when the control state changes and potentially produces a parameter to search on.
        /// </summary>
        public event EventHandler Changed;
        protected virtual void OnChanged()
        {
            if (Changed != null)
                Changed(this, EventArgs.Empty);
        }

        /// <summary>
        /// Occurs when this control indicates a search has been requested by the user.
        /// </summary>
        public event EventHandler Search;
        protected virtual void OnSearch()
        {
            if (Search != null)
                Search(this, EventArgs.Empty);
        }

        /// <summary>
        /// Disposes the UI elements.
        /// </summary>
        public virtual void Dispose() { }
    }
}
