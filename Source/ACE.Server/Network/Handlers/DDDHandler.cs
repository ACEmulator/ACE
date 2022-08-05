using ACE.DatLoader;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Managers;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages;
using ACE.Server.Network.GameMessages.Messages;
using System.Collections.Generic;

namespace ACE.Server.Network.Handlers
{
    public static class DDDHandler
    {
        public static IEnumerable<object> ItersWIthKeys { get; private set; }

        [GameMessage(GameMessageOpcode.DDD_InterrogationResponse, SessionState.AuthConnected)]
        public static void DDD_InterrogationResponse(ClientMessage message, Session session)
        {
            if (PropertyManager.GetBool("show_dat_warning").Item)
            {
                message.Payload.ReadUInt32(); // m_ClientLanguage

                var ItersWithKeys = CAllIterationList.Read(message.Payload);
                // var ItersWithoutKeys = CAllIterationList.Read(message.Payload); // Not seen this populated in any pcap.
                // message.Payload.ReadUInt32(); // m_dwFlags - We don't need this

                foreach (var entry in ItersWithKeys.Lists)
                {
                    switch (entry.DatFileId)
                    {
                        case 1: // PORTAL
                            if (entry.Ints[0] != DatManager.PortalDat.Iteration)
                                session.DatWarnPortal = true;
                            break;
                        case 2: // CELL
                            if (entry.Ints[0] != DatManager.CellDat.Iteration)
                                session.DatWarnCell = true;
                            break;
                        case 3: // LANGUAGE
                            if (entry.Ints[0] != DatManager.LanguageDat.Iteration)
                                session.DatWarnLanguage = true;
                            break;
                    }
                }
            }

            GameMessageDDDEndDDD patchStatusMessage = new GameMessageDDDEndDDD();
            session.Network.EnqueueSend(patchStatusMessage);
        }

        [GameMessage(GameMessageOpcode.DDD_EndDDD, SessionState.AuthConnected)]
        public static void DDD_EndDDD(ClientMessage message, Session session)
        {
            // We don't need to reply to this message.
        }

        [GameMessage(GameMessageOpcode.DDD_RequestDataMessage, SessionState.WorldConnected)]
        public static void DDD_RequestDataMessage(ClientMessage message, Session session)
        {
            if (!PropertyManager.GetBool("show_dat_warning").Item) return;

            // True DAT patching would be triggered by this msg, but as we're not supporting that, respond instead with warning and push to external download

            var msg = PropertyManager.GetString("dat_warning_msg").Item;
            var popupMsg = new GameEventPopupString(session, msg);
            var chatMsg = new GameMessageSystemChat(msg, ChatMessageType.WorldBroadcast);
            var transientMsg = new GameEventCommunicationTransientString(session, msg);

            var resourceType = message.Payload.ReadUInt32();
            var dataId = message.Payload.ReadUInt32();
            var errorType = 1u; // unknown enum... this seems to trigger reattempt request by client.

            var dddErrorMsg = new GameMessageDDDErrorMessage(resourceType, dataId, errorType);

            if (session.Player.FirstEnterWorldDone) // Boot client with msg
            {
                session.Network.EnqueueSend(new GameMessageBootAccount($"\n{msg}"), dddErrorMsg);
                session.LogOffPlayer(true);
            }
            else // cannot cleanly boot player that hasn't completed first login, client crashes so msg wouldn't be seen, instead spam msgs until server auto boots them or they disconnect.
                session.Network.EnqueueSend(popupMsg, chatMsg, transientMsg, dddErrorMsg);

        }
    }
}
