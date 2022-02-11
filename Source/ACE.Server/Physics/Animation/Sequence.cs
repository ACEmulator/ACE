using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ACE.Entity.Enum;
using ACE.DatLoader.Entity;
using ACE.Server.Physics.Hooks;

namespace ACE.Server.Physics.Animation
{
    public class Sequence
    {
        public int ID;
        public LinkedList<AnimSequenceNode> AnimList;
        public LinkedListNode<AnimSequenceNode> FirstCyclic;
        public Vector3 Velocity;
        public Vector3 Omega;
        public PhysicsObj HookObj;
        public float FrameNumber;
        public LinkedListNode<AnimSequenceNode> CurrAnim;
        public AnimationFrame PlacementFrame;
        public int PlacementFrameID;
        public bool IsTrivial;

        public static HashSet<uint> PlayerIdleAnims;

        static Sequence()
        {
            PlayerIdleAnims = new HashSet<uint>();
            PlayerIdleAnims.Add(0x03000001);    // NonCombat
            PlayerIdleAnims.Add(0x0300049E);    // AtlatlCombat
            PlayerIdleAnims.Add(0x0300045A);    // BowCombat
            PlayerIdleAnims.Add(0x03000474);    // CrossbowCombat
            PlayerIdleAnims.Add(0x03000CA8);    // DualWieldCombat
            PlayerIdleAnims.Add(0x03000448);    // HandCombat
            PlayerIdleAnims.Add(0x0300076C);    // Magic
            PlayerIdleAnims.Add(0x0300043D);    // SwordCombat
            PlayerIdleAnims.Add(0x03000426);    // SwordShieldCombat
            PlayerIdleAnims.Add(0x030008DF);    // ThrownShieldCombat
            PlayerIdleAnims.Add(0x0300049E);    // ThrownWeaponCombat
            PlayerIdleAnims.Add(0x03000B05);    // TwoHandedSwordCombat
        }

        public bool is_idle_anim()
        {
            return CurrAnim == null || PlayerIdleAnims.Contains(CurrAnim.Value.Anim.ID);
        }

        public bool is_first_cyclic()
        {
            return CurrAnim == null || CurrAnim.Equals(FirstCyclic);
        }

        public Sequence()
        {
            Init();
        }

        public Sequence(int id)
        {
            ID = id;
            Init();
        }

        public Sequence(bool id)
        {
            ID = id ? 1 : 0;
            Init();
        }

        public void Clear()
        {
            clear_animations();
            clear_physics();

            PlacementFrame = null;
            PlacementFrameID = 0;
        }

        /// <summary>
        /// Adds to the sequence velocity / omega
        /// </summary>
        public void CombinePhysics(Vector3 velocity, Vector3 omega)
        {
            Velocity += velocity;
            Omega += omega;
        }

        public AnimationFrame GetCurrAnimFrame()
        {
            if (CurrAnim == null)
                return PlacementFrame;

            return CurrAnim.Value.get_part_frame(get_curr_frame_number());
        }

        public bool HasAnims()
        {
            return AnimList != null && AnimList.Count != 0;
        }

        public void Init()
        {
            Velocity = Vector3.Zero;
            Omega = Vector3.Zero;
            FrameNumber = 0.0f;

            AnimList = new LinkedList<AnimSequenceNode>();
        }

        public void SetObject(PhysicsObj obj)
        {
            HookObj = obj;
        }

        public void SetOmega(Vector3 omega)
        {
            Omega = omega;
        }

        public void SetPlacementFrame(AnimationFrame frame, int id)
        {
            PlacementFrame = frame;
            PlacementFrameID = id;
        }

        public void SetVelocity(Vector3 velocity)
        {
            Velocity = velocity;
        }

        public void Update(float quantum, ref AFrame offsetFrame)
        {
            if (AnimList.First != null)
            {
                update_internal(quantum, ref CurrAnim, ref FrameNumber, ref offsetFrame);
                apricot();
            }
            else if (offsetFrame != null)
            {
                apply_physics(offsetFrame, quantum, quantum);
            }
        }

