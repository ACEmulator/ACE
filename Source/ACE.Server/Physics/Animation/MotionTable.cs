using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Numerics;

using ACE.DatLoader;
using ACE.DatLoader.Entity;
using ACE.DatLoader.Entity.AnimationHooks;
using ACE.Entity.Enum;
using ACE.Server.Physics.Animation.Internal;

namespace ACE.Server.Physics.Animation
{
    public class MotionTable
    {
        public uint ID;
        public Dictionary<uint, uint> StyleDefaults;
        public Dictionary<uint, MotionData> Cycles;
        public Dictionary<uint, MotionData> Modifiers;
        public Dictionary<uint, Dictionary<uint, MotionData>> Links;
        public uint DefaultStyle;

        public static ConcurrentDictionary<uint, float> WalkSpeed { get; set; }
        public static ConcurrentDictionary<uint, float> RunSpeed { get; set; }
        public static ConcurrentDictionary<uint, float> TurnSpeed { get; set; }

        static MotionTable()
        {
            WalkSpeed = new ConcurrentDictionary<uint, float>();
            RunSpeed = new ConcurrentDictionary<uint, float>();
            TurnSpeed = new ConcurrentDictionary<uint, float>();
        }

        public MotionTable()
        {
            Cycles = new Dictionary<uint, MotionData>();
            Modifiers = new Dictionary<uint, MotionData>();
        }

        public MotionTable(DatLoader.FileTypes.MotionTable mtable)
        {
            ID = mtable.Id;
            StyleDefaults = mtable.StyleDefaults;
            Cycles = mtable.Cycles;
            Modifiers = mtable.Modifiers;
            Links = mtable.Links;
            DefaultStyle = mtable.DefaultStyle;
        }

        public MotionTable Allocator()
        {
            return new MotionTable();
        }

        public bool DoObjectMotion(uint motion, MotionState currState, Sequence sequence, float speedMod, ref uint numAnims)
        {
            return GetObjectSequence(motion, currState, sequence, speedMod, ref numAnims, false);
        }

