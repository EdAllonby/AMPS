using System;
using System.Collections.Generic;
using System.Linq;

namespace Utility
{
    public static class EnumUtility
    {
        /// <summary>
        /// Convert an enum into a list of enums.
        /// </summary>
        /// <typeparam name="TEnum">The enum to convert.</typeparam>
        /// <returns>A list of <see cref="TEnum" />s.</returns>
        public static IEnumerable<TEnum> EnumToEnumerable<TEnum>()
            // It's unlikely we'll have a type to pass in which satisfies all these conditions that isn't an enum.
            // C# Doesn't have a way of specifying an Enum. So let's say that an Enum is defined as below.
            where TEnum : struct, IConvertible, IComparable, IFormattable
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
        }
    }
}