        public void advance_to_next_animation(float timeElapsed, ref LinkedListNode<AnimSequenceNode> animNode, ref float frameNum, ref AFrame frame)
        {
            var currAnim = animNode.Value;

            if (timeElapsed >= 0.0f)
            {
                if (frame != null && currAnim.Framerate < 0.0f)
                {
                    if (currAnim.Anim.PosFrames.Count > 0)
                        frame.Subtract(currAnim.get_pos_frame((int)frameNum));
                    if (Math.Abs(currAnim.Framerate) > PhysicsGlobals.EPSILON)
                        apply_physics(frame, 1.0f / currAnim.Framerate, timeElapsed);
                }
                if (animNode.Next != null)
                    animNode = animNode.Next;
                else
                    animNode = FirstCyclic;

                currAnim = animNode.Value;

                frameNum = currAnim.get_starting_frame();

                if (frame != null && currAnim.Framerate > 0.0f)
                {
                    if (currAnim.Anim.PosFrames.Count > 0)
                        frame = AFrame.Combine(frame, currAnim.get_pos_frame((int)frameNum));
                    if (Math.Abs(currAnim.Framerate) > PhysicsGlobals.EPSILON)
                        apply_physics(frame, 1.0f / currAnim.Framerate, timeElapsed);
                }
            }
            else
            {
                if (frame != null && currAnim.Framerate >= 0.0f)
                {
                    if (currAnim.Anim.PosFrames.Count > 0)
                        frame.Subtract(currAnim.get_pos_frame((int)frameNum));
                    if (Math.Abs(currAnim.Framerate) > PhysicsGlobals.EPSILON)
                        apply_physics(frame, 1.0f / currAnim.Framerate, timeElapsed);
                }
                if (animNode.Previous != null)
                    animNode = animNode.Previous;
                else
                    animNode = animNode.List.Last;

                currAnim = animNode.Value;

                frameNum = currAnim.get_ending_frame();

                if (frame != null && currAnim.Framerate < 0.0f)
                {
                    if (currAnim.Anim.PosFrames.Count > 0)
                        frame = AFrame.Combine(frame, currAnim.get_pos_frame((int)frameNum));
                    if (Math.Abs(currAnim.Framerate) > PhysicsGlobals.EPSILON)
                        apply_physics(frame, 1.0f / currAnim.Framerate, timeElapsed);
                }
            }
        }

        public void append_animation(AnimData animData)
        {
            var node = new AnimSequenceNode(animData);
            if (!node.has_anim()) return;

            AnimList.AddLast(node);
            FirstCyclic = AnimList.Last;

            if (CurrAnim == null)
            {
                CurrAnim = AnimList.First;
                FrameNumber = CurrAnim.Value.get_starting_frame();
            }
        }

        /// <summary>
        /// Performs the translation and rotation on the frame
        /// </summary>
        public void apply_physics(AFrame frame, float quantum, float sign)
        {
            if (sign >= 0.0)
                quantum = Math.Abs(quantum);
            else
                quantum = -Math.Abs(quantum);

            frame.Origin += Velocity * quantum;
            frame.Rotate(Omega * quantum);
        }

        public void apricot()
        {
            var node = AnimList.First;
            while (!node.Equals(CurrAnim))
            {
                if (node.Equals(FirstCyclic))
                    break;

                AnimList.Remove(node);
                node = AnimList.First;
            }
        }

        public void clear_animations()
        {
            AnimList.Clear();
            FirstCyclic = null;
            FrameNumber = 0;
            CurrAnim = null;
        }

        /// <summary>
        /// Sets the sequence velocity/omega to zero
        /// </summary>
        public void clear_physics()
        {
            Velocity = Vector3.Zero;
            Omega = Vector3.Zero;
        }

        public void execute_hooks(AnimationFrame animFrame, AnimationHookDir dir)
        {
            if (animFrame == null || HookObj == null) return;
            foreach (var hook in animFrame.Hooks)
            {
                if (hook.Direction == AnimationHookDir.Both || hook.Direction == dir)
                    HookObj.add_anim_hook(hook);
            }
        }

        public int get_curr_frame_number()
        {
            return (int)Math.Floor(FrameNumber);
        }

        public void multiply_cyclic_animation_framerate(float rate)
        {
            var currNode = FirstCyclic;

            while (currNode != null)
            {
                currNode.Value.multiply_framerate(rate);

                currNode = currNode.Next;
            }
        }

        public void remove_all_link_animations()
        {
            while (FirstCyclic != null && FirstCyclic.Previous != null)
            {
                if (CurrAnim.Equals(FirstCyclic.Previous))
                {
                    CurrAnim = FirstCyclic;
                    if (CurrAnim != null)
                        FrameNumber = CurrAnim.Value.get_starting_frame();
                }
                AnimList.Remove(FirstCyclic.Previous);
            }
        }

