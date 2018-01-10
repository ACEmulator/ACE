using System;
using System.IO;
using ACE.Entity.Enum;
using ACE.Common;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ACE.Entity
{
    public class Position : ICloneable
    {
        [JsonProperty("landblockId")]
        private LandblockId landblockId;

        [JsonIgnore]
        public LandblockId LandblockId
        {
            get { return landblockId.Raw != 0 ? landblockId : new LandblockId(Cell); }
            set
            {
                landblockId = value;
            }
        }

        // TODO: This is just named wrong needs to be fixed.
        [JsonIgnore]
        public uint Cell { get; set; }

        [JsonProperty("positionX")]
        public float PositionX { get; set; }

        [JsonProperty("positionY")]
        public float PositionY { get; set; }

        [JsonProperty("positionZ")]
        public float PositionZ { get; set; }

        [JsonProperty("rotationW")]
        public float RotationW { get; set; }

        [JsonProperty("rotationX")]
        public float RotationX { get; set; }

        [JsonProperty("rotationY")]
        public float RotationY { get; set; }

        [JsonProperty("rotationZ")]
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

            if (newCell.ToString("X8").EndsWith("0000"))
                CalculateObjCell(newCell);
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

        public void Serialize(BinaryWriter payload, UpdatePositionFlag updatePositionFlags, int animationFrame, bool writeLandblock = true)
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
                payload.Write(animationFrame);
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

        private float GetZFromCellXy(uint cell, float xOffset, float yOffset)
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

        public string ToLOCString()
        {
            return $"0x{LandblockId.Raw:X} [{PositionX} {PositionY} {PositionZ}] {RotationW} {RotationX} {RotationY} {RotationZ}";
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

        // Thank you fellow traveller!
        private const uint blockid_mask = 0xFFFF0000;
        private const uint lbi_cell_id = 0x0000FFFE;
        private const uint block_cell_id = 0x0000FFFF;
        private const uint first_envcell_id = 0x100;
        private const uint last_envcell_id = 0xFFFD;
        private const uint first_lcell_id = 1;
        private const uint last_lcell_id = 64;
        private const long max_block_width = 0xFF;
        private const uint max_block_shift = 8;
        private const uint blockx_mask = 0xFF00;
        private const uint blocky_mask = 0x00FF;
        private const uint block_part_shift = 16;
        private const uint cellid_mask = 0x0000FFFF;
        private const uint terrain_byte_offset = 2;

        // Initialized later on.
        private const long side_vertex_count = 9;
        private const float half_square_length = 12.0f;
        private const float square_length = 24.0f;
        private const float block_length = 192.0f;
        private const long lblock_shift = 3;
        private const long lblock_side = 8;
        private const long lblock_mask = 7;
        private const long land_width = 0x7F8;
        private const long land_length = 0x7F8;
        private const long num_block_length = 0xFF;
        private const long num_block_width = 0xFF;
        private const long num_blocks = 0xFF * 0xFF;
        ////private const float inside_val; // ? ....
        ////private const float outside_val;
        ////private const float max_object_height;
        ////private const float road_width;
        ////private const float sky_height;
        private const long vertex_per_cell = 1;
        private const long polys_per_landcell = 2;

        public void CalculateObjCell(uint newCell)
        {          
            float X = (((((int)newCell >> (int)block_part_shift) & (int)blockx_mask) >> (int)max_block_shift) << (int)lblock_shift);
            float Y = ((((int)newCell >> (int)block_part_shift) & (int)blocky_mask) << (int)lblock_shift);

            X += PositionX / square_length;
            Y += PositionY / square_length;

            Cell = GetCellFromBase((uint)X, (uint)Y);
            LandblockId = new LandblockId(Cell);
            // System.Diagnostics.Debug.WriteLine($"Cell came in as {newCell.ToString("X8")}, should be {Cell.ToString("X8")} ");
        }
    }
}
