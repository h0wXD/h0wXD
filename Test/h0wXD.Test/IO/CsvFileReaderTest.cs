using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using h0wXD.Common.IO;
using h0wXD.Common.IO.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace h0wXD.Common.Test.IO
{
    [TestClass]
    public class CsvFileReaderTest
    {
        private ICsvFileReader m_csvFileReader;

        private class FileNames
        {
            public const string CommaCsvFileUTF8 = @"UnitTestData\CommaCsvFileUTF8.csv";
            public const string CommaCommentCsvFileUTF8 = @"UnitTestData\CommaCommentCsvFileUTF8.csv";
            public const string SemicolonCsvFileUnicode = @"UnitTestData\SemicolonCsvFileUnicode.csv";
            public const string MixedSpacesQuotesCsvFileUnicode = @"UnitTestData\MixedSpacesQuotesCsvFileUnicode.csv";

            public static readonly string [] All = {CommaCsvFileUTF8, CommaCommentCsvFileUTF8, SemicolonCsvFileUnicode, MixedSpacesQuotesCsvFileUnicode};
        }

        [TestInitialize]
        [DeploymentItem(FileNames.CommaCsvFileUTF8, "UnitTestData")]
        [DeploymentItem(FileNames.CommaCommentCsvFileUTF8, "UnitTestData")]
        [DeploymentItem(FileNames.SemicolonCsvFileUnicode, "UnitTestData")]
        [DeploymentItem(FileNames.MixedSpacesQuotesCsvFileUnicode, "UnitTestData")]
        public void TestInitialize()
        {
            m_csvFileReader = new CsvFileReader();
        }

        [TestMethod]
        public void TestInitialize_DeployedCsvFiles_Exist()
        {
            foreach (var sTestFile in FileNames.All)
            {
                Assert.IsTrue(File.Exists(sTestFile));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void Open_NonExistingFile_ExceptionThrown()
        {
            const string sNonExistingFile = "NonExistingFile.csv";

            Assert.IsFalse(File.Exists(sNonExistingFile));

            m_csvFileReader.Open(sNonExistingFile, Encoding.UTF8);
        }

        [TestMethod]
        public void Open_DeployedCsvFiles_OpenSucceeded()
        {
            m_csvFileReader.Open(FileNames.CommaCsvFileUTF8, Encoding.UTF8);
            Assert.AreEqual(FileNames.CommaCsvFileUTF8, m_csvFileReader.FileName);

            m_csvFileReader.Open(FileNames.SemicolonCsvFileUnicode, Encoding.Unicode);
            Assert.AreEqual(FileNames.SemicolonCsvFileUnicode, m_csvFileReader.FileName);

            m_csvFileReader.Open(FileNames.MixedSpacesQuotesCsvFileUnicode, Encoding.Unicode);
            Assert.AreEqual(FileNames.MixedSpacesQuotesCsvFileUnicode, m_csvFileReader.FileName);
        }

        [TestMethod]
        public void Open_DeployedCsvFiles_MaintainsPreviousSettings()
        {
            var bSkipFirstLine = true;
            var bHasFieldsEnclosedInQuotes = true;
            var bTrimWhiteSpace = true;
            var sCommentTokensArray = new [] {"#", "//"};
            var sDelimitersArray = new [] {",", ";", "|"};

            m_csvFileReader.SkipFirstLine = bSkipFirstLine;
            m_csvFileReader.HasFieldsEnclosedInQuotes = bHasFieldsEnclosedInQuotes;
            m_csvFileReader.TrimWhiteSpace = bTrimWhiteSpace;
            m_csvFileReader.CommentTokens = sCommentTokensArray;
            m_csvFileReader.Delimiters = sDelimitersArray;

            m_csvFileReader.Open(FileNames.CommaCsvFileUTF8, Encoding.UTF8);

            Assert.AreEqual(FileNames.CommaCsvFileUTF8, m_csvFileReader.FileName);

            m_csvFileReader.Open(FileNames.SemicolonCsvFileUnicode, Encoding.Unicode);
            Assert.AreEqual(FileNames.SemicolonCsvFileUnicode, m_csvFileReader.FileName);
            Assert.AreEqual(bSkipFirstLine, m_csvFileReader.SkipFirstLine);
            Assert.AreEqual(bHasFieldsEnclosedInQuotes, m_csvFileReader.HasFieldsEnclosedInQuotes);
            Assert.AreEqual(bTrimWhiteSpace, m_csvFileReader.TrimWhiteSpace);
            CollectionAssert.AreEqual(sCommentTokensArray, m_csvFileReader.CommentTokens);
            CollectionAssert.AreEqual(sDelimitersArray, m_csvFileReader.Delimiters);
            
            bSkipFirstLine = false;
            bHasFieldsEnclosedInQuotes = false;
            bTrimWhiteSpace = false;
            sCommentTokensArray = new [] {"//"};
            sDelimitersArray = new [] {"|"};

            m_csvFileReader.SkipFirstLine = bSkipFirstLine;
            m_csvFileReader.HasFieldsEnclosedInQuotes = bHasFieldsEnclosedInQuotes;
            m_csvFileReader.TrimWhiteSpace = bTrimWhiteSpace;
            m_csvFileReader.CommentTokens = sCommentTokensArray;
            m_csvFileReader.Delimiters = sDelimitersArray;

            m_csvFileReader.Open(FileNames.MixedSpacesQuotesCsvFileUnicode, Encoding.Unicode);
            Assert.AreEqual(FileNames.MixedSpacesQuotesCsvFileUnicode, m_csvFileReader.FileName);
            Assert.AreEqual(bSkipFirstLine, m_csvFileReader.SkipFirstLine);
            Assert.AreEqual(bHasFieldsEnclosedInQuotes, m_csvFileReader.HasFieldsEnclosedInQuotes);
            Assert.AreEqual(bTrimWhiteSpace, m_csvFileReader.TrimWhiteSpace);
            CollectionAssert.AreEqual(sCommentTokensArray, m_csvFileReader.CommentTokens);
            CollectionAssert.AreEqual(sDelimitersArray, m_csvFileReader.Delimiters);
        }

        [TestMethod]
        public void ReadLine_UTF8CsvFile_FirstLineSkipHeaders()
        {
            m_csvFileReader.Open(FileNames.CommaCsvFileUTF8, Encoding.UTF8);
            m_csvFileReader.SkipFirstLine = true;

            var sActualFirstDataLine = m_csvFileReader.ReadLine();

            Assert.AreEqual("1,完春,趙,test1gmail.com", sActualFirstDataLine);
        }

        [TestMethod]
        public void ReadLine_UnicodeCsvFile_FirstLineWithSpacesSkipHeaders()
        {
            m_csvFileReader.Open(FileNames.MixedSpacesQuotesCsvFileUnicode, Encoding.UTF8);
            m_csvFileReader.SkipFirstLine = true;

            var sActualFirstDataLine = m_csvFileReader.ReadLine();

            Assert.AreEqual("1| 完春,趙; test1gmail.com", sActualFirstDataLine);
        }

        //[TestMethod]
        //public void ReadLine_UnicodeCsvFile_ShouldSkipComments()
        //{
        //    m_csvFileReader.Open(FileNames.CommaCommentCsvFileUTF8, Encoding.UTF8);
        //    m_csvFileReader.SkipFirstLine = true;
        //    m_csvFileReader.CommentTokens = new [] {"#", "//"};

        //    // This is simply wrong.... TextFieldParser is broken.... Actual should not be #1,完春,趙,test1gmail.com here.
        //    // However ReadFields_UTF8CommaSeparatedComment_StringArraySplitByDelimiters is skipped correctly (ReadFields method)...
        //    //Assert.AreEqual("#1,完春,趙,test1gmail.com", m_csvFileReader.ReadLine());

        //    Assert.AreEqual("2,久美子,秋本,test3@gmail.com", m_csvFileReader.ReadLine());
        //    Assert.IsTrue(m_csvFileReader.EndOfData);
        //}

        [TestMethod]
        [ExpectedException(typeof(AccessViolationException))]
        public void ReadFields_UTF8CommaSeparated_EndOfDataExceptionThrown()
        {
            m_csvFileReader.Open(FileNames.CommaCsvFileUTF8, Encoding.UTF8);
            m_csvFileReader.SkipFirstLine = true;
            m_csvFileReader.ReadFields();

            Assert.IsFalse(m_csvFileReader.EndOfData);

            m_csvFileReader.ReadFields();

            Assert.IsTrue(m_csvFileReader.EndOfData);

            m_csvFileReader.ReadFields();
        }

        [TestMethod]
        public void ReadFields_UTF8CommaSeparated_StringArraySplitByDelimiters()
        {
            m_csvFileReader.Open(FileNames.CommaCsvFileUTF8, Encoding.UTF8);

            var sActualHeaderArray = m_csvFileReader.ReadFields().ToList();
            var sExpectedHeaderArray = new [] {"Id","First Name","Last Name","Email"}.ToList();

            CollectionAssert.AreEqual(sExpectedHeaderArray, sActualHeaderArray);
            
            sActualHeaderArray = m_csvFileReader.ReadFields().ToList();
            sExpectedHeaderArray = new [] {"1","完春","趙","test1gmail.com"}.ToList();

            CollectionAssert.AreEqual(sExpectedHeaderArray, sActualHeaderArray);

            sActualHeaderArray = m_csvFileReader.ReadFields().ToList();
            sExpectedHeaderArray = new [] {"2","John","Doe","test4@gmail.com"}.ToList();

            CollectionAssert.AreEqual(sExpectedHeaderArray, sActualHeaderArray);
        }

        [TestMethod]
        public void ReadFields_MixedSpacesQuotesUnicode_TrimSplitByDelimiters()
        {
            m_csvFileReader.Open(FileNames.MixedSpacesQuotesCsvFileUnicode, Encoding.UTF8);
            m_csvFileReader.Delimiters = new [] {",", ";", "|"}; 
            m_csvFileReader.SkipFirstLine = false;
            m_csvFileReader.HasFieldsEnclosedInQuotes = true;
            m_csvFileReader.TrimWhiteSpace = true;

            var sActualHeaderArray = m_csvFileReader.ReadFields().ToList();
            var sExpectedHeaderArray = new [] {"Id","First Name","Last Name","Email"}.ToList();

            CollectionAssert.AreEqual(sExpectedHeaderArray, sActualHeaderArray);
            
            sActualHeaderArray = m_csvFileReader.ReadFields().ToList();
            sExpectedHeaderArray = new [] {"1","完春","趙","test1gmail.com"}.ToList();

            CollectionAssert.AreEqual(sExpectedHeaderArray, sActualHeaderArray);
            
            sActualHeaderArray = m_csvFileReader.ReadFields().ToList();
            sExpectedHeaderArray = new [] {"2","久美子","秋本","test3@gmail.com"}.ToList();

            CollectionAssert.AreEqual(sExpectedHeaderArray, sActualHeaderArray);

            sActualHeaderArray = m_csvFileReader.ReadFields().ToList();
            sExpectedHeaderArray = new [] {"3","John","Doe;|,Doe","test4@gmail.com"}.ToList();

            CollectionAssert.AreEqual(sExpectedHeaderArray, sActualHeaderArray);
        }
        

        [TestMethod]
        public void ReadFields_UTF8CommaSeparatedComment_StringArraySplitByDelimiters()
        {
            m_csvFileReader.Open(FileNames.CommaCommentCsvFileUTF8, Encoding.UTF8);
            m_csvFileReader.SkipFirstLine = true;
            m_csvFileReader.CommentTokens = new [] {"#", "//"};

            var sActualHeaderArray = m_csvFileReader.ReadFields().ToList();
            var sExpectedHeaderArray = new [] {"2","久美子","秋本","test3@gmail.com"}.ToList();

            CollectionAssert.AreEqual(sExpectedHeaderArray, sActualHeaderArray);
            Assert.IsTrue(m_csvFileReader.EndOfData);
        }
    }
}
