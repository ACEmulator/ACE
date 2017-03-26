using ACE.Common;
using ACE.Entity.Enum;
using MySql.Data.MySqlClient;

namespace ACE.Entity
{
    [DbTable("character_position")]
    public class CharacterPosition
    {
        [DbField("character_id", (int)MySqlDbType.UInt32, Update = false, IsCriteria = true)]
        public virtual uint character_id { get; set; }

        [DbField("cell", (int)MySqlDbType.UInt32)]
        public virtual uint cell { get; set; }

        [DbField("positionType", (int)MySqlDbType.UInt32, Update = false, IsCriteria = true)]
        public virtual uint positionType { get; set; }

        [DbField("positionX", (int)MySqlDbType.Float)]
        public virtual float positionX { get; set; }

        [DbField("positionY", (int)MySqlDbType.Float)]
        public virtual float positionY { get; set; }

        [DbField("positionZ", (int)MySqlDbType.Float)]
        public virtual float positionZ { get; set; }

        [DbField("rotationX", (int)MySqlDbType.Float)]
        public virtual float rotationX { get; set; }

        [DbField("rotationY", (int)MySqlDbType.Float)]
        public virtual float rotationY { get; set; }

        [DbField("rotationZ", (int)MySqlDbType.Float)]
        public virtual float rotationZ { get; set; }

        [DbField("rotationW", (int)MySqlDbType.Float)]
        public virtual float rotationW { get; set; }
    }
}
