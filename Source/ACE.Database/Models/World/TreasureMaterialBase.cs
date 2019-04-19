using System;
using System.Collections.Generic;
using System.Text;

namespace ACE.Database.Models.World
{
    public partial class TreasureMaterialBase
    {
        public uint Id { get; set; }
        public int MaterialCode { get; set; }
        public int Tier { get; set; }
        public float Probability { get; set; }
        public int MaterialID { get; set; }
    }
}
