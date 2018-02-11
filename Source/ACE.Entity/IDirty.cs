namespace ACE.Entity
{
    public interface IDirty
    {
        bool IsDirty { get; set; }

        /// <summary>
        /// flag to indicate whether or not this instance came from the database
        /// or was created by the game engine.  use case: when calling "SaveObject"
        /// in the database, we need to know whether to insert or update.  There's 
        /// really no other way to tell at present.
        /// </summary>
        bool HasEverBeenSavedToDatabase { get; set; }
    }
}
