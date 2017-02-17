using System;
using System.IO;
using System.Numerics;

namespace ACE.Entity
{
    public class Position
    {
        public uint Cell { get; set; }
        public uint Dungeon { get; set; }
        public Vector3 Offset { get; set; }
        public Quaternion Facing { get; set; }

        public Position(uint cell, float x, float y, float z, float qx = 0.0f, float qy = 0.0f, float qz = 0.0f, float qw = 0.0f)
        {
            Cell    = cell;
            Dungeon = cell >> 16;
            Offset  = new Vector3(x, y, z);
            Facing  = new Quaternion(qx, qy, qz, qw);
        }

        public Position(BinaryReader payload)
        {
            Cell    = payload.ReadUInt32();
            Dungeon = Cell >> 16;
            Offset  = new Vector3(payload.ReadSingle(), payload.ReadSingle(), payload.ReadSingle());

            // packet stream isn't the same order as the quaternion constructor
            float qw = payload.ReadSingle();
            Facing  = new Quaternion(payload.ReadSingle(), payload.ReadSingle(), payload.ReadSingle(), qw);
        }

        public Position(float northSouth, float eastWest)
        {
            northSouth -= 0.5f;
            eastWest -= 0.5f;
            northSouth *= 10.0f;
            eastWest *= 10.0f;

            uint baseX = (uint)(eastWest + 0x400);
            uint baseY = (uint)(northSouth + 0x400);

            if (baseX < 0 || baseX >= 0x7F8 || baseY < 0 || baseY >= 0x7F8)
                throw new Exception("Bad coordinates");  // TODO: Instead of throwing exception should we set to a default location?

            float xOffset = ((baseX & 7) * 24.0f) + 12;
            float yOffset = ((baseY & 7) * 24.0f) + 12;
            float zOffset = GetZFromCellXY(Cell, xOffset, yOffset);

            Cell = GetCellFromBase(baseX, baseY);
            Offset = new Vector3(xOffset, yOffset, zOffset);
            Facing = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
        }

        public void Write(BinaryWriter payload, bool quaternion = true)
        {
            payload.Write(Cell);
            payload.Write(Offset.X);
            payload.Write(Offset.Y);
            payload.Write(Offset.Z);

            if (quaternion)
            {
                payload.Write(Facing.W);
                payload.Write(Facing.X);
                payload.Write(Facing.Y);
                payload.Write(Facing.Z);
            }
        }

        private float GetZFromCellXY(uint cell, float xOffset, float yOffset)
        {
            // TODO: Load correct z from file
            return 200.0f;
        }

        private uint GetCellFromBase(uint baseX, uint baseY)
        {
            byte blockX = (byte)(baseX >> 3);
            byte blockY = (byte)(baseY >> 3);
            byte cellX = (byte)(baseX & 7);
            byte cellY = (byte)(baseY & 7);

            uint block = (uint)((blockX << 8) | blockY);
            uint cell = (uint)((cellX << 3) | cellY);

            return (block << 16) | (cell + 1);
        }
    }
}
