using ACE.Entity;
using ACE.Network.Enum;
using ACE.Network.GameEvent;
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
        public QueuedGameAction(uint objectId, GameEventMessage message, GameActionType actionType)
        {
            this.ObjectId = objectId;
            this.ActionEvent = message;
            this.ActionType = actionType;
        }

        /// <summary>
        /// Queued Game Message example
        /// </summary>
        /// <param name="sendToAll">false will limit broadcast to source sender, true will send the message to all</param>
        public QueuedGameAction(uint objectId, GameEventMessage message, bool sendToAll)
        {
            this.ObjectId = objectId;
            this.ActionEvent = message;

            if (sendToAll)
            {
                this.ActionType = GameActionType.OutboundEvent;
            } else
            {
                this.ActionType = GameActionType.OutboundEventForOthers;
            }
        }

        public GameEventMessage ActionEvent { get; set; }

        public uint ObjectId { get; private set; }

        public uint SecondaryObjectId { get; private set; }

        public GameActionType ActionType { get; private set; }

        public GeneralMotion Motion { get; private set; }
    }
}
