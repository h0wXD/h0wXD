using System.Runtime.CompilerServices;

namespace h0wXD.Helpers
{
    public static class StringHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string AddBefore(string _sString, string _sToAdd)
        {
            if (!_sString.StartsWith(_sToAdd))
            {
                _sString = _sToAdd + _sString;
            }

            return _sString;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string AddAfter(string _sString, string _sToAdd)
        {
            if (!_sString.EndsWith(_sToAdd))
            {
                _sString = _sString + _sToAdd;
            }

            return _sString;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Add(string _sString, string _sToAdd)
        {
            return AddBefore(AddAfter(_sString, _sToAdd), _sToAdd);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Add(string _sString, string _sToAddBefore, string _sToAddAfter)
        {
            return AddBefore(AddAfter(_sString, _sToAddAfter), _sToAddBefore);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Strip(string _sString, string _sToStrip)
        {
            if (_sString.StartsWith(_sToStrip))
            {
                _sString = _sString.Substring(_sToStrip.Length);
            }

            if (_sString.EndsWith(_sToStrip))
            {
                _sString = _sString.Substring(0, _sString.Length - _sToStrip.Length);
            }

            return _sString;
        }
    }
}
