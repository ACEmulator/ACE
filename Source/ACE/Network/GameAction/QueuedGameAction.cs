using ACE.Entity;
using ACE.Network.Enum;
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

        // public QueuedGameAction(uint objectId, GeneralMotion motion, MotionStance stance, MotionItem item, MotionCommand command, GameActionType actionType)
        public QueuedGameAction(WorldObject sender, GeneralMotion motion, GameActionType actionType)
        {
            this.WorldObject = sender;
            this.Motion = motion;
            // this.MotionStance = stance;
            // this.MotionItem = item;
            // this.MotionCommand = command;
            this.ActionType = actionType;
        }

        public WorldObject WorldObject { get; private set; }

        public uint ObjectId { get; private set; }

        public uint SecondaryObjectId { get; private set; }

        public GameActionType ActionType { get; private set; }

        public GeneralMotion Motion { get; private set; }

        public MotionStance MotionStance { get; private set; }

        public MotionItem MotionItem { get; private set; }

        public MotionCommand MotionCommand { get; private set; }
    }
}
