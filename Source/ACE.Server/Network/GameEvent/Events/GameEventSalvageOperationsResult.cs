using System.Collections.Generic;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Network.Structure;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventSalvageOperationsResult : GameEventMessage
    {
        public GameEventSalvageOperationsResult(Session session, Skill skill, List<SalvageMessage> messages)
            : base(GameEventType.SalvageOperationsResult, GameMessageGroup.UIQueue, session)
        {
            Writer.Write((uint)skill);
            Writer.Write(0);    // not salvagable item guid list?
            Writer.Write(messages.Count);
            var augBonus = 0;
            foreach (var message in messages)
            {
                Writer.Write(new SalvageResult(message));
                augBonus += message.AugBonus;
            }
            Writer.Write(augBonus);
        }
    }
}
