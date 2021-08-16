using System.Collections.Concurrent;

using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameEvent.Events;

using log4net;

namespace ACE.Server.WorldObjects.Managers
{
    public class ConfirmationManager
    {
        private Player Player;

        private ConcurrentDictionary<ConfirmationType, Confirmation> confirmations = new ConcurrentDictionary<ConfirmationType, Confirmation>();

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly double confirmationTimeout = 30;

        public ConfirmationManager(Player player)
        {
            Player = player;
        }

        /// <summary>
        /// Builds a new confirmation request on the server,
        /// and sends the request to the client
        /// </summary>
        public bool EnqueueSend(Confirmation confirmation, string text)
        {
            if (confirmations.TryAdd(confirmation.ConfirmationType, confirmation))
            {
                Player.Session.Network.EnqueueSend(new GameEventConfirmationRequest(Player.Session, confirmation.ConfirmationType, (uint)confirmation.ConfirmationType, text));
                var timeoutConfirmation = new ActionChain();
                timeoutConfirmation.AddDelaySeconds(confirmationTimeout);
                timeoutConfirmation.AddAction(Player, () => HandleResponse(confirmation.ConfirmationType, (uint)confirmation.ConfirmationType, false, true));
                timeoutConfirmation.EnqueueChain();
            }
            else
            {
                log.Error($"{Player.Name}.ConfirmationManager.EnqueueSend({confirmation.ConfirmationType}) - duplicate confirmation type");
                return false;
            }

            return true;
        }

        /// <summary>
        /// This only needs to be sent in the rare event the server needs to force close
        /// a confirmation dialog that is still active on the client
        /// </summary>
        public void EnqueueAbort(ConfirmationType confirmationType, uint contextId)
        {
            Player.Session.Network.EnqueueSend(new GameEventConfirmationDone(Player.Session, confirmationType, contextId));
        }

        /// <summary>
        /// The client has responded to a confirmation box
        /// </summary>
        public bool HandleResponse(ConfirmationType confirmType, uint contextId, bool response, bool timeout = false)
        {
            // these should match up in current implementation
            if ((uint)confirmType != contextId)
            {
                log.Error($"{Player.Name}.ConfirmationManager.HandleResponse({confirmType}, {contextId}, {response}) - confirmType != contextId");
                return false;
            }

            if (!confirmations.TryRemove(confirmType, out var confirm))
            {
                if (!timeout)
                    log.Error($"{Player.Name}.ConfirmationManager.HandleResponse({confirmType}, {contextId}, {response}) - confirmType not found");

                return false;
            }

            if (timeout)
                EnqueueAbort(confirmType, contextId);

            confirm.ProcessConfirmation(response);

            return true;
        }
    }
}
