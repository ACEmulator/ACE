using System;
using System.Collections.Generic;
using System.IO;

using ACE.Entity;

namespace ACE.DatLoader.Entity
{
    public class AnimationFrame : IUnpackable
    {
        public List<Position> Locations { get; } = new List<Position>();
        public List<AnimationHook> Hooks { get; } = new List<AnimationHook>();

        /// <summary>
        /// You must use the Unpack(BinaryReader reader, int numParts) method.
        /// </summary>
        /// <exception cref="NotSupportedException">You must use the Unpack(BinaryReader reader, int numParts) method.</exception>
        public void Unpack(BinaryReader reader)
        {
            throw new NotSupportedException();
        }

        public void Unpack(BinaryReader reader, uint numParts)
        {
            for (uint i = 0; i < numParts; i++)
            {
                Position position = new Position();
                position.ReadFrame(reader);
                Locations.Add(position);
            }

            uint numHooks = reader.ReadUInt32();
            for (uint i = 0; i < numHooks; i++)
            {
                var hook = AnimationHook.ReadHook(reader);
                Hooks.Add(hook);
            }
        }
    }
}
