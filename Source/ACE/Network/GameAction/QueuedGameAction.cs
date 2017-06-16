using ACE.Entity;
using ACE.InGameManager;
using ACE.InGameManager.Enums;
using ACE.Managers;
using ACE.Network.Motion;
using System.Collections.Generic;

namespace ACE.Network.GameAction
{
    public class QueuedGameAction
    {
        public InGameType InGameType;

        public void Handler(GameMediator mediator, Player obj) { Handle(mediator, obj); }
        protected virtual void Handle(GameMediator mediator, Player obj) { }

        public LandblockId LandBlockId { get; protected set; }

        public uint ObjectId { get; protected set; }

        public uint SecondaryObjectId { get; protected set; }

        public WorldObject WorldObject { get; protected set; }

        public string ActionBroadcastMessage { get; protected set; }

        // this is weird, its only realy used now for teleport ?
        // maybe it can go away soon enough.
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
