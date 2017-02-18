﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Command
{
    public enum CommandHandlerResponse
    {
        Ok,
        InvalidCommand,
        NoConsoleInvoke,
        InvalidParameterCount,
        NotInWorld
    }
}
