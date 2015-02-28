﻿using System.Collections.Generic;
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
        
        void AddDirectory(WatchDirectory _directory);
        void AddDirectory(string _sDirectory, string _sFileMask, bool _bWatchSubdirectories = false);

        void Start();
        void Stop();
    }
}
