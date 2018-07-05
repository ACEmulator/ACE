using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ACE.DatLoader.Entity
{
    public class SkillBase : IUnpackable
    {
        public string Description { get; private set; }
        public string Name { get; private set; }
        public uint IconId { get; private set; }
        public uint TrainedCost { get; private set; }
        public uint SpecializedCost { get; private set; }
        public uint Category { get; private set; }
        public uint ChargenUse { get; private set; }
        public uint MinLevel { get; private set; }

        public SkillFormula Formula { get; private set; } = new SkillFormula();

        public double UpperBound { get; private set; }
        public double LowerBound { get; private set; }
        public double LearnMod { get; private set; }

        public void Unpack(BinaryReader reader)
        {
            Description = reader.ReadPString(); reader.AlignBoundary();
            Name = reader.ReadPString(); reader.AlignBoundary();
            IconId = reader.ReadUInt32();
            TrainedCost = reader.ReadUInt32();
            SpecializedCost = reader.ReadUInt32();
            Category = reader.ReadUInt32();
            ChargenUse = reader.ReadUInt32();
            MinLevel = reader.ReadUInt32();
            Formula.Unpack(reader);
            UpperBound = reader.ReadDouble();
            LowerBound = reader.ReadDouble();
            LearnMod = reader.ReadDouble();
        }
    }
}
