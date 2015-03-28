using System;
using System.IO;
using h0wXD.Diagnostics;
using h0wXD.Helpers;
using h0wXD.IO.Helpers;

namespace h0wXD.IO
{
    public static class Junction
    {
        public static bool Exists(string _sJunctionPath)
        {
            return Directory.Exists(_sJunctionPath) &&
                   File.GetAttributes(_sJunctionPath).HasFlag(FileAttributes.ReparsePoint);
        }

        public static void Create(string _sJunctionPath, string _sDestinationPath, bool _bMoveContents = false)
        {
            if (Exists(_sJunctionPath))
            {
                throw new IOException("Junction already exists!");
            }

            Directory.CreateDirectory(_sDestinationPath);

            if (_bMoveContents)
            {
                if (!Directory.Exists(_sJunctionPath))
                {
                    throw new IOException("Directory should exist!");
                }

                FileHelper.Copy(_sJunctionPath, _sDestinationPath);
                Directory.Delete(_sJunctionPath, true);
            }
            else
            {
                if (Directory.Exists(_sJunctionPath))
                {
                    throw new IOException("Directory should not exist!");
                }
            }

            var processOutput = new StealthProcess().Execute("mklink", 
                "/J", 
                StringHelper.Add(_sDestinationPath, TechnicalConstants.Diagnostics.DoubleQuote), 
                StringHelper.Add(_sJunctionPath, TechnicalConstants.Diagnostics.DoubleQuote)
            );

            if (processOutput.ExitCode != TechnicalConstants.Diagnostics.ExitCodeSuccess)
            {
                throw new IOException(processOutput.Output);
            }
        }

        public static void Delete(string _sJunctionPath)
        {
            if (!Exists(_sJunctionPath))
            {
                throw new IOException("Junction does not exist!");
            }

            var processOutput = new StealthProcess().Execute("rd", StringHelper.Add(_sJunctionPath, TechnicalConstants.Diagnostics.DoubleQuote));

            if (processOutput.ExitCode != TechnicalConstants.Diagnostics.ExitCodeSuccess)
            {
                throw new IOException(processOutput.Output);
            }
        }
    }
}
