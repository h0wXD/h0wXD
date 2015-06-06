using System.Collections.Generic;
using System.IO;
using h0wXD.IO.Domain;

namespace h0wXD.IO.Interfaces
{
    public interface IDirectoryWatcher
    {
        List<WatchDirectory> Directories { get; }
        bool Active { get; }

        event FileSystemEventHandler Created;
        event FileSystemEventHandler Changed;
        event FileSystemEventHandler Deleted;
        event FileSystemEventHandler Renamed;
        
        void AddDirectory(WatchDirectory directory);
        void AddDirectory(string directory, string fileMask, bool watchSubdirectories = false);

        void Start();
        void Stop();
    }
}
