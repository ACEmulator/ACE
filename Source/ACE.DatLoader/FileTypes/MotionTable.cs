using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
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
                        if ((anim.LowFrame == 0) && (anim.HighFrame == uint.MaxValue))
                        {
                            var animation = DatManager.PortalDat.ReadFromDat<Animation>(anim.AnimId);
                            numFrames = animation.NumFrames;
                        }
                        else
                            numFrames = anim.HighFrame - anim.LowFrame;

                        length += numFrames / Math.Abs(anim.Framerate); // Framerates can be negative, which tells the client to play in reverse
                    }
                }
            }

            return length;
        }

        public ACE.Entity.Position GetAnimationFinalPositionFromStart(ACE.Entity.Position position, float objScale, MotionCommand motion)
        {
            MotionStance defaultStyle = (MotionStance)DefaultStyle;

            // get the default motion for the default
            MotionCommand defaultMotion = GetDefaultMotion(defaultStyle);
            return GetAnimationFinalPositionFromStart(position, objScale, defaultMotion, defaultStyle, motion);
        }

        public ACE.Entity.Position GetAnimationFinalPositionFromStart(ACE.Entity.Position position, float objScale, MotionCommand currentMotionState, MotionStance style, MotionCommand motion)
        {
            float length = 0; // init our length var...will return as 0 if not found

            //Frame finalFrame = new Frame();
            //ACE.Entity.Position finalPosition = position;
            ACE.Entity.Position finalPosition = new ACE.Entity.Position();

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
                        if ((anim.LowFrame == 0) && (anim.HighFrame == uint.MaxValue))
                        {
                            var animation = DatManager.PortalDat.ReadFromDat<Animation>(anim.AnimId);
                            numFrames = animation.NumFrames;

                            if (animation.PosFrames.Count > 0)
                            {
                                finalPosition = position;
                                //var origin = new Vector3(position.PositionX, position.PositionY, position.PositionZ);
                                //var orientation = new Quaternion(position.RotationX, position.RotationY, position.RotationZ, position.RotationW);
                                //foreach (var posFrame in animation.PosFrames)
                                //{
                                //    //origin += (posFrame.Origin * objScale);
                                //    origin.X += (posFrame.Origin.X * objScale);
                                //    origin.Y += (posFrame.Origin.Y * objScale);
                                //    origin.Z += (posFrame.Origin.Z * objScale);

                                //    orientation *= posFrame.Orientation * objScale;
                                //    orientation = Quaternion.Normalize(orientation);
                                //}
                                var origin = new Vector3(position.PositionX, position.PositionY, position.PositionZ);
                                var worldRotate = new Quaternion(position.RotationX, position.RotationY, position.RotationZ, position.RotationW);
                                var orientation = new Quaternion(position.RotationX, position.RotationY, position.RotationZ, position.RotationW);
                                foreach (var posFrame in animation.PosFrames)
                                {
                                    origin += Vector3.Transform(posFrame.Origin, worldRotate) * objScale;

                                    orientation *= posFrame.Orientation;
                                    orientation = Quaternion.Normalize(orientation);
                                }

                                finalPosition.PositionX = origin.X;
                                finalPosition.PositionY = origin.Y;
                                finalPosition.PositionZ = origin.Z;

                                finalPosition.RotationW = orientation.W;
                                finalPosition.RotationX = orientation.X;
                                finalPosition.RotationY = orientation.Y;
                                finalPosition.RotationZ = orientation.Z;
                            }
                            else
                                return position;
                        }
                        else
                            numFrames = anim.HighFrame - anim.LowFrame;

                        length += numFrames / Math.Abs(anim.Framerate); // Framerates can be negative, which tells the client to play in reverse
                    }
                }
            }

            return finalPosition;
        }
    }
}
