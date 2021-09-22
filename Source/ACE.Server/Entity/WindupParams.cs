namespace ACE.Server.Entity
{
    public class WindupParams
    {
        public uint TargetGuid;
        public uint SpellId;
        public bool BuiltInSpell;
        public long TurnTries;

        public WindupParams(uint targetGuid, uint spellId, bool builtInSpell)
        {
            TargetGuid = targetGuid;
            SpellId = spellId;
            BuiltInSpell = builtInSpell;
            TurnTries = 0;
        }

        public override string ToString()
        {
            return $"TargetGuid: {TargetGuid:X8}, SpellID: {SpellId}, BuiltInSpell: {BuiltInSpell}";
        }
    }
}
