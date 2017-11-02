﻿using ACE.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class LightInfo
    {
        public Position ViewerspaceLocation { get; set; }
        public uint Color { get; set; } // _RGB Color. Red is bytes 3-4, Green is bytes 5-6, Blue is bytes 7-8. Bytes 1-2 are always FF (?)
        public float Intensity { get; set; }
        public float Falloff { get; set; }
        public float ConeAngle { get; set; }

        public static LightInfo Read(DatReader datReader)
        {
            LightInfo obj = new LightInfo();

            obj.ViewerspaceLocation = PositionExtensions.ReadPosition(datReader);
            obj.Color = datReader.ReadUInt32();
            obj.Intensity = datReader.ReadSingle();
            obj.Falloff = datReader.ReadSingle();
            obj.ConeAngle = datReader.ReadSingle();

            return obj;
        }
    }
}