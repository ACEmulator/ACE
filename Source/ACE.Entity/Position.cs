﻿namespace ACE.Entity
{
    using System;
    using System.IO;
    using Enum;
    using Common;
    using MySql.Data.MySqlClient;

    [DbTable("character_position")]
    [DbGetList("vw_character_positions", 23, "character_id")]
    public class Position
    {
        public LandblockId LandblockId { get; set; }

        [DbField("character_id", (int)MySqlDbType.UInt32, Update = false, IsCriteria = true)]
        public virtual uint CharacterId { get; set; }

        [DbField("cell", (int)MySqlDbType.UInt32)]
        public uint Cell { get; set; }

        [DbField("positionType", (int)MySqlDbType.UInt32, Update = false, IsCriteria = true)]
        public virtual uint DbPositionType { get; set; }

        public PositionType PositionType
        {
            get
            {
                return (PositionType)DbPositionType;
            }
            set
            {
                DbPositionType = (uint)value;
            }
        }

        [DbField("positionX", (int)MySqlDbType.Float)]
        public float PositionX { get; set; }

        [DbField("positionY", (int)MySqlDbType.Float)]
        public float PositionY { get; set; }

        [DbField("positionZ", (int)MySqlDbType.Float)]
        public float PositionZ { get; set; }

        [DbField("rotationX", (int)MySqlDbType.Float)]
        public float RotationX { get; set; }

        [DbField("rotationY", (int)MySqlDbType.Float)]
        public float RotationY { get; set; }

        [DbField("rotationZ", (int)MySqlDbType.Float)]
        public float RotationZ { get; set; }

        [DbField("rotationW", (int)MySqlDbType.Float)]
        public float RotationW { get; set; }

        private const float xyMidPoint = 96f;

        public bool IsInQuadrant(Quadrant q)
        {
            // check for easy short circuit
            if (q == Quadrant.All)
                return true;

            if ((q & Quadrant.NorthEast) > 0 && PositionX > xyMidPoint && PositionY > xyMidPoint)
                return true;

            if ((q & Quadrant.NorthWest) > 0 && PositionX <= xyMidPoint && PositionY > xyMidPoint)
                return true;

            if ((q & Quadrant.SouthEast) > 0 && PositionX <= xyMidPoint && PositionY <= xyMidPoint)
                return true;

            if ((q & Quadrant.SouthWest) > 0 && PositionX <= xyMidPoint && PositionY <= xyMidPoint)
                return true;

            return false;
        }
        public Position InFrontOf(double distanceInFront = 3.0f)
        {
            float qw = RotationW; // north
            float qz = RotationZ; // south

            double x = 2 * qw * qz;
            double y = 1 - 2 * qz * qz;

            var heading = Math.Atan2(x, y);
            var dx = -1 * Convert.ToSingle(Math.Sin(heading) * distanceInFront);
            var dy = Convert.ToSingle(Math.Cos(heading) * distanceInFront);

            // move the Z slightly up and let gravity pull it down.  just makes things easier.
            return new Position(LandblockId.Raw, PositionX + dx, PositionY + dy, PositionZ + 0.5f, 0f, 0f, 0f, 0f);
        }

        public Position() : base() {
        }

        public Position(uint characterId, PositionType type, uint newCell, float newPositionX, float newPositionY, float newPositionZ, float newRotationX, float newRotationY, float newRotationZ, float newRotationW)
        {
            LandblockId = new LandblockId(Cell);
            
            CharacterId = characterId;
            PositionType = type;
            Cell = newCell;
            PositionX = newPositionX;
            PositionY = newPositionY;
            PositionZ = newPositionZ;
            RotationX = newRotationX;
            RotationY = newRotationY;
            RotationZ = newRotationZ;
            RotationW = newRotationW;
        }

        public Position(uint landblock, float x, float y, float z, float qx = 0.0f, float qy = 0.0f, float qz = 0.0f, float qw = 0.0f)
        {
            LandblockId = new LandblockId(landblock);
            Cell = LandblockId.Raw;
            PositionX = x;
            PositionY = y;
            PositionZ = z;
            RotationX = qx;
            RotationY = qy;
            RotationZ = qz;
            RotationW = qw;
        }

        public Position(BinaryReader payload)
        {
            LandblockId = new LandblockId(payload.ReadUInt32());
            Cell = LandblockId.Raw;
            // Offset  = new Vector3(payload.ReadSingle(), payload.ReadSingle(), payload.ReadSingle());
            PositionX = payload.ReadSingle();
            PositionY = payload.ReadSingle();
            PositionZ = payload.ReadSingle();
            // float qw = payload.ReadSingle();
            RotationW = payload.ReadSingle();
            // Facing  = new Quaternion(payload.ReadSingle(), payload.ReadSingle(), payload.ReadSingle(), qw);
            RotationX = payload.ReadSingle();
            RotationY = payload.ReadSingle();
            RotationZ = payload.ReadSingle();
            // packet stream isn't the same order as the quaternion constructor
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
            float zOffset = GetZFromCellXY(LandblockId.Raw, xOffset, yOffset);

            LandblockId = new LandblockId(GetCellFromBase(baseX, baseY));
            // Offset 
            PositionX = xOffset;
            PositionY = yOffset;
            PositionZ = zOffset;
            // Facing 
            RotationX = 0.0f;
            RotationY = 0.0f;
            RotationZ = 0.0f;
            RotationW = 1.0f;
        }

        public void Serialize(BinaryWriter payload, UpdatePositionFlag updatePositionFlags, bool writeLandblock = true)
        {
            payload.Write((uint)updatePositionFlags);

            if (writeLandblock)
                payload.Write(LandblockId.Raw);

            payload.Write(PositionX);
            payload.Write(PositionY);
            payload.Write(PositionZ);

            if ((updatePositionFlags & UpdatePositionFlag.ZeroQw) == 0)
            {
                payload.Write(RotationW);
            }

            if ((updatePositionFlags & UpdatePositionFlag.ZeroQx) == 0)
            {
                payload.Write(RotationX);
            }

            if ((updatePositionFlags & UpdatePositionFlag.ZeroQy) == 0)
            {
                payload.Write(RotationY);
            }

            if ((updatePositionFlags & UpdatePositionFlag.ZeroQz) == 0)
            {
                payload.Write(RotationZ);
            }

            if ((updatePositionFlags & UpdatePositionFlag.Placement) != 0)
            {
                // TODO: this is current animationframe_id when we are animating (?) - when we are not, how are we setting on the ground Position_id.
                payload.Write((uint)0x65);
            }
            else
            {
                payload.Write((uint)0);
            }

            if ((updatePositionFlags & UpdatePositionFlag.Velocity) != 0)
            {
                // velocity would go here
                payload.Write((float)0f);
                payload.Write((float)0f);
                payload.Write((float)0f);
            }
        }

        public void Serialize(BinaryWriter payload, bool writeQuaternion = true, bool writeLandblock = true)
        {
            if (writeLandblock)
                payload.Write(LandblockId.Raw);

            payload.Write(PositionX);
            payload.Write(PositionY);
            payload.Write(PositionZ);

            if (writeQuaternion)
            {
                payload.Write(RotationW);
                payload.Write(RotationX);
                payload.Write(RotationY);
                payload.Write(RotationZ);
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

        /// <summary>
        /// calculates the square of the distance to the referenced position
        /// </summary>
        public float SquaredDistanceTo(Position p)
        {
            if (p.LandblockId == this.LandblockId)
            {
                var dx = this.PositionX - p.PositionX;
                var dy = this.PositionY - p.PositionY;
                var dz = this.PositionZ - p.PositionZ;
                return dx * dx + dy * dy + dz * dz;
            }
            else if (p.LandblockId.MapScope == MapScope.Outdoors && this.LandblockId.MapScope == MapScope.Outdoors)
            {
                var dx = (this.LandblockId.LandblockX - p.LandblockId.LandblockX) * 192 + this.PositionX - p.PositionX;
                var dy = (this.LandblockId.LandblockY - p.LandblockId.LandblockY) * 192 + this.PositionY - p.PositionY;
                var dz = this.PositionZ - p.PositionZ;
                return dx * dx + dy * dy + dz * dz;
            }
            else
            {
                return float.NaN;
            }
        }

        public override string ToString()
        {
            return $"{LandblockId.Landblock.ToString("X")}: {PositionX} {PositionY} {PositionZ}";
        }
    }
}