        public bool GetObjectSequence(uint motion, MotionState currState, Sequence sequence, float speedMod, ref uint numAnims, bool stopModifiers)
        {
            numAnims = 0;
            if (currState.Style == 0 || currState.Substate == 0)
                return false;

            MotionData motionData = null;
            MotionData motionData_ = null;
            MotionData cycles = null;
            uint substate = 0;

            StyleDefaults.TryGetValue(currState.Style, out substate);

            if (motion == substate && !stopModifiers && (currState.Substate & (uint)CommandMask.Modifier) != 0)
                return true;

            if ((motion & (uint)CommandMask.Style) != 0)
            {
                if (currState.Style == motion) return true;

                if (substate != currState.Substate)
                    motionData = get_link(currState.Style, currState.Substate, currState.SubstateMod, substate, speedMod);

                if (substate != 0)
                {
                    Cycles.TryGetValue((motion << 16) | (substate & 0xFFFFFF), out cycles);
                    if (cycles != null)
                    {
                        if ((cycles.Bitfield & 1) != 0)
                            currState.clear_modifiers();

                        var link = get_link(currState.Style, substate, currState.SubstateMod, motion, speedMod);
                        if (link == null && currState.Style != motion)
                        {
                            link = get_link(currState.Style, substate, 1.0f, DefaultStyle, 1.0f);

                            uint defaultStyle = 0;
                            StyleDefaults.TryGetValue(DefaultStyle, out defaultStyle);
                            motionData_ = get_link(DefaultStyle, defaultStyle, 1.0f, motion, 1.0f);
                        }
                        sequence.clear_physics();
                        sequence.remove_cyclic_anims();

                        add_motion(sequence, motionData, speedMod);
                        add_motion(sequence, link, speedMod);
                        add_motion(sequence, motionData_, speedMod);
                        add_motion(sequence, cycles, speedMod);

                        currState.Substate = substate;
                        currState.Style = motion;
                        currState.SubstateMod = speedMod;

                        re_modify(sequence, currState);

                        numAnims = (uint)((motionData != null ? motionData.Anims.Count : 0) + (link != null ? link.Anims.Count : 0) +
                            (motionData_ != null ? motionData_.Anims.Count : 0) + (cycles != null ? cycles.Anims.Count : 0) - 1);

                        return true;
                    }
                }
            }
            if ((motion & (uint)CommandMask.SubState) != 0)
            {
                var motionID = motion & 0xFFFFFF;

                Cycles.TryGetValue(currState.Style << 16 | motionID, out motionData);
                if (motionData == null)
                    Cycles.TryGetValue(DefaultStyle << 16 | motionID, out motionData);
                if (motionData != null)
                {
                    if (is_allowed(motion, motionData, currState))
                    {
                        if (motion == currState.Substate && sequence.HasAnims() && Math.Sign(speedMod) == Math.Sign(currState.SubstateMod))
                        {
                            change_cycle_speed(sequence, motionData, currState.SubstateMod, speedMod);
                            subtract_motion(sequence, motionData, currState.SubstateMod);
                            combine_motion(sequence, motionData, speedMod);
                            currState.SubstateMod = speedMod;
                            return true;
                        }

                        if ((motionData.Bitfield & 1) != 0)
                            currState.clear_modifiers();

                        var link = get_link(currState.Style, currState.Substate, currState.SubstateMod, motion, speedMod);    // verify
                        if (link == null || Math.Sign(speedMod) != Math.Sign(currState.SubstateMod))
                        {
                            uint defaultMotion = 0;
                            StyleDefaults.TryGetValue(currState.Style, out defaultMotion);
                            link = get_link(currState.Style, currState.Substate, currState.SubstateMod, defaultMotion, 1.0f);
                            motionData_ = get_link(currState.Style, defaultMotion, 1.0f, motion, speedMod);
                        }
                        sequence.clear_physics();
                        sequence.remove_cyclic_anims();

                        if (motionData_ != null)
                        {
                            add_motion(sequence, link, currState.SubstateMod);
                            add_motion(sequence, motionData_, speedMod);
                        }
                        else
                        {
                            var newSpeedMod = speedMod;
                            if (currState.SubstateMod < 0.0f && speedMod > 0.0f)
                                newSpeedMod *= -1.0f;
                            add_motion(sequence, link, newSpeedMod);
                        }

                        add_motion(sequence, motionData, speedMod);

                        if (currState.Substate != motion && (currState.Substate & (uint)CommandMask.Modifier) != 0)
                        {
                            uint defaultMotion = 0;
                            StyleDefaults.TryGetValue(currState.Style, out defaultMotion);
                            if (defaultMotion != motion)
                                currState.add_modifier_no_check(currState.Substate, currState.SubstateMod);
                        }

                        currState.SubstateMod = speedMod;
                        currState.Substate = motion;
                        re_modify(sequence, currState);

                        numAnims = (uint)((motionData == null ? 0 : motionData.Anims.Count) + (link == null ? 0 : link.Anims.Count) +
                            (motionData_ == null ? 0 : motionData_.Anims.Count) - 1);

                        return true;
                    }
                }
            }
            if ((motion & (uint)CommandMask.Action) != 0)
            {
                var cycleKey = (currState.Style << 16) | (currState.Substate & 0xFFFFFF);
                Cycles.TryGetValue(cycleKey, out motionData);
                if (motionData != null)
                {
                    var link = get_link(currState.Style, currState.Substate, currState.SubstateMod, motion, speedMod);
                    if (link != null)
                    {
                        currState.add_action(motion, speedMod);
                        sequence.clear_physics();
                        sequence.remove_cyclic_anims();

                        add_motion(sequence, link, speedMod);
                        add_motion(sequence, motionData, currState.SubstateMod);
                        re_modify(sequence, currState);

                        numAnims = (uint)link.Anims.Count;
                        return true;
                    }
                    else
                    {
                        StyleDefaults.TryGetValue(currState.Style, out substate);
                        motionData = get_link(currState.Style, currState.Substate, currState.SubstateMod, substate, 1.0f);
                        if (motionData != null)
                        {
                            link = get_link(currState.Style, substate, 1.0f, motion, speedMod);
                            if (link != null && Cycles.TryGetValue(cycleKey, out cycles))
                            {
                                motionData_ = get_link(currState.Style, substate, 1.0f, currState.Substate, currState.SubstateMod);
                                currState.add_action(motion, speedMod);
                                sequence.clear_physics();
                                sequence.remove_cyclic_anims();
                                add_motion(sequence, motionData, 1.0f);
                                add_motion(sequence, link, speedMod);
                                add_motion(sequence, motionData_, 1.0f);
                                add_motion(sequence, cycles, currState.SubstateMod);
                                re_modify(sequence, currState);
                                numAnims = (uint)(motionData.Anims.Count + link.Anims.Count + (motionData_ == null ? 0 : motionData.Anims.Count));
                                return true;
                            }
                        }
                    }
                }
            }
            if ((motion & (uint)CommandMask.Modifier) != 0)
            {
                var styleKey = currState.Style << 16;
                Cycles.TryGetValue(styleKey | (currState.Substate & 0xFFFFFF), out cycles);
                if (cycles != null && (cycles.Bitfield & 1) == 0)
                {
                    Modifiers.TryGetValue(styleKey | (motion & 0xFFFFFF), out motionData);
                    if (motionData == null)
                        Modifiers.TryGetValue(motion & 0xFFFFFF, out motionData);
                    if (motionData != null)
                    {
                        if (!currState.add_modifier(motion, speedMod))
                        {
                            StopSequenceMotion(motion, 1.0f, currState, sequence, ref numAnims);
                            if (!currState.add_modifier(motion, speedMod))
                                return false;
                        }
                        combine_motion(sequence, motionData, speedMod);
                        return true;
                    }
                }
            }
            return false;
        }

