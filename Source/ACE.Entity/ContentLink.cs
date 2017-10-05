using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Common;
using MySql.Data.MySqlClient;

namespace ACE.Entity
{
    public class ContentLink
    {
        /// <summary>
        /// content identifier
        /// </summary>
        [DbField("contentId1", (int)MySqlDbType.UInt32, Update = false, IsCriteria = true, ListGet = true, ListDelete = true)]
        public uint ContentId { get; set; }

        /// <summary>
        /// content identifier
        /// </summary>
        [DbField("contentId2", (int)MySqlDbType.UInt32, ListGet = true, ListDelete = true)]
        public uint AssociatedContentId { get; set; }
    }
}
