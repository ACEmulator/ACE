using ACE.Entity.Enum;

namespace ACE.Network.GameEvent.Events
{
    public class GameEventInflictMeleeDamage : GameEventMessage
    {
        public GameEventInflictMeleeDamage(Session session, string targetName, DamageType damageType, double severity, uint amount, bool critical, ulong attackConditions)
            : base(GameEventType.InflictMeleeDamage, GameMessageGroup.Group09, session)
        {
            Writer.WriteString16L(targetName);
            Writer.Write((uint)damageType);
            Writer.Write(severity);
            Writer.Write(amount);
            Writer.Write(critical);
            Writer.Write(attackConditions); // sneak attack, dirty fighting
            Writer.Align();
        }
    }
}
