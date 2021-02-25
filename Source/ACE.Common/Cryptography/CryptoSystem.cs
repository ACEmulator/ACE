using System;
using System.Collections.Generic;

namespace ACE.Common.Cryptography
{
    public class CryptoSystem : ISAAC
    {
        public const int MaximumEffortLevel = 256;
        public HashSet<uint> xors = new HashSet<uint>();
        public uint CurrentKey;
        public CryptoSystem(uint seed) : base(BitConverter.GetBytes(seed))
        {
            CurrentKey = Next();
        }
        public CryptoSystem(byte[] seed) : base(seed)
        {
            CurrentKey = Next();
        }
        public void ConsumeKey(uint x)
        {
            if (CurrentKey == x)
            {
                CurrentKey = Next();
            }
            else
            {
                xors.Remove(x);
            }
        }
        public bool Search(uint x)
        {
            if (CurrentKey == x)
            {
                return true;
            }
            if (xors.Contains(x))
            {
                return true;
            }
            int g = xors.Count;
            for (int i = 0; i < MaximumEffortLevel - g; i++)
            {
                xors.Add(CurrentKey);
                ConsumeKey(CurrentKey);
                if (CurrentKey == x)
                    return true;
            }
            return false;
        }
        public new void ReleaseResources()
        {
            xors?.Clear();
            xors = null;
            base.ReleaseResources();
        }
    }
}
