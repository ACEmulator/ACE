using ACE.Common;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    [DbTable("portal_destination")]
    [DbGetList("portal_destination", 14, "weenieClassId")]
    public class PortalDestination
    {
        [DbField("weenieClassId", (int)MySqlDbType.UInt16, IsCriteria = true)]
        public ushort WeenieClassId { get; set; }

        public Position Position
        {
            get { return new Position(Cell, PosX, PosY, PosZ, QX, QY, QZ, QW); }
        }

        [DbField("cell", (int)MySqlDbType.UInt32)]
        public uint Cell { get; set; }

        [DbField("x", (int)MySqlDbType.Float)]
        public float PosX { get; set; }

        [DbField("y", (int)MySqlDbType.Float)]
        public float PosY { get; set; }

        [DbField("z", (int)MySqlDbType.Float)]
        public float PosZ { get; set; }

        [DbField("qx", (int)MySqlDbType.Float)]
        public float QW { get; set; }

        [DbField("qy", (int)MySqlDbType.Float)]
        public float QX { get; set; }

        [DbField("qz", (int)MySqlDbType.Float)]
        public float QY { get; set; }

        [DbField("qw", (int)MySqlDbType.Float)]
        public float QZ { get; set; }

        [DbField("min_lvl", (int)MySqlDbType.UInt32)]
        public uint MinLvl { get; set; }

        [DbField("max_lvl", (int)MySqlDbType.UInt32)]
        public uint MaxLvl { get; set; }
    }
}
