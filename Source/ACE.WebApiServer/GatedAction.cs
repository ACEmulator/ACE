using System;
using System.Threading;

namespace ACE.WebApiServer
{
    internal class GatedAction
    {
        public Action Action { get; set; } = null;
        public ManualResetEvent CompletionToken { get; set; } = null;
    }
}
