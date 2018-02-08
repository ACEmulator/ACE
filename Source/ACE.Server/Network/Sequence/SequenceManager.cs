using System;
using System.Collections.Generic;

namespace ACE.Server.Network.Sequence
{
    public class SequenceManager
    {
        private Dictionary<SequenceType, ISequence> sequenceList = new Dictionary<SequenceType, ISequence>();

        public void InitializeAllSequences()
        {
            var values = System.Enum.GetValues(typeof(SequenceType));
            foreach (var value in values)
            {
                AddOrSetSequence((SequenceType)value, new ByteSequence(false));
            }
        }

        public void AddOrSetSequence(SequenceType type, ISequence sequence)
        {
            if (!sequenceList.ContainsKey(type))
            {
                sequenceList.Add(type, sequence);
            }
            else
            {
                sequenceList[type] = sequence;
            }
        }

        public byte[] GetCurrentSequence(SequenceType type)
        {
            if (!sequenceList.ContainsKey(type))
                throw new ArgumentOutOfRangeException("type");
            return sequenceList[type].CurrentBytes;
        }

        public byte[] GetNextSequence(SequenceType type)
        {
            if (!sequenceList.ContainsKey(type))
                throw new ArgumentOutOfRangeException("type");
            return sequenceList[type].NextBytes;
        }
    }
}
