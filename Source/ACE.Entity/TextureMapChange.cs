namespace ACE.Entity
{
    public class TextureMapChange
    {
        public byte PartIndex { get; set; }
        public uint OldTexture { get; set; }
        public uint NewTexture { get; set; }
    }
}
