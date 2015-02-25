using System;
using System.IO;
using h0wXD.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace h0wXD.Test.Logging
{
    [TestClass]
    public class TestLoggerTest
    {
        private static TestLogger ms_testLogger;

        [ClassInitialize]
        public static void ClassInitialize(TestContext a)
        {
            ms_testLogger = new TestLogger();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
        }

        [TestMethod]
        public void Constructor_Default_FileExists()
        {
            Assert.IsTrue(File.Exists(ms_testLogger.LogToFileBehavior.CurrentFile));
            Assert.IsTrue(File.Exists(ms_testLogger.LogToFileBehaviorSwapDaily.CurrentFile));
        }

        [TestMethod]
        public void Log_LogString_DelegateInvoked()
        {
            var sLogMessage = "Log_LogString_DelegateInvoked {0}";
            var sExpectedLogMessage = String.Format(sLogMessage, "test");
            var bHandlerInvoked = false;

            EventHandler<LogEventArgs> eventHandler = (_sender, _args) =>
            {
                Assert.AreEqual(sExpectedLogMessage, _args.Message);
                Assert.AreEqual(LogType.Normal, _args.LogType);
                Assert.AreEqual(DateTime.Now.DayOfYear, _args.Date.DayOfYear);
                bHandlerInvoked = true;
            };

            ms_testLogger.Log += eventHandler; 

            ms_testLogger.Write(sLogMessage, "test");
            Assert.AreEqual(sExpectedLogMessage, ms_testLogger.LastMessage);
            Assert.IsTrue(bHandlerInvoked);

            ms_testLogger.Log -= eventHandler;
        }

        [TestMethod]
        public void LogMessage_LogString_DelegateInvoked()
        {
            var sLogMessage = "LogMessage_LogString_DelegateInvoked {0}";
            var sExpectedLogMessage = String.Format(sLogMessage, "test2");
            var bHandlerInvoked = false;

            EventHandler<string> eventHandler = (_sender, _sMessage) =>
            {
                Assert.AreEqual(sExpectedLogMessage, _sMessage);
                bHandlerInvoked = true;
            };

            ms_testLogger.LogMessage += eventHandler;

            ms_testLogger.Write(sLogMessage, "test2");
            Assert.AreEqual(sExpectedLogMessage, ms_testLogger.LastMessage);
            Assert.IsTrue(bHandlerInvoked);

            ms_testLogger.LogMessage -= eventHandler;
        }

        [TestMethod]
        public void Debug_LogString_LogEventArgsTypeDebug()
        {
            EventHandler<LogEventArgs> eventHandler = (_sender, _args) =>
            {
                Assert.AreEqual(LogType.Debug, _args.LogType);
            };

            ms_testLogger.Log += eventHandler;
            ms_testLogger.Debug("Something");
            ms_testLogger.Log -= eventHandler;
        }

        [TestMethod]
        public void Info_LogString_LogEventArgsTypeDebug()
        {
            EventHandler<LogEventArgs> eventHandler = (_sender, _args) =>
            {
                Assert.AreEqual(LogType.Info, _args.LogType);
            };

            ms_testLogger.Log += eventHandler;
            ms_testLogger.Info("Something");
            ms_testLogger.Log -= eventHandler;
        }

        [TestMethod]
        public void Warning_LogString_LogEventArgsTypeDebug()
        {
            EventHandler<LogEventArgs> eventHandler = (_sender, _args) =>
            {
                Assert.AreEqual(LogType.Warning, _args.LogType);
            };

            ms_testLogger.Log += eventHandler;
            ms_testLogger.Warning("Something");
            ms_testLogger.Log -= eventHandler;
        }

        [TestMethod]
        public void Error_LogString_LogEventArgsTypeDebug()
        {
            EventHandler<LogEventArgs> eventHandler = (_sender, _args) =>
            {
                Assert.AreEqual(LogType.Error, _args.LogType);
            };

            ms_testLogger.Log += eventHandler;
            ms_testLogger.Error("Something");
            ms_testLogger.Log -= eventHandler;
        }

        [TestMethod]
        public void Fatal_LogString_LogEventArgsTypeDebug()
        {
            EventHandler<LogEventArgs> eventHandler = (_sender, _args) =>
            {
                Assert.AreEqual(LogType.Fatal, _args.LogType);
            };

            ms_testLogger.Log += eventHandler;
            ms_testLogger.Fatal("Something");
            ms_testLogger.Log -= eventHandler;
        }
        
        [TestMethod]
        public void LogToFileWrite_DateChange_StartsNewLogFile()
        {
            var args = new LogEventArgs()
            {
                Date = DateTime.Now.AddDays(1),
                Message = "Message from the future!",
                LogType = LogType.Normal
            };

            var sCurrentLogFile = ms_testLogger.LogToFileBehaviorSwapDaily.CurrentFile;
            ms_testLogger.LogToFileBehaviorSwapDaily.Write(args);
            var sNextLogFile = ms_testLogger.LogToFileBehaviorSwapDaily.CurrentFile;

            Assert.AreNotEqual(sCurrentLogFile, sNextLogFile);
        }
        
        [TestMethod]
        public void LogToFileWrite_DateChange_LogsDateChangeInFile()
        {
            var args = new LogEventArgs()
            {
                Date = DateTime.Now.AddDays(1),
                Message = "Message from the future!",
                LogType = LogType.Normal
            };

            var sCurrentLogFile = ms_testLogger.LogToFileBehavior.CurrentFile;
            ms_testLogger.LogToFileBehavior.Write(args);
            var sNextLogFile = ms_testLogger.LogToFileBehavior.CurrentFile;

            Assert.AreEqual(sCurrentLogFile, sNextLogFile);
        }
    }
}
