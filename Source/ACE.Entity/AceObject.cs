using ACE.Common;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    [DbTable("ace_object")]
    [DbGetList("vw_ace_object", 3, "landblock")]
    public class AceObject : BaseAceObject
    {
        [DbField("baseAceObjectId", (int)MySqlDbType.UInt32, Update = false, IsCriteria = true)]
        public override uint AceObjectId { get; set; }

        [DbField("weenieClassId", (int)MySqlDbType.UInt16, IsCriteria = true)]
        public ushort WeenieClassId { get; set; }
        
        public Position Position
        {
            get { return new Position((((uint)Landblock) << 16) + Cell, PosX, PosY, PosZ, QX, QY, QZ, QW); }
        }

        [DbField("landblock", (int)MySqlDbType.UInt16)]
        public ushort Landblock { get; set; }

        [DbField("cell", (int)MySqlDbType.UInt16)]
        public ushort Cell { get; set; }

        [DbField("posX", (int)MySqlDbType.Float)]
        public float PosX { get; set; }

        [DbField("posY", (int)MySqlDbType.Float)]
        public float PosY { get; set; }

        [DbField("posX", (int)MySqlDbType.Float)]
        public float PosZ { get; set; }

        [DbField("qW", (int)MySqlDbType.Float)]
        public float QW { get; set; }

        [DbField("qX", (int)MySqlDbType.Float)]
        public float QX { get; set; }

        [DbField("qY", (int)MySqlDbType.Float)]
        public float QY { get; set; }

        [DbField("qZ", (int)MySqlDbType.Float)]
        public float QZ { get; set; }
    }
}
