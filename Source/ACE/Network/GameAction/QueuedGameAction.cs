using ACE.Entity;
using ACE.Managers;
using ACE.Network.Motion;
using System.Collections.Generic;

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

        public QueuedGameAction(uint objectId, UniversalMotion motion, GameActionType actionType)
        {
            this.ObjectId = objectId;
            this.Motion = motion;
            this.ActionType = actionType;
            this.StartTime = WorldManager.PortalYearTicks;
            this.EndTime = this.StartTime;
        }

        public QueuedGameAction(uint objectId, UniversalMotion motion, double duration, bool respectDelay, GameActionType actionType)
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

        /// <summary>
        /// Sends a player to a location
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="newQueuedPosition">Teleport location</param>
        public QueuedGameAction(uint objectId, Position newQueuedPosition, GameActionType actionType)
        {
            this.ObjectId = objectId;
            this.ActionLocation = newQueuedPosition;
            this.ActionType = actionType;
        }

        /// <summary>
        /// Send a player killed event
        /// </summary>
        /// <param name="objectId">Victim's Object Guid</param>
        /// <param name="secondaryObjectId">Killer's Object Guid</param>
        /// <param name="broadcastMessage">Text string for chat output, usually containing the death message</param>
        public QueuedGameAction(string broadcastMessage, uint objectId, uint secondaryObjectId, GameActionType actionType)
        {
            this.ObjectId = objectId;
            this.SecondaryObjectId = secondaryObjectId;
            this.ActionBroadcastMessage = broadcastMessage;
            this.ActionType = actionType;
        }

        public QueuedGameAction(uint objectId, List<ItemProfile> profileItems, GameActionType actionType)
        {
            this.ObjectId = objectId;
            this.ProfileItems = profileItems;
            this.ActionType = actionType;
        }

        public void Handler(Player player) { Handle(player); }
        protected virtual void Handle(Player player) { }

        public void Handler(Monster monster) { Handle(monster); }
        protected virtual void Handle(Monster monster) { }

        public uint ObjectId { get; private set; }

        public uint SecondaryObjectId { get; private set; }

        public WorldObject WorldObject { get; private set; }

        public string ActionBroadcastMessage { get; private set; }

        public GameActionType ActionType { get; private set; }

        public Position ActionLocation { get; private set; }

        public UniversalMotion Motion { get; private set; }

        public bool OnlyRemove { get; private set; }

        public bool RespectDelay { get; private set; }

        public double StartTime { get; set; }

        public double EndTime { get; set; }

        public List<ItemProfile> ProfileItems = new List<ItemProfile>();
    }
}