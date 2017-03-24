using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network.Sequence
{
    public class SequenceManager
    {
        private Dictionary<SequenceType, ISequence> sequenceList = new Dictionary<SequenceType, ISequence>();

        public void AddSequence(SequenceType type, ISequence sequence)
        {
            if (!sequenceList.ContainsKey(type))
            {
                sequenceList.Add(type, sequence);
            }
        }

        public byte[] GetCurrentSequence(SequenceType type)
        {
            if (!sequenceList.ContainsKey(type))
                throw new ArgumentOutOfRangeException("type");
            return sequenceList[type].CurrentValue;
        }

        public byte[] GetNextSequence(SequenceType type)
        {
            if (!sequenceList.ContainsKey(type))
                throw new ArgumentOutOfRangeException("type");
            return sequenceList[type].NextValue;
        }
    }
}
