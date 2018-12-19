using System;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Entity.Enum;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Physics.Animation;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public bool InitMoveListener;

        public override void MoveTo(WorldObject target, float runRate = 0.0f)
        {
            if (runRate == 0.0f)
                runRate = GetRunRate();

            //Console.WriteLine($"{Name}.MoveTo({target.Name})");

            var motion = new Motion(this, target, MovementType.MoveToObject);
            motion.MoveToParameters.MovementParameters |= MovementParams.CanCharge | MovementParams.FailWalk | MovementParams.UseFinalHeading | MovementParams.Sticky | MovementParams.MoveAway;
            motion.MoveToParameters.WalkRunThreshold = 1.0f;
            motion.MoveToParameters.Speed = 1.5f;   // charge modifier
            motion.MoveToParameters.FailDistance = 15.0f;
            motion.RunRate = runRate;

            CurrentMotionState = motion;

            EnqueueBroadcastMotion(motion);

            var mvp = GetChargeParameters();
            PhysicsObj.MoveToObject(target.PhysicsObj, mvp);

            IsMoving = true;

            if (!InitMoveListener)
            {
                PhysicsObj.add_moveto_listener(OnMoveComplete);
                InitMoveListener = true;
            }

            MoveTo_Tick();
        }

        public static float MoveToRate = 0.1f;

        public void MoveTo_Tick()
        {
            //Console.WriteLine($"{Name}.MoveTo_Tick()");

            PhysicsObj.update_object();

            if (IsMoving)
                Enqueue_NextMoveTick();
        }

        public void Enqueue_NextMoveTick()
        {
            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(MoveToRate);
            actionChain.AddAction(this, MoveTo_Tick);
            actionChain.EnqueueChain();
        }

        public override void OnMoveComplete(WeenieError status)
        {
            //Console.WriteLine($"{Name}.OnMoveComplete()");
            IsMoving = false;

            switch (status)
            {
                case WeenieError.None:
                    Attack(MeleeTarget);
                    break;
                default:
                    Session.Network.EnqueueSend(new GameEventWeenieError(Session, status));
                    HandleActionCancelAttack();
                    break;
            }
        }

        public MovementParameters GetChargeParameters()
        {
            var mvp = new MovementParameters();

            // set non-default params for player melee charge
            mvp.Flags &= ~MovementParamFlags.CanWalk;
            mvp.Flags |= MovementParamFlags.CanCharge | MovementParamFlags.FailWalk | MovementParamFlags.UseFinalHeading | MovementParamFlags.Sticky | MovementParamFlags.MoveAway;
            mvp.HoldKeyToApply = HoldKey.Run;
            mvp.FailDistance = 15.0f;
            mvp.Speed = 1.5f;

            return mvp;
        }
    }
}
