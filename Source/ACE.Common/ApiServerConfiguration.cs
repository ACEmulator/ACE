using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Common
{
    public class ApiServerConfiguration
    {
        /// <summary>
        /// this is the url that is included in tickets coming from the auth server.  it should
        /// be reachable by anyone connecting to your server.  if this says "localhost", only your
        /// computer will be able to reach it.
        /// </summary>
        public string Url { get; set; }
    }
}