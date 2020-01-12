using System;
using ACE.Entity.Enum;
using ACE.Server.Network.Enum;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventDefenderNotification : GameEventMessage
    {
        public GameEventDefenderNotification(Session session, string attackerName, DamageType damageType, float percent, uint damage, DamageLocation damageLocation, bool criticalHit, AttackConditions attackConditions)
            : base(GameEventType.DefenderNotification, GameMessageGroup.UIQueue, session)
        {
            Writer.WriteString16L(attackerName);
            Writer.Write((uint)damageType);
            Writer.Write((double)percent);
            Writer.Write(damage);
            Writer.Write((uint)damageLocation);
            Writer.Write(Convert.ToUInt32(criticalHit));
            Writer.Write((ulong)attackConditions);
            Writer.Align();
        }
    }
}
