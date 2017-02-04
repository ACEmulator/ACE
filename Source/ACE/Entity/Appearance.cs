using ACE.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    public class Appearance
    {
        public Appearance()
        {
        }

        public static Appearance FromFragment(ClientPacketFragment fragment)
        {
            Appearance appearance = new Appearance()
            {
                Race = fragment.Payload.ReadUInt32(),
                Gender = fragment.Payload.ReadUInt32(),
                Eyes = fragment.Payload.ReadUInt32(),
                Nose = fragment.Payload.ReadUInt32(),
                Mouth = fragment.Payload.ReadUInt32(),
                HairColor = fragment.Payload.ReadUInt32(),
                EyeColor = fragment.Payload.ReadUInt32(),
                HairStyle = fragment.Payload.ReadUInt32(),
                HeadgearStyle = fragment.Payload.ReadUInt32(),
                HeadgearColor = fragment.Payload.ReadUInt32(),
                ShirtStyle = fragment.Payload.ReadUInt32(),
                ShirtColor = fragment.Payload.ReadUInt32(),
                PantsStyle = fragment.Payload.ReadUInt32(),
                PantsColor = fragment.Payload.ReadUInt32(),
                FootwearStyle = fragment.Payload.ReadUInt32(),
                FootwearColor = fragment.Payload.ReadUInt32(),
                SkinHue = fragment.Payload.ReadDouble(),
                HairHue = fragment.Payload.ReadDouble(),
                HeadgearHue = fragment.Payload.ReadDouble(),
                ShirtHue = fragment.Payload.ReadDouble(),
                PantsHue = fragment.Payload.ReadDouble(),
                FootwearHue = fragment.Payload.ReadDouble()
            };
            return appearance;
        }

        public uint Race { get; set; }

        public uint Gender { get; set; }

        public uint Eyes { get; set; }

        public uint Nose { get; set; }

        public uint Mouth { get; set; }

        public uint HairColor { get; set; }

        public uint EyeColor { get; set; }

        public uint HairStyle { get; set; }

        public uint HeadgearStyle { get; set; }

        public uint HeadgearColor { get; set; }

        public uint ShirtStyle { get; set; }

        public uint ShirtColor { get; set; }

        public uint PantsStyle { get; set; }

        public uint PantsColor { get; set; }

        public uint FootwearStyle { get; set; }

        public uint FootwearColor { get; set; }

        public double SkinHue { get; set; }

        public double HairHue { get; set; }

        public double HeadgearHue { get; set; }

        public double ShirtHue { get; set; }

        public double PantsHue { get; set; }

        public double FootwearHue { get; set; }
    }
}
