using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Command
{
    [Flags]
    public enum CommandHandlerFlag
    {
        None = 0x00,
        ConsoleInvoke = 0x01,
        RequiresWorld = 0x02
    }
}
