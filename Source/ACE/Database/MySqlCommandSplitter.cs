using System;
using System.Text;
using System.IO;

namespace ACE.Database
{
    // Based on code from https://github.com/DbUp/DbUp
    public class MySqlCommandSplitter : StringReader
    {
        private readonly string sqlText;
        private int currentIndex;

        private const char NullChar = (char)0;
        private const char EndOfLineChar = '\n';
        private const char CarriageReturn = '\r';
        private const char SingleQuoteChar = (char)39;
        private const char DashChar = '-';
        private const char SlashChar = '/';
        private const char StarChar = '*';
        private const char OpenBracketChar = '[';
        private const char CloseBracketChar = ']';

        protected const int FailedRead = -1;

        public MySqlCommandSplitter(string sqlText, string delimiter = ";", bool delimiterRequiresWhitespace = true) : base(sqlText)
        {
            this.sqlText = sqlText;
            Delimiter = delimiter;
            DelimiterRequiresWhitespace = delimiterRequiresWhitespace;
            currentIndex = -1;
            commandScriptBuilder = new StringBuilder();
        }

        protected string Delimiter { get; set; }
        protected bool DelimiterRequiresWhitespace { get; set; }

        protected int CurrentIndex
        {
            get { return currentIndex; }
            private set
            {
                currentIndex = value;
                LastChar = sqlText[currentIndex - 1];
                CurrentChar = sqlText[currentIndex];
            }
        }

        protected void Parse()
        {
            while (Read() != FailedRead)
            {
                if (IsCustomStatement)
                {
                    ReadCustomStatement();
                }
                else if (IsQuote)
                {
                    ReadQuotedString();
                }
                else if (IsBeginningOfBracketedText)
                {
                    ReadBracketedText();
                }
                else if (IsBeginningOfDashDashComment)
                {
                    ReadDashDashComment();
                }
                else if (IsBeginningOfSlashStarComment)
                {
                    ReadSlashStarComment();
                }
                else if (IsBeginningOfDelimiter)
                {
                    if (ReadDelimiter() && CommandEnded != null)
                        CommandEnded();
                }
                else
                {
                    ReadCharacter(CharacterType.Command, CurrentChar);
                }
            }
            if (CommandEnded != null)
                CommandEnded();
        }

        #region Events to be subscribed to by consuming or deriving classes

        protected event Action CommandEnded;
        protected event Action<CharacterType, char> ReadCharacter;

        protected void OnReadCharacter(CharacterType type, char c)
        {
            if (ReadCharacter != null)
            {
                ReadCharacter(type, c);
            }

        }
        #endregion

        public override int Read()
        {
            var result = base.Read();
            if (result != FailedRead)
            {
                currentIndex++;
                LastChar = CurrentChar;
                CurrentChar = (char)result;

                if (CurrentChar == CarriageReturn && PeekChar() != EndOfLineChar)
                    return Read();

                return result;
            }

            return result;
        }

        public override int Read(char[] buffer, int index, int count)
        {
            var read = base.Read(buffer, index, count);
            CurrentIndex += read;
            CurrentChar = sqlText[CurrentIndex];
            return read;
        }

        public override int ReadBlock(char[] buffer, int index, int count)
        {
            var read = base.ReadBlock(buffer, index, count);
            CurrentIndex += read;
            return read;
        }

        public override string ReadLine()
        {
            var readLine = base.ReadLine();
            if (readLine != null)
                CurrentIndex += readLine.Length;
            return readLine;
        }

        public override string ReadToEnd()
        {
            CurrentIndex = sqlText.Length - 1;
            return base.ReadToEnd();
        }

        protected char LastChar { get; private set; }
        protected char CurrentChar { get; private set; }

        protected bool HasReachedEnd
        {
            get
            {
                return Peek() == -1;
            }
        }

        protected bool IsEndOfLine
        {
            get
            {
                return CurrentChar == EndOfLineChar;
            }
        }

        protected bool IsCurrentCharEqualTo(char comparisonChar)
        {
            return IsCharEqualTo(CurrentChar, comparisonChar);
        }

        protected bool IsLastCharEqualTo(char comparisonChar)
        {
            return IsCharEqualTo(LastChar, comparisonChar);
        }

