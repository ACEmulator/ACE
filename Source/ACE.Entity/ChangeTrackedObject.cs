using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Common;
using MySql.Data.MySqlClient;

namespace ACE.Entity
{
    /// <summary>
    ///  simple class that implements IDirty to make inheriting from it easy
    /// </summary>
    public class ChangeTrackedObject : IDirty
    {
        public virtual bool IsDirty { get; set; } = false;

        public virtual bool HasEverBeenSavedToDatabase { get; set; } = false;
        
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
