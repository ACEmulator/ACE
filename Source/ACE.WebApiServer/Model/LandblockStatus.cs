using ACE.Server.WorldObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACE.WebApiServer.Model
{
    public class WorldObjectStatus
    {
        public uint Id { get; set; }
        public string Name { get; set; }
    }
    public class LandblockStatus
    {
        public List<WorldObjectStatus> Players { get; set; }
        public List<WorldObjectStatus> Creatures { get; set; }
        public List<WorldObjectStatus> Missiles { get; set; }
        public List<WorldObjectStatus> Other { get; set; }
        public string Id { get; set; }
    }
    public class LandblockStatusResponseModel
    {
        public List<LandblockStatus> Active { get; set; }
    }
}
