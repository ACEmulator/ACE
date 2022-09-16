using System.Linq;

using ACE.DatLoader;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Managers;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.Network.Handlers
{
    public static class DDDHandler
    {
        [GameMessage(GameMessageOpcode.DDD_InterrogationResponse, SessionState.AuthConnected)]
        public static void DDD_InterrogationResponse(ClientMessage message, Session session)
        {
            var clientIsMissingIterations = false;

            CAllIterationList.CMostlyConsecutiveIntSet clientPortalDatIntSet = new CAllIterationList.CMostlyConsecutiveIntSet();
            CAllIterationList.CMostlyConsecutiveIntSet clientCellDatIntSet = new CAllIterationList.CMostlyConsecutiveIntSet();
            CAllIterationList.CMostlyConsecutiveIntSet clientLanguageDatIntSet = new CAllIterationList.CMostlyConsecutiveIntSet();

            var showDatWarning = PropertyManager.GetBool("show_dat_warning").Item;

            message.Payload.ReadUInt32(); // m_ClientLanguage

            var ItersWithKeys = CAllIterationList.Read(message.Payload);
            //var ItersWithoutKeys = CAllIterationList.Read(message.Payload); // Not seen this populated in any pcap.
            // message.Payload.ReadUInt32(); // m_dwFlags - We don't need this

            foreach (var entry in ItersWithKeys.Lists)
            {
                switch (entry.DatFileId)
                {
                    case 1: // PORTAL
                        clientPortalDatIntSet = entry;
                        if (entry.Iterations < DatManager.PortalDat.Iteration)
                        {
                            if (showDatWarning)
                                session.DatWarnPortal = true;

                            clientIsMissingIterations = true;
                        }
                        break;
                    case 2: // CELL
                        clientCellDatIntSet = entry;
                        if (entry.Iterations < DatManager.CellDat.Iteration)
                        {
                            if (showDatWarning)
                                session.DatWarnCell = true;

                            clientIsMissingIterations = true;
                        }
                        break;
                    case 3: // LANGUAGE
                        clientLanguageDatIntSet = entry;
                        if (entry.Iterations < DatManager.LanguageDat.Iteration)
                        {
                            if (showDatWarning)
                                session.DatWarnLanguage = true;

                            clientIsMissingIterations = true;
                        }
                        break;
                }
            }

            if (clientIsMissingIterations)
            {
                var totalMissingIterations = DDDManager.GetMissingIterations(clientPortalDatIntSet, clientCellDatIntSet, clientLanguageDatIntSet, out var totalFileSize, out var missingIterations);
                GameMessageDDDBeginDDD patchStatusMessage = new GameMessageDDDBeginDDD(totalMissingIterations, totalFileSize, missingIterations);
                session.Network.EnqueueSend(patchStatusMessage);

                var hasPortalMissingIterations = missingIterations.TryGetValue(DatDatabaseType.Portal, out var portalMissingIterations);
                var hasLanguageMissingIterations = missingIterations.TryGetValue(DatDatabaseType.Language, out var languageMissingIterations);

                if (hasPortalMissingIterations)
                {
                    foreach (var iteration in portalMissingIterations.Values)
                    {
                        foreach (var fileId in iteration.OrderBy(f => f))
                        {
                            if (DatManager.PortalDat.AllFiles.TryGetValue(fileId, out var datFile))
                                //session.Network.EnqueueSend(new GameMessageDDDDataMessage(datFile, DatDatabaseType.Portal));
                                DDDManager.AddToQueue(session, fileId, DatDatabaseType.Portal);
                        }
                    }
                }

                if (hasLanguageMissingIterations)
                {
                    foreach (var iteration in languageMissingIterations.Values)
                    {
                        foreach (var fileId in iteration.OrderBy(f => f))
                        {
                            if (DatManager.LanguageDat.AllFiles.TryGetValue(fileId, out var datFile))
                                //session.Network.EnqueueSend(new GameMessageDDDDataMessage(datFile, DatDatabaseType.Language));
                                DDDManager.AddToQueue(session, fileId, DatDatabaseType.Language);
                        }
                    }
                }
            }
            else
            {
                GameMessageDDDEndDDD patchStatusMessage = new GameMessageDDDEndDDD();
                session.Network.EnqueueSend(patchStatusMessage);
            }
        }

        [GameMessage(GameMessageOpcode.DDD_EndDDD, SessionState.AuthConnected)]
        public static void DDD_EndDDD(ClientMessage message, Session session)
        {
            // We don't need to reply to this message.

            session.Network.EnqueueSend(new GameMessageDDDEndDDD());
        }

        [GameMessage(GameMessageOpcode.DDD_RequestDataMessage, SessionState.WorldConnected)]
        public static void DDD_RequestDataMessage(ClientMessage message, Session session)
        {
            var qdid_type = (DatFileType)message.Payload.ReadUInt32();
            var qdid_ID = message.Payload.ReadUInt32();

            // Landblock also needs to send the LandBlockInfo (0xFFFE) file with it...
            if (qdid_type == DatFileType.LandBlock)
            {
                var qdid_ID_FFFE = qdid_ID - 1;
                if (DatManager.CellDat.AllFiles.TryGetValue(qdid_ID_FFFE, out var datFileFFFE))
                    //session.Network.EnqueueSend(new GameMessageDDDDataMessage(qdid_ID_FFFE, DatDatabaseType.Cell));
                    DDDManager.AddToQueue(session, qdid_ID_FFFE, DatDatabaseType.Cell);
            }

            //GameMessageDDDDataMessage dataMessage = new GameMessageDDDDataMessage(qdid_ID, DatDatabaseType.Cell);
            if (DatManager.CellDat.AllFiles.TryGetValue(qdid_ID, out var datFile))
                //session.Network.EnqueueSend(new GameMessageDDDDataMessage(qdid_ID, DatDatabaseType.Cell));
                DDDManager.AddToQueue(session, qdid_ID, DatDatabaseType.Cell);

            return;

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
}
