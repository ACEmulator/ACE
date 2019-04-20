using System;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    public class GeneratorRegistryNode
    {
        public WorldObject WorldObject;

        public uint WeenieClassId;
        public DateTime Timestamp;
        public uint TreasureType;
        public uint Checkpointed;
        public uint Shop;
        public uint Amount;
    }
}
