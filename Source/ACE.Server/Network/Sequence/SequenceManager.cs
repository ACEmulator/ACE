using System;
using System.Collections.Generic;

namespace ACE.Server.Network.Sequence
{
    public class SequenceManager
    {
        private readonly Dictionary<SequenceType, ISequence> sequenceList = new Dictionary<SequenceType, ISequence>();

        public void SetSequence(SequenceType type, ISequence sequence)
        {
            sequenceList[type] = sequence;
        }

        public byte[] GetCurrentSequence(SequenceType type)
        {
            return sequenceList[type].CurrentBytes;
        }

        public byte[] GetNextSequence(SequenceType type)
        {
            return sequenceList[type].NextBytes;
        }
    }
}