        protected bool IsCharEqualTo(char comparisonChar, char compareTo)
        {
            return char.ToLowerInvariant(comparisonChar) == char.ToLowerInvariant(compareTo);
        }

        protected bool IsQuote
        {
            get
            {
                // TODO should match double?
                return CurrentChar == SingleQuoteChar;
            }
        }

        protected bool IsWhiteSpace
        {
            get
            {
                return char.IsWhiteSpace(CurrentChar);
            }
        }

        protected char PeekChar()
        {
            if (HasReachedEnd)
            {
                return NullChar;
            }
            return (char)Peek();
        }

        protected bool TryPeek(int numberOfCharacters, out string result)
        {
            var currentIndex = CurrentIndex;
            if (currentIndex + numberOfCharacters >= sqlText.Length)
            {
                result = null;
                return false;
            }

            result = sqlText.Substring(CurrentIndex + 1, numberOfCharacters);
            return true;
        }

        private bool IsBeginningOfBracketedText
        {
            get
            {
                return CurrentChar == OpenBracketChar;
            }
        }

        private bool IsEndOfBracketedText
        {
            get
            {
                return CurrentChar == CloseBracketChar;
            }
        }

        private bool IsBeginningOfDashDashComment
        {
            get
            {
                if (CurrentChar != DashChar)
                {
                    return false;
                }
                return Peek() == DashChar;
            }
        }

        private bool IsBeginningOfSlashStarComment
        {
            get
            {
                return CurrentChar == SlashChar && Peek() == StarChar;
            }
        }

        private bool IsBeginningOfDelimiter
        {
            get
            {
                var lastCharIsNullOrEmpty = char.IsWhiteSpace(LastChar) || LastChar == NullChar || !DelimiterRequiresWhitespace;
                var isCurrentCharacterStartOfDelimiter = IsCurrentCharEqualTo(Delimiter[0]);
                string result;
                return
                    lastCharIsNullOrEmpty &&
                    isCurrentCharacterStartOfDelimiter &&
                    TryPeek(Delimiter.Length - 1, out result) &&
                    string.Equals(result, Delimiter.Substring(1), StringComparison.OrdinalIgnoreCase);
            }
        }

        private bool IsEndOfSlashStarComment
        {
            get
            {
                return LastChar == StarChar && CurrentChar == SlashChar;
            }
        }

        private void ReadQuotedString()
        {
            ReadCharacter(CharacterType.QuotedString, CurrentChar);
            while (Read() != FailedRead)
            {
                ReadCharacter(CharacterType.QuotedString, CurrentChar);
                if (IsQuote)
                {
                    return;
                }
            }
        }

