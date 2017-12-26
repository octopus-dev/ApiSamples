using System;

namespace OctopusApiSamples.Utils
{
    public static class Requires
    {
        public static void NotNull<T>(T obj, string parameterName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        public static void NotNullOrEmpty(string str, string parameterName)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        public static void MustGreaterThan<T>(T value, T compareValue, string parameterName) where T : IComparable<T>
        {
            if (value.CompareTo(compareValue) <= 0)
            {
                throw new ArgumentOutOfRangeException(parameterName, $"{parameterName} must greater than {compareValue}");
            }
        }
    }
}
