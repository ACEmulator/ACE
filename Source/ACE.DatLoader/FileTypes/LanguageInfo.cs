using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// This is in client_local_English.dat with the ID of 0x41000000.
    /// 
    /// Contains some very basic language and formatting rules.
    /// </summary>
    [DatFileType(DatFileType.StringState)]
    public class LanguageInfo : FileType
    {
        internal const uint FILE_ID = 0x41000000;

        public int Version;
        public short Base;
        public short NumDecimalDigits;
        public bool LeadingZero;

        public short GroupingSize;
        public List<char> Numerals;
        public List<char> DecimalSeperator;
        public List<char> GroupingSeperator;
        public List<char> NegativeNumberFormat;
        public bool IsZeroSingular;
        public bool IsOneSingular;
        public bool IsNegativeOneSingular;
        public bool IsTwoOrMoreSingular;
        public bool IsNegativeTwoOrLessSingular;

        public List<char> TreasurePrefixLetters;
        public List<char> TreasureMiddleLetters;
        public List<char> TreasureSuffixLetters;
        public List<char> MalePlayerLetters;
        public List<char> FemalePlayerLetters;
        public uint ImeEnabledSetting;

        public uint SymbolColor;
        public uint SymbolColorText;
        public uint SymbolHeight;
        public uint SymbolTranslucence;
        public uint SymbolPlacement;
        public uint CandColorBase;
        public uint CandColorBorder;
        public uint CandColorText;
        public uint CompColorInput;
        public uint CompColorTargetConv;
        public uint CompColorConverted;
        public uint CompColorTargetNotConv;
        public uint CompColorInputErr;
        public uint CompTranslucence;
        public uint CompColorText;
        public uint OtherIME;

        public int WordWrapOnSpace;
        public List<char> AdditionalSettings;
        public uint AdditionalFlags;

        public override void Unpack(BinaryReader reader)
        {
            Version = reader.ReadInt32();
            Base = reader.ReadInt16();
            NumDecimalDigits = reader.ReadInt16();
            LeadingZero = reader.ReadBoolean();

            GroupingSize = reader.ReadInt16();
            Numerals = UnpackList(reader);
            DecimalSeperator = UnpackList(reader);
            GroupingSeperator = UnpackList(reader);
            NegativeNumberFormat = UnpackList(reader);
            IsZeroSingular = reader.ReadBoolean();
            IsOneSingular = reader.ReadBoolean();
            IsNegativeOneSingular = reader.ReadBoolean();
            IsTwoOrMoreSingular = reader.ReadBoolean();
            IsNegativeTwoOrLessSingular = reader.ReadBoolean();
            reader.AlignBoundary();

            TreasurePrefixLetters = UnpackList(reader);
            TreasureMiddleLetters = UnpackList(reader);
            TreasureSuffixLetters = UnpackList(reader);
            MalePlayerLetters = UnpackList(reader);
            FemalePlayerLetters = UnpackList(reader);
            ImeEnabledSetting = reader.ReadUInt32();

            SymbolColor = reader.ReadUInt32();
            SymbolColorText = reader.ReadUInt32();
            SymbolHeight = reader.ReadUInt32();
            SymbolTranslucence = reader.ReadUInt32();
            SymbolPlacement = reader.ReadUInt32();
            CandColorBase = reader.ReadUInt32();
            CandColorBorder = reader.ReadUInt32();
            CandColorText = reader.ReadUInt32();
            CompColorInput = reader.ReadUInt32();
            CompColorTargetConv = reader.ReadUInt32();
            CompColorConverted = reader.ReadUInt32();
            CompColorTargetNotConv = reader.ReadUInt32();
            CompColorInputErr = reader.ReadUInt32();
            CompTranslucence = reader.ReadUInt32();
            CompColorText = reader.ReadUInt32();
            OtherIME = reader.ReadUInt32();

            WordWrapOnSpace = reader.ReadInt32();
            AdditionalSettings = UnpackList(reader);
            AdditionalFlags = reader.ReadUInt32();
        }

        // just a little helper function...
        private List<char> UnpackList(BinaryReader reader)
        {
            List<char> l = new List<char>();

            byte numElements = reader.ReadByte();
            for (int i = 0; i < numElements; i++)
            {
                ushort c = reader.ReadUInt16();
                l.Add((char)c);
            }

            return l;
        }
    }
}
