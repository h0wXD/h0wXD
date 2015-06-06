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
        protected readonly List<FileSystemWatcher> _watchers;

        public event FileSystemEventHandler Created = delegate {};
        public event FileSystemEventHandler Changed = delegate {};
        public event FileSystemEventHandler Deleted = delegate {};
        public event FileSystemEventHandler Renamed = delegate {};
        
        public List<WatchDirectory> Directories { get; private set; }
        public bool Active { get; private set; }

        public DirectoryWatcher()
        {
            Active = false;
            _watchers = new List<FileSystemWatcher>();
            Directories = new List<WatchDirectory>();
        }

        public DirectoryWatcher(WatchDirectory _directory) :
            this(new List<WatchDirectory> {_directory})
        {
        }

        public DirectoryWatcher(List<WatchDirectory> directories) :
            this()
        {
            if (directories == null)
            {
                throw new ArgumentNullException("directories");
            }
            if (directories.Any(x => x == null))
            {
                throw new ArgumentException("Directories cannot contain null values.");
            }

            foreach (var directory in directories)
            {
                AddDirectory(directory);
            }
        }

        public void AddDirectory(WatchDirectory directory)
        {
            if (directory == null)
            {
                throw new ArgumentNullException("WatchDirectory");
            }
            if (String.IsNullOrWhiteSpace(directory.Path))
            {
                throw new ArgumentException("Path cannot be empty.", "directory");
            }
            if (String.IsNullOrWhiteSpace(directory.FileMask))
            {
                throw new ArgumentException("FileMask cannot be empty.", "directory");
            }
            if (!Directory.Exists(directory.Path))
            {
                throw new DirectoryNotFoundException(String.Format("Directory ({0}) does not exist.", directory.Path));
            }
            
            var fileMasks = directory.FileMask.Split(';');

            foreach (var fileMask in fileMasks)
            {
                if (_watchers.Any(x => x.Path == directory.Path && x.Filter == fileMask))
                {
                    throw new ArgumentException(String.Format("Directory ({0}) is already being watched.", Path.Combine(directory.Path, fileMask)));
                }

                var watcher = new FileSystemWatcher(directory.Path, fileMask)
                {
                    IncludeSubdirectories = directory.WatchSubDirectories
                };

                watcher.Created += FileCreated;
                watcher.Changed += FileChanged;
                watcher.Deleted += FileDeleted;
                watcher.Renamed += FileRenamed;

                _watchers.Add(watcher);
            }

            Directories.Add(directory);
        }

        public void AddDirectory(string directory, string fileMask, bool watchSubdirectories = false)
        {
            AddDirectory(new WatchDirectory(directory, fileMask, watchSubdirectories));
        }

        public void Start()
        {
            if (_watchers.Count == 0)
            {
                throw new InvalidOperationException("Cannot start when no directories where added.");
            }

            if (!Active)
            {
                Active = true;

                foreach (var watcher in _watchers)
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

                foreach (var watcher in _watchers)
                {
                    watcher.EnableRaisingEvents = false;
                }
            }
        }

        private void FileCreated(object sender, FileSystemEventArgs e)
        {
            Created(this, e);
        }

        private void FileChanged(object sender, FileSystemEventArgs e)
        {
            Changed(this, e);
        }

        private void FileDeleted(object sender, FileSystemEventArgs e)
        {
            Deleted(this, e);
        }

        private void FileRenamed(object sender, RenamedEventArgs e)
        {
            Renamed(this, e);
        }
    }
}
