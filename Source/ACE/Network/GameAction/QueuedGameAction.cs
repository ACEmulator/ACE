using ACE.Entity;
using ACE.Managers;
using ACE.Network.Motion;

namespace ACE.Network.GameAction
{
    public class QueuedGameAction
    {
        public QueuedGameAction(uint objectId, GameActionType actionType)
        {
            ObjectId = objectId;
            ActionType = actionType;
            StartTime = WorldManager.PortalYearTicks;
            EndTime = StartTime;
        }

        public QueuedGameAction(uint objectId, uint secondaryObjectId, GameActionType actionType)
        {
            ObjectId = objectId;
            SecondaryObjectId = secondaryObjectId;
            ActionType = actionType;
            StartTime = WorldManager.PortalYearTicks;
            EndTime = StartTime;
        }

        public QueuedGameAction(uint objectId, UniversalMotion motion, GameActionType actionType)
        {
            ObjectId = objectId;
            Motion = motion;
            ActionType = actionType;
            StartTime = WorldManager.PortalYearTicks;
            EndTime = StartTime;
        }

        public QueuedGameAction(uint objectId, UniversalMotion motion, double duration, bool respectDelay, GameActionType actionType)
        {
            ObjectId = objectId;
            Motion = motion;
            RespectDelay = respectDelay;
            ActionType = actionType;
            StartTime = WorldManager.PortalYearTicks;
            EndTime = StartTime + duration;
        }

        public QueuedGameAction(uint objectId, WorldObject worldObject, bool respectDelay, GameActionType actionType)
        {
            ObjectId = objectId;
            WorldObject = worldObject;
            RespectDelay = respectDelay;
            ActionType = actionType;
            StartTime = WorldManager.PortalYearTicks;
            EndTime = StartTime;
        }

        public QueuedGameAction(uint objectId, WorldObject worldObject, bool respectDelay, bool onlyRemove, GameActionType actionType)
        {
            ObjectId = objectId;
            WorldObject = worldObject;
            RespectDelay = respectDelay;
            OnlyRemove = onlyRemove;
            ActionType = actionType;
            StartTime = WorldManager.PortalYearTicks;
            EndTime = StartTime;
        }

        /// <summary>
        /// Sends a player to a location
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="newQueuedPosition">Teleport location</param>
        public QueuedGameAction(uint objectId, Position newQueuedPosition, GameActionType actionType)
        {
            ObjectId = objectId;
            ActionLocation = newQueuedPosition;
            ActionType = actionType;
        }

        /// <summary>
        /// Send a player killed event
        /// </summary>
        /// <param name="objectId">Victim's Object Guid</param>
        /// <param name="secondaryObjectId">Killer's Object Guid</param>
        /// <param name="broadcastMessage">Text string for chat output, usually containing the death message</param>
        public QueuedGameAction(string broadcastMessage, uint objectId, uint secondaryObjectId, GameActionType actionType)
        {
            ObjectId = objectId;
            SecondaryObjectId = secondaryObjectId;
            ActionBroadcastMessage = broadcastMessage;
            ActionType = actionType;
        }

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
    }
}
