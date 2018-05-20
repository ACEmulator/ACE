using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World
{
    public partial class Event
    {
        public string Name { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public int State { get; set; }
    }
}
