namespace ACE.Entity.Enum
{
    /// <summary>
    /// Used to select a database when calling a function.
    /// </summary>
    public enum DatabaseSelectionOption
    {
        None = 0,
        Authentication = 1,
        Shard = 2,
        World = 3,
        All = 4
    }
}
