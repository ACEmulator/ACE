using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Common;
using MySql.Data.MySqlClient;

namespace ACE.Entity
{
    public class ContentLandblock
    {
        /// <summary>
        /// content identifier
        /// </summary>
        [DbField("contentId", (int)MySqlDbType.UInt32, Update = false, IsCriteria = true, ListGet = true)]
        public uint ContentId { get; set; }

        public LandblockId LandblockId { get; set; }

        /// <summary>
        /// for use with binding in our ORM tools
        /// </summary>
        [DbField("landblockId", (int)MySqlDbType.UInt32, Update = false, IsCriteria = true, ListGet = true)]
        public uint LandblockId_Binder
        {
            get { return LandblockId.Raw; }
            set { this.LandblockId = new LandblockId(value); }
        }

        [DbField("comment", (int)MySqlDbType.Text)]
        public string Comment { get; set; }
    }
}
