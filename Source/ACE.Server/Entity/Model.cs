namespace ACE.Server.Entity
{
    /// <summary>
    /// Used to replace model objects with other model objects.
    /// For example, you can swap out characters Head 0x10 with another Random Model.
    /// </summary>
    public class Model
    {
        /// <summary>
        /// Index of Model being replace
        /// </summary>
        public byte Index { get; }

        /// <summary>
        /// Model portal.dat entry
        /// </summary>
        public uint ModelID { get; }

        public Model(byte index, uint modelID)
        {
            Index = index;
            ModelID = modelID;
        }
    }
}
