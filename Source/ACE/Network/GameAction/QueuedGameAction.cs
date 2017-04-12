using ACE.Entity;
using ACE.Entity.Events;
using ACE.Network.Enum;
using ACE.Network.GameEvent;
using ACE.Network.GameMessages;
using ACE.Network.Motion;
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

        public QueuedGameAction(uint objectId, GeneralMotion motion, GameActionType actionType)
        {
            this.ObjectId = objectId;
            this.Motion = motion;
            this.ActionType = actionType;
        }

        /// <summary>
        /// This is a proof of concept to show that we may need a seporate queue for outbound events, if we do not combind the event types,
        /// Like I have done with the GameActionType of `OutboundEvent` and `OutboundEventForOthers`
        /// </summary>
        public QueuedGameAction(uint objectId, GameMessage message, GameActionType actionType)
        {
            this.ObjectId = objectId;
            this.ActionType = actionType;
            this.OutboundMessageArgs = new OutboundMessageArgs();
            this.OutboundMessageArgs.Message = message;
        }

        public OutboundMessageArgs OutboundMessageArgs { get; set; }

        public uint ObjectId { get; private set; }

        public uint SecondaryObjectId { get; private set; }

        public GameActionType ActionType { get; private set; }

        public GeneralMotion Motion { get; private set; }
    }
}
