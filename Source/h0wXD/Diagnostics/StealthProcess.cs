using h0wXD.Diagnostics.Interfaces;
using h0wXD.Diagnostics.Domain;
using System.Text;

namespace h0wXD.Diagnostics
{
    public class StealthProcess : IStealthProcess
    {
        public ProcessOutput Execute(ProcessArguments _processArguments)
        {
            using (var process = new System.Diagnostics.Process())
            {
                process.StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = _processArguments.File,
                    Arguments = _processArguments.Arguments,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    ErrorDialog = false,
                };
                
                var outputBuilder = new StringBuilder(1024);
                var errorOutputBuilder = new StringBuilder(1024);

                process.OutputDataReceived += (_sender, _args) =>
                {
                    outputBuilder.Append(_args.Data);
                };

                process.ErrorDataReceived += (_sender, _e) =>
                {
                    errorOutputBuilder.Append(_e.Data);
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();

                return new ProcessOutput
                {
                    Arguments = _processArguments.Arguments,
                    File = _processArguments.File,
                    ExitCode = process.ExitCode,
                    Output = outputBuilder.ToString(),
                    ErrorOutput = errorOutputBuilder.ToString()
                };
            }
        }
    }
}
