using System;
using ACE.Entity.Enum;
using ACE.Server.Network.Enum;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventAttackerNotification : GameEventMessage
    {
        public GameEventAttackerNotification(Session session, string defenderName, DamageType damageType, float percent, uint damage, bool criticalHit, AttackConditions attackConditions)
            : base(GameEventType.AttackerNotification, GameMessageGroup.UIQueue, session)
        {
            Writer.WriteString16L(defenderName);
            Writer.Write((uint)damageType);
            Writer.Write((double)percent);
            Writer.Write(damage);
            Writer.Write(Convert.ToUInt16(criticalHit));
            Writer.Write((UInt64)attackConditions);
        }
    }
}
