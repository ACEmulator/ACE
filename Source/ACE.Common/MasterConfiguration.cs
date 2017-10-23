using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Common
{
    public class MasterConfiguration
    {
        public GameConfiguration Server { get; set; }

        public ApiServerConfiguration ApiServer { get; set; }

        public AuthServerConfiguration AuthServer { get; set; }

        public DatabaseConfiguration MySql { get; set; }
    }
}