        public static MotionTable Get(uint motionTableID)
        {
            //return ObjCache.GetMotionTable(mtableID);
            var motionTable = new MotionTable(DatManager.PortalDat.ReadFromDat<DatLoader.FileTypes.MotionTable>(motionTableID));
            return motionTable;
        }

        public bool SetDefaultState(MotionState state, Sequence sequence, ref uint numAnims)
        {
            uint defaultSubstate = 0;
            if (!StyleDefaults.TryGetValue(DefaultStyle, out defaultSubstate))
                return false;

            state.clear_modifiers();
            state.clear_actions();

            var cycle = (DefaultStyle << 16) | (defaultSubstate & 0xFFFFFF);

            MotionData motionData = null;
            if (!Cycles.TryGetValue(cycle, out motionData))
                return false;

            numAnims = (uint)motionData.Anims.Count - 1;
            state.Style = DefaultStyle;
            state.Substate = defaultSubstate;
            state.SubstateMod = 1.0f;

            sequence.clear_physics();
            sequence.clear_animations();

            add_motion(sequence, motionData, state.SubstateMod);
            return true;
        }

        public bool StopObjectCompletely(MotionState currState, Sequence sequence, ref uint numAnims)
        {
            var node = currState.Modifiers.First;
            var speed = 1.0f;
            var success = false;
            while (node != null)
            {
                var motion = node.Value;
                speed = motion.SpeedMod;
                if (StopSequenceMotion(motion.ID, motion.SpeedMod, currState, sequence, ref numAnims))
                    success = true;

                node = currState.Modifiers.First;
            }
            speed = currState.SubstateMod;

            if (!StopSequenceMotion(currState.Substate, speed, currState, sequence, ref numAnims))
                return success;
            else
                return true;
        }

        public bool StopObjectMotion(uint motion, float speed, MotionState currState, Sequence sequence, ref uint numAnims)
        {
            return StopSequenceMotion(motion, speed, currState, sequence, ref numAnims);
        }

        public bool StopSequenceMotion(uint motion, float speed, MotionState currState, Sequence sequence, ref uint numAnims)
        {
            numAnims = 0;
            if ((motion & (uint)CommandMask.SubState) != 0 && currState.Substate == motion)
            {
                uint style = 0;
                StyleDefaults.TryGetValue(currState.Style, out style);
                GetObjectSequence(style, currState, sequence, 1.0f, ref numAnims, true);
                return true;
            }
            if ((motion & (uint)CommandMask.Modifier) == 0)
                return false;

            var modifier = currState.Modifiers.First;
            LinkedListNode<Motion> prevMod = null;

            while (modifier != null)
            {
                if (modifier.Value.ID == motion)
                {
                    var key = (currState.Style << 16) | (motion & 0xFFFFFF);

                    Modifiers.TryGetValue(key, out var motionData);
                    if (motionData == null)
                        Modifiers.TryGetValue(motion & 0xFFFFFF, out motionData);
                    if (motionData == null)
                        return false;

                    subtract_motion(sequence, motionData, modifier.Value.SpeedMod);
                    currState.remove_modifier(modifier);
                    return true;
                }
                prevMod = modifier;
                modifier = modifier.Next;
            }
            return false;
        }