        private void ReadBracketedText()
        {
            ReadCharacter(CharacterType.BracketedText, CurrentChar);
            while (Read() != FailedRead)
            {
                ReadCharacter(CharacterType.BracketedText, CurrentChar);
                if (IsEndOfBracketedText)
                {
                    var peekChar = PeekChar();
                    // Close brackets within brackets are escaped with another
                    // Close bracket. e.g. [a[b]]c] => a[b]c
                    if (peekChar == CloseBracketChar)
                    {
                        Read();
                        ReadCharacter(CharacterType.BracketedText, CurrentChar);
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        private void ReadDashDashComment()
        {
            // Writes the current dash.
            ReadCharacter(CharacterType.DashComment, CurrentChar);
            // Read until we hit the end of line.
            do
            {
                if (Read() == FailedRead)
                {
                    break;
                }
                ReadCharacter(CharacterType.DashComment, CurrentChar);
            }
            while (!IsEndOfLine);
        }

        private void ReadSlashStarComment()
        {
            // Write the current slash.
            ReadCharacter(CharacterType.SlashStarComment, CurrentChar);
            // Read until we find a the ending of the slash star comment,
            // Or a nested slash star comment.
            while (Read() != FailedRead)
            {
                if (IsBeginningOfSlashStarComment)
                {
                    // Nested comment found - using recursive call to read it.
                    ReadSlashStarComment();
                }
                else
                {
                    ReadCharacter(CharacterType.SlashStarComment, CurrentChar);
                    if (IsEndOfSlashStarComment)
                    {
                        return;
                    }
                }
            }
        }

        private bool ReadDelimiter()
        {
            var previousChar = CurrentChar;
            var buffer = new char[Delimiter.Length - 1];

            // read the rest of the delimiter
            Read(buffer, 0, Delimiter.Length - 1);

            // Support terminator.
            var peekChar = PeekChar();
            char? terminator = null;
            if (peekChar == ';' || peekChar == NullChar)
            {
                // if NullChar read will not succeed and CurrentChar will not change
                if (Read() > 0)
                {
                    terminator = CurrentChar;
                }
            }
            // Check that the statement is indeed a GO and not text starting with Go
            // If it is not a go, add text to buffer and continue
            else if (!char.IsWhiteSpace(peekChar) && DelimiterRequiresWhitespace)
            {
                ReadCharacter(CharacterType.Command, previousChar);

                foreach (var @char in buffer)
                {
                    ReadCharacter(CharacterType.Command, @char);
                }

                return false;
            }

            // add the first char of the delimiter
            ReadCharacter(CharacterType.Delimiter, previousChar);

            // add the body of the delimiter
            foreach (var @char in buffer)
            {
                ReadCharacter(CharacterType.Delimiter, @char);
            }

            // add the ';' terminator, if present
            if (terminator.HasValue)
                ReadCharacter(CharacterType.Delimiter, terminator.Value);

            return true;
        }

        public enum CharacterType
        {
            /// <summary>Character belongs to a commment</summary>
            Command,
            /// <summary>Character belongs to a /* comment</summary>
            SlashStarComment,
            /// <summary>Character belongs to a -- comment</summary>
            DashComment,
            /// <summary>Character belongs to [bracketed] text</summary>
            BracketedText,
            /// <summary>Character belongs to "quoted" text</summary>
            QuotedString,
            /// <summary>Character belongs do a delimiter (like GO)</summary>
            Delimiter,
            /// <summary>Character is a custom statement (open for new implementation)</summary>
            CustomStatement,

        }

        private readonly StringBuilder commandScriptBuilder;

        public void ReadAllCommands(Action<string> handleCommand)
        {
            while (!HasReachedEnd)
            {
                this.ReadCharacter += (type, c) =>
                {
                    switch (type)
                    {
                        case CharacterType.Command:
                        case CharacterType.SlashStarComment:
                        case CharacterType.DashComment:
                        case CharacterType.BracketedText:
                        case CharacterType.QuotedString:
                        case CharacterType.CustomStatement:
                            commandScriptBuilder.Append(c);
                            break;
                        case CharacterType.Delimiter:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("type", type, null);
                    }

                };
                this.CommandEnded += () =>
                {
                    var commandText = GetCurrentCommandTextFromBuffer();
                    if (commandText.Length > 0)
                    {
                        handleCommand(commandText);
                        ResetCommandBuffer();
                    }
                };

                Parse();
            }
        }

        private void ResetCommandBuffer()
        {
            commandScriptBuilder.Length = 0;
        }

        private string GetCurrentCommandTextFromBuffer()
        {
            return commandScriptBuilder.ToString().Trim();
        }

        private const string DelimiterKeyword = "DELIMITER";

        protected bool IsCustomStatement
        {
            get
            {
                string statement;
                return TryPeek(DelimiterKeyword.Length, out statement) &&
                       string.Equals(DelimiterKeyword, statement, StringComparison.OrdinalIgnoreCase);
            }
        }

        protected void ReadCustomStatement()
        {
            // Move past Delimiter keyword
            var count = DelimiterKeyword.Length + 1;
            Read(new char[count], 0, count);

            SkipWhitespace();
            // Read until we hit the end of line.
            var delimiter = new StringBuilder();
            do
            {
                delimiter.Append(CurrentChar);
                if (Read() == FailedRead)
                {
                    break;
                }
            }
            while (!IsEndOfLine && !IsWhiteSpace);

            Delimiter = delimiter.ToString();
        }

        private void SkipWhitespace()
        {
            while (char.IsWhiteSpace(CurrentChar))
            {
                Read();
            }
        }
    }
}
