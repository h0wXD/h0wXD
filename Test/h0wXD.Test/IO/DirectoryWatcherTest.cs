using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using h0wXD.IO;
using h0wXD.IO.Domain;
using h0wXD.IO.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace h0wXD.Test.IO
{
    
    [TestClass]
    public class DirectoryWatcherTest
    {
        private IDirectoryWatcher m_directoryWatcher;
        private const string m_sTempDirectory = @".\Temp";
        
        [TestInitialize]
        public void TestInitialize()
        {
            m_directoryWatcher = new DirectoryWatcher();

            if (!Directory.Exists(m_sTempDirectory))
            {
                Directory.CreateDirectory(m_sTempDirectory);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullListVariable_ArgumentNullExceptionThrown()
        {
            List<WatchDirectory> lst = null;
            new DirectoryWatcher(lst);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_ListOfNulls_ArgumentExceptionThrown()
        {
            new DirectoryWatcher(new List<WatchDirectory> {null, null, null});
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_NullVariable_ArgumentExceptionThrown()
        {
            WatchDirectory dir = null;
            new DirectoryWatcher(dir);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddDirectory_NullVariable_ArgumentNullExceptionThrown()
        {
            m_directoryWatcher.AddDirectory(null);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddDirectory_NullPath_ArgumentExceptionThrown()
        {
            m_directoryWatcher.AddDirectory(new WatchDirectory(null));
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddDirectory_EmptyPath_ArgumentExceptionThrown()
        {
            m_directoryWatcher.AddDirectory(new WatchDirectory(""));
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddDirectory_WhiteSpacePath_ArgumentExceptionThrown()
        {
            m_directoryWatcher.AddDirectory(new WatchDirectory("    "));
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddDirectory_NullFileMask_ArgumentExceptionThrown()
        {
            m_directoryWatcher.AddDirectory(new WatchDirectory(@"D:\nonexistingpath", null));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddDirectory_EmptyFileMask_ArgumentExceptionThrown()
        {
            m_directoryWatcher.AddDirectory(new WatchDirectory(@"D:\nonexistingpath", ""));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddDirectory_WhiteSpaceFileMask_ArgumentExceptionThrown()
        {
            m_directoryWatcher.AddDirectory(new WatchDirectory(@"D:\nonexistingpath", "    "));
        }
        
        [TestMethod]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void AddDirectory_NonExistingPath_DirectoryNotFoundExceptionThrown()
        {
            m_directoryWatcher.AddDirectory(new WatchDirectory(@"D:\nonexistingpath", "*.txt"));
        }

        [TestMethod]
        public void AddDirectory_ValidPathTwiceDifferentMask_NoExceptionThrown()
        {
            m_directoryWatcher.AddDirectory(new WatchDirectory(@"C:\Windows", "*.dll"));
            m_directoryWatcher.AddDirectory(new WatchDirectory(@"C:\Windows", "*.exe"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddDirectory_ValidPathTwiceSameMask_ArgumentExceptionThrown()
        {
            m_directoryWatcher.AddDirectory(new WatchDirectory(@"C:\Windows", "*.dll"));
            m_directoryWatcher.AddDirectory(new WatchDirectory(@"C:\Windows", "*.dll"));
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddDirectory_ValidPathTwiceSameMaskWithSplit_ArgumentExceptionThrown()
        {
            m_directoryWatcher.AddDirectory(new WatchDirectory(@"C:\Windows", "*.exe"));
            m_directoryWatcher.AddDirectory(new WatchDirectory(@"C:\Windows", "*.dll;*.exe"));
        }

        [TestMethod]
        public void FileCreated_NewTextFile_DelegateInvokedWhenStarted()
        {
            m_directoryWatcher.AddDirectory(new WatchDirectory(m_sTempDirectory, false));
            var bHandlerInvoked = false;
            var sFile = Path.Combine(m_sTempDirectory, "Test.txt");

            if (File.Exists(sFile))
            {
                File.Delete(sFile);
            }
            Assert.IsFalse(File.Exists(sFile));

            m_directoryWatcher.Created += (_sender, _e) =>
            {
                Assert.AreEqual(Path.GetFileName(sFile), _e.Name);
                bHandlerInvoked = true;
            };
            
            File.Create(sFile).Dispose();
            Thread.Sleep(500);
            Assert.IsFalse(bHandlerInvoked);
            File.Delete(sFile);

            m_directoryWatcher.Start();

            File.Create(sFile).Dispose();
            Thread.Sleep(500);
            Assert.IsTrue(bHandlerInvoked);
            File.Delete(sFile);

            m_directoryWatcher.Stop();
            bHandlerInvoked = false;
            
            File.Create(sFile).Dispose();
            Thread.Sleep(500);
            Assert.IsFalse(bHandlerInvoked);
        }

        [TestMethod]
        public void FileChanged_AddContentsToFile_DelegateInvokedWhenStarted()
        {
            m_directoryWatcher.AddDirectory(new WatchDirectory(m_sTempDirectory, false));
            var bHandlerInvoked = false;
            var sFile = Path.Combine(m_sTempDirectory, "Test2.txt");

            if (!File.Exists(sFile))
            {
                File.Create(sFile);
            }
            Assert.IsTrue(File.Exists(sFile));

            m_directoryWatcher.Changed += (_sender, _e) =>
            {
                Assert.AreEqual(Path.GetFileName(sFile), _e.Name);
                bHandlerInvoked = true;
            };

            File.WriteAllText(sFile, "testing... testing...");
            Thread.Sleep(500);
            Assert.IsFalse(bHandlerInvoked);

            m_directoryWatcher.Start();

            File.WriteAllText(sFile, "testing... testing...");
            Thread.Sleep(500);
            Assert.IsTrue(bHandlerInvoked);
            
            m_directoryWatcher.Stop();
            bHandlerInvoked = false;

            File.WriteAllText(sFile, "testing... testing...");
            Thread.Sleep(500);
            Assert.IsFalse(bHandlerInvoked);
        }

        [TestMethod]
        public void FileDeleted_FileToDelete_DelegateInvokedWhenStarted()
        {
            m_directoryWatcher.AddDirectory(new WatchDirectory(m_sTempDirectory, false));
            var bHandlerInvoked = false;
            var sFile = Path.Combine(m_sTempDirectory, "Test3.txt");
            FileStream fileStream = null;

            if (!File.Exists(sFile))
            {
                File.Create(sFile).Dispose();
            }
            Assert.IsTrue(File.Exists(sFile));

            m_directoryWatcher.Deleted += (_sender, _e) =>
            {
                Assert.AreEqual(Path.GetFileName(sFile), _e.Name);
                bHandlerInvoked = true;
            };

            File.Delete(sFile);
            File.Create(sFile).Dispose();
            Thread.Sleep(500);
            Assert.IsFalse(bHandlerInvoked);

            m_directoryWatcher.Start();
            
            File.Delete(sFile);
            File.Create(sFile).Dispose();
            Thread.Sleep(500);
            Assert.IsTrue(bHandlerInvoked);

            m_directoryWatcher.Stop();
            bHandlerInvoked = false;
            
            File.Delete(sFile);
            Thread.Sleep(500);
            Assert.IsFalse(bHandlerInvoked);
        }
        
        [TestMethod]
        public void FileDeleted_FileToRename_DelegateInvokedWhenStarted()
        {
            m_directoryWatcher.AddDirectory(new WatchDirectory(m_sTempDirectory, false));
            var bHandlerInvoked = false;
            var sFile1 = Path.Combine(m_sTempDirectory, "Test4.txt");
            var sFile2 = Path.Combine(m_sTempDirectory, "Test4_renamed.txt");

            if (!File.Exists(sFile1))
            {
                File.Create(sFile1).Dispose();
            }
            Assert.IsTrue(File.Exists(sFile1));
            if (File.Exists(sFile2))
            {
                File.Delete(sFile2);
            }
            Assert.IsFalse(File.Exists(sFile2));

            m_directoryWatcher.Renamed += (_sender, _e) =>
            {
                Assert.AreEqual(Path.GetFileName(sFile1), _e.Name);
                bHandlerInvoked = true;
            };

            File.Move(sFile1, sFile2);
            Thread.Sleep(500);
            Assert.IsFalse(bHandlerInvoked);

            m_directoryWatcher.Start();

            File.Move(sFile2, sFile1);
            Thread.Sleep(500);
            Assert.IsTrue(bHandlerInvoked);

            m_directoryWatcher.Stop();
            bHandlerInvoked = false;

            File.Move(sFile1, sFile2);
            Thread.Sleep(500);
            Assert.IsFalse(bHandlerInvoked);
        }
    }
}
