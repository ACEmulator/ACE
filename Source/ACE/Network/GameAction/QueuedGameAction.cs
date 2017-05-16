using ACE.Entity;
using ACE.Managers;
using ACE.Network.Motion;
using System.Collections.Generic;

namespace ACE.Network.GameAction
{
    public class QueuedGameAction
    {
        public void Handler(Player obj) { Handle(obj); }
        protected virtual void Handle(Player obj) { }

        public void Handler(Monster obj) { Handle(obj); }
        protected virtual void Handle(Monster obj) { }

        public void Handler(Door obj) { Handle(obj); }
        protected virtual void Handle(Door obj) { }

        public void Handler(Container obj) { Handle(obj); }
        protected virtual void Handle(Container obj) { }

        public void Handler(WorldObject obj) { Handle(obj); }
        protected virtual void Handle(WorldObject obj) { }

        public LandblockId LandBlockId { get; protected set; }

        public uint ObjectId { get; protected set; }

        public uint SecondaryObjectId { get; protected set; }

        public WorldObject WorldObject { get; protected set; }

        public string ActionBroadcastMessage { get; protected set; }

        public GameActionType ActionType { get; protected set; }

        public Position ActionLocation { get; protected set; }

        public UniversalMotion Motion { get; protected set; }

        public bool OnlyRemove { get; protected set; }

        public bool RespectDelay { get; protected set; }

        public double StartTime { get; set; }

        public double EndTime { get; set; }

        public List<ItemProfile> ProfileItems { get; protected set; }

        // this is going to be the only one left once am done converting them all.
        public QueuedGameAction()
        {
        }

        // All the code below this line is to be chucked.
        public QueuedGameAction(uint objectId, GameActionType actionType)
        {
            this.ObjectId = objectId;
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
    }
}