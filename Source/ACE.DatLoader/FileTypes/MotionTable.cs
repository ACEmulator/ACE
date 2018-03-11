using System;
using System.Collections.Generic;
using System.IO;

using ACE.DatLoader.Entity;
using ACE.Entity.Enum;

namespace ACE.DatLoader.FileTypes
{
    [DatFileType(DatFileType.ModelTable)]
    public class MotionTable : FileType
    {
        public uint DefaultStyle { get; private set; }
        public Dictionary<uint, uint> StyleDefaults { get; } = new Dictionary<uint, uint>();
        public Dictionary<uint, MotionData> Cycles { get; } = new Dictionary<uint, MotionData>();
        public Dictionary<uint, MotionData> Modifiers { get; } = new Dictionary<uint, MotionData>();
        public Dictionary<uint, Dictionary<uint, MotionData>> Links { get; } = new Dictionary<uint, Dictionary<uint, MotionData>>();

        public override void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();

            DefaultStyle = reader.ReadUInt32();

            uint numStyleDefaults = reader.ReadUInt32();
            for (uint i = 0; i < numStyleDefaults; i++)
                StyleDefaults.Add(reader.ReadUInt32(), reader.ReadUInt32());

            Cycles.Unpack(reader);

            Modifiers.Unpack(reader);

            Links.Unpack(reader);
        }

        /// <summary>
        /// This is the simplest of uses to get the length of an animation.
        /// Usage: MotionTable.GetAnimationLength((uint)MotionTableId, MotionCommand)
        /// </summary>
        public static float GetAnimationLength(MotionTable motionTable, MotionCommand motion)
        {
            return motionTable.GetAnimationLength(motion);
        }

        public static float GetAnimationLength(MotionTable motionTable, MotionStance style, MotionCommand motion)
        {
            return motionTable.GetAnimationLength(style, motion);
        }

        public static float GetAnimationLength(MotionTable motionTable, MotionCommand currentMotionState, MotionStance style, MotionCommand motion)
        {
            return motionTable.GetAnimationLength(currentMotionState, style, motion);
        }

        /// <summary>
        /// Gets the default style for the requested MotionStance
        /// </summary>
        /// <returns>The default style or MotionCommand.Invalid if not found</returns>
        private MotionCommand GetDefaultMotion(MotionStance style)
        {
            if (StyleDefaults.ContainsKey((uint)style))
                return (MotionCommand)StyleDefaults[(uint)style];

            return MotionCommand.Invalid;
        }

        public float GetAnimationLength(MotionCommand motion)
        {
            MotionStance defaultStyle = (MotionStance)DefaultStyle;

            // get the default motion for the default
            MotionCommand defaultMotion = GetDefaultMotion(defaultStyle);
            return GetAnimationLength(defaultMotion, defaultStyle, motion);
        }

        public float GetAnimationLength(MotionStance style, MotionCommand motion)
        {
            // get the default motion for the selected style
            MotionCommand defaultMotion = GetDefaultMotion(style);
            return GetAnimationLength(defaultMotion, style, motion);
        }

        public float GetAnimationLength(MotionCommand currentMotionState, MotionStance style, MotionCommand motion)
        {
            float length = 0; // init our length var...will return as 0 if not found

            uint motionHash = ((uint)currentMotionState & 0xFFFFFF) | ((uint)style << 16);

            if (Links.ContainsKey(motionHash))
            {
                Dictionary<uint, MotionData> links = Links[motionHash];

                if (links.ContainsKey((uint)motion))
                {
                    // loop through all that animations to get our total count
                    for (int i = 0; i < links[(uint)motion].Anims.Count; i++)
                    {
                        AnimData anim = links[(uint)motion].Anims[i];

                        uint numFrames;

                        // check if the animation is set to play the whole thing, in which case we need to get the numbers of frames in the raw animation
                        if ((anim.LowFrame == 0) && (anim.HighFrame == 0xFFFFFFFF))
                        {
                            var animation = DatManager.PortalDat.ReadFromDat<Animation>(anim.AnimId);
                            numFrames = animation.NumFrames;
                        }
                        else
                            numFrames = (uint)(anim.HighFrame - anim.LowFrame);

                        length += numFrames / Math.Abs(anim.Framerate); // Framerates can be negative, which tells the client to play in reverse
                    }
                }
            }

            return length;
        }
    }
}
