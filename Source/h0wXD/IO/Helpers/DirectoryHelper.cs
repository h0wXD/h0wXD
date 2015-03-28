using System.IO;

namespace h0wXD.IO.Helpers
{
    public static class DirectoryHelper
    {
        public static void Copy(string _sSourcePath, string _sDestinationPath, string _sMaskAny = TechnicalConstants.IO.MaskAny, bool _bRecursive = true)
        {
            foreach (var sDirectory in Directory.GetDirectories(_sSourcePath, _sMaskAny, _bRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
            {
                Directory.CreateDirectory(sDirectory.Replace(_sSourcePath, _sDestinationPath));
            }
        }
    }
}
