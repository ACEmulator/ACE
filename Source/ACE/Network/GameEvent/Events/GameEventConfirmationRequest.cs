using ACE.Network.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network.GameEvent.Events
{
    public class GameEventConfirmationRequest : GameEventMessage
    {
        public GameEventConfirmationRequest(Session session, ConfirmationType confirmationType, uint context, string text)
            : base(GameEventType.CharacterConfirmationRequest, GameMessageGroup.Group09, session)
        {
            Writer.Write((uint)confirmationType);
            Writer.Write(context);
            Writer.WriteString16L(text);
        }
    }
}
