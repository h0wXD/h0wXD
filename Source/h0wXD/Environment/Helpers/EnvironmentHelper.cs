using System;
using System.Runtime.CompilerServices;

namespace h0wXD.Environment.Helpers
{
    public static class EnvironmentHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetEnvironmentVariable<T>(string key, T defaultValue = default(T))
        {
            var value = System.Environment.GetEnvironmentVariable(key);

            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
