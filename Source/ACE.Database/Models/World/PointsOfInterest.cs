using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World
{
    public partial class PointsOfInterest
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public uint WeenieClassId { get; set; }
        public DateTime LastModified { get; set; }
    }
}
