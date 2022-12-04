using System;
using System.Linq;

using ACE.Common;
using ACE.DatLoader;
using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Structure;

using log4net;

namespace ACE.Server.Network.Handlers
{
    public static class DDDHandler
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static bool Debug = false;

        [GameMessage(GameMessageOpcode.DDD_InterrogationResponse, SessionState.AuthConnected)]
        public static void DDD_InterrogationResponse(ClientMessage message, Session session)
        {
            var clientIsMissingIterations = false;

            var clientHasExtraIterations = false;

            var clientPortalDatIntSet = new CMostlyConsecutiveIntSet();
            var clientCellDatIntSet = new CMostlyConsecutiveIntSet();
            var clientLanguageDatIntSet = new CMostlyConsecutiveIntSet();

            var showDatWarning = PropertyManager.GetBool("show_dat_warning").Item;

            message.Payload.ReadUInt32(); // m_ClientLanguage

            var ItersWithKeys = message.Payload.ReadCAllIterationList();
            //var ItersWithoutKeys = message.Payload.ReadCAllIterationList(); // Not seen this populated in any pcap.
            // message.Payload.ReadUInt32(); // m_dwFlags - We don't need this

            foreach (var entry in ItersWithKeys.Lists)
            {
                switch (entry.DatFileId)
                {
                    case 1: // PORTAL
                        clientPortalDatIntSet = entry.List;
                        if (entry.List.Iterations < DatManager.PortalDat.Iteration)
                        {
                            if (showDatWarning)
                                session.DatWarnPortal = true;

                            clientIsMissingIterations = true;
                        }
                        else if (entry.List.Iterations > DatManager.PortalDat.Iteration)
                        {
                            if (showDatWarning)
                                session.DatWarnPortal = true;

                            clientHasExtraIterations = true;
                        }
                        break;
                    case 2: // CELL
                        clientCellDatIntSet = entry.List;
                        if (entry.List.Iterations < DatManager.CellDat.Iteration)
                        {
                            if (showDatWarning)
                                session.DatWarnCell = true;

                            clientIsMissingIterations = true;
                        }
                        else if (entry.List.Iterations > DatManager.CellDat.Iteration)
                        {
                            if (showDatWarning)
                                session.DatWarnCell = true;

                            clientHasExtraIterations = true;
                        }
                        break;
                    case 3: // LANGUAGE
                        clientLanguageDatIntSet = entry.List;
                        if (entry.List.Iterations < DatManager.LanguageDat.Iteration)
                        {
                            if (showDatWarning)
                                session.DatWarnLanguage = true;

                            clientIsMissingIterations = true;
                        }
                        else if (entry.List.Iterations > DatManager.LanguageDat.Iteration)
                        {
                            if (showDatWarning)
                                session.DatWarnLanguage = true;

                            clientHasExtraIterations = true;
                        }
                        break;
                }
            }

            if (Debug)
            {
                Console.WriteLine($"{session.Account} client_portal.dat:" + Environment.NewLine + clientPortalDatIntSet);
                Console.WriteLine($"{session.Account} client_cell_1.dat:" + Environment.NewLine + clientCellDatIntSet);
                Console.WriteLine($"{session.Account} client_Local_English.dat:" + Environment.NewLine + clientLanguageDatIntSet);
            }

            var enableDATpatching = ConfigManager.Config.DDD.EnableDATPatching;

            var logMsg = $"[DDD] client {session.Account} responded to Interrogation:\n client_portal.dat: {clientPortalDatIntSet.Iterations} | client_cell_1.dat: {clientCellDatIntSet.Iterations} | client_Local_English.dat: {clientLanguageDatIntSet.Iterations}";
            if (clientHasExtraIterations) logMsg += " | client has more iterations than server, cannot update";
            else if (clientIsMissingIterations) logMsg += " | update required";
            else logMsg += " | no update required";
            if (clientIsMissingIterations && !enableDATpatching) logMsg += ", but DAT patching is disabled";
            log.Info(logMsg);

            if (clientHasExtraIterations)
            {
                var msg = PropertyManager.GetString("dat_newer_warning_msg").Item;
                session.Terminate(SessionTerminationReason.DATsNewerThanServer, new GameMessageBootAccount($" because {msg[..^1]}"));
            }
            else if (clientIsMissingIterations && enableDATpatching)
            {
                var totalMissingIterations = DDDManager.GetMissingIterations(clientPortalDatIntSet, clientCellDatIntSet, clientLanguageDatIntSet, out var totalFileSize, out var missingIterations);                
                var patchStatusMessage = new GameMessageDDDBeginDDD(totalMissingIterations, totalFileSize, missingIterations);
                session.Network.EnqueueSend(patchStatusMessage);
                session.BeginDDDSentTime = DateTime.UtcNow;
                session.BeginDDDSent = true;

                log.Info($"[DDD] client {session.Account} informed with BeginDDD payload:\n Total Missing Iterations: {totalMissingIterations} | Expected Data Transfer Size: {totalFileSize / 1024:N0} kB");

                var hasPortalMissingIterations = missingIterations.TryGetValue(DatDatabaseType.Portal, out var portalMissingIterations);
                var hasLanguageMissingIterations = missingIterations.TryGetValue(DatDatabaseType.Language, out var languageMissingIterations);

                if (hasPortalMissingIterations)
                {
                    foreach (var iteration in portalMissingIterations.Values)
                    {
                        foreach (var fileId in iteration.OrderBy(f => f))
                        {
                            if (DatManager.PortalDat.AllFiles.TryGetValue(fileId, out _))
                                //session.Network.EnqueueSend(new GameMessageDDDDataMessage(fileId, DatDatabaseType.Portal));
                                DDDManager.AddToQueue(session, fileId, DatDatabaseType.Portal);
                            else
                                log.Warn($"[DDD] DDD_InterrogationResponse: DDDManager.AddToQueue failed: DatManager.PortalDat.AllFiles does not contain 0x{fileId:X8}");
                        }
                    }
                }

                if (hasLanguageMissingIterations)
                {
                    foreach (var iteration in languageMissingIterations.Values)
                    {
                        foreach (var fileId in iteration.OrderBy(f => f))
                        {
                            if (DatManager.LanguageDat.AllFiles.TryGetValue(fileId, out _))
                                //session.Network.EnqueueSend(new GameMessageDDDDataMessage(fileId, DatDatabaseType.Language));
                                DDDManager.AddToQueue(session, fileId, DatDatabaseType.Language);
                            else
                                log.Warn($"[DDD] DDD_InterrogationResponse: DDDManager.AddToQueue failed: DatManager.LanguageDat.AllFiles does not contain 0x{fileId:X8}");
                        }
                    }
                }
            }
            else if (clientIsMissingIterations && !enableDATpatching)
            {
                var msg = PropertyManager.GetString("dat_older_warning_msg").Item;
                session.Terminate(SessionTerminationReason.DATsPatchingDisabled, new GameMessageBootAccount($" because {msg.TrimEnd('.')}"));
            }
            else // client dat files are up to date
            {
                session.Network.EnqueueSend(new GameMessageDDDEndDDD());
            }
        }

