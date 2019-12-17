using System;
using ACE.Entity.Enum;
using ACE.Server.Physics;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Common;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public Action<bool> MoveToCallback { get; set; }
        public bool IsPlayerMovingTo { get; set; }

        public void CreateMoveToChain(WorldObject target, Action<bool> callback, float? useRadius = null, bool rotate = true)
        {
            if (IsPlayerMovingTo)
                StopExistingMoveToChains();

            if (target.Location == null)
            {
                log.Error($"{Name}.MoveTo({target.Name}): target.Location is null");
                callback(false);
                return;
            }

            var withinUseRadius = CurrentLandblock.WithinUseRadius(this, target.Guid, out var targetValid, useRadius);

            if (withinUseRadius)
            {
                if (rotate)
                    CreateTurnToChain(target, callback);
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

            IsPlayerMovingTo = true;
            MoveToCallback = callback;

            PhysicsObj.MoveToObject(target.PhysicsObj, mvp);
            //PhysicsObj.LastMoveWasAutonomous = false;

            PhysicsObj.update_object();
        }

        public void CreateTurnToChain(WorldObject target, Action<bool> callback, bool stopCompletely = false)
        {
            if (IsPlayerMovingTo)
                StopExistingMoveToChains();

            var rotateTarget = target.GetCurrentOrWielder(CurrentLandblock);

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

            IsPlayerMovingTo = true;
            MoveToCallback = callback;

            PhysicsObj.TurnToObject(rotateTarget.PhysicsObj, mvp);
            //PhysicsObj.LastMoveWasAutonomous = false;

            PhysicsObj.update_object();
        }

        public void StopExistingMoveToChains()
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

        public void OnMoveComplete_MoveTo(WeenieError status)
        {
            if (DebugPlayerMoveToStatePhysics)
                Console.WriteLine($"{Name}.OnMoveComplete_MoveTo({status})");

            IsPlayerMovingTo = false;

            var success = status == WeenieError.None;

            if (MoveToCallback != null)
                MoveToCallback(success);

            MoveToCallback = null;
        }
    }
}
