
namespace h0wXD.IO.Domain
{
    public class WatchDirectory
    {
        public bool WatchSubDirectories { get; private set; }
        public string Path { get; private set; }
        public string FileMask { get; private set; }

        public WatchDirectory(string _sPath, string _sFileMask, bool _bWatchSubDirectories = false)
        {
            Path = _sPath;
            FileMask = _sFileMask;
            WatchSubDirectories = _bWatchSubDirectories;
        }
        
        public WatchDirectory(string _sPath, bool _bWatchSubDirectories = false, string _sFileMask = TechnicalConstants.IO.FileMaskAny) :
            this(_sPath, _sFileMask, _bWatchSubDirectories)
        {
        }
    }
}
