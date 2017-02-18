using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Command
{
    public class CommandHandlerInfo
    {
        public Delegate Handler { get; set; }
        public CommandHandlerAttribute Attribute { get; set; }
    }
}
