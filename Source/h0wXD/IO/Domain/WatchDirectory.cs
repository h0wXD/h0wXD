
namespace h0wXD.IO.Domain
{
    public class WatchDirectory
    {
        public bool WatchSubDirectories { get; private set; }
        public string Path { get; private set; }
        public string FileMask { get; private set; }

        public WatchDirectory(string path, string fileMask, bool watchSubDirectories = false)
        {
            Path = path;
            FileMask = fileMask;
            WatchSubDirectories = watchSubDirectories;
        }

        public WatchDirectory(string path, bool watchSubDirectories = false, string fileMask = TechnicalConstants.IO.FileMaskAny) :
            this(path, fileMask, watchSubDirectories)
        {
        }
    }
}
