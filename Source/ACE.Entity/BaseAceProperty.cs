using ACE.Common;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    public abstract class BaseAceProperty : IDirty
    {
        [DbField("aceObjectId", (int)MySqlDbType.UInt32, IsCriteria = true, ListGet = true, ListDelete = true, Update = false)]
        public uint AceObjectId { get; set; }

        public bool IsDirty { get; set; } = false;

        public bool HasEverBeenSavedToDatabase { get; set; } = false;

        public void ClearDirtyFlags()
        {
            this.IsDirty = false;
            this.HasEverBeenSavedToDatabase = true;
        }
    }
}
