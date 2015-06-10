using System;
using System.Collections.Generic;
using dbqf.Criterion;
using dbqf.Criterion.Builders;

namespace dbqf.Display
{
    /// <summary>
    /// A factory to create multiple parameter builders to use for a given field path.
    /// </summary>
    public class ParameterBuilderFactory : IParameterBuilderFactory
    {
        /// <summary>
        /// Creates a list of builders relevant to the given field path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public virtual IList<ParameterBuilder> Build(FieldPath path)
        {
            var builders = new List<ParameterBuilder>();

            if (path.Last.DataType == typeof(bool))
            {
                builders.Add(new BooleanBuilder(true));
                builders.Add(new BooleanBuilder(false));
                builders.Add(new NullBuilder());
                builders.Add(new NotBuilder(new NullBuilder()) { Label = "is not null" });
            }
            else if (path.Last.DataType == typeof(string))
            {
                builders.Add(new SimpleBuilder("="));
                builders.Add(new LikeBuilder(MatchMode.Anywhere));
                builders.Add(new LikeBuilder(MatchMode.Start));
                builders.Add(new LikeBuilder(MatchMode.End));
                builders.Add(new NotBuilder(new SimpleBuilder("=")));
                builders.Add(new NotBuilder(new LikeBuilder(MatchMode.Anywhere)) { Label = "does not contain" });
                builders.Add(new NotBuilder(new LikeBuilder(MatchMode.Start)) { Label = "does not start with" });
                builders.Add(new NotBuilder(new LikeBuilder(MatchMode.End)) { Label = "does not end with" });
                builders.Add(new NullBuilder());
                builders.Add(new NotBuilder(new NullBuilder()) { Label = "is not null" });
            }
            else if (path.Last.DataType == typeof(DateTime))
            {
                builders.Add(new DateEqualsBuilder());
                builders.Add(new NotBuilder(new DateEqualsBuilder()));
                builders.Add(new DateGtBuilder());
                builders.Add(new DateGtEqualBuilder());
                builders.Add(new DateLtBuilder());
                builders.Add(new DateLtEqualBuilder());
                builders.Add(new DateBetweenBuilder());
                builders.Add(new NotBuilder(new DateBetweenBuilder()));
                builders.Add(new NullBuilder());
                builders.Add(new NotBuilder(new NullBuilder()) { Label = "is not null" });
            }
            else if (IsNumeric(path.Last.DataType))
            {
                builders.Add(new SimpleBuilder("="));
                builders.Add(new NotBuilder(new SimpleBuilder("=")));
                builders.Add(new SimpleBuilder(">"));
                builders.Add(new SimpleBuilder(">="));
                builders.Add(new SimpleBuilder("<"));
                builders.Add(new SimpleBuilder("<="));
                builders.Add(new BetweenBuilder());
                builders.Add(new NotBuilder(new BetweenBuilder()));
                builders.Add(new NullBuilder());
                builders.Add(new NotBuilder(new NullBuilder()) { Label = "is not null" });
            }
            else
            {
                // catch all - but if someone is using custom types they can extend the factory and deal with it
                builders.Add(new SimpleBuilder("="));
                builders.Add(new NotBuilder(new SimpleBuilder("=")));
                builders.Add(new NullBuilder());
                builders.Add(new NotBuilder(new NullBuilder()) { Label = "is not null" });
            }

            // pretty much always a disjunction when combining multiple values
            for (int i = 0; i < builders.Count; i++)
                builders[i] = new JunctionBuilder(JunctionBuilder.JunctionType.Disjunction, builders[i]);

            return builders;
        }

        /// <summary>
        /// Creates a builder that can be used as a default for a field path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public virtual ParameterBuilder GetDefault(FieldPath path)
        {
            ParameterBuilder b;
            if (path.Last.DataType == typeof(string))
                b = new LikeBuilder(MatchMode.Anywhere);
            else if (path.Last.DataType == typeof(DateTime))
                b = new DateBetweenBuilder();
            else if (IsNumeric(path.Last.DataType))
                b = new BetweenBuilder();
            else
                b = new SimpleBuilder("=");

            return new JunctionBuilder(JunctionBuilder.JunctionType.Disjunction, b);
        }

        private bool IsNumeric(Type t)
        {
            foreach (var numericType in new Type[] { typeof(sbyte), typeof(short), typeof(int), typeof(long), typeof(byte), typeof(ushort), typeof(uint), typeof(ulong), typeof(Single), typeof(double), typeof(decimal) })
                if (t == numericType)
                    return true;

            return false;
        }
    }
}
