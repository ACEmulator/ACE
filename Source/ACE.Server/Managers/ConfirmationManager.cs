using System.Collections.Concurrent;
using System.Collections.Generic;
using ACE.Server.Entity;

namespace ACE.Server.Managers
{
    public static class ConfirmationManager
    {
        static ConcurrentDictionary<uint, Confirmation> confirmations = new ConcurrentDictionary<uint, Confirmation>();

        public static bool AddConfirmation(Confirmation confirmation)
        {
            return confirmations.TryAdd(confirmation.ConfirmationID, confirmation);
        }

        public static void ProcessConfirmation(uint contextId, bool response)
        {
            if (!confirmations.Remove(contextId, out var confirm))
                return;

            confirm.ProcessConfirmation(response);
        }
    }
}
