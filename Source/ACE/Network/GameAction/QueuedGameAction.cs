using ACE.Entity;
using ACE.Managers;
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
            this.StartTime = WorldManager.PortalYearTicks;
            this.EndTime = this.StartTime;
        }

        public QueuedGameAction(uint objectId, uint secondaryObjectId, GameActionType actionType)
        {
            this.ObjectId = objectId;
            this.SecondaryObjectId = secondaryObjectId;
            this.ActionType = actionType;
            this.StartTime = WorldManager.PortalYearTicks;
            this.EndTime = this.StartTime;
        }

        public QueuedGameAction(uint objectId, GeneralMotion motion, GameActionType actionType)
        {
            this.ObjectId = objectId;
            this.Motion = motion;
            this.ActionType = actionType;
            this.StartTime = WorldManager.PortalYearTicks;
            this.EndTime = this.StartTime;
        }

        public QueuedGameAction(uint objectId, GeneralMotion motion, double duration, bool respectDelay, GameActionType actionType)
        {
            this.ObjectId = objectId;
            this.Motion = motion;
            this.RespectDelay = respectDelay;
            this.ActionType = actionType;
            this.StartTime = WorldManager.PortalYearTicks;
            this.EndTime = this.StartTime + duration;
        }

        public QueuedGameAction(uint objectId, WorldObject worldObject, bool respectDelay, GameActionType actionType)
        {
            this.ObjectId = objectId;
            this.WorldObject = worldObject;
            this.RespectDelay = respectDelay;
            this.ActionType = actionType;
            this.StartTime = WorldManager.PortalYearTicks;
            this.EndTime = this.StartTime;
        }

        public QueuedGameAction(uint objectId, WorldObject worldObject, bool respectDelay, bool onlyRemove, GameActionType actionType)
        {
            this.ObjectId = objectId;
            this.WorldObject = worldObject;
            this.RespectDelay = respectDelay;
            this.OnlyRemove = onlyRemove;
            this.ActionType = actionType;
            this.StartTime = WorldManager.PortalYearTicks;
            this.EndTime = this.StartTime;
        }

        public QueuedGameAction(uint objectId, Position newQueuedPosition, GameActionType actionType)
        {
            this.ObjectId = objectId;
            this.QueuedPosition = newQueuedPosition;
            this.ActionType = actionType;
        }

        /* 
         * // MELDED EVENTS THAT RESIDE IN THE ACTION QUEUE: 
         * // This is the birth of one concurrent queue, for all player messages 
         */
        
        /// <summary>
        /// Send a death message event
        /// </summary>
        /// <param name="objectId">Typically this is the Players Guid</param>
        /// <param name="broadcastMessage">Text string for chat output, usually containing the death message</param>
        /// <param name="actionType">This is default to GameActionType of GameActionEvent</param>
        public QueuedGameAction(string broadcastMessage, uint objectId, GameEventType eventType,
            GameActionType actionType = GameActionType.GameActionEvent)
        {
            this.ObjectId = objectId;
            this.TextMessage = broadcastMessage;
            this.EventType = eventType;
            this.ActionType = actionType;
        }

        public QueuedGameAction(string broadcastMessage, uint objectId, uint secondaryObjectId, GameEventType eventType, 
            GameActionType actionType = GameActionType.GameActionEvent)
        {
            this.ObjectId = objectId;
            this.SecondaryObjectId = secondaryObjectId;
            this.TextMessage = broadcastMessage;
            this.EventType = eventType;
            this.ActionType = actionType;
        }

        public uint ObjectId { get; private set; }

        public uint SecondaryObjectId { get; private set; }

        public WorldObject WorldObject { get; private set; }

        public string TextMessage { get; private set; }

        public GameActionType ActionType { get; private set; }

        public GameEventType EventType { get; private set; }

        public Position QueuedPosition { get; private set; }

        public GeneralMotion Motion { get; private set; }

        public bool OnlyRemove { get; private set; }

        public bool RespectDelay { get; private set; }

        public double StartTime { get; set; }

        public double EndTime { get; set; }
    }
}