        public void remove_cyclic_anims()
        {
            var node = FirstCyclic;
            while (node != null)
            {
                if (CurrAnim.Equals(node))
                {
                    CurrAnim = node.Previous;
                    if (CurrAnim != null)
                        FrameNumber = CurrAnim.Value.get_ending_frame();
                    else
                        FrameNumber = 0.0f;
                }
                var next = node.Next;   // handle linked list properly
                AnimList.Remove(node.Value);
                node = next;
            }

            FirstCyclic = AnimList.Last;
        }

        public void remove_link_animations(uint amount)
        {
            for (var i = 0; i < amount; i++)
            {
                if (FirstCyclic.Previous == null)
                    return;

                if (CurrAnim.Equals(FirstCyclic.Previous))
                {
                    CurrAnim = FirstCyclic;

                    if (CurrAnim != null)
                        FrameNumber = CurrAnim.Value.get_starting_frame();
                }
                AnimList.Remove(FirstCyclic.Previous);
            }
        }

        /// <summary>
        /// Subtracts from the sequence velocity / omega
        /// </summary>
        public void subtract_physics(Vector3 velocity, Vector3 omega)
        {
            Velocity -= velocity;
            Omega -= omega;
        }

        public void update_internal(float timeElapsed, ref LinkedListNode<AnimSequenceNode> animNode, ref float frameNum, ref AFrame frame)
        {
            var currAnim = animNode.Value;

            var framerate = currAnim.Framerate;
            var frametime = framerate * timeElapsed;

            var lastFrame = (int)Math.Floor(frameNum);

            frameNum += frametime;
            var frameTimeElapsed = 0.0f;
            var animDone = false;

            if (frametime > 0.0f)
            {
                if (currAnim.get_high_frame() < Math.Floor(frameNum))
                {
                    var frameOffset = frameNum - currAnim.get_high_frame() - 1.0f;
                    if (frameOffset < 0.0f)
                        frameOffset = 0.0f;

                    if (Math.Abs(framerate) > PhysicsGlobals.EPSILON)
                        frameTimeElapsed = frameOffset / framerate;

                    frameNum = currAnim.get_high_frame();
                    animDone = true;
                }
                while (Math.Floor(frameNum) > lastFrame)
                {
                    if (frame != null)
                    {
                        if (currAnim.Anim.PosFrames != null)
                            frame = AFrame.Combine(frame, currAnim.get_pos_frame(lastFrame));

                        if (Math.Abs(framerate) > PhysicsGlobals.EPSILON)
                            apply_physics(frame, 1.0f / framerate, timeElapsed);
                    }

                    execute_hooks(currAnim.get_part_frame(lastFrame), AnimationHookDir.Forward);
                    lastFrame++;
                }
            }
            else if (frametime < 0.0f)
            {
                if (currAnim.get_low_frame() > Math.Floor(frameNum))
                {
                    var frameOffset = frameNum - currAnim.get_low_frame();
                    if (frameOffset > 0.0f)
                        frameOffset = 0.0f;

                    if (Math.Abs(framerate) > PhysicsGlobals.EPSILON)
                        frameTimeElapsed = frameOffset / framerate;

                    frameNum = currAnim.get_low_frame();
                    animDone = true;
                }
                while (Math.Floor(frameNum) < lastFrame)
                {
                    if (frame != null)
                    {
                        if (currAnim.Anim.PosFrames != null)
                            frame.Subtract(currAnim.get_pos_frame(lastFrame));

                        if (Math.Abs(framerate) > PhysicsGlobals.EPSILON)
                            apply_physics(frame, 1.0f / framerate, timeElapsed);
                    }

                    execute_hooks(currAnim.get_part_frame(lastFrame), AnimationHookDir.Backward);
                    lastFrame--;
                }
            }
            else
            {
                if (frame != null && Math.Abs(timeElapsed) > PhysicsGlobals.EPSILON)
                    apply_physics(frame, timeElapsed, timeElapsed);
            }

            if (!animDone)
                return;

            if (HookObj != null)
            {
                var node = AnimList.First;
                if (!node.Equals(FirstCyclic))
                    HookObj.add_anim_hook(AnimationHook.AnimDoneHook);
            }

            advance_to_next_animation(timeElapsed, ref animNode, ref frameNum, ref frame);
            timeElapsed = frameTimeElapsed;

            // loop to next anim
            update_internal(timeElapsed, ref animNode, ref frameNum, ref frame);    
        }
    }
}
