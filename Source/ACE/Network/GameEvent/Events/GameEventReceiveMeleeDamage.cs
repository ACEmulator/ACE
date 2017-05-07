using ACE.Entity.Enum;

namespace ACE.Network.GameEvent.Events
{
    public class GameEventReceiveMeleeDamage : GameEventMessage
    {
        public GameEventReceiveMeleeDamage(Session session, string attackerName, DamageType damageType, double severity, uint amount, bool critical, DamageLocation damageLocation, ulong attackConditions)
            : base(GameEventType.ReceiveMeleeDamage, GameMessageGroup.Group09, session)
        {
            Writer.WriteString16L(attackerName);
            Writer.Write((uint)damageType);
            Writer.Write(severity);
            Writer.Write(amount);
            Writer.Write((uint)damageLocation);
            Writer.Write(critical);
            Writer.Write(attackConditions);  // sneak attack, dirty fighting
            Writer.Align();
        }
    }
}
