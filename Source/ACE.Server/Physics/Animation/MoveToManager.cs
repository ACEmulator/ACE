using System.Collections.Generic;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics.Animation
{
    public class MoveToManager
    {
        public MovementType MovementType;
        public Position SoughtPosition;
        public Position CurrentTargetPosition;
        public Position StartingPosition;
        public MovementParameters MovementParams;
        public float PreviousHeading;
        public float PreviousDistance;
        public double PreviousDistanceTime;
        public float OriginalDistance;
        public double OriginalDistanceTime;
        public int FailProgressCount;
        public int SoughtObjectID;
        public int TopLevelObjectID;
        public float SoughtObjectRadius;
        public float SoughtObjectHeight;
        public int CurrentCommand;
        public int AuxCommand;
        public int MovingAway;
        public int Initialized;
        public List<MovementNode> PendingActions;
        public PhysicsObj PhysicsObj;
        public WeenieObject WeenieObj;
    }
}