        public void add_motion(Sequence sequence, MotionData motionData, float speed)
        {
            if (motionData == null) return;

            sequence.SetVelocity(motionData.Velocity * speed);
            sequence.SetOmega(motionData.Omega * speed);

            for (var i = 0; i < motionData.Anims.Count; i++)
            {
                var animData = new AnimData(motionData.Anims[i], speed);
                sequence.append_animation(animData);
            }
        }

        public void change_cycle_speed(Sequence sequence, MotionData motionData, float substateMod, float speedMod)
        {
            if (Math.Abs(substateMod) > PhysicsGlobals.EPSILON)
                sequence.multiply_cyclic_animation_framerate(speedMod / substateMod);

            else if (Math.Abs(speedMod) < PhysicsGlobals.EPSILON)
                sequence.multiply_cyclic_animation_framerate(0);
        }

        public void combine_motion(Sequence sequence, MotionData motionData, float speed)
        {
            if (motionData == null) return;

            sequence.CombinePhysics(motionData.Velocity * speed, motionData.Omega * speed);
        }

        public void subtract_motion(Sequence sequence, MotionData motionData, float speed)
        {
            if (motionData == null) return;

            sequence.subtract_physics(motionData.Velocity * speed, motionData.Omega * speed);
        }

        public MotionData get_link(uint style, uint substate, float substateSpeed, uint motion, float speed)
        {
            if (speed < 0.0f || substateSpeed < 0.0f)
            {
                if (Links.TryGetValue((style << 16) | (motion & 0xFFFFFF), out var link))
                {
                    if (link.TryGetValue(substate, out var result))
                        return result;
                }

                if (StyleDefaults.TryGetValue(style, out var defaultMotion) && Links.TryGetValue((style << 16) | (substate & 0xFFFFFF), out var sublink))
                {
                    sublink.TryGetValue(defaultMotion, out var result);
                    return result;
                }
            }
            else
            {
                if (Links.TryGetValue((style << 16) | (substate & 0xFFFFFF), out var link))
                {
                    if (link.TryGetValue(motion, out var result))
                        return result;
                }

                if (Links.TryGetValue(style << 16, out var sublink))
                {
                    sublink.TryGetValue(motion, out var result);
                    return result;
                }
            }
            return null;
        }

        public bool is_allowed(uint motion, MotionData motionData, MotionState state)
        {
            if (motionData == null) return false;

            if ((motionData.Bitfield & 2) == 0 || motion == state.Substate)
                return true;

            uint style = 0;
            StyleDefaults.TryGetValue(state.Style, out style);
            return style == state.Substate;
        }

        public void re_modify(Sequence sequence, MotionState pstate)
        {
            if (pstate.Modifiers.First == null)
                return;

            var state = new MotionState(pstate);

            while (state.Modifiers.First != null)
            {
                var speedMod = pstate.Modifiers.First.Value.SpeedMod;
                var motion = pstate.Modifiers.First.Value.ID;

                pstate.remove_modifier(pstate.Modifiers.First); // second param null?
                state.remove_modifier(state.Modifiers.First);

                uint numAnims = 0;
                GetObjectSequence(motion, pstate, sequence, speedMod, ref numAnims, false);
            }
        }

        private static readonly List<(float, AttackHook)> emptyList = new List<(float, AttackHook)>();

        public static List<(float time, AttackHook attackHook)> GetAttackFrames(uint motionTableId, MotionStance stance, MotionCommand motion)
        {
            if (motionTableId == 0) return emptyList;

            var motionTable = DatManager.PortalDat.ReadFromDat<DatLoader.FileTypes.MotionTable>(motionTableId);
            return motionTable.GetAttackFrames(motionTableId, stance, motion);
        }

        public static float GetAnimationLength(uint motionTableId, MotionStance stance, MotionCommand motion, float speed = 1.0f)
        {
            if (motionTableId == 0) return 0;

            var motionTable = DatManager.PortalDat.ReadFromDat<DatLoader.FileTypes.MotionTable>(motionTableId);
            return motionTable.GetAnimationLength(stance, motion, null) / speed;
        }

        public static float GetAnimationLength(uint motionTableId, MotionStance stance, MotionCommand currentMotion, MotionCommand motion, float speed = 1.0f)
        {
            if (motionTableId == 0) return 0;

            var motionTable = DatManager.PortalDat.ReadFromDat<DatLoader.FileTypes.MotionTable>(motionTableId);

            var animLength = 0.0f;
            if (((uint)motion & (uint)CommandMask.Style) != 0 && currentMotion != MotionCommand.Ready)
            {
                animLength += motionTable.GetAnimationLength(stance, MotionCommand.Ready, currentMotion) / speed;
                currentMotion = MotionCommand.Ready;
            }

            animLength += motionTable.GetAnimationLength(stance, motion, currentMotion) / speed;
            return animLength;
        }

