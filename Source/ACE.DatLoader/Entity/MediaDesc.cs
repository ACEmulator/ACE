using ACE.Entity.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ACE.DatLoader.Entity
{
    public class MediaDesc : IUnpackable
    {
        public MediaType Type { get; private set; }

        // We could break all these properties into MediaDesc inherted types...but as this is just for reference, this is easier.
        public string FileName;
        public bool StretchToFullScreen;

        public uint File;

        public float Duration;
        public uint DrawMode;
        public List<uint> Frames;
        public uint JumpItemIndex;

        public uint XHotspot;
        public uint YHotspot;
        public uint MessageId;
        public float Probability;

        public float MinDuration;
        public float MaxDuration;
        public uint SType;
        public uint StateId;
        public float StartAlpha;
        public float EndAlpha;


        public virtual void Unpack(BinaryReader reader)
        {
            Type = (MediaType)reader.ReadUInt32();

            reader.ReadUInt32(); // Type again, which is read in the MediaType below...
            switch (Type)
            {
                case MediaType.Movie:
                    FileName = reader.ReadPString(1);
                    StretchToFullScreen = reader.ReadBoolean();
                    break;
                case MediaType.Alpha:
                    File = reader.ReadUInt32();
                    break;
                case MediaType.Animation:
                    Duration = reader.ReadSingle();
                    DrawMode = reader.ReadUInt32();
                    var numFrames = reader.ReadUInt32();
                    Frames = new List<uint>();
                    for(var i =0; i < numFrames; i++)
                    {
                        Frames.Add(reader.ReadUInt32());
                    }
                    break;
                case MediaType.Cursor:
                    File = reader.ReadUInt32();
                    XHotspot = reader.ReadUInt32();
                    YHotspot = reader.ReadUInt32();
                    break;
                case MediaType.Image:
                    File = reader.ReadUInt32();
                    DrawMode = reader.ReadUInt32();
                    break;
                case MediaType.Jump:
                    JumpItemIndex = reader.ReadUInt32();
                    Probability = reader.ReadSingle();
                    break;
                case MediaType.Message:
                    MessageId = reader.ReadUInt32();
                    Probability = reader.ReadSingle();
                    break;
                case MediaType.Pause:
                    MinDuration = reader.ReadSingle();
                    MaxDuration = reader.ReadSingle();
                    break;
                case MediaType.Sound:
                    File = reader.ReadUInt32();
                    SType = reader.ReadUInt32();
                    break;
                case MediaType.State:
                    StateId = reader.ReadUInt32();
                    Probability = reader.ReadSingle();
                    break;
                case MediaType.Fade:
                    StartAlpha = reader.ReadSingle();
                    EndAlpha = reader.ReadSingle();
                    Duration = reader.ReadSingle();
                    break;
            }
        }
    }
}
