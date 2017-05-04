namespace ACE.Entity
{
    public class CachedCharacter
    {
        public ObjectGuid Guid { get; }
        public byte SlotId { get; }
        public string Name { get; }
        public ulong DeleteTime { get; }

        public CachedCharacter(ObjectGuid guid, byte slotId, string name, ulong deleteTime)
        {
            Guid = guid;
            SlotId = slotId;
            Name = name;
            DeleteTime = deleteTime;
        }
    }
}