        public static float GetCycleLength(uint motionTableId, MotionStance stance, MotionCommand motion, float speed = 1.0f)
        {
            if (motionTableId == 0) return 0;

            var motionTable = DatManager.PortalDat.ReadFromDat<DatLoader.FileTypes.MotionTable>(motionTableId);
            return motionTable.GetCycleLength(stance, motion) / speed;
        }

        /// <summary>
        /// Returns the distance per second for a running animation
        /// </summary>
        public static float GetRunSpeed(uint motionTableID)
        {
            if (RunSpeed.TryGetValue(motionTableID, out float runSpeed))
                return runSpeed;

            uint runMotion = (uint)MotionCommand.RunForward;
            var motionData = GetMotionData(motionTableID, runMotion);
            if (motionData == null)
                return 0.0f;

            var speed = GetAnimDist(motionData);
            RunSpeed[motionTableID] = speed;
            return speed;
        }

        /// <summary>
        /// Returns the rotational velocity / omega for a turning animation
        /// </summary>
        public static float GetTurnSpeed(uint motionTableID)
        {
            if (TurnSpeed.TryGetValue(motionTableID, out float turnSpeed))
                return turnSpeed;

            uint turnMotion = (uint)MotionCommand.TurnRight;
            var motionData = GetMotionData(motionTableID, turnMotion);
            if (motionData == null)
                return 0.0f;

            var speed = Math.Abs(motionData.Omega.Z);
            TurnSpeed[motionTableID] = speed;
            return speed;
        }

        /// <summary>
        /// Returns the MotionData for a motionTable and motion ID
        /// </summary>
        public static MotionData GetMotionData(uint motionTableID, uint motion, uint? currentStyle = null)
        {
            if (motionTableID == 0) return null;

            var motionTable = DatManager.PortalDat.ReadFromDat<DatLoader.FileTypes.MotionTable>(motionTableID);
            if (currentStyle == null)
                currentStyle = motionTable.DefaultStyle;
            var motionID = motion & 0xFFFFFF;
            var key = currentStyle.Value << 16 | motionID;
            motionTable.Cycles.TryGetValue(key, out var motionData);
            return motionData;
        }

        public static MotionData GetLinkData(uint motionTableID, uint motion, uint? currentStyle = null)
        {
            if (motionTableID == 0) return null;

            var motionTable = DatManager.PortalDat.ReadFromDat<DatLoader.FileTypes.MotionTable>(motionTableID);
            if (currentStyle == null)
                currentStyle = motionTable.DefaultStyle;
            var key = (currentStyle.Value << 16) | (int)MotionCommand.Ready & 0xFFFF;
            motionTable.Links.TryGetValue(key, out var links);
            if (links == null) return null;
            links.TryGetValue(motion, out var motionData);
            return motionData;
        }

        /// <summary>
        /// Returns the movement distance per second from an animation
        /// </summary>
        public static float GetAnimDist(MotionData motionData)
        {
            var offset = Vector3.Zero;
            var totalFrames = 0;
            foreach (var anim in motionData.Anims)
            {
                var animation = DatManager.PortalDat.ReadFromDat<DatLoader.FileTypes.Animation>(anim.AnimId);
                foreach (var frame in animation.PosFrames)
                {
                    // orientation?
                    offset += frame.Origin;
                    totalFrames++;
                }
            }
            var dist = offset.Length();
            if (dist == 0.0f) return 0.0f;
            return dist / totalFrames * motionData.Anims[0].Framerate;
        }

        /// <summary>
        /// Returns TRUE if this animation has a DefaultScript hook type
        /// </summary>
        public static bool HasDefaultScript(uint motionTableID, uint motion, uint currentStyle)
        {
            var motionData = GetLinkData(motionTableID, motion, currentStyle);
            if (motionData == null)
                return false;

            foreach (var anim in motionData.Anims)
            {
                var animation = DatManager.PortalDat.ReadFromDat<DatLoader.FileTypes.Animation>(anim.AnimId);
                if (animation == null) continue;

                foreach (var frame in animation.PartFrames)
                {
                    foreach (var hook in frame.Hooks)
                        if (hook.HookType == AnimationHookType.DefaultScript)
                            return true;
                }
            }
            return false;
        }
    }
}
