using ACE.Server.Physics.Common;

namespace ACE.Server.Physics.Animation
{
    public class MovementManager
    {
        public MotionInterp MotionInterpreter;
        public MoveToManager MoveToManager;
        public PhysicsObj PhysicsObj;
        public WeenieObject WeenieObj;

        public void LeaveGround()
        {

        }

        public void HitGround()
        {

        }

        public void SetWeenieObject(WeenieObject wobj)
        {

        }

        public MotionInterp get_minterp()
        {
            return null;
        }

        public static MovementManager Create(PhysicsObj obj, WeenieObject wobj)
        {
            return null;
        }

        public void EnterDefaultState()
        {

        }

        public void UseTime()
        {

        }

        public void unpack_movement(object addr, int size)
        {

        }

        public void CancelMoveTo(int err)
        {

        }

        public bool MotionsPending()
        {
            return false;
        }

        public void ReportExhaustion()
        {

        }
    }
}
