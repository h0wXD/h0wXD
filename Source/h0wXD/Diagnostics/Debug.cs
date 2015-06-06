using System;
using System.Runtime.CompilerServices;

namespace h0wXD.Diagnostics
{
    public static class Debug
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void If(Func<bool> condition)
        {
            if (System.Diagnostics.Debugger.IsAttached && condition())
            {
                System.Diagnostics.Debugger.Break();
            }
        }
    }
}