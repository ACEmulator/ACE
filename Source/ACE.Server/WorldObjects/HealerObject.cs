using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Server.WorldObjects
{
    public class HealerObject
    {
        public uint StaminaCost { get; set; }

        public bool Critical { get; set; }

        public uint HealAmount { get; set; }

        public uint MissingHealth { get; set; }
    }
}
