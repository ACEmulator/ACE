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

        // Implement your own methods in your own QueuedGameAction class.
        public QueuedGameAction()
        {
        }
    }
}