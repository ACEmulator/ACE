using ACE.Entity.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ACE.DatLoader.Entity
{
    public class ElementDesc : StateDesc
    {
        public uint UiReadOrder;

        // Enum names for these are in EnumMapper 0x2200001B (UIElementID)
        public uint ElementId; 
        public uint Type;
        public uint BaseElement;
        public uint BaseLayout;
        public uint DefaultState;

        public uint X;
        public uint Y;
        public uint Width;
        public uint Height;
        public uint ZLevel;
        public uint LeftEdge;
        public uint TopEdge;
        public uint RightEdge;
        public uint BottomEdge;

        public Dictionary<uint, StateDesc> States = new Dictionary<uint, StateDesc>();
        public Dictionary<uint, ElementDesc> Children = new Dictionary<uint, ElementDesc>();

        public override void Unpack(BinaryReader reader)
        {
            base.Unpack(reader);
            UnpackElementDesc(reader);
        }

        public void UnpackElementDesc(BinaryReader reader)
        {
            UiReadOrder = reader.ReadUInt32();
            ElementId = reader.ReadUInt32();
            Type = reader.ReadUInt32();
            BaseElement = reader.ReadUInt32();
            BaseLayout = reader.ReadUInt32();
            DefaultState = reader.ReadUInt32();

            if ((UiIncorporationFlags & IncorporationFlags.X) != 0)
                X = reader.ReadUInt32();
            if ((UiIncorporationFlags & IncorporationFlags.Y) != 0)
                Y = reader.ReadUInt32();
            if ((UiIncorporationFlags & IncorporationFlags.Width) != 0)
                Width = reader.ReadUInt32();
            if ((UiIncorporationFlags & IncorporationFlags.Height) != 0)
                Height = reader.ReadUInt32();
            if ((UiIncorporationFlags & IncorporationFlags.ZLevel) != 0)
                ZLevel = reader.ReadUInt32();

            LeftEdge = reader.ReadUInt32();
            TopEdge = reader.ReadUInt32();
            RightEdge = reader.ReadUInt32();
            BottomEdge = reader.ReadUInt32();

            reader.ReadByte();
            var totalObjects = reader.ReadByte();
            for (int i = 0; i < totalObjects; i++)
            {
                var key = reader.ReadUInt32();

                var item = new StateDesc();
                item.Unpack(reader);
                States.Add(key, item);
            }

            reader.ReadByte();
            var totalChildren = reader.ReadByte();
            for (int i = 0; i < totalChildren; i++)
            {
                var key = reader.ReadUInt32();

                var item = new ElementDesc();
                item.Unpack(reader);
                Children.Add(key, item);
            }

        }
    }
}
