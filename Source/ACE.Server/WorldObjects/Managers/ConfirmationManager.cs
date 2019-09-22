using System.Collections.Concurrent;

using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Network.GameEvent.Events;

using log4net;

namespace ACE.Server.WorldObjects.Managers
{
    public class ConfirmationManager
    {
        private Player Player;

        private ConcurrentDictionary<ConfirmationType, Confirmation> confirmations = new ConcurrentDictionary<ConfirmationType, Confirmation>();

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ConfirmationManager(Player player)
        {
            Player = player;
        }

        /// <summary>
        /// Builds a new confirmation request on the server,
        /// and sends the request to the client
        /// </summary>
        public void EnqueueSend(Confirmation confirmation, string text)
        {
            if (confirmations.TryAdd(confirmation.ConfirmationType, confirmation))
            {
                Player.Session.Network.EnqueueSend(new GameEventConfirmationRequest(Player.Session, confirmation.ConfirmationType, (uint)confirmation.ConfirmationType, text));
            }
            else
            {
                log.Error($"{Player.Name}.ConfirmationManager.EnqueueSend({confirmation.ConfirmationType}) - duplicate confirmation type");
            }
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
        public bool HandleResponse(ConfirmationType confirmType, uint contextId, bool response)
        {
            // these should match up in current implementation
            if ((uint)confirmType != contextId)
            {
                log.Error($"{Player.Name}.ConfirmationManager.HandleResponse({confirmType}, {contextId}, {response}) - confirmType != contextId");
                return false;
            }

            if (!confirmations.TryRemove(confirmType, out var confirm))
            {
                log.Error($"{Player.Name}.ConfirmationManager.HandleResponse({confirmType}, {contextId}, {response}) - confirmType not found");
                return false;
            }

            confirm.ProcessConfirmation(response);

            return true;
        }
    }
}
