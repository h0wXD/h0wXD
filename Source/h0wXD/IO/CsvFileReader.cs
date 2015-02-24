using System;
using System.Text;
using Microsoft.VisualBasic.FileIO;
using System.IO;
using h0wXD.IO.Interfaces;

namespace h0wXD.IO
{
    /// <summary>
    /// CsvFileReader is a customized version of the Visual Basic TextFieldParser class.
    /// </summary>
    public class CsvFileReader : ICsvFileReader
    {
        private TextFieldParser m_textFieldParser;
        private bool m_bReadFirstLine;
        private bool m_bSkipFirstLine;
        private Encoding m_encoding;
        
        public bool EndOfData { get { return m_textFieldParser.EndOfData; } }
        public int LineNumber { get; private set; }
        public string FileName { get; private set; }

        public bool SkipFirstLine
        {
            get { return m_bSkipFirstLine; }
            set { m_bSkipFirstLine = value; m_bReadFirstLine = !value; }
        }

        private bool m_bTempHasFieldsEnclosedInQuotes = false;
        public bool HasFieldsEnclosedInQuotes
        {
            get { return m_textFieldParser.HasFieldsEnclosedInQuotes; } 
            set
            {
                if (m_textFieldParser == null)
                {
                    m_bTempHasFieldsEnclosedInQuotes = value;
                }
                else
                {
                    m_textFieldParser.HasFieldsEnclosedInQuotes = value;
                }
            }
        }

        private bool m_bTempTrimWhiteSpace = true;
        public bool TrimWhiteSpace
        {
            get { return m_textFieldParser.TrimWhiteSpace; } 
            set
            {
                if (m_textFieldParser == null)
                {
                    m_bTempTrimWhiteSpace = value;
                }
                else
                {
                    m_textFieldParser.TrimWhiteSpace = value;
                }
            }
        }

        private string [] m_sTempCommentTokensArray = {"#"};
        public string [] CommentTokens
        {
            get { return m_textFieldParser.CommentTokens; } 
            set
            {
                if (m_textFieldParser == null)
                {
                    m_sTempCommentTokensArray = value;
                }
                else
                {
                    m_textFieldParser.CommentTokens = value;
                }
            }
        }
        
        private string [] m_sTempDelimitersArray = {","};
        public string [] Delimiters
        {
            get { return m_textFieldParser.Delimiters; } 
            set
            {
                if (m_textFieldParser == null)
                {
                    m_sTempDelimitersArray = value;
                }
                else
                {
                    m_textFieldParser.Delimiters = value;
                }
            }
        }

        public CsvFileReader()
        {
            SkipFirstLine = false;
        }

        public void Open(string _sFileName, Encoding _encoding)
        {
            if (!File.Exists(_sFileName))
            {
                throw new FileNotFoundException(TechnicalConstants.CsvFileReader.Exceptions.FileNotFound, _sFileName);
            }

            m_encoding = _encoding;
            FileName = _sFileName;
            Reset();
        }

        public void Close()
        {
            if (m_textFieldParser != null)
            {
                m_textFieldParser.Close();
                m_textFieldParser = null;
            }
        }

        public void Reset()
        {
            if (null == m_textFieldParser)
            {
                m_textFieldParser = new TextFieldParser(FileName, m_encoding)
                {
                    CommentTokens = m_sTempCommentTokensArray, 
                    Delimiters = m_sTempDelimitersArray, 
                    HasFieldsEnclosedInQuotes = m_bTempHasFieldsEnclosedInQuotes,
                    TrimWhiteSpace = m_bTempTrimWhiteSpace,
                };
            }
            else
            {
                var parser = new TextFieldParser(FileName, m_encoding)
                {
                    CommentTokens = m_textFieldParser.CommentTokens,
                    Delimiters = m_textFieldParser.Delimiters,
                    HasFieldsEnclosedInQuotes = m_textFieldParser.HasFieldsEnclosedInQuotes,
                    TrimWhiteSpace = m_textFieldParser.TrimWhiteSpace
                };

                m_bReadFirstLine = !SkipFirstLine;
                m_textFieldParser = parser;
            }

            LineNumber = 0;
        }

        public string ReadLine()
        {
            if (EndOfData)
            {
                throw new AccessViolationException(TechnicalConstants.CsvFileReader.Exceptions.EndOfData);
            }

            if (m_textFieldParser == null)
            {
                Reset();
            }

            if (!m_bReadFirstLine)
            {
                m_textFieldParser.ReadLine();
                m_bReadFirstLine = true;
                LineNumber++;

                if (EndOfData)
                {
                    throw new AccessViolationException(TechnicalConstants.CsvFileReader.Exceptions.EndOfData);
                }
            }

            LineNumber++;
            var sLine = m_textFieldParser.ReadLine();

            // Ms messed up somehow?, TextFieldParser is not skipping all comments....

            return sLine;
        }

        public string [] ReadFields()
        {
            if (EndOfData)
            {
                throw new AccessViolationException(TechnicalConstants.CsvFileReader.Exceptions.EndOfData);
            }

            if (m_textFieldParser == null)
            {
                Reset();
            }

            if (!m_bReadFirstLine)
            {
                m_textFieldParser.ReadFields();
                m_bReadFirstLine = true;
                LineNumber++;

                if (EndOfData)
                {
                    throw new AccessViolationException(TechnicalConstants.CsvFileReader.Exceptions.EndOfData);
                }
            }

            LineNumber++;
            return m_textFieldParser.ReadFields();
        }
    }
}
