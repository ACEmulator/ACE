using ACE.Entity;
using ACE.Managers;
using ACE.Network.Motion;

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

        public uint ObjectId { get; private set; }

        public uint SecondaryObjectId { get; private set; }

        public WorldObject WorldObject { get; private set; }

        public GameActionType ActionType { get; private set; }

        public GeneralMotion Motion { get; private set; }

        public bool OnlyRemove { get; private set; }

        public bool RespectDelay { get; private set; }

        public double StartTime { get; set; }

        public double EndTime { get; set; }
    }
}
