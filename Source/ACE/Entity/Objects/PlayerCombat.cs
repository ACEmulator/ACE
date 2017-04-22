using ACE.Common.Extensions;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Network;
using ACE.Network.Enum;
using ACE.Network.GameAction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity.Objects
{
    public partial class Player
    {
        [GameAction(GameActionType.Suicide)]
        private void CharacterSuicide(ClientMessage message)
        {   
        }
    }
}
