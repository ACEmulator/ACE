using System;
using System.Threading;

namespace ACE.Server.API
{
    internal class GatedAction
    {
        public Action Action { get; set; } = null;
        public ManualResetEvent CompletionToken { get; set; } = null;
    }
}
