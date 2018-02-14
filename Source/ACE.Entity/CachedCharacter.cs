
namespace ACE.Entity
{
    public class CachedCharacter
    {
        public uint FullGuid { get; set; }

        public ObjectGuid Guid
        {
            get => new ObjectGuid(FullGuid);
            set => FullGuid = value.Full;
        }

        public byte SlotId { get; set; }

        public uint AccountId { get; set; }

        public string Name { get; set; }

        public bool Deleted { get; set; }

        public ulong DeleteTime { get; set; }

        public double LoginTimestamp { get; set; }

        public CachedCharacter() { }

        public CachedCharacter(ObjectGuid guid, byte slotId, string name, ulong deleteTime)
        {
            Guid = guid;
            SlotId = slotId;
            Name = name;
            DeleteTime = deleteTime;
        }
    }
}
