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
        private TextFieldParser _textFieldParser;
        private bool _readFirstLine;
        private bool _skipFirstLine;
        private Encoding _encoding;
        
        public bool EndOfData { get { return _textFieldParser.EndOfData; } }
        public int LineNumber { get; private set; }
        public string FileName { get; private set; }

        public bool SkipFirstLine
        {
            get { return _skipFirstLine; }
            set { _skipFirstLine = value; _readFirstLine = !value; }
        }

        private bool _tempHasFieldsEnclosedInQuotes = false;
        public bool HasFieldsEnclosedInQuotes
        {
            get { return _textFieldParser.HasFieldsEnclosedInQuotes; } 
            set
            {
                if (_textFieldParser == null)
                {
                    _tempHasFieldsEnclosedInQuotes = value;
                }
                else
                {
                    _textFieldParser.HasFieldsEnclosedInQuotes = value;
                }
            }
        }

        private bool _tempTrimWhiteSpace = true;
        public bool TrimWhiteSpace
        {
            get { return _textFieldParser.TrimWhiteSpace; } 
            set
            {
                if (_textFieldParser == null)
                {
                    _tempTrimWhiteSpace = value;
                }
                else
                {
                    _textFieldParser.TrimWhiteSpace = value;
                }
            }
        }

        private string [] _tempCommentTokensArray = {"#"};
        public string [] CommentTokens
        {
            get { return _textFieldParser.CommentTokens; } 
            set
            {
                if (_textFieldParser == null)
                {
                    _tempCommentTokensArray = value;
                }
                else
                {
                    _textFieldParser.CommentTokens = value;
                }
            }
        }
        
        private string [] _tempDelimitersArray = {","};
        public string [] Delimiters
        {
            get { return _textFieldParser.Delimiters; } 
            set
            {
                if (_textFieldParser == null)
                {
                    _tempDelimitersArray = value;
                }
                else
                {
                    _textFieldParser.Delimiters = value;
                }
            }
        }

        public CsvFileReader()
        {
            SkipFirstLine = false;
        }

        public void Open(string fileName, Encoding encoding)
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException(TechnicalConstants.IO.CsvFileReader.Exceptions.FileNotFound, fileName);
            }

            _encoding = encoding;
            FileName = fileName;
            Reset();
        }

        public void Close()
        {
            if (_textFieldParser != null)
            {
                _textFieldParser.Close();
                _textFieldParser = null;
            }
        }

        public void Reset()
        {
            if (_textFieldParser == null)
            {
                _textFieldParser = new TextFieldParser(FileName, _encoding)
                {
                    CommentTokens = _tempCommentTokensArray, 
                    Delimiters = _tempDelimitersArray, 
                    HasFieldsEnclosedInQuotes = _tempHasFieldsEnclosedInQuotes,
                    TrimWhiteSpace = _tempTrimWhiteSpace,
                };
            }
            else
            {
                var parser = new TextFieldParser(FileName, _encoding)
                {
                    CommentTokens = _textFieldParser.CommentTokens,
                    Delimiters = _textFieldParser.Delimiters,
                    HasFieldsEnclosedInQuotes = _textFieldParser.HasFieldsEnclosedInQuotes,
                    TrimWhiteSpace = _textFieldParser.TrimWhiteSpace
                };

                _readFirstLine = !SkipFirstLine;
                _textFieldParser = parser;
            }

            LineNumber = 0;
        }

        public string ReadLine()
        {
            if (_textFieldParser == null)
            {
                Reset();
            }

            if (EndOfData)
            {
                throw new AccessViolationException(TechnicalConstants.IO.CsvFileReader.Exceptions.EndOfData);
            }

            if (!_readFirstLine)
            {
                _textFieldParser.ReadLine();
                _readFirstLine = true;
                LineNumber++;

                if (EndOfData)
                {
                    throw new AccessViolationException(TechnicalConstants.IO.CsvFileReader.Exceptions.EndOfData);
                }
            }

            LineNumber++;
            var line = _textFieldParser.ReadLine();

            // Ms messed up somehow?, TextFieldParser is not skipping all comments....

            return line;
        }

        public string [] ReadFields()
        {
            if (_textFieldParser == null)
            {
                Reset();
            }

            if (EndOfData)
            {
                throw new AccessViolationException(TechnicalConstants.IO.CsvFileReader.Exceptions.EndOfData);
            }

            if (!_readFirstLine)
            {
                _textFieldParser.ReadFields();
                _readFirstLine = true;
                LineNumber++;

                if (EndOfData)
                {
                    throw new AccessViolationException(TechnicalConstants.IO.CsvFileReader.Exceptions.EndOfData);
                }
            }

            LineNumber++;
            return _textFieldParser.ReadFields();
        }
    }
}
