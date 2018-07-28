using System;
using System.Collections.Generic;
using System.Text;
using ACE.Server.Entity.Actions;
using ACE.Server.Physics.Common;
using ACE.Server.Physics.Util;
using System.Numerics;
using ACE.Entity;
using ACE.Server.Managers;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        /// <summary>
        /// Starts the process of monster turning towards target
        /// using the simplified method
        /// </summary>
        public void StartTurnSimple()
        {
            if (DebugMove)
                Console.WriteLine($"{Name} ({Guid}) - StartTurn");

            if (MoveSpeed == 0.0f)
                GetMovementSpeed();

            IsTurning = true;

            if (IsRanged)
                TurnTo(AttackTarget);
            else
                MoveTo(AttackTarget, RunRate);

            var time = EstimateTurnTo();

            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(time);
            actionChain.AddAction(this, () => OnTurnComplete());
            actionChain.EnqueueChain();
        }

        public void MovementSimple()
        {
            if (!IsRanged)
            {
                UpdatePosition();
                LastMoveTime = Timer.CurrentTime;
            }

            if (IsAttackRange())
                OnMoveComplete();

            if (GetDistanceToTarget() >= MaxChaseRange)
                Sleep();
        }

        /// <summary>
        /// Updates monster position and rotation
        /// </summary>
        public void UpdatePositionSimple()
        {
            // determine the time interval for this movement
            var deltaTime = (float)(Timer.CurrentTime - LastMoveTime);
            if (deltaTime > 2.0f) return;   // FIXME: state persist?

            var dir = Vector3.Normalize(AttackTarget.Location.GlobalPos - Location.GlobalPos);
            var movement = dir * deltaTime * MoveSpeed;
            var newPos = Location.Pos + movement;

            // stop at destination
            var dist = GetDistanceToTarget();
            if (movement.Length() > dist)
                newPos = GetDestination();

            //UpdatePosition_Inner(newPos, dir);
            UpdatePosition_PhysicsInner(newPos, dir);

            // set cached velocity
            var velocity = movement / deltaTime;
            PhysicsObj.CachedVelocity = velocity;

            SendUpdatePosition(ForcePos);
            //MoveTo(AttackTarget, RunRate, true);

            if (DebugMove)
                Console.WriteLine($"{Name} ({Guid}) - UpdatePositionSimple");
        }

        public void UpdatePosition_Inner(Vector3 newPos, Vector3 dir)
        {
            // update position, and landblock if required
            Location.Rotate(dir);
            var blockCellUpdate = Location.SetPosition(newPos);
            if (Location.Indoors)
                UpdateIndoorCells(newPos);
            PhysicsObj.Position.Frame.Origin = newPos;
            if (blockCellUpdate.Item1)
                UpdateLandblock();
            if (blockCellUpdate.Item1 || blockCellUpdate.Item2)
                UpdateCell();
        }

        /// <summary>
        /// Updates the position using the simple physics method
        /// </summary>
        public void UpdatePosition_PhysicsInner(Vector3 requestPos, Vector3 dir)
        {
            Location.Rotate(dir);
            PhysicsObj.Position.Frame.Orientation = Location.Rotation;

            var cell = LScape.get_landcell(Location.Cell);

            PhysicsObj.set_request_pos(requestPos, Location.Rotation, null, Location.LandblockId.Raw);

            // simulate running forward for this amount of time
            PhysicsObj.update_object_server(false);

            UpdatePosition_SyncLocation();
        }

        public void UpdateIndoorCells(Vector3 newPos)
        {
            var adjustCell = AdjustCell.Get(Location.LandblockId.Raw >> 16);
            if (adjustCell == null) return;
            var newCell = adjustCell.GetCell(newPos);
            if (newCell == null) return;
            if (newCell.Value == Location.LandblockId.Raw) return;
            Location.LandblockId = new LandblockId(newCell.Value);
            //Console.WriteLine("Moving " + Name + " to indoor cell " + newCell.Value.ToString("X8"));
        }

        /// <summary>
        /// Called when a monster changes landblocks
        /// </summary>
        public void UpdateLandblock()
        {
            PreviousLocation = Location;
            LandblockManager.RelocateObjectForPhysics(this);
            //Console.WriteLine("Relocating " + Name + " to " + Location.LandblockId);
        }

        /// <summary>
        /// Called when a monster changes cells
        /// </summary>
        public void UpdateCell()
        {
            var curCell = LScape.get_landcell(Location.LandblockId.Raw);
            //Console.WriteLine("Moving " + Name + " to " + curCell.ID.ToString("X8"));
            PhysicsObj.change_cell_server(curCell);
            //PhysicsObj.remove_shadows_from_cells();
            PhysicsObj.add_shadows_to_cell(curCell);
        }

    }
}
