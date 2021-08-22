using System.Collections.Concurrent;

using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.Sequence;
using log4net;

namespace ACE.Server.WorldObjects.Managers
{
    public class ConfirmationManager
    {
        private Player Player;

        private ConcurrentDictionary<ConfirmationType, Confirmation> confirmations = new ConcurrentDictionary<ConfirmationType, Confirmation>();

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly double confirmationTimeout = 30;

        private UIntSequence contextSequence = new UIntSequence();

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
            confirmation.ContextId = contextSequence.NextValue;
            if (confirmations.TryAdd(confirmation.ConfirmationType, confirmation))
            {
                Player.Session.Network.EnqueueSend(new GameEventConfirmationRequest(Player.Session, confirmation.ConfirmationType, confirmation.ContextId, text));
                var timeoutConfirmation = new ActionChain();
                timeoutConfirmation.AddDelaySeconds(confirmationTimeout);
                timeoutConfirmation.AddAction(Player, () => EnqueueAbort(confirmation.ConfirmationType, confirmation.ContextId));
                timeoutConfirmation.EnqueueChain();
            }
            else
            {
                log.Error($"{Player.Name}.ConfirmationManager.EnqueueSend({confirmation.ConfirmationType}, {confirmation.ContextId}) - duplicate confirmation type");
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
            if (confirmations.TryGetValue(confirmationType, out var confirm) && confirm.ContextId == contextId)
                Player.Session.Network.EnqueueSend(new GameEventConfirmationDone(Player.Session, confirmationType, contextId));
        }

        /// <summary>
        /// The client has responded to a confirmation box
        /// </summary>
        public bool HandleResponse(ConfirmationType confirmType, uint contextId, bool response)
        {
            if (!confirmations.TryRemove(confirmType, out var confirm))
            {
                log.Error($"{Player.Name}.ConfirmationManager.HandleResponse({confirmType}, {contextId}, {response}) - confirmType not found");

                return false;
            }

            if (confirm.ContextId != contextId)
            {
                log.Error($"{Player.Name}.ConfirmationManager.HandleResponse({confirmType}, {contextId}, {response}) - contextId != confirm.ContextId");

                if (!confirmations.TryAdd(confirm.ConfirmationType, confirm))
                    log.Error($"{Player.Name}.ConfirmationManager.HandleResponse({confirm.ConfirmationType}, {confirm.ContextId}) - Unable to re-add confirmation, duplicate confirmation type");

                return false;
            }

            confirm.ProcessConfirmation(response);

            return true;
        }
    }
}
