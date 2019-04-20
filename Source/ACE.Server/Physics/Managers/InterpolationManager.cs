using System.Collections.Generic;
using System.Linq;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics.Animation
{
    public class InterpolationManager
    {
        public LinkedList<InterpolationNode> PositionQueue;
        public PhysicsObj PhysicsObj;
        public bool KeepHeading;
        public int FrameCounter;
        public float OriginalDistance;
        public float ProgressQuantum;
        public int NodeFailCounter;
        public Position BlipToPosition;

        public static readonly float LargeDistance = 999999.0f;
        public static readonly float MaxInterpolatedVelocity = 7.5f;

        public static bool UseAdjustedSpeed = true;

        public InterpolationManager() { }

        public InterpolationManager(PhysicsObj obj)
        {
            OriginalDistance = LargeDistance;
            SetPhysicsObject(obj);
        }

        public static InterpolationManager Create(PhysicsObj obj)
        {
            return new InterpolationManager(obj);
        }

        public void InterpolateTo(Position position, bool keepHeading)
        {
            if (PhysicsObj == null)
                return;

            var dest = PositionQueue.Count > 0 && PositionQueue.Last.Value.Type == InterpolationNodeType.PositionType ?
                PositionQueue.Last.Value.Position : PhysicsObj.Position;

            var dist = dest.Distance(position);

            if (PhysicsObj.GetAutonomyBlipDistance() >= dist)
            {
                if (PhysicsObj.Position.Distance(position) > 0.05f)
                {
                    while (PositionQueue.Count > 0)
                    {
                        var lastNode = PositionQueue.Last.Value;
                        if (lastNode.Type != InterpolationNodeType.PositionType || lastNode.Position.Distance(position) >= 0.05f)
                            break;

                        PositionQueue.RemoveLast();
                    }
                    while (PositionQueue.Count >= 20)
                        PositionQueue.RemoveFirst();

                    var interpolationNode = new InterpolationNode(InterpolationNodeType.PositionType, position);
                    if (keepHeading)
                        interpolationNode.Position.Frame.set_heading(PhysicsObj.get_heading());

                    PositionQueue.AddLast(interpolationNode);
                }
                else
                {
                    if (!keepHeading)
                        PhysicsObj.set_heading(position.Frame.get_heading(), true);

                    StopInterpolating();
                }
            }
            else
            {
                var interpolationNode = new InterpolationNode(InterpolationNodeType.PositionType, position);
                if (keepHeading)
                    interpolationNode.Position.Frame.set_heading(PhysicsObj.get_heading());

                PositionQueue.AddLast(interpolationNode);
                NodeFailCounter = 4;
            }
        }

        public bool IsInterpolating()
        {
            return PositionQueue.Count > 0;
        }

        public void NodeCompleted(bool success)
        {
            if (PhysicsObj == null)
                return;

            FrameCounter = 0;
            ProgressQuantum = 0.0f;

            var head = PositionQueue.Count == 0 ? null : PositionQueue.First.Value;
            var next = PositionQueue.Count <= 1 ? null : PositionQueue.ElementAt(1);

            if (PositionQueue.Count > 1)
            {
                if (next.Type == InterpolationNodeType.PositionType)
                    OriginalDistance = PhysicsObj.Position.Distance(next.Position);

                else if (!success)
                {
                    if (head == null) return;
                    BlipToPosition = head.Position;
                }
            }
            else
            {
                OriginalDistance = LargeDistance;
                if (!success)
                {
                    if (head == null) return;
                    BlipToPosition = head.Position;
                }
                else
                    StopInterpolating();
            }
            if (PositionQueue.Count > 0)
                PositionQueue.RemoveFirst();
        }

        public void SetPhysicsObject(PhysicsObj obj)
        {
            PhysicsObj = obj;
        }

        public void StopInterpolating()
        {
            PositionQueue.Clear();
            OriginalDistance = LargeDistance;
            FrameCounter = 0;
            ProgressQuantum = 0.0f;
            NodeFailCounter = 0;
        }

        public void UseTime()
        {
            if (PhysicsObj == null)
                return;

            if (NodeFailCounter > 3 || PositionQueue.Count == 0)
            {
                if (NodeFailCounter <= 0) return;

                var last = PositionQueue.Last.Value;
                if (last.Type != InterpolationNodeType.JumpType && last.Type != InterpolationNodeType.VelocityType)
                {
                    if (PhysicsObj.SetPositionSimple(last.Position, true) != SetPositionError.OK)
                        return;

                    StopInterpolating();
                    return;
                }

                if (PositionQueue.Count > 1)
                {
                    for (var i = PositionQueue.Count; i >= 0; --i)
                    {
                        var node = PositionQueue.ElementAt(i);
                        if (node.Type == InterpolationNodeType.PositionType)
                        {
                            if (PhysicsObj.SetPositionSimple(node.Position, true) != SetPositionError.OK)
                                return;

                            PhysicsObj.set_velocity(last.Velocity, true);
                            StopInterpolating();
                            return;
                        }
                    }
                }

                if (PhysicsObj.SetPositionSimple(BlipToPosition, true) != SetPositionError.OK)
                    return;

                StopInterpolating();
                return;
            }

            var first = PositionQueue.First.Value;
            switch (first.Type)
            {
                case InterpolationNodeType.JumpType:
                    NodeCompleted(true);
                    break;

                case InterpolationNodeType.VelocityType:
                    PhysicsObj.set_velocity(first.Velocity, true);
                    NodeCompleted(true);
                    break;
            }
        }

        public void adjust_offset(AFrame frame, double quantum)
        {
            if (PositionQueue.Count == 0 || PhysicsObj == null || !PhysicsObj.TransientState.HasFlag(TransientStateFlags.Contact))
                return;

            var first = PositionQueue.First.Value;
            if (first.Type == InterpolationNodeType.JumpType || first.Type == InterpolationNodeType.VelocityType)
                return;

            var dist = PhysicsObj.Position.Distance(first.Position);
            if (dist < 0.05f)
            {
                NodeCompleted(true);
                return;
            }
            var maxSpeed = 0.0f;
            var minterp = PhysicsObj.get_minterp();
            if (minterp != null)
            {
                if (UseAdjustedSpeed)
                    maxSpeed = minterp.get_adjusted_max_speed() * 2.0f;
                else
                    maxSpeed = minterp.get_max_speed() * 2.0f;
            }
            if (maxSpeed < PhysicsGlobals.EPSILON)
                maxSpeed = MaxInterpolatedVelocity;

            var delta = OriginalDistance - dist;
            ProgressQuantum += (float)quantum;
            FrameCounter++;

            if (FrameCounter < 5 || (PhysicsObj.get_sticky_object() != 0 ||
                delta > PhysicsGlobals.EPSILON && delta / ProgressQuantum / maxSpeed >= 0.3f))
            {
                if (FrameCounter >= 5)
                {
                    FrameCounter = 0;
                    ProgressQuantum = 0.0f;
                    OriginalDistance = dist;
                }

                var offset = first.Position.Subtract(PhysicsObj.Position);
                var maxQuantum = maxSpeed * quantum;
                var distance = offset.Origin.Length();

                if (distance <= 0.05f)
                    NodeCompleted(true);

                if (distance > maxQuantum)
                    offset.Origin *= (float)maxQuantum / distance;

                if (KeepHeading)
                    offset.set_heading(0.0f);

                frame = offset;
                return;
            }
            NodeFailCounter++;
            NodeCompleted(false);
        }
    }
}
