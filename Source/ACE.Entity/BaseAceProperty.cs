using ACE.Common;
using ACE.Entity.Enum;

using Newtonsoft.Json;

namespace ACE.Entity
{
    public abstract class BaseAceProperty : IDirty
    {
        [JsonIgnore]
        //[DbField("aceObjectId", (int)MySqlDbType.UInt32, IsCriteria = true, ListGet = true, ListDelete = true, Update = false)]
        public uint AceObjectId { get; set; }

        [JsonIgnore]
        public bool IsDirty { get; set; }

        [JsonIgnore]
        public bool HasEverBeenSavedToDatabase { get; set; }

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
        
        [JsonIgnore]
        public virtual uint PropertyId { get; set; }

        [JsonIgnore]
        public virtual AceObjectPropertyType PropertyType { get; }
    }
}
