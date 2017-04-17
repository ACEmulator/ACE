using ACE.Common.Extensions;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Network;
using ACE.Network.GameAction;
using ACE.Network.GameEvent.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    public partial class Player
    {
        [GameAction(GameActionType.AllegianceUpdateRequest)]
        private void AllegianceUpdateRequestAction(ClientMessage message)
        {
            var unknown1 = message.Payload.ReadUInt32();
            // TODO

            var allegianceUpdate = new GameEventAllegianceUpdate(Session);

            Session.EnqueueSend(allegianceUpdate);
        }
    }
}
