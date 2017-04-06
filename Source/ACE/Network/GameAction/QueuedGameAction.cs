using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network.GameAction
{
    public class QueuedGameAction
    {
        public QueuedGameAction(uint objectId, GameActionType actionType)
        {
            this.ObjectId = objectId;
            this.ActionType = actionType;
        }

        public QueuedGameAction(uint objectId, uint secondaryObjectId, GameActionType actionType)
        {
            this.ObjectId = objectId;
            this.SecondaryObjectId = secondaryObjectId;
            this.ActionType = actionType;
        }

        public uint ObjectId { get; private set; }

        public uint SecondaryObjectId { get; private set; }

        public GameActionType ActionType { get; private set; }
    }
}
