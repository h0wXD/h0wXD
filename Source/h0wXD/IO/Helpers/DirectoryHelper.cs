using System.IO;

namespace h0wXD.IO.Helpers
{
    public static class DirectoryHelper
    {
        public static void Copy(string sourcePath, string destinationPath, string fileMask = TechnicalConstants.IO.MaskAny, bool recursive = true)
        {
            foreach (var directory in Directory.GetDirectories(sourcePath, fileMask, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
            {
                Directory.CreateDirectory(directory.Replace(sourcePath, destinationPath));
            }
        }
    }
}
