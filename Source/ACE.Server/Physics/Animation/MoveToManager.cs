using System.Collections.Generic;
using ACE.Server.Physics.Combat;
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

        public MoveToManager() { }

        public MoveToManager(PhysicsObj obj, WeenieObject wobj)
        {
            PhysicsObj = obj;
            WeenieObj = wobj;
        }

        public void CancelMoveTo(int retval)
        {

        }

        public static MoveToManager Create(PhysicsObj obj, WeenieObject wobj)
        {
            return null;
        }

        public void HandleUpdateTarget(TargetInfo targetInfo)
        {

        }

        public void HitGround()
        {

        }

        public Sequence PerformMovement(MovementStruct mvs)
        {
            return null;
        }

        public void SetWeenieObject(WeenieObject wobj)
        {

        }

        public void UseTime()
        {

        }

        public bool is_moving_to()
        {
            return false;
        }
    }
}
