﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    // TODO: refactor to use existing PaletteOverride object
    public class SubPalette
    {
        public uint SubID { get; set; }
        public uint Offset { get; set; }
        public uint NumColors { get; set; }
    }
}
