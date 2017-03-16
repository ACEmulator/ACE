namespace ACE.Entity
{
    public class Model
    {
        public byte Index { get; } //index of model

        public ushort Guid { get; }  //- 0x01000000

        public Model(byte index, ushort guid)
        {
            Index = index;
            Guid = guid;
        }
    }
}
