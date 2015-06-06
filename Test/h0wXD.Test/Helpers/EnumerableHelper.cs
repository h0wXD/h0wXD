using System.Collections;
using System.Linq;

namespace h0wXD.Test.Helpers
{
    public static class EnumerableHelper
    {
        public static IEnumerable AsWeakEnumerable(this IEnumerable source)
        {
            return source.Cast<object>();
        }
    }
}