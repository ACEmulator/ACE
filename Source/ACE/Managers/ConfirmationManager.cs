using ACE.Entity;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ACE.Managers
{
    public static class ConfirmationManager
    {
        static ConcurrentDictionary<uint, Confirmation> confirmations = new ConcurrentDictionary<uint, Confirmation>();

        public static void AddConfirmation(Confirmation confirmation)
        {
            var isAlreadyInQueue = (from conf in confirmations
                                    where conf.Key == confirmation.ConfirmationID
                                    select conf).Count();

            if (isAlreadyInQueue == 0)
                confirmations.TryAdd(confirmation.ConfirmationID, confirmation);
        }

        public static void ProcessConfirmation(uint contextId, bool response)
        {
            Confirmation confirmationToProcess = null;
            if (confirmations.Remove<uint, Confirmation>(contextId, out confirmationToProcess))
            { 
                Player newMember = WorldManager.GetPlayerByGuidId(confirmationToProcess.Target);
                //newMember.CompleteConfirmation(isConfirmationInQueue.ConfirmationType,
                //    isConfirmationInQueue.ConfirmationID);
                Player player = WorldManager.GetPlayerByGuidId(confirmationToProcess.Initiator);
                switch (confirmationToProcess.ConfirmationType)
                {
                    case Network.Enum.ConfirmationType.Fellowship:
                        player.CompleteConfirmation(confirmationToProcess.ConfirmationType, confirmationToProcess.ConfirmationID);
                        player.Fellowship.AddConfirmedMember(player, newMember, response);
                        break;
                    case Network.Enum.ConfirmationType.SwearAllegiance:
                        break;
                    default:
                        break;
                }
                
            }
        }
    }
}
