using System;
using System.Collections.Generic;
using ACE.Entity.Enum;
using ACE.Server.Physics.Common;
using ACE.Server.Physics.Combat;

namespace ACE.Server.Physics.Animation
{
    public class MovementManager
    {
        public MotionInterp MotionInterpreter;
        public MoveToManager MoveToManager;
        public PhysicsObj PhysicsObj;
        public WeenieObject WeenieObj;

        public MovementManager() { }

        public MovementManager(PhysicsObj obj, WeenieObject wobj)
        {
            PhysicsObj = obj;
            WeenieObj = wobj;

            MotionInterpreter = new MotionInterp(obj, wobj);
            MoveToManager = new MoveToManager(obj, wobj);
        }

        public void CancelMoveTo(WeenieError error)
        {
            if (MoveToManager != null)
                MoveToManager.CancelMoveTo(error);
        }

        public static MovementManager Create(PhysicsObj obj, WeenieObject wobj)
        {
            return new MovementManager(obj, wobj);
        }

        public void EnterDefaultState()
        {
            if (PhysicsObj == null) return;

            if (MotionInterpreter == null)
                MotionInterpreter = MotionInterp.Create(PhysicsObj, WeenieObj);

            MotionInterpreter.enter_default_state();
        }

        public void HandleEnterWorld()
        {
            //if (MotionInterpreter != null)
                //NoticeHandler.RecvNotice_PrevSpellSelection(MotionInterpreter);
        }

        public void HandleExitWorld()
        {
            if (MotionInterpreter != null)
                MotionInterpreter.HandleExitWorld();
        }

        public void HandleUpdateTarget(TargetInfo targetInfo)
        {
            if (MoveToManager != null)
                MoveToManager.HandleUpdateTarget(targetInfo);
        }

        public void HitGround()
        {
            if (MotionInterpreter != null)
                MotionInterpreter.HitGround();

            if (MoveToManager != null)
                MoveToManager.HitGround();
        }

        public InterpretedMotionState InqInterpretedMotionState()
        {
            if (MotionInterpreter == null)
            {
                MotionInterpreter = MotionInterp.Create(PhysicsObj, WeenieObj);
                if (PhysicsObj != null)
                    MotionInterpreter.enter_default_state();
            }
            return MotionInterpreter.InterpretedState;
        }

        public RawMotionState InqRawMotionState()
        {
            if (MotionInterpreter == null)
            {
                MotionInterpreter = MotionInterp.Create(PhysicsObj, WeenieObj);
                if (PhysicsObj != null)
                    MotionInterpreter.enter_default_state();
            }
            return MotionInterpreter.RawState;
        }

        public bool IsMovingTo()
        {
            if (MoveToManager == null) return false;

            return MoveToManager.is_moving_to();
        }

        public void LeaveGround()
        {
            if (MotionInterpreter != null)
                MotionInterpreter.LeaveGround();

            // NoticeHandler::RecvNotice_PrevSpellSection
        }

        public void MakeMoveToManager()
        {
            if (MoveToManager == null)
                MoveToManager = MoveToManager.Create(PhysicsObj, WeenieObj);
        }

        public void MotionDone(uint motion, bool success)
        {
            if (MotionInterpreter != null)
                MotionInterpreter.MotionDone(success);
        }

        public WeenieError PerformMovement(MovementStruct mvs)
        {
            PhysicsObj.set_active(true);

            switch (mvs.Type)
            {
                case MovementType.RawCommand:
                case MovementType.InterpretedCommand:
                case MovementType.StopRawCommand:
                case MovementType.StopInterpretedCommand:
                case MovementType.StopCompletely:

                    if (MotionInterpreter == null)
                    {
                        MotionInterpreter = MotionInterp.Create(PhysicsObj, WeenieObj);
                        if (PhysicsObj != null)
                            MotionInterpreter.enter_default_state();
                    }
                    return MotionInterpreter.PerformMovement(mvs);

                case MovementType.MoveToObject:
                case MovementType.MoveToPosition:
                case MovementType.TurnToObject:
                case MovementType.TurnToHeading:

                    if (MoveToManager == null)
                        MoveToManager = MoveToManager.Create(PhysicsObj, WeenieObj);

                    return MoveToManager.PerformMovement(mvs);

                default:
                    return WeenieError.GeneralMovementFailure;
            }
        }

        public void ReportExhaustion()
        {
            if (MotionInterpreter != null)
                MotionInterpreter.ReportExhaustion();

            // NoticeHandler::RecvNotice_PrevSpellSelection
        }

        public void SetWeenieObject(WeenieObject wobj)
        {
            WeenieObj = wobj;
            if (MotionInterpreter != null)
                MotionInterpreter.SetWeenieObject(wobj);
            if (MoveToManager != null)
                MoveToManager.SetWeenieObject(wobj);
        }

        public void UseTime()
        {
            if (MoveToManager != null) MoveToManager.UseTime();
        }

        public MotionInterp get_minterp()
        {
            if (MotionInterpreter == null)
            {
                MotionInterpreter = MotionInterp.Create(PhysicsObj, WeenieObj);
                if (PhysicsObj != null)
                    MotionInterpreter.enter_default_state();
            }
            return MotionInterpreter;
        }

        /// <summary>
        /// Alternatively, you can use PhysicsObj.IsAnimating for better performance.
        /// </summary>
        public bool motions_pending()
        {
            if (MotionInterpreter == null) return false;

            return MotionInterpreter.motions_pending();
        }

        public void move_to_interpreted_state(InterpretedMotionState state)
        {
            if (MotionInterpreter == null)
            {
                MotionInterpreter = MotionInterp.Create(PhysicsObj, WeenieObj);
                if (PhysicsObj != null)
                    MotionInterpreter.enter_default_state();
            }
            MotionInterpreter.move_to_interpreted_state(state);
        }

        public void unpack_movement(object addr, uint size) { }
    }
}
