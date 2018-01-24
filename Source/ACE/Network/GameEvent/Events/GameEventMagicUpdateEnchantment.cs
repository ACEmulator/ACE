using ACE.DatLoader.Entity;

namespace ACE.Network.GameEvent.Events
{
    public class GameEventMagicUpdateEnchantment : GameEventMessage
    {
        public GameEventMagicUpdateEnchantment(Session session, SpellBase spellBase, uint layer, int cooldownId, uint enchantmentTypeFlag)
            : base(GameEventType.MagicUpdateEnchantment, GameMessageGroup.Group09, session)
        {
            const double startTime = 0;
            const double lastTimeDegraded = 0;
            ////val is something that is currently not available directly from the spell tables. I am working on fixing this for the SpellManager that is being worked on.
            const float val = 35;
            const ushort spellSetId = 0;
            Writer.Write((ushort)spellBase.MetaSpellId);
            Writer.Write((ushort)layer);
            Writer.Write((ushort)spellBase.Category);
            Writer.Write(spellSetId);
            Writer.Write(spellBase.Power);
            Writer.Write(startTime); // FIXME: this needs to be passed it.
            Writer.Write(spellBase.Duration);
            Writer.Write(session.Player.Guid.Full);
            Writer.Write(spellBase.DegradeModifier);
            Writer.Write(spellBase.DegradeLimit);
            Writer.Write(lastTimeDegraded);////This needs timer updates to work correctly
            Writer.Write(enchantmentTypeFlag);////This is something that needs to be worked on. There is currently no way to get correct flags based on the spell table itself. They are in the logs though and we could eventually add this to the spell table.
            Writer.Write(spellBase.Category);
            Writer.Write(val);
            Writer.Write(spellSetId);
            Writer.Align();
        }
    }
}
