using System;
using ACE.Common;
using MySql.Data.MySqlClient;
namespace ACE.Entity
{
    [DbTable("ace_position")]
    public class AceObjectPropertiesPosition : BaseAceProperty, ICloneable
    {
        // private variables for things that mean "this changed".
        // position type and position id are _relatively_ immutable

        private uint _cell = 0;
        private float _positionX = 0;
        private float _positionY = 0;
        private float _positionZ = 0;
        private float _rotationW = 0;
        private float _rotationX = 0;
        private float _rotationY = 0;
        private float _rotationZ = 0;
        
        [DbField("landblockRaw", (int)MySqlDbType.UInt32)]
        public uint Cell
        {
            get { return _cell; }
            set
            {
                _cell = value;
                IsDirty = true;
            }
        }

        [DbField("positionType", (int)MySqlDbType.UInt16, Update = false, IsCriteria = true)]
        public ushort DbPositionType { get; set; }

        [DbField("positionId", (int)MySqlDbType.UInt32)]
        public uint PositionId { get; set; }

        [DbField("posX", (int)MySqlDbType.Float)]
        public float PositionX
        {
            get { return _positionX; }
            set
            {
                _positionX = value;
                IsDirty = true;
            }
        }

        [DbField("posY", (int)MySqlDbType.Float)]
        public float PositionY
        {
            get { return _positionY; }
            set
            {
                _positionY = value;
                IsDirty = true;
            }
        }

        [DbField("posZ", (int)MySqlDbType.Float)]
        public float PositionZ
        {
            get { return _positionZ; }
            set
            {
                _positionZ = value;
                IsDirty = true;
            }
        }

        [DbField("qW", (int)MySqlDbType.Float)]
        public float RotationW
        {
            get { return _rotationW; }
            set
            {
                _rotationW = value;
                IsDirty = true;
            }
        }

        [DbField("qX", (int)MySqlDbType.Float)]
        public float RotationX
        {
            get { return _rotationX; }
            set
            {
                _rotationX = value;
                IsDirty = true;
            }
        }

        [DbField("qY", (int)MySqlDbType.Float)]
        public float RotationY
        {
            get { return _rotationY; }
            set
            {
                _rotationY = value;
                IsDirty = true;
            }
        }

        [DbField("qZ", (int)MySqlDbType.Float)]
        public float RotationZ
        {
            get { return _rotationZ; }
            set
            {
                _rotationZ = value;
                IsDirty = true;
            }
        }
        
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
