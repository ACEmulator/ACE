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

        public void CreateMoveToChain(WorldObject target, Action<bool> callback, float? useRadius = null)
        {
            if (IsPlayerMovingTo)
                StopExistingMoveToChains();

            if (target.Location == null)
            {
                log.Error($"{Name}.MoveTo({target.Name}): target.Location is null");
                callback(false);
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

            PhysicsObj.update_object();
        }

        public void CreateTurnToChain(WorldObject target, Action<bool> callback)
        {
            if (IsPlayerMovingTo)
                StopExistingMoveToChains();

            if (target.Location == null)
            {
                log.Error($"{Name}.TurnTo({target.Name}): target.Location is null");
                callback(false);
                return;
            }

            if (DebugPlayerMoveToStatePhysics)
                Console.WriteLine("*** CreateTurnToChain ***");

            // send command to client
            TurnToObject(target);

            // start on server
            // forward this to PhysicsObj.MoveManager.MoveToManager
            var mvp = GetTurnToParams();

            if (!PhysicsObj.IsMovingOrAnimating)
                //PhysicsObj.UpdateTime = PhysicsTimer.CurrentTime - PhysicsGlobals.MinQuantum;
                PhysicsObj.UpdateTime = PhysicsTimer.CurrentTime;

            IsPlayerMovingTo = true;
            MoveToCallback = callback;

            PhysicsObj.TurnToObject(target.Guid.Full, mvp);

            PhysicsObj.update_object();
        }

        public void StopExistingMoveToChains()
        {
            PhysicsObj.cancel_moveto();
        }

        public MovementParameters GetMoveToParams(WorldObject target, float? useRadius = null)
        {
            var mvp = new MovementParameters();
            mvp.DistanceToObject = useRadius ?? target.UseRadius ?? 0.6f;

            // copied from Creature.SetWalkRunTreshold
            var dist = Location.DistanceTo(target.Location);
            if (dist >= mvp.WalkRunThreshold / 2.0f)
                mvp.CanCharge = false;

            // move directly to portal origin
            //if (target is Portal)
                //mvp.UseSpheres = false;

            return mvp;
        }

        public MovementParameters GetTurnToParams()
        {
            var mvp = new MovementParameters();

            //mvp.HoldKeyToApply = HoldKey.Run;
            //mvp.StopCompletely = false;
            //mvp.ModifyInterpretedState = false;

            return mvp;
        }

        public void OnMoveComplete_MoveTo(WeenieError status, int cycles)
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
