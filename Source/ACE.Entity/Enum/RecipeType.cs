namespace ACE.Entity.Enum
{
    /// <summary>
    /// these are not from the client, but rather a classification of how a "Use A on B" formula is
    /// intended to work.  this logic drives the basic flow of how these interations work
    /// </summary>
    public enum RecipeType
    {
        None = 0,
        CreateItem = 1,
        Healing = 2,
        Tinkering = 3,
        Dyeing = 4,
        Unlocking = 5,
        ManaStone = 6
    }
}