        [GameMessage(GameMessageOpcode.DDD_EndDDD, SessionState.AuthConnected)]
        public static void DDD_EndDDD(ClientMessage message, Session session)
        {
            // We don't need to reply to this message unless GameMessageDDDBeginDDD was sent.

            if (session.BeginDDDSent)
            {
                session.BeginDDDSent = false;

                session.Network.EnqueueSend(new GameMessageDDDEndDDD());

                session.DatWarnPortal = false;
                session.DatWarnCell = false;
                session.DatWarnLanguage = false;

                log.Info($"[DDD] client {session.Account} reported it successfully received and patched its DAT files with expected BeginDDD payload");
            }
        }

        [GameMessage(GameMessageOpcode.DDD_RequestDataMessage, SessionState.WorldConnected)]
        public static void DDD_RequestDataMessage(ClientMessage message, Session session)
        {
            var enableDATpatching = ConfigManager.Config.DDD.EnableDATPatching;
            var showDatWarning = PropertyManager.GetBool("show_dat_warning").Item;

            var qdid_type = (DatFileType)message.Payload.ReadUInt32();
            var qdid_ID = message.Payload.ReadUInt32();

            log.Info($"[DDD] client {session.Account} requested data on 0x{qdid_ID:X8} | {qdid_type}{(!enableDATpatching ? $"; DAT patching is disabled{(showDatWarning ? " and client will be booted" : "")}" : "")}");

            if (!enableDATpatching)
            {
                if (showDatWarning)
                {
                    var msg = PropertyManager.GetString("dat_older_warning_msg").Item;
                    var popupMsg = new GameEventPopupString(session, msg);
                    var chatMsg = new GameMessageSystemChat(msg, ChatMessageType.WorldBroadcast);
                    var transientMsg = new GameEventCommunicationTransientString(session, msg);

                    //var resourceType = message.Payload.ReadUInt32();
                    //var dataId = message.Payload.ReadUInt32();
                    var errorType = 1u; // unknown enum... this seems to trigger reattempt request by client.

                    var dddErrorMsg = new GameMessageDDDErrorMessage((uint)qdid_type, qdid_ID, errorType);

                    if (session.Player.FirstEnterWorldDone) // Boot client with msg
                    {
                        //session.Network.EnqueueSend(new GameMessageBootAccount($"\n{msg}"), dddErrorMsg);
                        //session.LogOffPlayer(true);
                        session.Terminate(SessionTerminationReason.DATsPatchingDisabled, new GameMessageBootAccount($" because {msg.TrimEnd('.')}"));
                    }
                    else // cannot cleanly boot player that hasn't completed first login, client crashes so msg wouldn't be seen, instead spam msgs until server auto boots them or they disconnect.
                        session.Network.EnqueueSend(popupMsg, chatMsg, transientMsg, dddErrorMsg);
                }

                return;
            }

            // Landblock also needs to send the LandBlockInfo (0xFFFE) file with it...
            if (qdid_type == DatFileType.LandBlock)
            {
                var qdid_ID_FFFE = qdid_ID - 1;
                if (DatManager.CellDat.AllFiles.TryGetValue(qdid_ID_FFFE, out var datFileFFFE))
                    //session.Network.EnqueueSend(new GameMessageDDDDataMessage(qdid_ID_FFFE, DatDatabaseType.Cell));
                    DDDManager.AddToQueue(session, qdid_ID_FFFE, DatDatabaseType.Cell);
                //else
                //    log.Warn($"[DDD] The server does not have the requested data on 0x{qdid_ID_FFFE:X8} | {qdid_type} to send.");
            }

            if (qdid_type == DatFileType.LandBlock || qdid_type == DatFileType.LandBlockInfo || qdid_type == DatFileType.EnvCell)
            {
                if (DatManager.CellDat.AllFiles.TryGetValue(qdid_ID, out var datFile))
                    //session.Network.EnqueueSend(new GameMessageDDDDataMessage(qdid_ID, DatDatabaseType.Cell));
                    DDDManager.AddToQueue(session, qdid_ID, DatDatabaseType.Cell);
                else if (qdid_type != DatFileType.LandBlockInfo)
                    log.Warn($"[DDD] DDD_RequestDataMessage: The server does not have the requested data on 0x{qdid_ID:X8} | {qdid_type} to send to client {session.Account}.");
            }
            else
            {
                log.Warn($"[DDD] DDD_RequestDataMessage: client {session.Account} requested data on 0x{qdid_ID:X8} | {qdid_type} which has been ignored.");
            }
        }
    }
}
