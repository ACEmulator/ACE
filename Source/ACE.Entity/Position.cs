using System;
using System.IO;
using ACE.Entity.Enum;
using ACE.Common;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace ACE.Entity
{
    public class Position : ICloneable
    {
        private LandblockId landblockId;

        public LandblockId LandblockId
        {
            get { return landblockId.Raw != 0 ? landblockId : new LandblockId(Cell); }
            set
            {
                landblockId = value;
            }
        }

        // TODO: This is just named wrong needs to be fixed.
        public uint Cell { get; set; }

        public float PositionX { get; set; }

        public float PositionY { get; set; }

        public float PositionZ { get; set; }

        public float RotationW { get; set; }

        public float RotationX { get; set; }

        public float RotationY { get; set; }

        public float RotationZ { get; set; }

        private const float xyMidPoint = 96f;

        public bool Indoors
        {
            get { return landblockId.MapScope != MapScope.Outdoors; }
        }

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
            return new Position(LandblockId.Raw, PositionX + dx, PositionY + dy, PositionZ + 0.5f, 0f, 0f, qz, qw);
            // return new Position(LandblockId.Raw, PositionX + dx, PositionY + dy, GetZFromCellXy(Cell, PositionX + dx, PositionY + dy), 0f, 0f, qz, qw);
            // return new Position(LandblockId.Raw, PositionX + dx, PositionY + dy, 0, 0f, 0f, qz, qw);
        }

        public Position() : base() {
        }

        public Position(uint newCell, float newPositionX, float newPositionY, float newPositionZ, float newRotationX, float newRotationY, float newRotationZ, float newRotationW)
        {
            LandblockId = new LandblockId(newCell);
            Cell = newCell;
            PositionX = newPositionX;
            PositionY = newPositionY;
            PositionZ = newPositionZ;
            RotationX = newRotationX;
            RotationY = newRotationY;
            RotationZ = newRotationZ;
            RotationW = newRotationW;
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

            if (baseX >= 0x7F8 || baseY >= 0x7F8)
                throw new Exception("Bad coordinates");  // TODO: Instead of throwing exception should we set to a default location?

            float xOffset = ((baseX & 7) * 24.0f) + 12;
            float yOffset = ((baseY & 7) * 24.0f) + 12;
            // float zOffset = GetZFromCellXY(LandblockId.Raw, xOffset, yOffset);
            const float zOffset = 0.0f;

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

        public Position(AceObjectPropertiesPosition aoPos)
        {
            Cell = aoPos.Cell;
            LandblockId = new LandblockId(Cell);
            PositionX = aoPos.PositionX;
            PositionY = aoPos.PositionY;
            PositionZ = aoPos.PositionZ;
            RotationW = aoPos.RotationW;
            RotationX = aoPos.RotationX;
            RotationY = aoPos.RotationY;
            RotationZ = aoPos.RotationZ;
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

        private List<ushort> Height { get; set; } = new List<ushort>();

        private class Point3d
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float Z { get; set; }
        }

        private float GetPointOnPlane(Point3d p1, Point3d p2, Point3d p3, float x, float y)
        {
            Point3d v1 = new Point3d();
            Point3d v2 = new Point3d();
            Point3d abc = new Point3d();

            v1.X = p1.X - p3.X;
            v1.Y = p1.Y - p3.Y;
            v1.Z = p1.Z - p3.Z;

            v2.X = p2.X - p3.X;
            v2.Y = p2.Y - p3.Y;
            v2.Z = p2.Z - p3.Z;

            abc.X = (v1.Y * v2.Z) - (v1.Z * v2.Y);
            abc.Y = (v1.Z * v2.X) - (v1.X * v2.Z);
            abc.Z = (v1.X * v2.Y) - (v1.Y * v2.X);

            float d = (abc.X * p3.X) + (abc.Y * p3.Y) + (abc.Z * p3.Z);

            float z = (d - (abc.X * x) - (abc.Y * y)) / abc.Z;

            return z;
        }

        private float GetZFromCellXy(uint cell, float xOffset, float yOffset)
        {
            // TODO: Load correct z from file
            ////uint tileX = (uint)Math.Ceiling(xOffset / 24) - 1; // Subract 1 to 0-index these
            ////uint tileY = (uint)Math.Ceiling(yOffset / 24) - 1; // Subract 1 to 0-index these

            ////uint v1 = tileX * 9 + tileY;
            ////uint v2 = tileX * 9 + tileY + 1;
            ////uint v3 = (tileX + 1) * 9 + tileY;

            ////Point3d p1 = new Point3d();
            ////p1.X = tileX * 24;
            ////p1.Y = tileY * 24;
            ////p1.Z = Height[(int)v1] * 2;

            ////Point3d p2 = new Point3d();
            ////p2.X = tileX * 24;
            ////p2.Y = (tileY + 1) * 24;
            ////p2.Z = Height[(int)v2] * 2;

            ////Point3d p3 = new Point3d();
            ////p3.X = (tileX + 1) * 24;
            ////p3.Y = tileY * 24;
            ////p3.Z = Height[(int)v3] * 2;

            ////float z = GetPointOnPlane(p1, p2, p3, xOffset, yOffset);
            ////return z;

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

            if (p.LandblockId.MapScope == MapScope.Outdoors && this.LandblockId.MapScope == MapScope.Outdoors)
            {
                var dx = (this.LandblockId.LandblockX - p.LandblockId.LandblockX) * 192 + this.PositionX - p.PositionX;
                var dy = (this.LandblockId.LandblockY - p.LandblockId.LandblockY) * 192 + this.PositionY - p.PositionY;
                var dz = this.PositionZ - p.PositionZ;
                return dx * dx + dy * dy + dz * dz;
            }
            return float.NaN;
        }

        public override string ToString()
        {
            return $"{LandblockId.Landblock:X}: {PositionX} {PositionY} {PositionZ}";
        }

        public AceObjectPropertiesPosition GetAceObjectPosition(ObjectGuid guid, PositionType type)
        {
            return GetAceObjectPosition(guid.Full, type);
        }

        public AceObjectPropertiesPosition GetAceObjectPosition(uint guid, PositionType type)
        {
            AceObjectPropertiesPosition ret = new AceObjectPropertiesPosition();
            ret.AceObjectId = guid;
            ret.DbPositionType = (ushort)type;
            ret.PositionId = 0;
            ret.Cell = Cell;
            ret.PositionX = PositionX;
            ret.PositionY = PositionY;
            ret.PositionZ = PositionZ;
            ret.RotationW = RotationW;
            ret.RotationX = RotationX;
            ret.RotationY = RotationY;
            ret.RotationZ = RotationZ;

            return ret;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
