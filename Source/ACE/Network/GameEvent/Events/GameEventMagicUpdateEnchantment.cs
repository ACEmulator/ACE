using ACE.DatLoader.Entity;

namespace ACE.Network.GameEvent.Events
{
    public class GameEventMagicUpdateEnchantment : GameEventMessage
    {
        public GameEventMagicUpdateEnchantment(Session session, SpellBase spellBase)
            : base(GameEventType.MagicUpdateEnchantment, GameMessageGroup.Group09, session)
        {
            const ushort layer = 1;
            const double startTime = 0;
            const double lastTimeDegraded = 0;
            const uint key = 0;
            const float val = 0;
            const uint spellSetId = 0;

            Writer.Write(spellBase.MetaSpellId);
            Writer.Write(layer); // FIXME: this needs to increment each time the same spell is cast and dec on spell removal
            Writer.Write((ushort)spellBase.Category & 0xFFFF);
            Writer.Write((ushort)(spellBase.Category << 16) >= 1);
            Writer.Write(spellBase.Power);
            Writer.Write(startTime); // FIXME: this needs to be passed it.
            Writer.Write(spellBase.Duration);
            Writer.Write(session.Player.Guid.Full);
            Writer.Write(spellBase.DegradeModifier);
            Writer.Write(spellBase.DegradeLimit);
            Writer.Write(lastTimeDegraded);
            Writer.Write(spellBase.Category);
            // FIXME: These next 2 may be depreicated need more research.
            Writer.Write(key);
            Writer.Write(val);
            Writer.Write(spellSetId);

            Writer.Align();
        }
    }
}
