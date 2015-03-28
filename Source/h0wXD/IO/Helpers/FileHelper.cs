using System;
using System.IO;

namespace h0wXD.IO.Helpers
{
	public static class FileHelper
	{
		public static void Copy(string _sSourcePath, string _sDestinationPath, string _sFileMask = TechnicalConstants.IO.FileMaskAny, bool _bRecursive = true)
		{
			foreach (var sFile in Directory.GetFiles(_sSourcePath, _sFileMask, _bRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
			{
                var sDestinationFileName = sFile.Replace(_sSourcePath, _sDestinationPath);
                var sDirectory = Path.GetDirectoryName(sDestinationFileName);

                if (!String.IsNullOrWhiteSpace(sDirectory) &&
                    !Directory.Exists(sDirectory))
                {
                    Directory.CreateDirectory(sDirectory);
                }

				File.Copy(sFile, sDestinationFileName, true);
			}
		}
    }
}
