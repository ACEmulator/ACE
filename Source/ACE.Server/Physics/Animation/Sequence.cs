using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ACE.Entity.Enum;
using ACE.DatLoader.Entity;

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
                update_internal(quantum, CurrAnim.Value, FrameNumber, ref offsetFrame);
                apricot();
            }
            else if (offsetFrame != null)
            {
                apply_physics(offsetFrame, quantum, quantum);
            }
        }

        public void advance_to_next_animation(float timeElapsed, AnimSequenceNode currAnim, ref float frameNum, AFrame frame)
        {
            var firstFrame = currAnim.Framerate >= 0.0f;
            var secondFrame = currAnim.Framerate < 0.0f;

            if (timeElapsed >= 0.0)
            {
                firstFrame = currAnim.Framerate < 0.0f;
                secondFrame = currAnim.Framerate > 0.0f;
            }
            advance_to_next_animation_inner(timeElapsed, currAnim, frameNum, frame, firstFrame, true);

            if (currAnim.GetNext() != null)
                currAnim = currAnim.GetNext();
            else
                currAnim = FirstCyclic.Value;

            // ref?
            frameNum = currAnim.get_starting_frame();

            advance_to_next_animation_inner(timeElapsed, currAnim, frameNum, frame, secondFrame, false);
        }

        public void advance_to_next_animation_inner(float timeElapsed, AnimSequenceNode currAnim, double frameNum, AFrame frame, bool checkFrame, bool firstCheck)
        {
            if (frame != null && checkFrame)
            {
                if (currAnim.Anim.PosFrames.Count > 0)
                {
                    if (firstCheck)
                        frame.Subtract(currAnim.get_pos_frame((int)Math.Floor(frameNum)));
                    else
                        frame = AFrame.Combine(frame, currAnim.get_pos_frame((int)Math.Floor(frameNum)));
                }
                if (Math.Abs(currAnim.Framerate) > PhysicsGlobals.EPSILON)
                    apply_physics(frame, 1.0f / currAnim.Framerate, timeElapsed);
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
            if (FirstCyclic == null) return;
            var cyclic = false;
            foreach (var animFrame in AnimList)
            {
                if (animFrame.Equals(FirstCyclic))
                    cyclic = true;

                if (cyclic)
                    animFrame.multiply_framerate(rate);
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

        public void update_internal(float timeElapsed, AnimSequenceNode currAnim, float frameNum, ref AFrame frame)
        {
            var framerate = currAnim.Framerate;
            var frametime = framerate * timeElapsed;

            var lastFrame = (int)Math.Floor(frameNum);

            // ref?
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
                            frame = AFrame.Combine(frame, currAnim.get_pos_frame(lastFrame));

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
                {
                    var animHook = new AnimationHook();
                    HookObj.add_anim_hook(animHook);
                }
            }
            advance_to_next_animation(timeElapsed, currAnim, ref frameNum, frame);
            timeElapsed = frameTimeElapsed;

            // loop to next anim
            update_internal(timeElapsed, currAnim, frameNum, ref frame);    
        }
    }
}
