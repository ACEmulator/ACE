using ACE.DatLoader.Entity;
using ACE.Entity.Enum;
using System;
using System.Collections.Generic;

namespace ACE.DatLoader.FileTypes
{
    public class MotionTable
    {
        public uint Id { get; set; }
        public uint DefaultStyle { get; set; }
        public Dictionary<uint, uint> StyleDefaults { get; set; } = new Dictionary<uint, uint>();
        public Dictionary<uint, MotionData> Cycles { get; set; } = new Dictionary<uint, MotionData>();
        public Dictionary<uint, MotionData> Modifiers { get; set; } = new Dictionary<uint, MotionData>();
        public Dictionary<uint, Dictionary<uint, MotionData>> Links { get; set; } = new Dictionary<uint, Dictionary<uint, MotionData>>();

        public static MotionTable ReadFromDat(uint fileId)
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.ContainsKey(fileId))
            {
                return (MotionTable)DatManager.PortalDat.FileCache[fileId];
            }
            else
            {
                DatReader datReader = DatManager.PortalDat.GetReaderForFile(fileId);
                MotionTable m = new MotionTable();
                m.Id = datReader.ReadUInt32();

                m.DefaultStyle = datReader.ReadUInt32();

                uint numStyleDefaults = datReader.ReadUInt32();
                for (uint i = 0; i < numStyleDefaults; i++)
                    m.StyleDefaults.Add(datReader.ReadUInt32(), datReader.ReadUInt32());

                uint numCycles = datReader.ReadUInt32();
                for (uint i = 0; i < numCycles; i++)
                {
                    uint key = datReader.ReadUInt32();
                    MotionData md = MotionData.Read(datReader);
                    m.Cycles.Add(key, md);
                }

                uint numModifiers = datReader.ReadUInt32();
                for (uint i = 0; i < numModifiers; i++)
                {
                    uint key = datReader.ReadUInt32();
                    MotionData md = MotionData.Read(datReader);
                    m.Modifiers.Add(key, md);
                }

                uint numLinks = datReader.ReadUInt32();
                for (uint i = 0; i < numLinks; i++)
                {
                    uint firstKey = datReader.ReadUInt32();
                    uint numSubLinks = datReader.ReadUInt32();
                    Dictionary<uint, MotionData> links = new Dictionary<uint, MotionData>();
                    for (uint j = 0; j < numSubLinks; j++)
                    {
                        uint subKey = datReader.ReadUInt32();
                        MotionData md = MotionData.Read(datReader);
                        links.Add(subKey, md);
                    }
                    m.Links.Add(firstKey, links);
                }

                // Store this object in the FileCache
                DatManager.PortalDat.FileCache[fileId] = m;

                return m;
            }
        }

        /// <summary>
        /// This is the simplest of uses to get the length of an animation.
        /// Usage: MotionTable.GetAnimationLength((uint)MotionTableId, MotionCommand)
        /// </summary>
        public static float GetAnimationLength(uint motionTableId, MotionCommand motion)
        {
            MotionTable mt = ReadFromDat(motionTableId);
            return mt.GetAnimationLength(motion);
        }

        public static float GetAnimationLength(uint motionTableId, MotionStance style, MotionCommand motion)
        {
            MotionTable mt = ReadFromDat(motionTableId);
            return mt.GetAnimationLength(style, motion);
        }

        public static float GetAnimationLength(uint motionTableId, MotionCommand currentMotionState, MotionStance style, MotionCommand motion)
        {
            MotionTable mt = ReadFromDat(motionTableId);
            return mt.GetAnimationLength(currentMotionState, style, motion);
        }

        /// <summary>
        /// Gets the default style for the requested MotionStance
        /// </summary>
        /// <returns>The default style or MotionCommand.Invalid if not found</returns>
        private MotionCommand GetDefaultMotion(MotionStance style)
        {
            if (StyleDefaults.ContainsKey((uint)style))
                return (MotionCommand)StyleDefaults[(uint)style];
            else
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
                        uint numFrames = 0;
                        // check if the animation is set to play the whole thing, in which case we need to get the numbers of frames in the raw animation
                        if ((anim.LowFrame == 0) && (anim.HighFrame == 0xFFFFFFFF))
                        {
                            Animation animation = Animation.ReadFromDat(anim.AnimId);
                            numFrames = animation.NumFrames;
                        }
                        else
                            numFrames = (anim.HighFrame - anim.LowFrame);

                        length += numFrames / Math.Abs(anim.Framerate); // Framerates can be negative, which tells the client to play in reverse
                    }
                }
            }

            return length;
        }
    }
}
