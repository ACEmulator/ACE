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
    /// <summary>
    ///  simple class that implements IDirty to make inheriting from it easy
    /// </summary>
    public class ChangeTrackedObject : IDirty
    {
        [JsonProperty("isDirty")]
        public virtual bool IsDirty { get; set; } = false;

        [JsonIgnore]
        public virtual bool HasEverBeenSavedToDatabase { get; set; } = false;

        [JsonProperty("isNew")]
        public bool IsNew
        {
            get { return !HasEverBeenSavedToDatabase; }
            set { HasEverBeenSavedToDatabase = !value; }
        }

        public virtual void ClearDirtyFlags()
        {
            this.IsDirty = false;
            this.HasEverBeenSavedToDatabase = true;
        }

        public virtual void SetDirtyFlags()
        {
            this.IsDirty = true;
            this.HasEverBeenSavedToDatabase = false;
        }
    }
}
