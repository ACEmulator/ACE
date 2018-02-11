using Newtonsoft.Json;

namespace ACE.Entity
{
    /// <summary>
    ///  simple class that implements IDirty to make inheriting from it easy
    /// </summary>
    public class ChangeTrackedObject : IDirty
    {
        [JsonProperty("isDirty")]
        public virtual bool IsDirty { get; set; }

        [JsonIgnore]
        public virtual bool HasEverBeenSavedToDatabase { get; set; }

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
