using h0wXD.Diagnostics.Domain;

namespace h0wXD.Diagnostics.Interfaces
{
    public interface IStealthProcess
    {
        ProcessOutput Execute(ProcessArguments processArguments);
        ProcessOutput Execute(string fileName, params string [] args);
    }
}
