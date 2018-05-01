using System;
using System.IO;
using System.Numerics;
using ACE.Entity.Enum;

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
            get => landblockId.Raw != 0 ? landblockId : new LandblockId(Cell);
            set
            {
                landblockId = value;
            }
        }

        // TODO: This is just named wrong needs to be fixed.
        [JsonIgnore]
        public uint Cell { get; set; }

        public Vector3 Pos
        {
            get
            {
                return new Vector3(PositionX, PositionY, PositionZ);
            }
            set
            {
                SetPosition(value);
            }
        }

        public bool SetPosition(Vector3 pos)
        {
            PositionX = pos.X;
            PositionY = pos.Y;
            PositionZ = pos.Z;

            return SetLandblock();
        }

        public Quaternion Rotation
        {
            get
            {
                return new Quaternion(RotationX, RotationY, RotationZ, RotationW);
            }
            set
            {
                RotationW = value.W;
                RotationX = value.X;
                RotationY = value.Y;
                RotationZ = value.Z;
            }
        }

        public Vector3 GlobalPos
        {
            get
            {
                return ToGlobal();
            }
        }

        public void Rotate(Vector3 dir)
        {
            Rotation = Quaternion.CreateFromYawPitchRoll(0, 0, (float)Math.Atan2(dir.Y, dir.X)) * Quaternion.CreateFromYawPitchRoll(0, 0, -(float)Math.PI / 2.0f);
        }

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

        public bool Indoors => landblockId.MapScope != MapScope.Outdoors;

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

        /// <summary>
        /// Returns the normalized 2D heading direction
        /// </summary>
        public Vector3 GetCurrentDir()
        {
            return Vector3.Normalize(Vector3.Transform(Vector3.UnitY, Rotation));
        }

        /// <summary>
        /// Returns this vector as a unit vector
        /// with a length of 1
        /// </summary>
        public Vector3 Normalize(Vector3 v)
        {
            var invLen = 1.0f / v.Length();
            return v * invLen;
        }

        public Position InFrontOf(double distanceInFront = 3.0f, bool rotate180 = false)
        {
            float qw = RotationW; // north
            float qz = RotationZ; // south

            double x = 2 * qw * qz;
            double y = 1 - 2 * qz * qz;

            var heading = Math.Atan2(x, y);
            var dx = -1 * Convert.ToSingle(Math.Sin(heading) * distanceInFront);
            var dy = Convert.ToSingle(Math.Cos(heading) * distanceInFront);

            // move the Z slightly up and let gravity pull it down.  just makes things easier.
            if (rotate180)
            {
                var rotate = new Quaternion(0, 0, qz, qw) * Quaternion.CreateFromYawPitchRoll(0, 0, (float)Math.PI);
                return new Position(LandblockId.Raw, PositionX + dx, PositionY + dy, PositionZ + 0.5f, 0f, 0f, rotate.Z, rotate.W);
            }
            else
                return new Position(LandblockId.Raw, PositionX + dx, PositionY + dy, PositionZ + 0.5f, 0f, 0f, qz, qw);
        }

        /// <summary>
        /// Handles the Position crossing over landblock boundaries
        /// </summary>
        public bool SetLandblock()
        {
            var changedBlock = false;

            if (PositionX < 0)
            {
                var blockOffset = (int)PositionX / BlockLength - 1;
                if (LandblockId.TransitionX(blockOffset))
                {
                    PositionX += BlockLength * blockOffset;
                    changedBlock = true;
                }
                else
                    PositionX = 0;
            }

            if (PositionX > BlockLength)
            {
                var blockOffset = (int)PositionX / BlockLength;
                if (LandblockId.TransitionX(blockOffset))
                {
                    PositionX -= BlockLength * blockOffset;
                    changedBlock = true;
                }
                else
                    PositionX = BlockLength;
            }

            if (PositionY < 0)
            {
                var blockOffset = (int)PositionY / BlockLength - 1;
                if (LandblockId.TransitionY(blockOffset))
                {
                    PositionY += BlockLength * blockOffset;
                    changedBlock = true;
                }
                else
                    PositionY = 0;
            }

            if (PositionY > BlockLength)
            {
                var blockOffset = (int)PositionY / BlockLength;
                if (LandblockId.TransitionY(blockOffset))
                {
                    PositionY -= BlockLength * blockOffset;
                    changedBlock = true;
                }
                else
                    PositionY = BlockLength;
            }
            return changedBlock;
        }

        public Position() { }

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
            Cell = LandblockId.Raw;
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
        /// Returns the 3D squared distance between 2 objects
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
            //if (p.LandblockId.MapScope == MapScope.Outdoors && this.LandblockId.MapScope == MapScope.Outdoors)
            else
            {
                // verify this is working correctly if one of these is indoors
                var dx = (this.LandblockId.LandblockX - p.LandblockId.LandblockX) * 192 + this.PositionX - p.PositionX;
                var dy = (this.LandblockId.LandblockY - p.LandblockId.LandblockY) * 192 + this.PositionY - p.PositionY;
                var dz = this.PositionZ - p.PositionZ;
                return dx * dx + dy * dy + dz * dz;
            }
        }

        /// <summary>
        /// Returns the 2D distance between 2 objects
        /// </summary>
        public float Distance2D(Position p)
        {
            // originally this returned the offset instead of distance...
            if (p.LandblockId == this.LandblockId)
            {
                var dx = this.PositionX - p.PositionX;
                var dy = this.PositionY - p.PositionY;
                return (float)Math.Sqrt(dx * dx + dy * dy);
            }
            //if (p.LandblockId.MapScope == MapScope.Outdoors && this.LandblockId.MapScope == MapScope.Outdoors)
            else
            {
                // verify this is working correctly if one of these is indoors
                var dx = (this.LandblockId.LandblockX - p.LandblockId.LandblockX) * 192 + this.PositionX - p.PositionX;
                var dy = (this.LandblockId.LandblockY - p.LandblockId.LandblockY) * 192 + this.PositionY - p.PositionY;
                return (float)Math.Sqrt(dx * dx + dy * dy);
            }
        }

        /// <summary>
        /// Returns the 3D distance between 2 objects
        /// </summary>
        public float DistanceTo(Position p)
        {
            // originally this returned the offset instead of distance...
            if (p.LandblockId == this.LandblockId)
            {
                var dx = this.PositionX - p.PositionX;
                var dy = this.PositionY - p.PositionY;
                var dz = this.PositionZ - p.PositionZ;
                return (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
            }
            //if (p.LandblockId.MapScope == MapScope.Outdoors && this.LandblockId.MapScope == MapScope.Outdoors)
            else
            {
                // verify this is working correctly if one of these is indoors
                var dx = (this.LandblockId.LandblockX - p.LandblockId.LandblockX) * 192 + this.PositionX - p.PositionX;
                var dy = (this.LandblockId.LandblockY - p.LandblockId.LandblockY) * 192 + this.PositionY - p.PositionY;
                var dz = this.PositionZ - p.PositionZ;

                return (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
            }
        }

        /// <summary>
        /// Returns the offset from current position to input position
        /// </summary>
        public Vector3 GetOffset(Position p)
        {
            var dx = (p.LandblockId.LandblockX - LandblockId.LandblockX) * 192 + p.PositionX - PositionX;
            var dy = (p.LandblockId.LandblockY - LandblockId.LandblockY) * 192 + p.PositionY - PositionY;
            var dz = p.PositionZ - PositionZ;

            return new Vector3(dx, dy, dz);
        }

        public override string ToString()
        {
            return $"{LandblockId.Landblock:X}: {PositionX} {PositionY} {PositionZ}";
        }

        public string ToLOCString()
        {
            return $"0x{LandblockId.Raw:X} [{PositionX} {PositionY} {PositionZ}] {RotationW} {RotationX} {RotationY} {RotationZ}";
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

        public static readonly int BlockLength = 192;

        public Vector3 ToGlobal()
        {
            var x = LandblockId.LandblockX * BlockLength + PositionX;
            var y = LandblockId.LandblockY * BlockLength + PositionY;
            var z = PositionZ;

            return new Vector3(x, y, z);
        }

        public static Position FromGlobal(Vector3 pos)
        {
            var blockX = (uint)pos.X / BlockLength;
            var blockY = (uint)pos.Y / BlockLength;

            var localX = pos.X % BlockLength;
            var localY = pos.Y % BlockLength;

            var landblockID = blockX << 24 | blockY << 16 | 0xFFFF;

            var position = new Position();
            position.LandblockId = new LandblockId((byte)blockX, (byte)blockY);
            position.PositionX = localX;
            position.PositionY = localY;
            position.PositionZ = pos.Z;
            return position;
        }
    }
}
