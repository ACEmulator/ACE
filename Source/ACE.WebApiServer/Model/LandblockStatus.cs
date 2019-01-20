using System;
using System.Collections.Generic;
using System.Text;

namespace ACE.WebApiServer.Model
{
    public class LandblockStatus
    {
        public int Actions { get; set; }
        public int Objects { get; set; }
        public string Id { get; set; }
    }
    public class LandblockStatusResponseModel
    {
        public List<LandblockStatus> Active { get; set; }
    }
}
