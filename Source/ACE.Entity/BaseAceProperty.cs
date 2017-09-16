using ACE.Common;
using ACE.Entity.Enum;
using MySql.Data.MySqlClient;

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

        public void SetDirtyFlags()
        {
            this.IsDirty = true;
            this.HasEverBeenSavedToDatabase = false;
        }

        public virtual uint PropertyId { get; set; }

        public virtual AceObjectPropertyType PropertyType { get; }
    }
}
