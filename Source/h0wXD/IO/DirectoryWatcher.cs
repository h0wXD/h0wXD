using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using h0wXD.IO.Domain;
using h0wXD.IO.Interfaces;

namespace h0wXD.IO
{
    public class DirectoryWatcher : IDirectoryWatcher
    {
        protected readonly List<FileSystemWatcher> m_watchers;

        public event FileSystemEventHandler Created = delegate {};
        public event FileSystemEventHandler Changed = delegate {};
        public event FileSystemEventHandler Deleted = delegate {};
        public event FileSystemEventHandler Renamed = delegate {};
        
        public List<WatchDirectory> Directories { get; private set; }
        public bool Active { get; private set; }

        public DirectoryWatcher()
        {
            Active = false;
            m_watchers = new List<FileSystemWatcher>();
            Directories = new List<WatchDirectory>();
        }

        public DirectoryWatcher(WatchDirectory _directory) :
            this(new List<WatchDirectory> {_directory})
        {
        }

        public DirectoryWatcher(List<WatchDirectory> _directories) :
            this()
        {
            if (_directories == null)
            {
                throw new ArgumentNullException("_directories");
            }
            if (_directories.Any(x => x == null))
            {
                throw new ArgumentException("Directories cannot contain null values.");
            }

            Directories = _directories;
            
            foreach (var directory in Directories)
            {
                AddDirectory(directory);
            }
        }

        public void AddDirectory(WatchDirectory _directory)
        {
            if (_directory == null)
            {
                throw new ArgumentNullException("WatchDirectory");
            }
            if (String.IsNullOrWhiteSpace(_directory.Path))
            {
                throw new ArgumentException("Path cannot be empty.", "_directory");
            }
            if (String.IsNullOrWhiteSpace(_directory.FileMask))
            {
                throw new ArgumentException("FileMask cannot be empty.", "_directory");
            }
            if (!Directory.Exists(_directory.Path))
            {
                throw new DirectoryNotFoundException(String.Format("Directory ({0}) does not exist.", _directory.Path));
            }
            
            var sFileMasks = _directory.FileMask.Split(';');

            foreach (var sFileMask in sFileMasks)
            {
                if (m_watchers.Any(x => x.Path == _directory.Path && x.Filter == sFileMask))
                {
                    throw new ArgumentException(String.Format("Directory ({0}) is already being watched.", Path.Combine(_directory.Path, sFileMask)));
                }

                var watcher = new FileSystemWatcher(_directory.Path, sFileMask)
                {
                    IncludeSubdirectories = _directory.WatchSubDirectories
                };

                watcher.Created += FileCreated;
                watcher.Changed += FileChanged;
                watcher.Deleted += FileDeleted;
                watcher.Renamed += FileRenamed;

                m_watchers.Add(watcher);
            }

            Directories.Add(_directory);
        }

        public void AddDirectory(string _sDirectory, string _sFileMask, bool _bWatchSubdirectories = false)
        {
            AddDirectory(new WatchDirectory(_sDirectory, _sFileMask, _bWatchSubdirectories));
        }

        public void Start()
        {
            if (!Active)
            {
                Active = true;

                foreach (var watcher in m_watchers)
                {
                    watcher.EnableRaisingEvents = true;
                }
            }
        }

        public void Stop()
        {
            if (Active)
            {
                Active = false;

                foreach (var watcher in m_watchers)
                {
                    watcher.EnableRaisingEvents = false;
                }
            }
        }

        private void FileCreated(object _sender, FileSystemEventArgs _e)
        {
            Created(this, _e);
        }

        private void FileChanged(object _sender, FileSystemEventArgs _e)
        {
            Changed(this, _e);
        }

        private void FileDeleted(object _sender, FileSystemEventArgs _e)
        {
            Deleted(this, _e);
        }

        private void FileRenamed(object _sender, RenamedEventArgs _e)
        {
            Renamed(this, _e);
        }
    }
}
