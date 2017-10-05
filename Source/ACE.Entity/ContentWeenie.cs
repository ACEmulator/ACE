using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Common;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace ACE.Entity
{
    public class ContentWeenie
    {
        /// <summary>
        /// content identifier
        /// </summary>
        [DbField("contentId", (int)MySqlDbType.UInt32, Update = false, IsCriteria = true, ListGet = true)]
        public uint ContentId { get; set; }

        [DbField("weenieId", (int)MySqlDbType.UInt32)]
        public uint WeenieId { get; set; }

        [DbField("comment", (int)MySqlDbType.Text)]
        public string Comment { get; set; }
    }
}
