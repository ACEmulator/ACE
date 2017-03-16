namespace ACE.Entity
{
    /// <summary>
    /// Used to replace default textures / not needed unless you want too.
    /// </summary>
    public class ModelTexture
    {
        public byte Index { get; } //index of model to replace texture.
        public ushort OldGuid { get; }
        public ushort NewGuid { get; }

        public ModelTexture(byte index, ushort oldguid, ushort newguid)
        {
            Index = index;
            OldGuid = oldguid; // - 0x05000000
            NewGuid = newguid; // - 0x05000000
        }
    }
}

