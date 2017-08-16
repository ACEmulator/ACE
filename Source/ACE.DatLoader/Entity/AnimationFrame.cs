using ACE.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class AnimationFrame
    {
        public List<Position> Locations { get; set; } = new List<Position>();
        public List<AnimationHook> Hooks { get; set; } = new List<AnimationHook>();

        public static AnimationFrame Read(uint numParts, DatReader datReader)
        {
            AnimationFrame a = new AnimationFrame();

            for (uint i = 0; i < numParts; i++)
            {
                Position p = new Position();
                // Origin
                p.PositionX = datReader.ReadUInt32();
                p.PositionY = datReader.ReadUInt32();
                p.PositionZ = datReader.ReadUInt32();
                p.RotationW = datReader.ReadUInt32();
                p.RotationX = datReader.ReadUInt32();
                p.RotationY = datReader.ReadUInt32();
                p.RotationZ = datReader.ReadUInt32();
                a.Locations.Add(p);
            }

            uint numHooks = datReader.ReadUInt32();
            for (uint i = 0; i < numHooks; i++)
            {
                a.Hooks.Add(AnimationHook.Read(datReader));
            }

            return a;
        }
    }
}
