
namespace h0wXD.Diagnostics.Domain
{
    public class ProcessOutput : ProcessArguments
    {
        public int ExitCode { get; set; }
        public string Output { get; set; }
        public string ErrorOutput { get; set; }
    }
}
