using System;
using System.IO;

namespace h0wXD.IO.Helpers
{
	public static class FileHelper
	{
		public static void Copy(string sourcePath, string destinationPath, string fileMask = TechnicalConstants.IO.FileMaskAny, bool recursive = true)
		{
			foreach (var sFile in Directory.GetFiles(sourcePath, fileMask, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
			{
                var sDestinationFileName = sFile.Replace(sourcePath, destinationPath);
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
