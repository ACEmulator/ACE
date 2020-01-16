using System;
using System.Collections.Generic;
using System.Numerics;
using ACE.Entity.Enum;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics.Animation
{
    /// <summary>
    /// Motion network command interpretor
    /// </summary>
    public class MotionInterp
    {
        public bool Initted;
        public WeenieObject WeenieObj;
        public PhysicsObj PhysicsObj;
        public RawMotionState RawState;
        public InterpretedMotionState InterpretedState;
        public float CurrentSpeedFactor;
        public bool StandingLongJump;
        public float JumpExtent;
        public int ServerActionStamp;
        public float MyRunRate;
        public LinkedList<MotionNode> PendingMotions;

        public static readonly float BackwardsFactor = 6.4999998e-1f;
        public static readonly float MaxSidestepAnimRate = 3.0f;
        public static readonly float RunAnimSpeed = 4.0f;
        public static readonly float RunTurnFactor = 1.5f;
        public static readonly float SidestepAnimSpeed = 1.25f;
        public static readonly float SidestepFactor = 0.5f;
        public static readonly float WalkAnimSpeed = 3.1199999f;

        public MotionInterp() { }

        public MotionInterp(PhysicsObj obj, WeenieObject wobj)
        {
            CurrentSpeedFactor = 1.0f;
            MyRunRate = 1.0f;

            SetPhysicsObject(obj);
            SetWeenieObject(wobj);
        }

        public static MotionInterp Create(PhysicsObj obj, WeenieObject wobj)
        {
            var motionInterp = new MotionInterp(obj, wobj);
            return motionInterp;
        }

        public WeenieError DoInterpretedMotion(uint motion, MovementParameters movementParams)
        {
            if (PhysicsObj == null) return WeenieError.NoPhysicsObject;

            var result = WeenieError.None;

            if (contact_allows_move(motion))
            {
                if (StandingLongJump && (motion == (uint)MotionCommand.WalkForward || motion == (uint)MotionCommand.RunForward || motion == (uint)MotionCommand.SideStepRight))
                {
                    if (movementParams.ModifyInterpretedState)
                        InterpretedState.ApplyMotion(motion, movementParams);
                }
                else
                {
                    if (motion == (uint)MotionCommand.Dead)
                        PhysicsObj.RemoveLinkAnimations();

                    result = PhysicsObj.DoInterpretedMotion(motion, movementParams);

                    if (result == WeenieError.None)
                    {
                        var jump_error_code = WeenieError.None;

                        if (movementParams.DisableJumpDuringLink)
                            jump_error_code = WeenieError.YouCantJumpFromThisPosition;
                        else
                        {
                            jump_error_code = motion_allows_jump(motion);

                            if (jump_error_code == WeenieError.None && (motion & (uint)CommandMask.Action) == 0)
                                jump_error_code = motion_allows_jump(InterpretedState.ForwardCommand);
                        }

                        add_to_queue(movementParams.ContextID, motion, jump_error_code);

                        if (movementParams.ModifyInterpretedState)
                            InterpretedState.ApplyMotion(motion, movementParams);
                    }
                }
            }
            else
            {
                if ((motion & (uint)CommandMask.Action) != 0)
                    result = WeenieError.YouCantJumpWhileInTheAir;

                else
                {
                    if (movementParams.ModifyInterpretedState)
                        InterpretedState.ApplyMotion(motion, movementParams);

                    result = WeenieError.None;
                }
            }

            if (PhysicsObj.CurCell == null)
                PhysicsObj.RemoveLinkAnimations();

            return result;
        }

        public WeenieError DoMotion(uint motion, MovementParameters movementParams)
        {
            if (PhysicsObj == null)
                return WeenieError.NoPhysicsObject;

            // movementparams ref?
            var currentParams = new MovementParameters();
            currentParams.CopySome(movementParams);

            var currentMotion = motion;

            if (movementParams.CancelMoveTo)
                PhysicsObj.cancel_moveto();

            if (movementParams.SetHoldKey)
                SetHoldKey(movementParams.HoldKeyToApply, movementParams.CancelMoveTo);

            adjust_motion(ref currentMotion, ref currentParams.Speed, movementParams.HoldKeyToApply);

            if (InterpretedState.CurrentStyle != (uint)MotionCommand.NonCombat)
            {
                switch (motion)
                {
                    case (uint)MotionCommand.Crouch:
                        return WeenieError.CantCrouchInCombat;
                    case (uint)MotionCommand.Sitting:
                        return WeenieError.CantSitInCombat;
                    case (uint)MotionCommand.Sleeping:
                        return WeenieError.CantLieDownInCombat;
                }

                if ((motion & (uint)CommandMask.ChatEmote) != 0)
                    return WeenieError.CantChatEmoteInCombat;
            }

            if ((motion & (uint)CommandMask.Action) != 0)
            {
                if (InterpretedState.GetNumActions() >= 6)
                    return WeenieError.TooManyActions;
            }
            var result = DoInterpretedMotion(currentMotion, currentParams);

            if (result == WeenieError.None && movementParams.ModifyRawState)
                RawState.ApplyMotion(motion, movementParams);

            return result;
        }

        public void HandleExitWorld()
        {
            foreach (var pendingMotion in PendingMotions)
            {
                if (PhysicsObj != null && (pendingMotion.Motion & (uint)CommandMask.Action) != 0)
                {
                    PhysicsObj.unstick_from_object();
                    InterpretedState.RemoveAction();
                    RawState.RemoveAction();
                }
            }
            PendingMotions.Clear();
            if (PhysicsObj != null) PhysicsObj.IsAnimating = false;
        }

        public void HitGround()
        {
            if (PhysicsObj == null) return;

            if (WeenieObj != null && !WeenieObj.IsCreature()) return;

            if (!PhysicsObj.State.HasFlag(PhysicsState.Gravity)) return;

            PhysicsObj.RemoveLinkAnimations();
            apply_current_movement(false, true);
        }

        public long InqStyle()
        {
            return InterpretedState.CurrentStyle;
        }

        public void LeaveGround()
        {
            if (PhysicsObj == null) return;

            if (WeenieObj != null && !WeenieObj.IsCreature()) return;

            if (!PhysicsObj.State.HasFlag(PhysicsState.Gravity)) return;

            var velocity = get_leave_ground_velocity();
            PhysicsObj.set_local_velocity(velocity, true);

            StandingLongJump = false;
            JumpExtent = 0;

            PhysicsObj.RemoveLinkAnimations();
            apply_current_movement(false, true);
        }

        public void MotionDone(bool success)
        {
            if (PhysicsObj == null) return;

            var motionData = PendingMotions.First;

            // null or != last in list?
            if (motionData != null)
            {
                var pendingMotion = motionData.Value;
                if ((pendingMotion.Motion & (uint)CommandMask.Action) != 0)
                {
                    PhysicsObj.unstick_from_object();
                    InterpretedState.RemoveAction();
                    RawState.RemoveAction();
                }

                motionData = PendingMotions.First;
                if (motionData != null)
                {
                    PendingMotions.Remove(motionData);
                    PhysicsObj.IsAnimating = PendingMotions.Count > 0;
                }
            }
        }

        public WeenieError PerformMovement(MovementStruct mvs)
        {
            var result = WeenieError.None;

            switch (mvs.Type)
            {
                case MovementType.RawCommand:
                    result = DoMotion(mvs.Motion, mvs.Params);
                    break;
                case MovementType.InterpretedCommand:
                    result = DoInterpretedMotion(mvs.Motion, mvs.Params);
                    break;
                case MovementType.StopRawCommand:
                    result = StopMotion(mvs.Motion, mvs.Params);
                    break;
                case MovementType.StopInterpretedCommand:
                    result = StopInterpretedMotion(mvs.Motion, mvs.Params);
                    break;
                case MovementType.StopCompletely:
                    result = StopCompletely();
                    break;
                default:
                    return WeenieError.GeneralMovementFailure;
            }
            PhysicsObj.CheckForCompletedMotions();
            return result;
        }

        public void ReportExhaustion()
        {
            if (PhysicsObj == null || !Initted) return;

            if (WeenieObj == null || WeenieObj.IsCreature() && PhysicsObj.movement_is_autonomous())
                apply_raw_movement(false, false);
            else
                apply_interpreted_movement(false, false);
        }

        public void SetHoldKey(HoldKey key, bool cancelMoveTo)
        {
            if (key == RawState.CurrentHoldKey) return;
            switch (key)
            {
                case HoldKey.None:
                    if (RawState.CurrentHoldKey == HoldKey.Run)
                    {
                        RawState.CurrentHoldKey = HoldKey.None;
                        apply_current_movement(cancelMoveTo, true);
                    }
                    break;
            }
        }

        public void SetPhysicsObject(PhysicsObj obj)
        {
            PhysicsObj = obj;
            apply_current_movement(true, true);
        }

        public void SetWeenieObject(WeenieObject wobj)
        {
            WeenieObj = wobj;
            apply_current_movement(true, true);
        }

        public WeenieError StopCompletely()
        {
            if (PhysicsObj == null) return WeenieError.NoPhysicsObject;

            PhysicsObj.cancel_moveto();

            var jump = motion_allows_jump(InterpretedState.ForwardCommand);

            RawState.ForwardCommand = (uint)MotionCommand.Ready;
            RawState.ForwardSpeed = 1.0f;
            RawState.SideStepCommand = 0;
            RawState.TurnCommand = 0;

            InterpretedState.ForwardCommand = (uint)MotionCommand.Ready;
            InterpretedState.ForwardSpeed = 1.0f;
            InterpretedState.SideStepCommand = 0;
            InterpretedState.TurnCommand = 0;

            PhysicsObj.StopCompletely_Internal();

            add_to_queue(0, (uint)MotionCommand.Ready, jump);

            if (PhysicsObj.CurCell == null)
                PhysicsObj.RemoveLinkAnimations();

            return WeenieError.None;
        }

        public WeenieError StopInterpretedMotion(uint motion, MovementParameters movementParams)
        {
            if (PhysicsObj == null) return WeenieError.NoPhysicsObject;

            var result = WeenieError.None;

            if (contact_allows_move(motion))
            {
                if (StandingLongJump && (motion == (uint)MotionCommand.WalkForward || motion == (uint)MotionCommand.RunForward || motion == (uint)MotionCommand.SideStepRight))
                {
                    if (movementParams.ModifyInterpretedState)
                        InterpretedState.RemoveMotion(motion);
                }
                else
                {
                    result = PhysicsObj.StopInterpretedMotion(motion, movementParams);

                    if (result == WeenieError.None)
                    {
                        add_to_queue(movementParams.ContextID, (uint)MotionCommand.Ready, WeenieError.None);

                        if (movementParams.ModifyInterpretedState)
                            InterpretedState.RemoveMotion(motion);
                    }
                }
            }
            else
            {
                if (movementParams.ModifyInterpretedState)
                    InterpretedState.RemoveMotion(motion);
            }

            if (PhysicsObj.CurCell == null)
                PhysicsObj.RemoveLinkAnimations();

            return result;
        }

        public WeenieError StopMotion(uint motion, MovementParameters movementParams)
        {
            if (PhysicsObj == null) return WeenieError.NoPhysicsObject;

            if (movementParams.CancelMoveTo)
                PhysicsObj.cancel_moveto();

            var currentMotion = motion;
            var currentParams = new MovementParameters();
            currentParams.CopySome(movementParams);

            adjust_motion(ref currentMotion, ref currentParams.Speed, movementParams.HoldKeyToApply);

            var result = StopInterpretedMotion(currentMotion, currentParams);

            if (result == WeenieError.None && movementParams.ModifyRawState)
                RawState.RemoveMotion(motion);

            return result;
        }

        public void add_to_queue(int contextID, uint motion, WeenieError jumpErrorCode)
        {
            PendingMotions.AddLast(new MotionNode(contextID, motion, jumpErrorCode));
            PhysicsObj.IsAnimating = true;
        }

        public void adjust_motion(ref uint motion, ref float speed, HoldKey holdKey)
        {
            if (WeenieObj != null && !WeenieObj.IsCreature())
                return;

            switch (motion)
            {
                case (uint)MotionCommand.RunForward:
                    return;

                case (uint)MotionCommand.WalkBackwards:
                    motion = (uint)MotionCommand.WalkForward;
                    speed *= -BackwardsFactor;
                    break;

                case (uint)MotionCommand.TurnLeft:
                    motion = (uint)MotionCommand.TurnRight;
                    speed *= -1.0f;
                    break;

                case (uint)MotionCommand.SideStepLeft:
                    motion = (uint)MotionCommand.SideStepRight;
                    speed *= -1.0f;
                    break;
            }

            if (motion == (uint)MotionCommand.SideStepRight)
                speed *= SidestepFactor * (WalkAnimSpeed / SidestepAnimSpeed);

            if (holdKey == HoldKey.Invalid)
                holdKey = RawState.CurrentHoldKey;

            if (holdKey == HoldKey.Run)
                apply_run_to_command(ref motion, ref speed);
        }

        public void apply_current_movement(bool cancelMoveTo, bool allowJump)
        {
            if (PhysicsObj == null || !Initted) return;

            if (WeenieObj != null && !WeenieObj.IsCreature() || !PhysicsObj.movement_is_autonomous())
                apply_interpreted_movement(cancelMoveTo, allowJump);
            else
                apply_raw_movement(cancelMoveTo, allowJump);
        }

        public void apply_interpreted_movement(bool cancelMoveTo, bool allowJump)
        {
            if (PhysicsObj == null) return;

            var movementParams = new MovementParameters();

            movementParams.SetHoldKey = false;
            movementParams.ModifyInterpretedState = false;
            movementParams.CancelMoveTo = cancelMoveTo;
            movementParams.DisableJumpDuringLink = !allowJump;

            if (InterpretedState.ForwardCommand == (uint)MotionCommand.RunForward)
                MyRunRate = InterpretedState.ForwardSpeed;

            DoInterpretedMotion(InterpretedState.CurrentStyle, movementParams);

            if (contact_allows_move(InterpretedState.ForwardCommand))
            {
                if (!StandingLongJump)
                {
                    movementParams.Speed = InterpretedState.ForwardSpeed;
                    DoInterpretedMotion(InterpretedState.ForwardCommand, movementParams);

                    if (InterpretedState.SideStepCommand != 0)
                    {
                        movementParams.Speed = InterpretedState.SideStepSpeed;
                        DoInterpretedMotion(InterpretedState.SideStepCommand, movementParams);
                    }
                    else
                        StopInterpretedMotion((uint)MotionCommand.SideStepRight, movementParams);
                }
                else
                {
                    movementParams.Speed = 1.0f;
                    DoInterpretedMotion((uint)MotionCommand.Ready, movementParams);
                    StopInterpretedMotion((uint)MotionCommand.SideStepRight, movementParams);
                }
            }
            else
            {
                movementParams.Speed = 1.0f;
                DoInterpretedMotion((uint)MotionCommand.Falling, movementParams);
            }

            if (InterpretedState.TurnCommand != 0)
            {
                movementParams.Speed = InterpretedState.TurnSpeed;
                DoInterpretedMotion(InterpretedState.TurnCommand, movementParams);
            }
            else
            {
                var result = PhysicsObj.StopInterpretedMotion((uint)MotionCommand.TurnRight, movementParams);

                if (result == WeenieError.None)
                {
                    add_to_queue(movementParams.ContextID, (uint)MotionCommand.Ready, WeenieError.None);

                    if (movementParams.ModifyInterpretedState)
                        InterpretedState.RemoveMotion((uint)MotionCommand.TurnRight);
                }

                if (PhysicsObj.CurCell == null)
                    PhysicsObj.RemoveLinkAnimations();
            }
        }

        public void apply_raw_movement(bool cancelMoveTo, bool allowJump)
        {
            if (PhysicsObj == null) return;

            InterpretedState.CurrentStyle = RawState.CurrentStyle;
            InterpretedState.ForwardCommand = RawState.ForwardCommand;
            InterpretedState.ForwardSpeed = RawState.ForwardSpeed;
            InterpretedState.SideStepCommand = RawState.SideStepCommand;
            InterpretedState.SideStepSpeed = RawState.SideStepSpeed;
            InterpretedState.TurnCommand = RawState.TurnCommand;
            InterpretedState.TurnSpeed = RawState.TurnSpeed;

            adjust_motion(ref InterpretedState.ForwardCommand, ref InterpretedState.ForwardSpeed, RawState.ForwardHoldKey);
            adjust_motion(ref InterpretedState.SideStepCommand, ref InterpretedState.SideStepSpeed, RawState.SideStepHoldKey);
            adjust_motion(ref InterpretedState.TurnCommand, ref InterpretedState.TurnSpeed, RawState.TurnHoldKey);

            apply_interpreted_movement(cancelMoveTo, allowJump);
        }

        public void apply_run_to_command(ref uint motion, ref float speed)
        {
            var speedMod = 1.0f;

            if (WeenieObj != null)
            {
                var runFactor = 0.0f;
                if (WeenieObj.InqRunRate(ref runFactor))
                    speedMod = runFactor;
                else
                    speedMod = MyRunRate;
            }
            switch (motion)
            {
                case (uint)MotionCommand.WalkForward:
                    if (speed > 0.0f)
                        motion = (uint)MotionCommand.RunForward;

                    speed *= speedMod;
                    break;

                case (uint)MotionCommand.TurnRight:
                    speed *= RunTurnFactor;
                    break;

                case (uint)MotionCommand.SideStepRight:
                    speed *= speedMod;

                    if (MaxSidestepAnimRate < Math.Abs(speed))
                    {
                        if (speed > 0.0f)
                            speed = MaxSidestepAnimRate * 1.0f;
                        else
                            speed = MaxSidestepAnimRate * -1.0f;
                    }
                    break;
            }
        }

        public int charge_jump()
        {
            if (WeenieObj != null && !WeenieObj.CanJump(JumpExtent))
                return 0x49;

            var forward = InterpretedState.ForwardCommand;

            if (forward == (uint)MotionCommand.Falling || forward >= (uint)MotionCommand.Crouch && forward < (uint)MotionCommand.Sleeping)
                return 0x48;
            else
            {
                if (PhysicsObj.TransientState.HasFlag(TransientStateFlags.Contact | TransientStateFlags.OnWalkable) && forward == (uint)MotionCommand.Ready &&
                    InterpretedState.SideStepCommand == 0 && InterpretedState.TurnCommand == 0)
                {
                    StandingLongJump = true;
                }
            }
            return 0;
        }

        public bool contact_allows_move(uint motion)
        {
            if (PhysicsObj == null) return false;

            if (motion == (uint)MotionCommand.Dead || motion == (uint)MotionCommand.Falling || motion >= (uint)MotionCommand.TurnRight && motion <= (uint)MotionCommand.TurnLeft)
                return true;

            if (WeenieObj != null && !WeenieObj.IsCreature())
                return true;

            if (!PhysicsObj.State.HasFlag(PhysicsState.Gravity))
                return true;
            if (!PhysicsObj.TransientState.HasFlag(TransientStateFlags.Contact))
                return false;
            if (PhysicsObj.TransientState.HasFlag(TransientStateFlags.OnWalkable))
                return true;

            return false;
        }

        public void enter_default_state()
        {
            RawState = new RawMotionState();
            InterpretedState = new InterpretedMotionState();

            PhysicsObj.InitializeMotionTables();
            PendingMotions = new LinkedList<MotionNode>();  // ??

            add_to_queue(0, (uint)MotionCommand.Ready, 0);

            Initted = true;
            LeaveGround();
        }

        public float get_adjusted_max_speed()
        {
            var rate = 1.0f;

            if (WeenieObj != null)
            {
                if (!WeenieObj.InqRunRate(ref rate))
                    rate = MyRunRate;
            }

            if (InterpretedState.ForwardCommand == (uint)MotionCommand.RunForward)
                rate = InterpretedState.ForwardSpeed / CurrentSpeedFactor;

            return rate * 4.0f;
        }

        public float get_jump_v_z()
        {
            if (JumpExtent < PhysicsGlobals.EPSILON)
                return 0.0f;

            var extent = JumpExtent;

            if (extent > 1.0f)
                extent = 1.0f;

            if (WeenieObj == null)
                return 10.0f;

            float vz = extent;
            if (WeenieObj.InqJumpVelocity(extent, out vz))
                return vz;

            return 0.0f;
        }

        public Vector3 get_leave_ground_velocity()
        {
            var velocity = get_state_velocity();
            velocity.Z = get_jump_v_z();

            if (Vec.IsZero(velocity))
                velocity = PhysicsObj.Position.GlobalToLocalVec(PhysicsObj.Velocity);

            return velocity;
        }

        public float get_max_speed()
        {
            var rate = 1.0f;

            if (WeenieObj != null)
            {
                if (!WeenieObj.InqRunRate(ref rate))
                    rate = MyRunRate;
            }

            return RunAnimSpeed * rate;
        }

        public Vector3 get_state_velocity()
        {
            var velocity = Vector3.Zero;

            if (InterpretedState.SideStepCommand == (uint)MotionCommand.SideStepRight)
                velocity.X = SidestepAnimSpeed * InterpretedState.SideStepSpeed;
            if (InterpretedState.ForwardCommand == (uint)MotionCommand.WalkForward)
                velocity.Y = WalkAnimSpeed * InterpretedState.ForwardSpeed;
            else if (InterpretedState.ForwardCommand == (uint)MotionCommand.RunForward)
                velocity.Y = RunAnimSpeed * InterpretedState.ForwardSpeed;

            var rate = MyRunRate;

            if (WeenieObj != null) WeenieObj.InqRunRate(ref rate);

            var maxSpeed = RunAnimSpeed * rate;
            if (velocity.Length() > maxSpeed)
            {
                velocity = Vector3.Normalize(velocity);
                velocity *= maxSpeed;
            }
            return velocity;
        }

        public bool is_standing_still()
        {
            return PhysicsObj.TransientState.HasFlag(TransientStateFlags.Contact | TransientStateFlags.OnWalkable) &&
                InterpretedState.ForwardCommand == (uint)MotionCommand.Ready &&
                InterpretedState.SideStepCommand == 0 &&
                InterpretedState.TurnCommand == 0;
        }

        public WeenieError jump(float extent, int adjustStamina)
        {
            if (PhysicsObj == null) return WeenieError.NoPhysicsObject;

            PhysicsObj.cancel_moveto();

            var result = jump_is_allowed(extent, adjustStamina);

            if (result == WeenieError.None)
            {
                JumpExtent = extent;
                PhysicsObj.set_on_walkable(false);
            }
            else
                StandingLongJump = false;

            return result;
        }

        public WeenieError jump_charge_is_allowed()
        {
            if (WeenieObj != null && !WeenieObj.CanJump(JumpExtent))
                return WeenieError.CantJumpLoadedDown;

            var forward = InterpretedState.ForwardCommand;

            if (forward == (uint)MotionCommand.Fallen || forward >= (uint)MotionCommand.Crouch && forward <= (uint)MotionCommand.Sleeping)
                return WeenieError.YouCantJumpFromThisPosition;

            return WeenieError.None;
        }

        public WeenieError jump_is_allowed(float extent, int staminaCost)
        {
            if (PhysicsObj == null)
                return WeenieError.NoPhysicsObject;

            if (WeenieObj == null && !WeenieObj.IsCreature() || !PhysicsObj.State.HasFlag(PhysicsState.Gravity) ||
                PhysicsObj.TransientState.HasFlag(TransientStateFlags.Contact | TransientStateFlags.OnWalkable))
            {
                if (PhysicsObj.IsFullyConstrained())
                    return WeenieError.GeneralMovementFailure;

                if (PendingMotions.Count > 1 && PendingMotions.First.Value.JumpErrorCode != 0)
                    return PendingMotions.First.Value.JumpErrorCode;

                var jumpError = jump_charge_is_allowed();

                if (jumpError == WeenieError.None)
                {
                    jumpError = motion_allows_jump(InterpretedState.ForwardCommand);

                    if (jumpError == WeenieError.None && WeenieObj != null && WeenieObj.JumpStaminaCost(extent, staminaCost) == 0)
                        jumpError = WeenieError.GeneralMovementFailure;
                }
                return jumpError;
            }
            return WeenieError.YouCantJumpWhileInTheAir;
        }

        public WeenieError motion_allows_jump(uint substate)
        {
            if (substate >= (uint)MotionCommand.Reload && substate <= (uint)MotionCommand.Pickup || substate >= (uint)MotionCommand.TripleThrustLow && substate <= (uint)MotionCommand.MagicPowerUp07Purple ||
                substate >= (uint)MotionCommand.MagicPowerUp01 && substate <= (uint)MotionCommand.MagicPowerUp10 || substate >= (uint)MotionCommand.Crouch && substate <= (uint)MotionCommand.Sleeping ||
                substate >= (uint)MotionCommand.AimLevel && substate <= (uint)MotionCommand.MagicPray || substate == (uint)MotionCommand.Falling)
            {
                return WeenieError.YouCantJumpFromThisPosition;
            }
            return WeenieError.None;
        }

        /// <summary>
        /// Alternatively, you can use PhysicsObj.IsAnimating for better performance.
        /// </summary>
        public bool motions_pending()
        {
            return PendingMotions.Count > 0;
        }

        public bool move_to_interpreted_state(InterpretedMotionState state)
        {
            if (PhysicsObj == null) return false;

            RawState.CurrentStyle = state.CurrentStyle;
            PhysicsObj.cancel_moveto();

            var allowJump = motion_allows_jump(InterpretedState.ForwardCommand) == WeenieError.None ? true : false;

            InterpretedState.copy_movement_from(state);
            apply_current_movement(true, allowJump);

            var movementParams = new MovementParameters();

            foreach (var action in state.Actions)
            {
                var currentStamp = action.Stamp & 0x7FFF;
                var serverStamp = ServerActionStamp & 0x7FFFF;

                var deltaStamp = Math.Abs(currentStamp - serverStamp);

                var diff = deltaStamp <= 0x3FFF ? serverStamp < currentStamp : currentStamp < serverStamp;

                if (diff)
                {
                    if (WeenieObj != null && WeenieObj.IsCreature() || action.Autonomous)
                    {
                        ServerActionStamp = action.Stamp;
                        movementParams.Speed = action.Speed;
                        movementParams.Autonomous = action.Autonomous;
                        DoInterpretedMotion(action.Action, movementParams);
                    }
                }
            }
            return true;
        }

        public void set_hold_run(int val, bool cancelMoveTo)
        {
            if ((val == 0) != (RawState.CurrentHoldKey != HoldKey.Run))
            {
                RawState.CurrentHoldKey = val != 0 ? HoldKey.Run : HoldKey.None;
                apply_current_movement(cancelMoveTo, true);
            }
        }
    }
}
