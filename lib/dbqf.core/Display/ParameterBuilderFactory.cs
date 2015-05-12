using dbqf.Criterion;
using dbqf.Display.Builders;
using System;
using System.Collections.Generic;
using System.Text;

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
            foreach (var b in builders)
            {
                if (b is NotBuilder && ((NotBuilder)b).Other.Junction == null)
                    ((NotBuilder)b).Other.Junction = new Disjunction();
                else if (b.Junction == null)
                    b.Junction = new Disjunction(); 
            }

            return builders;
        }

        /// <summary>
        /// Creates a builder that can be used as a default for a field path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public virtual ParameterBuilder GetDefault(FieldPath path)
        {
            if (path.Last.DataType == typeof(string))
                return new LikeBuilder(MatchMode.Anywhere) { Junction = new Disjunction() };
            else if (path.Last.DataType == typeof(DateTime))
                return new DateBetweenBuilder() { Junction = new Disjunction() };
            else if (IsNumeric(path.Last.DataType))
                return new BetweenBuilder() { Junction = new Disjunction() };
            else
                return new SimpleBuilder("=") { Junction = new Disjunction() };
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
