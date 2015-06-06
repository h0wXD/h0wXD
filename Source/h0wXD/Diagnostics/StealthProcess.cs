using h0wXD.Diagnostics.Interfaces;
using h0wXD.Diagnostics.Domain;
using System.Text;
using System.Diagnostics;

namespace h0wXD.Diagnostics
{
    public class StealthProcess : IStealthProcess
    {
        public ProcessOutput Execute(ProcessArguments processArguments)
        {
            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = processArguments.File,
                    Arguments = processArguments.Arguments,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    ErrorDialog = false,
                };

                var outputBuilder = new StringBuilder(1024);
                var errorOutputBuilder = new StringBuilder(1024);

                process.OutputDataReceived += (server, e) =>
                {
                    outputBuilder.Append(e.Data);
                };

                process.ErrorDataReceived += (sender, e) =>
                {
                    errorOutputBuilder.Append(e.Data);
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();

                return new ProcessOutput
                {
                    Arguments = processArguments.Arguments,
                    File = processArguments.File,
                    ExitCode = process.ExitCode,
                    Output = outputBuilder.ToString(),
                    ErrorOutput = errorOutputBuilder.ToString()
                };
            }
        }

        public ProcessOutput Execute(string fileName, params string [] args)
        {
            var stringBuilder = new StringBuilder(128);

            foreach (var argument in args)
            {
                stringBuilder.Append(argument);
                stringBuilder.Append(" ");
            }

            return Execute(new ProcessArguments()
            {
                File = fileName, 
                Arguments = stringBuilder.ToString()
            });
        }
    }
}
