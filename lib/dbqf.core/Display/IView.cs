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
    /// Represents a specialised view with parts using covariance (well I would except I need .NET 4).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IView<T> : IGetParameter
        where T : IPartView
    {
        BindingList<T> Parts { get; }
    }
}
