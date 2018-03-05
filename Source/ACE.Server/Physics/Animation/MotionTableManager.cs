using System.Collections.Generic;
using System.Linq;

namespace ACE.Server.Physics.Animation
{
    public class MotionTableManager
    {
        public PhysicsObj PhysicsObj;
        public MotionTable Table;
        public MotionState State;
        public int AnimationCounter;
        public List<AnimNode> PendingAnimations;    // AnimSequenceNode?

        public MotionTableManager() { }

        public MotionTableManager(int mtableID)
        {
            if (mtableID != 0)
                SetMotionTableID(mtableID);
        }

        public void AnimationDone(bool success)
        {
            var node = PendingAnimations.First();
            while (node != null)
            {
                AnimationCounter++;
                do
                {
                    if (node.NumAnims > AnimationCounter)
                        break;

                    if ((node.Motion & 0x10000000) != 0)
                        State.remove_action_head();

                    var motionID = node.Motion;
                    PhysicsObj.MotionDone(motionID, success);
                    AnimationCounter -= node.NumAnims;

                    if (PendingAnimations.Count > 0)
                        PendingAnimations.RemoveAt(0);

                    if (PhysicsObj.WeenieObj != null)
                        PhysicsObj.WeenieObj.OnMotionDone(motionID, success);

                    node = PendingAnimations.First();
                }
                while (node != null);

                if (AnimationCounter != 0 && node == null)
                    AnimationCounter = 0;
            }
        }

        public void CheckForCompletedMotions()
        {
            while (PendingAnimations.Count > 0)
            {
                var pendingAnimation = PendingAnimations[0];

                if (pendingAnimation == null)
                    return;

                var motionID = pendingAnimation.Motion;

                if ((motionID & 0x10000000) != 0)
                    State.remove_action_head();

                PhysicsObj.MotionDone(motionID, true);

                if (PendingAnimations.Count > 0)
                    PendingAnimations.RemoveAt(0);

                if (PhysicsObj.WeenieObj != null)
                    PhysicsObj.WeenieObj.OnMotionDone(motionID, true);
            }
        }

        public static MotionTableManager Create(int mtableID)
        {
            return new MotionTableManager(mtableID);
        }

        public int GetMotionTableID(int mtableID)
        {
            return Table == null ? 0 : Table.ID;
        }

        public void HandleEnterWorld(Sequence sequence)
        {
            sequence.remove_all_link_animations();
            while (PendingAnimations.Count > 0)
                AnimationDone(false);
        }

        public void HandleExitWorld()
        {
            while (PendingAnimations.Count > 0)
                AnimationDone(false);
        }

        public Sequence PerformMovement(MovementStruct mvs, Sequence seq)
        {
            if (Table == null) return new Sequence(0x7);

            var counter = 0;
            switch (mvs.Type)
            {
                case MovementType.InterpretedCommand:
                    if (!Table.DoObjectMotion(mvs.Motion, State, seq, mvs.Params.Speed, ref counter))
                        return new Sequence(0x43);

                    add_to_queue(mvs.Motion, counter, seq);
                    return null;

                case MovementType.StopInterpretedCommand:
                    if (!Table.StopObjectMotion(mvs.Motion, mvs.Params.Speed, State, seq, ref counter))
                        return new Sequence(0x43);

                    add_to_queue(0x41000003, counter, seq);
                    return null;

                case MovementType.StopCompletely:
                    Table.StopObjectCompletely(State, seq, ref counter);
                    add_to_queue(0x41000003, counter, seq);
                    return null; ;

                default:
                    return seq;
            }
        }

        public bool SetMotionTableID(int mtableID)
        {
            Table = MotionTable.Get(mtableID);
            return Table != null;
        }

        public void SetPhysicsObject(PhysicsObj obj)
        {
            PhysicsObj = obj;
        }

        public void UseTime()
        {
            CheckForCompletedMotions();
        }

        public void add_to_queue(int motion, int num_anims, Sequence sequence)
        {
            PendingAnimations.Add(new AnimNode(motion, num_anims));
            remove_redundant_links(sequence);
        }

        public void initialize_state(Sequence sequence)
        {
            var numAnims = 0;

            if (Table != null)
                Table.SetDefaultState(State, sequence, ref numAnims);

            add_to_queue(0x41000003, numAnims, sequence);
        }

        public void remove_redundant_links(Sequence sequence)
        {
            for (var i = PendingAnimations.Count - 1; i >= 0; --i)
            {
                var entry = PendingAnimations.ElementAt(i);
                if (entry == null)
                    break;

                if (entry.NumAnims == 0)
                    continue;

                if ((entry.Motion & 0x40000000) != 0 || (entry.Motion & 0x20000000) != 0)
                {
                    if ((entry.Motion & 0x80000000) != 0)
                        return;

                    if (remove_redundant_links_inner(sequence, entry, i))
                        return;
                }
                else
                    if (remove_redundant_links_inner(sequence, entry, i))
                        return;
            }
        }

        public bool remove_redundant_links_inner(Sequence sequence, AnimNode entry, int i)
        {
            for (var j = i - 1; j >= 0; --j)
            {
                var prev = PendingAnimations.ElementAt(j);      // linked?
                if (prev == null)
                    break;

                if (prev.Motion == entry.Motion)
                {
                    trancuate_animation_list(prev, sequence);
                    return true;
                }
                if (prev.NumAnims > 0 && (prev.Motion & 0x70000000) != 0)
                    return true;
            }
            return false;
        }

        public void trancuate_animation_list(AnimNode node, Sequence sequence)
        {
            if (node == null) return;

            var totalAnims = 0;
            for (var i = PendingAnimations.Count - 1; i >= 0; --i)
            {
                var entry = PendingAnimations.ElementAt(i);
                if (entry == node) break;

                totalAnims += entry.NumAnims;
                entry.NumAnims = 0;
            }
            sequence.remove_link_animations(totalAnims);
        }
    }
}
