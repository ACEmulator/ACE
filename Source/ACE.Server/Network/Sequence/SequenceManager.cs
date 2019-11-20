using System.Collections.Generic;

using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Server.Network.Sequence
{
    public class SequenceManager
    {
        private readonly Dictionary<uint, ISequence> sequenceList = new Dictionary<uint, ISequence>();

        public byte[] GetCurrentSequence(SequenceType type)
        {
            return GetSequence(type, 0).CurrentBytes;
        }

        public byte[] GetNextSequence(SequenceType type)
        {
            return GetSequence(type, 0).NextBytes;
        }

        public byte[] GetCurrentSequence(SequenceType type, uint property)
        {
            return GetSequence(type, property).CurrentBytes;
        }

        public byte[] GetNextSequence(SequenceType type, uint property)
        {
            return GetSequence(type, property).NextBytes;
        }

        public byte[] GetCurrentSequence(SequenceType type, PropertyAttribute property)
        {
            return GetSequence(type, (uint)property).CurrentBytes;
        }

        public byte[] GetNextSequence(SequenceType type, PropertyAttribute property)
        {
            return GetSequence(type, (uint)property).NextBytes;
        }

        public byte[] GetCurrentSequence(SequenceType type, PropertyAttribute2nd property)
        {
            return GetSequence(type, (uint)property).CurrentBytes;
        }

        public byte[] GetNextSequence(SequenceType type, PropertyAttribute2nd property)
        {
            return GetSequence(type, (uint)property).NextBytes;
        }

        public byte[] GetCurrentSequence(SequenceType type, PropertyBool property)
        {
            return GetSequence(type, (uint)property).CurrentBytes;
        }

        public byte[] GetNextSequence(SequenceType type, PropertyBool property)
        {
            return GetSequence(type, (uint)property).NextBytes;
        }

        public byte[] GetCurrentSequence(SequenceType type, PropertyDataId property)
        {
            return GetSequence(type, (uint)property).CurrentBytes;
        }

        public byte[] GetNextSequence(SequenceType type, PropertyDataId property)
        {
            return GetSequence(type, (uint)property).NextBytes;
        }

        public byte[] GetNextSequence(SequenceType type, PropertyFloat property)
        {
            return GetSequence(type, (uint)property).NextBytes;
        }

        public byte[] GetCurrentSequence(SequenceType type, PropertyFloat property)
        {
            return GetSequence(type, (uint)property).CurrentBytes;
        }

        public byte[] GetNextSequence(SequenceType type, PropertyInstanceId property)
        {
            return GetSequence(type, (uint)property).NextBytes;
        }

        public byte[] GetCurrentSequence(SequenceType type, PropertyInstanceId property)
        {
            return GetSequence(type, (uint)property).CurrentBytes;
        }

        public byte[] GetCurrentSequence(SequenceType type, PropertyInt property)
        {
            return GetSequence(type, (uint)property).CurrentBytes;
        }

        public byte[] GetNextSequence(SequenceType type, PropertyInt property)
        {
            return GetSequence(type, (uint)property).NextBytes;
        }

        public byte[] GetCurrentSequence(SequenceType type, PropertyInt64 property)
        {
            return GetSequence(type, (uint)property).CurrentBytes;
        }

        public byte[] GetNextSequence(SequenceType type, PropertyInt64 property)
        {
            return GetSequence(type, (uint)property).NextBytes;
        }

        public byte[] GetCurrentSequence(SequenceType type, PropertyString property)
        {
            return GetSequence(type, (uint)property).CurrentBytes;
        }

        public byte[] GetNextSequence(SequenceType type, PropertyString property)
        {
            return GetSequence(type, (uint)property).NextBytes;
        }

        public byte[] GetCurrentSequence(SequenceType type, PositionType positionType)
        {
            return GetSequence(type, (uint)positionType).CurrentBytes;
        }

        public byte[] GetNextSequence(SequenceType type, PositionType positionType)
        {
            return GetSequence(type, (uint)positionType).NextBytes;
        }

        public byte[] GetCurrentSequence(SequenceType type, Skill skill)
        {
            return GetSequence(type, (uint)skill).CurrentBytes;
        }

        public byte[] GetNextSequence(SequenceType type, Skill skill)
        {
            return GetSequence(type, (uint)skill).NextBytes;
        }

        public byte[] GetCurrentSequence(SequenceType type, Vital vital)
        {
            return GetSequence(type, (uint)vital).CurrentBytes;
        }

        public byte[] GetNextSequence(SequenceType type, Vital vital)
        {
            return GetSequence(type, (uint)vital).NextBytes;
        }

        public ISequence GetSequence(SequenceType type, uint property)
        {
            var key = (uint)type << 16 | property;

            lock (sequenceList)
            {
                if (!sequenceList.TryGetValue(key, out var sequence))
                {
                    switch (type)
                    {
                        case SequenceType.ObjectPosition:
                        case SequenceType.ObjectMovement:
                        case SequenceType.ObjectState:
                        case SequenceType.ObjectVector:
                        case SequenceType.ObjectTeleport:
                        case SequenceType.ObjectServerControl:
                        case SequenceType.ObjectForcePosition:
                        case SequenceType.ObjectVisualDesc:
                        case SequenceType.ObjectInstance:
                            sequence = new UShortSequence();
                            break;

                        case SequenceType.Motion:
                            sequence = new UShortSequence(1, 0x7FFF); // MSB is reserved, so set max value to exclude it.
                            break;

                        default:
                            sequence = new ByteSequence(false);
                            break;
                    }

                    sequenceList.Add(key, sequence);
                }

                return sequence;
            }
        }

        public void SetSequence(SequenceType type, ISequence sequence)
        {
            var key = (uint)type << 16;

            lock (sequenceList)
                sequenceList[key] = sequence;
        }
    }
}
