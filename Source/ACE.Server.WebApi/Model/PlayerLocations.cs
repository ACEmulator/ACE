using System;
using System.Collections.Generic;
using System.Text;

namespace ACE.Server.WebApi.Model
{
    public class PlayerNameAndLocation
    {
        public string Location { get; set; }
        public string Name { get; set; }
    }
    public class PlayerLocationsResponseModel
    {
        public List<PlayerNameAndLocation> Locations { get; set; }
    }
}
