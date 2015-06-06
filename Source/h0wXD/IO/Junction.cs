using System;
using System.IO;
using h0wXD.Diagnostics;
using h0wXD.Helpers;
using h0wXD.IO.Helpers;

namespace h0wXD.IO
{
    public static class Junction
    {
        public static bool Exists(string junctionPath)
        {
            return Directory.Exists(junctionPath) &&
                   File.GetAttributes(junctionPath).HasFlag(FileAttributes.ReparsePoint);
        }

        public static void Create(string junctionPath, string destinationPath, bool moveContents = false)
        {
            if (Exists(junctionPath))
            {
                throw new IOException("Junction already exists!");
            }

            Directory.CreateDirectory(destinationPath);

            if (moveContents)
            {
                if (!Directory.Exists(junctionPath))
                {
                    throw new IOException("Directory should exist!");
                }

                FileHelper.Copy(junctionPath, destinationPath);
                Directory.Delete(junctionPath, true);
            }
            else
            {
                if (Directory.Exists(junctionPath))
                {
                    throw new IOException("Directory should not exist!");
                }
            }

            var processOutput = new StealthProcess().Execute("mklink", 
                "/J", 
                destinationPath.Wrap(TechnicalConstants.Diagnostics.DoubleQuote), 
                junctionPath.Wrap(TechnicalConstants.Diagnostics.DoubleQuote)
            );

            if (processOutput.ExitCode != TechnicalConstants.Diagnostics.ExitCodeSuccess)
            {
                throw new IOException(processOutput.Output);
            }
        }

        public static void Delete(string junctionPath)
        {
            if (!Exists(junctionPath))
            {
                throw new IOException("Junction does not exist!");
            }

            var processOutput = new StealthProcess().Execute("rd", junctionPath.Wrap(TechnicalConstants.Diagnostics.DoubleQuote));

            if (processOutput.ExitCode != TechnicalConstants.Diagnostics.ExitCodeSuccess)
            {
                throw new IOException(processOutput.Output);
            }
        }
    }
}
