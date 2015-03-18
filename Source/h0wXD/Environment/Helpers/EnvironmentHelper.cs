using System;

namespace h0wXD.Environment.Helpers
{
    public class EnvironmentHelper
    {
        public static T GetEnvironmentVariable<T>(string _sKey, T _defaultValue = default(T))
        {
            var value = System.Environment.GetEnvironmentVariable(_sKey);

            if (String.IsNullOrEmpty(value))
            {
                return _defaultValue;
            }

            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
