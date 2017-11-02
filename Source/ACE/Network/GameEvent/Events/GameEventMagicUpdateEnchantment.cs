using ACE.DatLoader.Entity;

namespace ACE.Network.GameEvent.Events
{
    public class GameEventMagicUpdateEnchantment : GameEventMessage
    {
        public GameEventMagicUpdateEnchantment(Session session, SpellBase spellBase, uint layer, uint spellCategory, int cooldownId, uint enchantmentTypeFlag)
            : base(GameEventType.MagicUpdateEnchantment, GameMessageGroup.Group09, session)
        {
            const double startTime = 0;
            const double lastTimeDegraded = 0;
            const uint key = 0;
            const float val = 0;
            const uint spellSetId = 0;
            uint spellId = layer | spellCategory | (uint)cooldownId; // spellId is made up of these 3 components
            Writer.Write(spellId);
            Writer.Write(layer | spellCategory); // packed spell category
            Writer.Write(spellBase.Power);
            Writer.Write(startTime); // FIXME: this needs to be passed it.
            Writer.Write(spellBase.Duration);
            Writer.Write(session.Player.Guid.Full);
            Writer.Write(spellBase.DegradeModifier);
            Writer.Write(spellBase.DegradeLimit);
            Writer.Write(lastTimeDegraded);
            Writer.Write(enchantmentTypeFlag);

            // FIXME: These next 2 may be depreciated need more research.
            Writer.Write(key);
            Writer.Write(val);
            Writer.Write(spellSetId);

            Writer.Align();
        }
    }
}
