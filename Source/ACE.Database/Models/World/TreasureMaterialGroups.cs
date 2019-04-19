using System;
using System.Collections.Generic;
using System.Text;

namespace ACE.Database.Models.World
{
    /// <summary>
    /// These are the sub groups of the TreasureMaterialBase. With the the exception of Ivory, the TreasureMaterialBase has probabilities in groups (e.g. "metal", "cloth", etc).
    /// This group is then consulted to figure out the final material.
    /// </summary>
    public partial class TreasureMaterialGroups
    {
        public uint Id { get; set; }
        public int MaterialGroup { get; set; }
        public int Tier { get; set; }
        public float Probability { get; set; }
        public int MaterialID { get; set; }
    }
}
