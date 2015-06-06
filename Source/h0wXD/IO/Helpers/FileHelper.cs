using System;
using System.IO;

namespace h0wXD.IO.Helpers
{
	public static class FileHelper
	{
		public static void Copy(string sourcePath, string destinationPath, string fileMask = TechnicalConstants.IO.FileMaskAny, bool recursive = true)
		{
			foreach (var fileName in Directory.GetFiles(sourcePath, fileMask, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
			{
                var destinationFileName = fileName.Replace(sourcePath, destinationPath);
                var directory = Path.GetDirectoryName(destinationFileName);

                if (!String.IsNullOrWhiteSpace(directory) &&
                    !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

				File.Copy(fileName, destinationFileName, true);
			}
		}
    }
}
