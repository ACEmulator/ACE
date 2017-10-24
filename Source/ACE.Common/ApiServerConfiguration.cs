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
        /// This is the URL that the AuthServer will listen to requests on.
        /// By default, http://*:8001 will listen on all addresses and would be best left unchanged.
        /// </summary>
        public string ListenUrl { get; set; }

        /// <summary>
        /// This is the URL that anyone connecting to your server for authorization uses.
        /// Tickets issued by this AuthServer will point to this address.
        /// If this address is not publically accessible on the internet, you're going to have a bad time.
        /// </summary>
        public string PublicUrl { get; set; }
    }
}