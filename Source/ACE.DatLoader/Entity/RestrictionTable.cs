namespace ACE.DatLoader.Entity
{
    /// <summary>
    /// Defines how access is controlled to a restricted landblock (e.g. a housing barrier)
    /// </summary>
    public class RestrictionTable
    {
        public uint Landblock { get; set; } // The landblock that is restricted
        public uint Iid { get; set; } // the guid of the world object that controls access to this block
    }
}
