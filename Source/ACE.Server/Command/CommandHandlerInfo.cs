using System;

namespace ACE.Server.Command
{
    public class CommandHandlerInfo
    {
        public Delegate Handler { get; set; }
        public CommandHandlerAttribute Attribute { get; set; }
    }
}
