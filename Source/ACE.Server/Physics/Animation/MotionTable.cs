using System;
using System.Collections.Generic;
using System.Linq;
using ACE.DatLoader;
using ACE.DatLoader.Entity;

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

            if (motion == substate && !stopModifiers && (substate & 0x20000000) != 0)
                return true;

            if ((motion & 0x80000000) != 0)
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

                        var link = get_link(currState.Style, substate, 1.0f, DefaultStyle, 1.0f);
                        if (link != null && currState.Style != motion)
                        {
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
                            (motionData_ != null ? motionData_.Anims.Count : 0) + (cycles != null ? cycles.Anims.Count : 0));

                        return true;
                    }
                }
            }
            if ((motion & 0x40000000) != 0)
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

                        if (currState.Substate != motion && (currState.Substate & 0x20000000) != 0)
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
                            (motionData_ == null ? 0 : motionData_.Anims.Count));

                        return true;
                    }
                }
            }
            if ((motion & 0x10000000) != 0)  // CM_Action
            {
                var cycleKey = (currState.Style << 16) | (substate & 0xFFFFFF);
                Cycles.TryGetValue(cycleKey, out motionData);
                if (motionData != null)
                {
                    var link = get_link(currState.Style, substate, currState.SubstateMod, motion, speedMod);
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
            if ((motion & 0x20000000) != 0) // CM_Modifier
            {
                var styleKey = currState.Style << 16;
                Cycles.TryGetValue(styleKey | (currState.Substate & 0xFFFFFF), out cycles);
                if (cycles != null && (motionData.Bitfield & 1) == 0)
                {
                    Modifiers.TryGetValue(styleKey | motion, out motionData);
                    Modifiers.TryGetValue(motion & 0xFFFFFF, out motionData_);
                    if (motionData != null || motionData_ != null)
                    {
                        StopSequenceMotion(motion, 1.0f, currState, sequence, ref numAnims);
                        if (!currState.add_modifier(motion, speedMod))
                            return false;
                    }
                    combine_motion(sequence, motionData, speedMod);
                    return true;
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

            numAnims = (uint)motionData.Anims.Count;    // - 1?
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
            if ((motion & 0x40000000) != 0 && currState.Substate == motion)
            {
                uint style = 0;
                StyleDefaults.TryGetValue(currState.Style, out style);
                GetObjectSequence(style, currState, sequence, 1.0f, ref numAnims, true);
                return true;
            }
            if ((motion & 0x20000000) == 0)
                return false;

            Motion lastMod = null;
            foreach (var modifier in currState.Modifiers)
            {
                if (modifier.ID == motion)
                {
                    var key = (modifier.ID << 16) | (motion & 0xFFFFFF);
                    MotionData motionData = null;
                    if (!Modifiers.TryGetValue(motion & 0xFFFFFF, out motionData))
                        return false;

                    subtract_motion(sequence, motionData, modifier.SpeedMod);
                    currState.remove_modifier(modifier);
                    return true;
                }
                lastMod = modifier;
            }
            return false;
        }

        public void add_motion(Sequence sequence, MotionData motionData, float speed)
        {
            if (motionData == null) return;

            sequence.SetVelocity(motionData.Velocity * speed);
            sequence.SetOmega(motionData.Omega * speed);

            for (var i = 0; i < motionData.Anims.Count; i++)
                sequence.append_animation(new AnimData(motionData.Anims[i], speed));
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
            if (motionData == null)
                return;
            sequence.CombinePhysics(motionData.Velocity * speed,
                motionData.Omega * speed);
        }

        public void subtract_motion(Sequence sequence, MotionData motionData, float speed)
        {
            if (motionData == null)
                return;
            sequence.subtract_physics(motionData.Velocity * speed,
                motionData.Omega * speed);
        }

        public MotionData get_link(uint style, uint substate, float substateSpeed, uint motion, float speed)
        {
            uint first = motion, second = substate;
            if (substateSpeed >= 0.0f && speed >= 0.0f)
            {
                first = substate;
                second = motion;
            }
            var motionData = get_link_inner(style, first, second);
            if (motionData != null)
                return motionData;

            return get_link_inner(style, first, second, false);
        }

        public MotionData get_link_inner(uint style, uint first, uint second, bool checkFirst = true)
        {
            Dictionary<uint, MotionData> link = null;
            MotionData motionData = null;

            var key = style << 16;
            if (checkFirst)
                key |= first & 0xFFFFF;
            Links.TryGetValue(key, out link);
            if (link == null)
                return null;

            link.TryGetValue(second, out motionData);
            return motionData;
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

        public void re_modify(Sequence sequence, MotionState state)
        {
            uint numAnims = 0;
            while (state.Modifiers.Count > 0)
            {
                var modifier = state.Modifiers.First();
                state.remove_modifier(modifier);
                GetObjectSequence(modifier.ID, state, sequence, modifier.SpeedMod, ref numAnims, false);
            }
        }
    }
}
