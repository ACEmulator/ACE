using System;

using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Physics;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Common;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public bool IsPlayerMovingTo2 { get; set; }

        public MoveToParams MoveToParams { get; set; }

        public void CreateMoveToChain2(WorldObject target, Action<bool> callback, float? useRadius = null, bool rotate = true)
        {
            if (IsPlayerMovingTo2)
                StopExistingMoveToChains2();

            if (MoveToParams != null)
                CheckMoveToParams();

            if (target.Location == null)
            {
                log.Error($"{Name}.MoveTo({target.Name}): target.Location is null");
                callback(false);
                return;
            }

            if (useRadius == null)
                useRadius = target.UseRadius ?? 0.6f;

            var withinUseRadius = CurrentLandblock.WithinUseRadius(this, target.Guid, out var targetValid, useRadius);

            if (withinUseRadius)
            {
                if (rotate)
                    CreateTurnToChain2(target, callback, useRadius);
                else
                    callback(true);

                return;
            }

            // send command to client
            MoveToObject(target);

            // start on server
            // forward this to PhysicsObj.MoveManager.MoveToManager
            var mvp = GetMoveToParams(target, useRadius);

            if (!PhysicsObj.IsMovingOrAnimating)
                //PhysicsObj.UpdateTime = PhysicsTimer.CurrentTime - PhysicsGlobals.MinQuantum;
                PhysicsObj.UpdateTime = PhysicsTimer.CurrentTime;

            IsPlayerMovingTo2 = true;

            MoveToParams = new MoveToParams(callback, target, useRadius);

            PhysicsObj.MoveToObject(target.PhysicsObj, mvp);
            //PhysicsObj.LastMoveWasAutonomous = false;

            PhysicsObj.update_object();
        }

        public void CreateTurnToChain2(WorldObject target, Action<bool> callback, float? useRadius = null, bool stopCompletely = false, bool alwaysTurn = false)
        {
            if (IsPlayerMovingTo2)
                StopExistingMoveToChains2();

            if (MoveToParams != null)
                CheckMoveToParams();

            var rotateTarget = target.Wielder ?? target;

            if (rotateTarget.Location == null)
            {
                log.Error($"{Name}.TurnTo({target.Name}): target.Location is null");
                callback(false);
                return;
            }

            if (DebugPlayerMoveToStatePhysics)
                Console.WriteLine("*** CreateTurnToChain ***");

            // send command to client
            TurnToObject(rotateTarget, stopCompletely);

            // start on server
            // forward this to PhysicsObj.MoveManager.MoveToManager
            var mvp = GetTurnToParams(stopCompletely);

            if (!PhysicsObj.IsMovingOrAnimating)
                //PhysicsObj.UpdateTime = PhysicsTimer.CurrentTime - PhysicsGlobals.MinQuantum;
                PhysicsObj.UpdateTime = PhysicsTimer.CurrentTime;

            IsPlayerMovingTo2 = true;

            MoveToParams = new MoveToParams(callback, target, useRadius);

            PhysicsObj.MovementManager.MoveToManager.AlwaysTurn = alwaysTurn;

            PhysicsObj.TurnToObject(rotateTarget.PhysicsObj, mvp);
            //PhysicsObj.LastMoveWasAutonomous = false;

            PhysicsObj.update_object();

            PhysicsObj.MovementManager.MoveToManager.AlwaysTurn = false;
        }

        public void StopExistingMoveToChains2()
        {
            //Console.WriteLine($"CancelMoveTo");
            PhysicsObj.cancel_moveto();
        }

        public MovementParameters GetMoveToParams(WorldObject target, float? useRadius = null)
        {
            var mvp = new MovementParameters();
            mvp.DistanceToObject = useRadius ?? target.UseRadius ?? 0.6f;

            // copied from Creature.SetWalkRunTreshold
            var dist = Location.DistanceTo(target.Location);
            if (dist >= mvp.WalkRunThreshold / 2.0f)
                mvp.CanRun = false;

            // move directly to portal origin
            //if (target is Portal)
            //mvp.UseSpheres = false;

            return mvp;
        }

        public MovementParameters GetTurnToParams(bool stopCompletely = false)
        {
            var mvp = new MovementParameters();

            //mvp.HoldKeyToApply = HoldKey.Run;
            mvp.StopCompletely = stopCompletely;
            //mvp.ModifyInterpretedState = false;

            return mvp;
        }

        public void OnMoveComplete_MoveTo2(WeenieError status)
        {
            if (DebugPlayerMoveToStatePhysics)
                Console.WriteLine($"{Name}.OnMoveComplete_MoveTo({status})");

            IsPlayerMovingTo2 = false;
            
            if (MoveToParams == null || MoveToParams.Callback == null)
            {
                // nothing to do -- we are done here
                MoveToParams = null;
                return;
            }

            var success = status == WeenieError.None;

            if (success)
            {
                MoveToParams.Callback(true);
                MoveToParams = null;
            }

            // if action cancelled, check again when player is stationary
            // through Player_Tick -> HandleMoveToCallback
        }

        public void CheckMoveToParams()
        {
            // because of the additional gap, it is now possible to queue up actions
            // we don't want to queue up multiple actions, but we still need to process the queue,
            // to prevent busy state on client

            // fail pending action
            if (MoveToParams.Callback != null)
                MoveToParams.Callback(false);

            MoveToParams = null;
        }

        public void HandleMoveToCallback()
        {
            var isFacing = IsFacing(MoveToParams.Target);

            var withinUseRadius = MoveToParams.UseRadius == null || CurrentLandblock.WithinUseRadius(this, MoveToParams.Target.Guid, out _, MoveToParams.UseRadius);

            var success = isFacing && withinUseRadius;

            MoveToParams.Callback(success);

            MoveToParams = null;
        }

        public void ClearRawState()
        {
            if (!FastTick) return;

            // clear raw state to mitigate client bugs
            // repro steps: hold strafe, drop an item, release strafe while crouching down
            // client does not send release command, similar to releasing movement key during stance swap

            // the opposite of this bug can be seen by holding 'run forward', dropping an item, and continuing to hold run forward during and after the drop
            // on the self-client, the player will resume running forward
            // on the server, and from the perspective of other clients, the player will not be moving, and will 'blip forward' every 1.875s
            // when it receives AutoPos updates from self-client

            // this will create consistent behavior on amongst the self-client / server / other clients for the original scenario,
            // but will introduce a desync bug between the self-client and server/other clients if the player were to continue to hold strafe after the drop
            // it turns that scenario into the 'opposite bug' though, which is the lesser of 2 evils compared to the original bug

            CurrentMotionState.SetTurnCommand(MotionCommand.Invalid);
            CurrentMotionState.SetSidestepCommand(MotionCommand.Invalid);

            var rawState = PhysicsObj.InqRawMotionState();

            rawState.TurnCommand = 0;
            rawState.TurnHoldKey = HoldKey.Invalid;
            rawState.TurnSpeed = 1.0f;

            rawState.SideStepCommand = 0;
            rawState.SideStepHoldKey = HoldKey.Invalid;
            rawState.TurnSpeed = 1.0f;
        }
    }
}
