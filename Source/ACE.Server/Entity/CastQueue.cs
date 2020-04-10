namespace ACE.Server.Entity
{
    /// <summary>
    /// questionable if this was ever in retail
    /// </summary>
    public class CastQueue
    {
        public CastQueueType Type;
        public uint TargetGuid;
        public uint SpellId;
        public bool BuiltInSpell;

        public CastQueue(CastQueueType type, uint targetGuid, uint spellId, bool builtInSpell)
        {
            Type = type;
            TargetGuid = targetGuid;
            SpellId = spellId;
            BuiltInSpell = builtInSpell;
        }
    }

    public enum CastQueueType
    {
        Targeted,
        Untargeted
    }
}
