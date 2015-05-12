using System;
using System.Collections.Generic;
using System.Text;

namespace DbQueryFramework
{
    public class Check
    {
        /// <summary>
        /// Represents a simple numeric type for checking in your application (possible use is casting).
        /// </summary>
        public enum NumericType
        {
            UnsignedInt,
            SignedInt,
            Decimal,
            NonNumeric
        }

        /// <summary>
        /// Given a type, find the numeric type or NonNumeric if not a number.
        /// </summary>
        /// <param name="t">The type which will be checked</param>
        /// <returns>The NumericType of the given type.  NumericType.NonNumeric if the given type is not a number.</returns>
        public static NumericType GetNumericType(Type t)
        {
            if ((t == typeof(sbyte)) || (t == typeof(short)) || (t == typeof(int)) || (t == typeof(long)))
                return NumericType.SignedInt;
            else if ((t == typeof(byte)) || (t == typeof(ushort)) || (t == typeof(uint)) || (t == typeof(ulong)))
                return NumericType.UnsignedInt;
            else if ((t == typeof(Single)) || (t == typeof(double)) || (t == typeof(decimal)))
                return NumericType.Decimal;

            return NumericType.NonNumeric;
        }

        /// <summary>
        /// Determine if the given object is a number (floating-point or not).  If the parameter is a string
        /// then the contents of the string are examined (see IsNumeric(string)).
        /// </summary>
        /// <param name="o">The object to check</param>
        /// <returns>True if the object is numeric.</returns>
        public static bool IsNumeric(object o)
        {
            if (o is string)
                return IsNumeric(o.ToString());

            return IsNumeric(o.GetType());
        }

        /// <summary>
        /// Determine if the given type is a number (floating-point or not).
        /// </summary>
        /// <param name="t">The type to check</param>
        /// <returns>True if the type is numeric.</returns>
        public static bool IsNumeric(Type t)
        {
            return (GetNumericType(t) != NumericType.NonNumeric);
        }

        /// <summary>
        /// Determine if the given string contains a number (floating-point or not).
        /// </summary>
        /// <param name="s">The string to check</param>
        /// <returns>True if the string contains a number.</returns>
        public static bool IsNumeric(string s)
        {
            double result;
            return Double.TryParse(s, out result);
        }
    }
}
