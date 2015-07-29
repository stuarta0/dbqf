using System;
using System.Collections.Generic;
using dbqf.Criterion.Values;

namespace dbqf.Parsers
{
    /// <summary>
    /// Cannot support covariance for primitive types, therefore a ConvertParser must implement type properties.
    /// </summary>
    public interface IConvertParser
    {
        Type From { get; }
        Type To { get; }
    }

    public enum FailBehaviour { ThrowException, ExcludeValue };

    /// <summary>
    /// Parses objects using built-in Convert.ChangeType().
    /// </summary>
    public class ConvertParser<Tfrom, Tto> : Parser, IConvertParser
    {
        public Type From { get { return typeof(Tfrom); } }
        public Type To { get { return typeof(Tto); } }

        /// <summary>
        /// Gets or sets the behaviour to use when a value cannot be parsed.
        /// </summary>
        public FailBehaviour Behaviour { get; set; }

        public ConvertParser()
        {
            ComputeHash();
        }

        /// <summary>
        /// Uses the built-in Convert class to try and convert the values.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public override object[] Parse(params object[] values)
        {
            return Parse(typeof(Tto), values);
        }

        /// <summary>
        /// Uses the built-in Convert class to try and convert the values.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public override object[] Revert(params object[] values)
        {
            return Parse(typeof(Tfrom), values);
        }

        protected virtual object[] Parse(Type to, params object[] values)
        {
            if (values == null)
                return null;

            var result = new List<object>();
            foreach (var v in values)
            {
                if (v is BetweenValue)
                {
                    var between = v as BetweenValue;
                    between.From = between.From == null ? null : Parse(to, between.From)[0];
                    between.To = between.To == null ? null : Parse(to, between.To)[0];
                    result.Add(between);
                }
                else if (v != null)
                {
                    try { result.Add(Convert.ChangeType(v, to)); }
                    catch (Exception ex)
                    {
                        if (Behaviour == FailBehaviour.ThrowException)
                            throw new FormatException(String.Format("Could not convert value '{0}' to {1}.", v, to.Name), ex);
                    }
                }
            }
            return result.ToArray();
        }

        public override bool Equals(object obj)
        {
            if (obj is IConvertParser)
            {
                return From.Equals(((IConvertParser)obj).From)
                    && To.Equals(((IConvertParser)obj).To);
            }
            return base.Equals(obj);
        }
        protected override void ComputeHash()
        {
            base.ComputeHash();
            unchecked
            {
                _hash = (_hash * 7) + From.GetHashCode();
                _hash = (_hash * 7) + To.GetHashCode();
            }
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
