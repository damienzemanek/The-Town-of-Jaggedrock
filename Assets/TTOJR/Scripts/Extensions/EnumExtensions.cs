using System;
using System.Linq;
using UnityEngine;

namespace Extensions
{
    public class EnumExtensions<T> where T : Enum
    {
        public static T Rand()
        {
            var include = Enum.GetValues(typeof(T))
                .Cast<T>()
                .ToArray();

            if (include.Length <= 0) return default;
            return EnumerationExtensions.Rand(include);
        }

        public static T Rand(params T[] exclude)
        {
            var enumVals = Enum.GetValues(typeof(T))
                .Cast<T>()
                .ToArray();

            var include = enumVals.Where(e => !exclude.Contains(e))
                .ToArray();
                
            if (include.Length <= 0) return default;

            return EnumerationExtensions.Rand(include);
        }

    }
}
