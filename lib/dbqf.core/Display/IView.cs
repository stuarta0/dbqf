using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using dbqf.Criterion;
using dbqf.Display.Builders;
using dbqf.Parsers;

namespace dbqf.Display
{
    /// <summary>
    /// Non-generic type for use without covariance.
    /// </summary>
    public interface IView : IGetParameter
    {
        /// <summary>
        /// Gets a list of parts contained in this view.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IPartView> GetParts();

        /// <summary>
        /// Sets a list of parts for a view.
        /// </summary>
        /// <param name="parts"></param>
        void SetParts(IEnumerable<IPartView> parts);

        /// <summary>
        /// Resets current parameters in the view.
        /// </summary>
        void Reset();
    }

    /// <summary>
    /// Represents a specialised view with parts using covariance (well I would except I need .NET 4).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IView<T> : IView
        where T : IPartView
    {
        BindingList<T> Parts { get; }
    }
}
