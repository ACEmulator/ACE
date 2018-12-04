using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

using ACE.Server.Physics.Combat;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics.Animation
{
    public class StickyManager
    {
        public uint TargetID;
        public float TargetRadius;
        public Position TargetPosition;
        public PhysicsObj PhysicsObj;
        public bool Initialized;
        public double StickyTimeoutTime;

        public List<Action> StickyCallbacks;
        public List<Action> UnstickyCallbacks;

        public static readonly float StickyRadius = 0.3f;

        public static readonly float StickyTime = 1.0f;

        public StickyManager()
        {
            Init();
        }

        public StickyManager(PhysicsObj obj)
        {
            Init();
            SetPhysicsObject(obj);
        }

        public void Init()
        {
            StickyCallbacks = new List<Action>();
            UnstickyCallbacks = new List<Action>();
        }

        public void ClearTarget()
        {
            if (TargetID == 0) return;

            TargetID = 0;
            Initialized = false;

            PhysicsObj.clear_target();
            PhysicsObj.cancel_moveto();

            foreach (var unstickyCallback in UnstickyCallbacks.ToList())
                unstickyCallback();
        }

        public static StickyManager Create(PhysicsObj obj)
        {
            return new StickyManager(obj);
        }

        public void HandleExitWorld()
        {
            ClearTarget();
        }

        public void HandleUpdateTarget(TargetInfo targetInfo)
        {
            if (targetInfo.ObjectID != TargetID)
                return;

            if (targetInfo.Status == TargetStatus.OK)
            {
                Initialized = true;
                TargetPosition = targetInfo.TargetPosition;
            }
            else if (TargetID != 0)
                ClearTarget();
        }

        public void SetPhysicsObject(PhysicsObj obj)
        {
            PhysicsObj = obj;
        }

        public void StickTo(uint objectID, float targetRadius, float targetHeight)
        {
            if (TargetID != 0)
                ClearTarget();

            TargetID = objectID;
            Initialized = false;
            TargetRadius = targetRadius;
            StickyTimeoutTime = PhysicsTimer.CurrentTime + StickyTime;
            PhysicsObj.set_target(0, objectID, 0.5f, 0.5f);

            foreach (var stickyCallback in StickyCallbacks.ToList())
                stickyCallback();
        }

        public void UseTime()
        {
            if (PhysicsTimer.CurrentTime > StickyTimeoutTime)
                ClearTarget();
        }

        public void adjust_offset(AFrame offset, double quantum)
        {
            if (PhysicsObj == null || TargetID == 0 || !Initialized)
                return;

            var target = PhysicsObj.GetObjectA(TargetID);
            var targetPosition = target == null ? TargetPosition : target.Position;

            offset.Origin = PhysicsObj.Position.GetOffset(targetPosition);
            offset.Origin = PhysicsObj.Position.GlobalToLocalVec(offset.Origin);
            offset.Origin.Z = 0.0f;

            var radius = PhysicsObj.GetRadius();
            var dist = Position.CylinderDistanceNoZ(radius, PhysicsObj.Position, TargetRadius, targetPosition) - StickyRadius;

            if (Vec.NormalizeCheckSmall(ref offset.Origin))
                offset.Origin = Vector3.Zero;

            var speed = 0.0f;
            var minterp = PhysicsObj.get_minterp();
            if (minterp != null)
                speed = minterp.get_max_speed() * 5.0f;

            if (speed < PhysicsGlobals.EPSILON)
                speed = 15.0f;

            var delta = speed * (float)quantum;
            if (delta >= Math.Abs(dist))
                delta = dist;

            offset.Origin *= delta;

            var curHeading = PhysicsObj.Position.Frame.get_heading();
            var targetHeading = PhysicsObj.Position.heading(targetPosition);
            var heading = targetHeading - curHeading;
            if (Math.Abs(heading) < PhysicsGlobals.EPSILON)
                heading = 0.0f;
            if (heading < -PhysicsGlobals.EPSILON)
                heading += 360.0f;

            //Console.WriteLine($"StickyManager.AdjustOffset(targetHeading={targetHeading}, curHeading={curHeading}, setHeading={heading})");
            offset.set_heading(heading);
        }

        public void add_sticky_listener(Action listener)
        {
            StickyCallbacks.Add(listener);
        }

        public void remove_sticky_listener(Action listener)
        {
            StickyCallbacks.Remove(listener);
        }

        public void add_unsticky_listener(Action listener)
        {
            UnstickyCallbacks.Add(listener);
        }

        public void remove_unsticky_listener(Action listener)
        {
            UnstickyCallbacks.Remove(listener);
        }
    }
}
