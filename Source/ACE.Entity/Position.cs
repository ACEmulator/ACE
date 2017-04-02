namespace ACE.Entity
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
        public virtual uint character_id { get; set; }

        [DbField("cell", (int)MySqlDbType.UInt32)]
        public uint cell { get; set; }

        [DbField("positionType", (int)MySqlDbType.UInt32, Update = false, IsCriteria = true)]
        public virtual uint dbpositionType { get; set; }

        public PositionType positionType
        {
            get
            {
                return (PositionType)dbpositionType;
            }
            set
            {
                dbpositionType = (uint)value;
            }
        }

        [DbField("positionX", (int)MySqlDbType.Float)]
        public float positionX { get; set; }

        [DbField("positionY", (int)MySqlDbType.Float)]
        public float positionY { get; set; }

        [DbField("positionZ", (int)MySqlDbType.Float)]
        public float positionZ { get; set; }

        [DbField("rotationX", (int)MySqlDbType.Float)]
        public float rotationX { get; set; }

        [DbField("rotationY", (int)MySqlDbType.Float)]
        public float rotationY { get; set; }

        [DbField("rotationZ", (int)MySqlDbType.Float)]
        public float rotationZ { get; set; }

        [DbField("rotationW", (int)MySqlDbType.Float)]
        public float rotationW { get; set; }

        private const float xyMidPoint = 96f;

        public bool IsInQuadrant(Quadrant q)
        {
            // check for easy short circuit
            if (q == Quadrant.All)
                return true;

            if ((q & Quadrant.NorthEast) > 0 && positionX > xyMidPoint && positionY > xyMidPoint)
                return true;

            if ((q & Quadrant.NorthWest) > 0 && positionX <= xyMidPoint && positionY > xyMidPoint)
                return true;

            if ((q & Quadrant.SouthEast) > 0 && positionX <= xyMidPoint && positionY <= xyMidPoint)
                return true;

            if ((q & Quadrant.SouthWest) > 0 && positionX <= xyMidPoint && positionY <= xyMidPoint)
                return true;

            return false;
        }
        public Position InFrontOf(double distanceInFront = 3.0f)
        {
            float qw = rotationW; // north
            float qz = rotationZ; // south

            double x = 2 * qw * qz;
            double y = 1 - 2 * qz * qz;

            var heading = Math.Atan2(x, y);
            var dx = -1 * Convert.ToSingle(Math.Sin(heading) * distanceInFront);
            var dy = Convert.ToSingle(Math.Cos(heading) * distanceInFront);

            // move the Z slightly up and let gravity pull it down.  just makes things easier.
            return new Position(LandblockId.Raw, positionX + dx, positionY + dy, positionZ + 0.5f, 0f, 0f, 0f, 0f);
        }

        public Position() : base() {

        }

        public Position(uint characterId, PositionType type, uint newCell, float newPositionX, float newPositionY, float newPositionZ, float newRotationX, float newRotationY, float newRotationZ, float newRotationW)
        {
            LandblockId = new LandblockId(cell);
            
            character_id = characterId;
            positionType = type;
            cell = newCell;
            positionX = newPositionX;
            positionY = newPositionY;
            positionZ = newPositionZ;
            rotationX = newRotationX;
            rotationY = newRotationY;
            rotationZ = newRotationZ;
            rotationW = newRotationW;
        }

        public Position(uint landblock, float x, float y, float z, float qx = 0.0f, float qy = 0.0f, float qz = 0.0f, float qw = 0.0f)
        {
            LandblockId = new LandblockId(landblock);
            cell = LandblockId.Raw;
            positionX = x;
            positionY = y;
            positionZ = z;
            rotationX = qx;
            rotationY = qy;
            rotationZ = qz;
            rotationW = qw;
        }

        public Position(BinaryReader payload)
        {
            LandblockId = new LandblockId(payload.ReadUInt32());
            cell = LandblockId.Raw;
            // Offset  = new Vector3(payload.ReadSingle(), payload.ReadSingle(), payload.ReadSingle());
            positionX = payload.ReadSingle();
            positionY = payload.ReadSingle();
            positionZ = payload.ReadSingle();
            // float qw = payload.ReadSingle();
            rotationW = payload.ReadSingle();
            // Facing  = new Quaternion(payload.ReadSingle(), payload.ReadSingle(), payload.ReadSingle(), qw);
            rotationX = payload.ReadSingle();
            rotationY = payload.ReadSingle();
            rotationZ = payload.ReadSingle();
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
            positionX = xOffset;
            positionY = yOffset;
            positionZ = zOffset;
            // Facing 
            rotationX = 0.0f;
            rotationY = 0.0f;
            rotationZ = 0.0f;
            rotationW = 1.0f;
        }

        public void Serialize(BinaryWriter payload, UpdatePositionFlag updatePositionFlags, bool writeLandblock = true)
        {
            payload.Write((uint)updatePositionFlags);

            if (writeLandblock)
                payload.Write(LandblockId.Raw);

            payload.Write(positionX);
            payload.Write(positionY);
            payload.Write(positionZ);

            if ((updatePositionFlags & UpdatePositionFlag.ZeroQw) != 0)
            {
                payload.Write(rotationW);
            }

            if ((updatePositionFlags & UpdatePositionFlag.ZeroQx) != 0)
            {
                payload.Write(rotationX);                
            }

            if ((updatePositionFlags & UpdatePositionFlag.ZeroQy) != 0)
            {
                payload.Write(rotationY);                
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

            if ((updatePositionFlags & UpdatePositionFlag.ZeroQz) != 0)
            {
                payload.Write(rotationZ);                
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

            payload.Write(positionX);
            payload.Write(positionY);
            payload.Write(positionZ);

            if (writeQuaternion)
            {
                payload.Write(rotationW);
                payload.Write(rotationX);
                payload.Write(rotationY);
                payload.Write(rotationZ);
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

        public override string ToString()
        {
            return $"{LandblockId.Landblock.ToString("X")}: {positionX} {positionY} {positionZ}";
        }
    }
}
