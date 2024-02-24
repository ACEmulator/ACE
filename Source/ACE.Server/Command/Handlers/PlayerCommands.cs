using System;
using System.Collections.Generic;
using System.Linq;

using log4net;

using ACE.Common;
using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects;
using ACE.Database.Models.Log;
using System.Text;
using ACE.Server.Entity.TownControl;
using ACE.Entity.Enum.Properties;
using System.Threading;
using ACE.Database.Models.Auth;
using ACE.DatLoader;
using ACE.Server.Network.Enum;

namespace ACE.Server.Command.Handlers
{
    public static class PlayerCommands
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // pop
        [CommandHandler("pop", AccessLevel.Player, CommandHandlerFlag.None, 0,
            "Show current world population",
            "")]
        public static void HandlePop(Session session, params string[] parameters)
        {
            CommandHandlerHelper.WriteOutputInfo(session, $"Current world population: {PlayerManager.GetOnlineCount():N0}", ChatMessageType.Broadcast);
        }

        // quest info (uses GDLe formatting to match plugin expectations)
        [CommandHandler("myquests", AccessLevel.Player, CommandHandlerFlag.RequiresWorld, "Shows your quest log")]
        public static void HandleQuests(Session session, params string[] parameters)
        {
            if (!PropertyManager.GetBool("quest_info_enabled").Item)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat("The command \"myquests\" is not currently enabled on this server.", ChatMessageType.Broadcast));
                return;
            }

            var quests = session.Player.QuestManager.GetQuests();

            if (quests.Count == 0)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat("Quest list is empty.", ChatMessageType.Broadcast));
                return;
            }

            foreach (var playerQuest in quests)
            {
                var text = "";
                var questName = QuestManager.GetQuestName(playerQuest.QuestName);
                var quest = DatabaseManager.World.GetCachedQuest(questName);
                if (quest == null)
                {
                    Console.WriteLine($"Couldn't find quest {playerQuest.QuestName}");
                    continue;
                }

                var minDelta = quest.MinDelta;
                if (QuestManager.CanScaleQuestMinDelta(quest))
                    minDelta = (uint)(quest.MinDelta * PropertyManager.GetDouble("quest_mindelta_rate").Item);

                text += $"{playerQuest.QuestName.ToLower()} - {playerQuest.NumTimesCompleted} solves ({playerQuest.LastTimeCompleted})";
                text += $"\"{quest.Message}\" {quest.MaxSolves} {minDelta}";

                session.Network.EnqueueSend(new GameMessageSystemChat(text, ChatMessageType.Broadcast));
            }
        }

        /// <summary>
        /// For characters/accounts who currently own multiple houses, used to select which house they want to keep
        /// </summary>
        [CommandHandler("house-select", AccessLevel.Player, CommandHandlerFlag.RequiresWorld, 1, "For characters/accounts who currently own multiple houses, used to select which house they want to keep")]
        public static void HandleHouseSelect(Session session, params string[] parameters)
        {
            HandleHouseSelect(session, false, parameters);
        }

        public static void HandleHouseSelect(Session session, bool confirmed, params string[] parameters)
        {
            if (!int.TryParse(parameters[0], out var houseIdx))
                return;

            // ensure current multihouse owner
            if (!session.Player.IsMultiHouseOwner(false))
            {
                log.Warn($"{session.Player.Name} tried to /house-select {houseIdx}, but they are not currently a multi-house owner!");
                return;
            }

            // get house info for this index
            var multihouses = session.Player.GetMultiHouses();

            if (houseIdx < 1 || houseIdx > multihouses.Count)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Please enter a number between 1 and {multihouses.Count}.", ChatMessageType.Broadcast));
                return;
            }

            var keepHouse = multihouses[houseIdx - 1];

            // show confirmation popup
            if (!confirmed)
            {
                var houseType = $"{keepHouse.HouseType}".ToLower();
                var loc = HouseManager.GetCoords(keepHouse.SlumLord.Location);

                var msg = $"Are you sure you want to keep the {houseType} at\n{loc}?";
                if (!session.Player.ConfirmationManager.EnqueueSend(new Confirmation_Custom(session.Player.Guid, () => HandleHouseSelect(session, true, parameters)), msg))
                    session.Player.SendWeenieError(WeenieError.ConfirmationInProgress);
                return;
            }

            // house to keep confirmed, abandon the other houses
            var abandonHouses = new List<House>(multihouses);
            abandonHouses.RemoveAt(houseIdx - 1);

            foreach (var abandonHouse in abandonHouses)
            {
                var house = session.Player.GetHouse(abandonHouse.Guid.Full);

                HouseManager.HandleEviction(house, house.HouseOwner ?? 0, true);
            }

            // set player properties for house to keep
            var player = PlayerManager.FindByGuid(keepHouse.HouseOwner ?? 0, out bool isOnline);
            if (player == null)
            {
                log.Error($"{session.Player.Name}.HandleHouseSelect({houseIdx}) - couldn't find HouseOwner {keepHouse.HouseOwner} for {keepHouse.Name} ({keepHouse.Guid})");
                return;
            }

            player.HouseId = keepHouse.HouseId;
            player.HouseInstance = keepHouse.Guid.Full;

            player.SaveBiotaToDatabase();

            // update house panel for current player
            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(3.0f);  // wait for slumlord inventory biotas above to save
            actionChain.AddAction(session.Player, session.Player.HandleActionQueryHouse);
            actionChain.EnqueueChain();

            Console.WriteLine("OK");
        }

        [CommandHandler("debugcast", AccessLevel.Player, CommandHandlerFlag.RequiresWorld, "Shows debug information about the current magic casting state")]
        public static void HandleDebugCast(Session session, params string[] parameters)
        {
            var physicsObj = session.Player.PhysicsObj;

            var pendingActions = physicsObj.MovementManager.MoveToManager.PendingActions;
            var currAnim = physicsObj.PartArray.Sequence.CurrAnim;

            session.Network.EnqueueSend(new GameMessageSystemChat(session.Player.MagicState.ToString(), ChatMessageType.Broadcast));
            session.Network.EnqueueSend(new GameMessageSystemChat($"IsMovingOrAnimating: {physicsObj.IsMovingOrAnimating}", ChatMessageType.Broadcast));
            session.Network.EnqueueSend(new GameMessageSystemChat($"  IsAnimating: {physicsObj.IsAnimating}", ChatMessageType.Broadcast));
            session.Network.EnqueueSend(new GameMessageSystemChat($"  PartArray.Sequence.is_first_cyclic(): {physicsObj.PartArray.Sequence.is_first_cyclic()}", ChatMessageType.Broadcast));
            session.Network.EnqueueSend(new GameMessageSystemChat($"  CachedVelocity: {physicsObj.CachedVelocity}", ChatMessageType.Broadcast));
            session.Network.EnqueueSend(new GameMessageSystemChat($"  Velocity: {physicsObj.Velocity}", ChatMessageType.Broadcast));
            session.Network.EnqueueSend(new GameMessageSystemChat($"  MovementManager.MotionInterpreter.InterpretedState.HasCommands(): {physicsObj.MovementManager.MotionInterpreter.InterpretedState.HasCommands()}", ChatMessageType.Broadcast));
            session.Network.EnqueueSend(new GameMessageSystemChat($"  MovementManager.MoveToManager.Initialized: {physicsObj.MovementManager.MoveToManager.Initialized}", ChatMessageType.Broadcast));

            session.Network.EnqueueSend(new GameMessageSystemChat($"TransientState: {physicsObj.TransientState}", ChatMessageType.Broadcast));
            session.Network.EnqueueSend(new GameMessageSystemChat($"State: {physicsObj.State}", ChatMessageType.Broadcast));
            session.Network.EnqueueSend(new GameMessageSystemChat($"JumpedThisFrame: {physicsObj.JumpedThisFrame}", ChatMessageType.Broadcast));
            session.Network.EnqueueSend(new GameMessageSystemChat($"Acceleration: {physicsObj.Acceleration}", ChatMessageType.Broadcast));
            session.Network.EnqueueSend(new GameMessageSystemChat($"Omega: {physicsObj.Omega}", ChatMessageType.Broadcast));
            session.Network.EnqueueSend(new GameMessageSystemChat($"CollidingWithEnvironment: {physicsObj.CollidingWithEnvironment}", ChatMessageType.Broadcast));            

            session.Network.EnqueueSend(new GameMessageSystemChat($"PendingActions: {pendingActions.Count}", ChatMessageType.Broadcast));
            session.Network.EnqueueSend(new GameMessageSystemChat($"CurrAnim: {currAnim?.Value.Anim.ID:X8}", ChatMessageType.Broadcast));
        }

        [CommandHandler("fixcast", AccessLevel.Player, CommandHandlerFlag.RequiresWorld, "Fixes magic casting if locked up for an extended time")]
        public static void HandleFixCast(Session session, params string[] parameters)
        {
            var magicState = session.Player.MagicState;

            if (magicState.IsCasting && DateTime.UtcNow - magicState.StartTime > TimeSpan.FromSeconds(5))
            {
                session.Network.EnqueueSend(new GameEventCommunicationTransientString(session, "Fixed casting state"));
                session.Player.SendUseDoneEvent();
                magicState.OnCastDone();
            }
        }

        [CommandHandler("castmeter", AccessLevel.Player, CommandHandlerFlag.RequiresWorld, "Shows the fast casting efficiency meter")]
        public static void HandleCastMeter(Session session, params string[] parameters)
        {
            if (parameters.Length == 0)
            {
                session.Player.MagicState.CastMeter = !session.Player.MagicState.CastMeter;
            }
            else
            {
                if (parameters[0].Equals("on", StringComparison.OrdinalIgnoreCase))
                    session.Player.MagicState.CastMeter = true;
                else
                    session.Player.MagicState.CastMeter = false;
            }
            session.Network.EnqueueSend(new GameMessageSystemChat($"Cast efficiency meter {(session.Player.MagicState.CastMeter ? "enabled" : "disabled")}", ChatMessageType.Broadcast));
        }

        private static List<string> configList = new List<string>()
        {
            "Common settings:\nConfirmVolatileRareUse, MainPackPreferred, SalvageMultiple, SideBySideVitals, UseCraftSuccessDialog",
            "Interaction settings:\nAcceptLootPermits, AllowGive, AppearOffline, AutoAcceptFellowRequest, DragItemOnPlayerOpensSecureTrade, FellowshipShareLoot, FellowshipShareXP, IgnoreAllegianceRequests, IgnoreFellowshipRequests, IgnoreTradeRequests, UseDeception",
            "UI settings:\nCoordinatesOnRadar, DisableDistanceFog, DisableHouseRestrictionEffects, DisableMostWeatherEffects, FilterLanguage, LockUI, PersistentAtDay, ShowCloak, ShowHelm, ShowTooltips, SpellDuration, TimeStamp, ToggleRun, UseMouseTurning",
            "Chat settings:\nHearAllegianceChat, HearGeneralChat, HearLFGChat, HearRoleplayChat, HearSocietyChat, HearTradeChat, HearPKDeaths, StayInChatMode",
            "Combat settings:\nAdvancedCombatUI, AutoRepeatAttack, AutoTarget, LeadMissileTargets, UseChargeAttack, UseFastMissiles, ViewCombatTarget, VividTargetingIndicator",
            "Character display settings:\nDisplayAge, DisplayAllegianceLogonNotifications, DisplayChessRank, DisplayDateOfBirth, DisplayFishingSkill, DisplayNumberCharacterTitles, DisplayNumberDeaths"
        };

        /// <summary>
        /// Mapping of GDLE -> ACE CharacterOptions
        /// </summary>
        private static Dictionary<string, string> translateOptions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            // Common
            { "ConfirmVolatileRareUse", "ConfirmUseOfRareGems" },
            { "MainPackPreferred", "UseMainPackAsDefaultForPickingUpItems" },
            { "SalvageMultiple", "SalvageMultipleMaterialsAtOnce" },
            { "SideBySideVitals", "SideBySideVitals" },
            { "UseCraftSuccessDialog", "UseCraftingChanceOfSuccessDialog" },

            // Interaction
            { "AcceptLootPermits", "AcceptCorpseLootingPermissions" },
            { "AllowGive", "LetOtherPlayersGiveYouItems" },
            { "AppearOffline", "AppearOffline" },
            { "AutoAcceptFellowRequest", "AutomaticallyAcceptFellowshipRequests" },
            { "DragItemOnPlayerOpensSecureTrade", "DragItemToPlayerOpensTrade" },
            { "FellowshipShareLoot", "ShareFellowshipLoot" },
            { "FellowshipShareXP", "ShareFellowshipExpAndLuminance" },
            { "IgnoreAllegianceRequests", "IgnoreAllegianceRequests" },
            { "IgnoreFellowshipRequests", "IgnoreFellowshipRequests" },
            { "IgnoreTradeRequests", "IgnoreAllTradeRequests" },
            { "UseDeception", "AttemptToDeceiveOtherPlayers" },

            // UI
            { "CoordinatesOnRadar", "ShowCoordinatesByTheRadar" },
            { "DisableDistanceFog", "DisableDistanceFog" },
            { "DisableHouseRestrictionEffects", "DisableHouseRestrictionEffects" },
            { "DisableMostWeatherEffects", "DisableMostWeatherEffects" },
            { "FilterLanguage", "FilterLanguage" },
            { "LockUI", "LockUI" },
            { "PersistentAtDay", "AlwaysDaylightOutdoors" },
            { "ShowCloak", "ShowYourCloak" },
            { "ShowHelm", "ShowYourHelmOrHeadGear" },
            { "ShowTooltips", "Display3dTooltips" },
            { "SpellDuration", "DisplaySpellDurations" },
            { "TimeStamp", "DisplayTimestamps" },
            { "ToggleRun", "RunAsDefaultMovement" },
            { "UseMouseTurning", "UseMouseTurning" },

            // Chat
            { "HearAllegianceChat", "ListenToAllegianceChat" },
            { "HearGeneralChat", "ListenToGeneralChat" },
            { "HearLFGChat", "ListenToLFGChat" },
            { "HearRoleplayChat", "ListentoRoleplayChat" },
            { "HearSocietyChat", "ListenToSocietyChat" },
            { "HearTradeChat", "ListenToTradeChat" },
            { "HearPKDeaths", "ListenToPKDeathMessages" },
            { "StayInChatMode", "StayInChatModeAfterSendingMessage" },

            // Combat
            { "AdvancedCombatUI", "AdvancedCombatInterface" },
            { "AutoRepeatAttack", "AutoRepeatAttacks" },
            { "AutoTarget", "AutoTarget" },
            { "LeadMissileTargets", "LeadMissileTargets" },
            { "UseChargeAttack", "UseChargeAttack" },
            { "UseFastMissiles", "UseFastMissiles" },
            { "ViewCombatTarget", "KeepCombatTargetsInView" },
            { "VividTargetingIndicator", "VividTargetingIndicator" },

            // Character Display
            { "DisplayAge", "AllowOthersToSeeYourAge" },
            { "DisplayAllegianceLogonNotifications", "ShowAllegianceLogons" },
            { "DisplayChessRank", "AllowOthersToSeeYourChessRank" },
            { "DisplayDateOfBirth", "AllowOthersToSeeYourDateOfBirth" },
            { "DisplayFishingSkill", "AllowOthersToSeeYourFishingSkill" },
            { "DisplayNumberCharacterTitles", "AllowOthersToSeeYourNumberOfTitles" },
            { "DisplayNumberDeaths", "AllowOthersToSeeYourNumberOfDeaths" },
        };

        /// <summary>
        /// Manually sets a character option on the server. Use /config list to see a list of settings.
        /// </summary>
        [CommandHandler("config", AccessLevel.Player, CommandHandlerFlag.RequiresWorld, 1, "Manually sets a character option on the server.\nUse /config list to see a list of settings.", "<setting> <on/off>")]
        public static void HandleConfig(Session session, params string[] parameters)
        {
            if (!PropertyManager.GetBool("player_config_command").Item)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat("The command \"config\" is not currently enabled on this server.", ChatMessageType.Broadcast));
                return;
            }

            // /config list - show character options
            if (parameters[0].Equals("list", StringComparison.OrdinalIgnoreCase))
            {
                foreach (var line in configList)
                    session.Network.EnqueueSend(new GameMessageSystemChat(line, ChatMessageType.Broadcast));

                return;
            }

            // translate GDLE CharacterOptions for existing plugins
            if (!translateOptions.TryGetValue(parameters[0], out var param) || !Enum.TryParse(param, out CharacterOption characterOption))
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Unknown character option: {parameters[0]}", ChatMessageType.Broadcast));
                return;
            }

            var option = session.Player.GetCharacterOption(characterOption);

            // modes of operation:
            // on / off / toggle

            // - if none specified, default to toggle
            var mode = "toggle";

            if (parameters.Length > 1)
            {
                if (parameters[1].Equals("on", StringComparison.OrdinalIgnoreCase))
                    mode = "on";
                else if (parameters[1].Equals("off", StringComparison.OrdinalIgnoreCase))
                    mode = "off";
            }

            // set character option
            if (mode.Equals("on"))
                option = true;
            else if (mode.Equals("off"))
                option = false;
            else
                option = !option;

            session.Player.SetCharacterOption(characterOption, option);

            session.Network.EnqueueSend(new GameMessageSystemChat($"Character option {parameters[0]} is now {(option ? "on" : "off")}.", ChatMessageType.Broadcast));

            // update client
            session.Network.EnqueueSend(new GameEventPlayerDescription(session));
        }

        /// <summary>
        /// Force resend of all visible objects known to this player. Can fix rare cases of invisible object bugs.
        /// Can only be used once every 5 mins max.
        /// </summary>
        [CommandHandler("objsend", AccessLevel.Player, CommandHandlerFlag.RequiresWorld, "Force resend of all visible objects known to this player. Can fix rare cases of invisible object bugs. Can only be used once every 5 mins max.")]
        public static void HandleObjSend(Session session, params string[] parameters)
        {
            // a good repro spot for this is the first room after the door in facility hub
            // in the portal drop / staircase room, the VisibleCells do not have the room after the door
            // however, the room after the door *does* have the portal drop / staircase room in its VisibleCells (the inverse relationship is imbalanced)
            // not sure how to fix this atm, seems like it triggers a client bug..

            if (DateTime.UtcNow - session.Player.PrevObjSend < TimeSpan.FromMinutes(5))
            {
                session.Player.SendTransientError("You have used this command too recently!");
                return;
            }

            var creaturesOnly = parameters.Length > 0 && parameters[0].Contains("creature", StringComparison.OrdinalIgnoreCase);

            var knownObjs = session.Player.GetKnownObjects();

            foreach (var knownObj in knownObjs)
            {
                if (creaturesOnly && !(knownObj is Creature))
                    continue;

                session.Player.RemoveTrackedObject(knownObj, false);
                session.Player.TrackObject(knownObj);
            }
            session.Player.PrevObjSend = DateTime.UtcNow;
        }

        // show player ace server versions
        [CommandHandler("aceversion", AccessLevel.Player, CommandHandlerFlag.RequiresWorld, "Shows this server's version data")]
        public static void HandleACEversion(Session session, params string[] parameters)
        {
            if (!PropertyManager.GetBool("version_info_enabled").Item)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat("The command \"aceversion\" is not currently enabled on this server.", ChatMessageType.Broadcast));
                return;
            }

            var msg = ServerBuildInfo.GetVersionInfo();

            session.Network.EnqueueSend(new GameMessageSystemChat(msg, ChatMessageType.WorldBroadcast));
        }

        // reportbug < code | content > < description >
        [CommandHandler("reportbug", AccessLevel.Player, CommandHandlerFlag.RequiresWorld, 2,
            "Generate a Bug Report",
            "<category> <description>\n" +
            "This command generates a URL for you to copy and paste into your web browser to submit for review by server operators and developers.\n" +
            "Category can be the following:\n" +
            "Creature\n" +
            "NPC\n" +
            "Item\n" +
            "Quest\n" +
            "Recipe\n" +
            "Landblock\n" +
            "Mechanic\n" +
            "Code\n" +
            "Other\n" +
            "For the first three options, the bug report will include identifiers for what you currently have selected/targeted.\n" +
            "After category, please include a brief description of the issue, which you can further detail in the report on the website.\n" +
            "Examples:\n" +
            "/reportbug creature Drudge Prowler is over powered\n" +
            "/reportbug npc Ulgrim doesn't know what to do with Sake\n" +
            "/reportbug quest I can't enter the portal to the Lost City of Frore\n" +
            "/reportbug recipe I cannot combine Bundle of Arrowheads with Bundle of Arrowshafts\n" +
            "/reportbug code I was killed by a Non-Player Killer\n"
            )]
        public static void HandleReportbug(Session session, params string[] parameters)
        {
            if (!PropertyManager.GetBool("reportbug_enabled").Item)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat("The command \"reportbug\" is not currently enabled on this server.", ChatMessageType.Broadcast));
                return;
            }

            var category = parameters[0];
            var description = "";

            for (var i = 1; i < parameters.Length; i++)
                description += parameters[i] + " ";

            description.Trim();

            switch (category.ToLower())
            {
                case "creature":
                case "npc":
                case "quest":
                case "item":
                case "recipe":
                case "landblock":
                case "mechanic":
                case "code":
                case "other":
                    break;
                default:
                    category = "Other";
                    break;
            }

            var sn = ConfigManager.Config.Server.WorldName;
            var c = session.Player.Name;

            var st = "ACE";

            //var versions = ServerBuildInfo.GetVersionInfo();
            var databaseVersion = DatabaseManager.World.GetVersion();
            var sv = ServerBuildInfo.FullVersion;
            var pv = databaseVersion.PatchVersion;

            //var ct = PropertyManager.GetString("reportbug_content_type").Item;
            var cg = category.ToLower();

            var w = "";
            var g = "";

            if (cg == "creature" || cg == "npc"|| cg == "item" || cg == "item")
            {
                var objectId = new ObjectGuid();
                if (session.Player.HealthQueryTarget.HasValue || session.Player.ManaQueryTarget.HasValue || session.Player.CurrentAppraisalTarget.HasValue)
                {
                    if (session.Player.HealthQueryTarget.HasValue)
                        objectId = new ObjectGuid((uint)session.Player.HealthQueryTarget);
                    else if (session.Player.ManaQueryTarget.HasValue)
                        objectId = new ObjectGuid((uint)session.Player.ManaQueryTarget);
                    else
                        objectId = new ObjectGuid((uint)session.Player.CurrentAppraisalTarget);

                    //var wo = session.Player.CurrentLandblock?.GetObject(objectId);

                    var wo = session.Player.FindObject(objectId.Full, Player.SearchLocations.Everywhere);

                    if (wo != null)
                    {
                        w = $"{wo.WeenieClassId}";
                        g = $"0x{wo.Guid:X8}";
                    }
                }
            }

            var l = session.Player.Location.ToLOCString();

            var issue = description;

            var urlbase = $"https://www.accpp.net/bug?";

            var url = urlbase;
            if (sn.Length > 0)
                url += $"sn={Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(sn))}";
            if (c.Length > 0)
                url += $"&c={Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(c))}";
            if (st.Length > 0)
                url += $"&st={Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(st))}";
            if (sv.Length > 0)
                url += $"&sv={Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(sv))}";
            if (pv.Length > 0)
                url += $"&pv={Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(pv))}";
            //if (ct.Length > 0)
            //    url += $"&ct={Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(ct))}";
            if (cg.Length > 0)
            {
                if (cg == "npc")
                    cg = cg.ToUpper();
                else
                    cg = char.ToUpper(cg[0]) + cg.Substring(1);
                url += $"&cg={Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(cg))}";
            }
            if (w.Length > 0)
                url += $"&w={Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(w))}";
            if (g.Length > 0)
                url += $"&g={Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(g))}";
            if (l.Length > 0)
                url += $"&l={Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(l))}";
            if (issue.Length > 0)
                url += $"&i={Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(issue))}";

            var msg = "\n\n\n\n";
            msg += "Bug Report - Copy and Paste the following URL into your browser to submit a bug report\n";
            msg += "-=-\n";
            msg += $"{url}\n";
            msg += "-=-\n";
            msg += "\n\n\n\n";

            session.Network.EnqueueSend(new GameMessageSystemChat(msg, ChatMessageType.AdminTell));
        }


        [CommandHandler("arena", AccessLevel.Player, CommandHandlerFlag.None, 1,
            "The arena command is used to join an arena event or get information about arena statistics")]
        public static void HandleArena(Session session, params string[] parameters)
        {
            if (parameters.Count() < 1)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Invalid parameters.  See the arena help file below for valid parameters.");
                parameters[0] = "help";
            }

            if (session.Player.LastArenaCommandTimestamp.HasValue && Time.GetDateTimeFromTimestamp(session.Player.LastArenaCommandTimestamp.Value) > DateTime.Now.AddSeconds(-3))
            {
                CommandHandlerHelper.WriteOutputInfo(session, "To prevent abuse, you can only issue one arena command every 3 seconds. Please try again.");
                return;
            }
            else
            {
                session.Player.LastArenaCommandTimestamp = Time.GetUnixTime(DateTime.Now);
            }

            var actionType = parameters[0];

            switch (actionType?.ToLower())
            {
                case "join":

                    string eventType = "1v1";
                    if (parameters.Length > 1)
                    {
                        eventType = parameters[1];

                        for (int i = 2; i < parameters.Length; i++)
                        {
                            eventType += parameters[i];
                        }

                        if(!ArenaManager.IsValidEventType(eventType))
                        {
                            CommandHandlerHelper.WriteOutputInfo(session, $"Invalid parameters.  The Join command does not support the event type {eventType}. Proper syntax is as follows...\n  To join a 1v1 arena match: /arena join\n  To join a specific type of arena match, replace eventType with the string code for the type of match you want to join, such as 1v1, 2v2, ffa. : /arena join eventType\n  To get your current character's stats: /arena stats\n  To get a named character's stats, replace characterName with the target character's name: /arena stats characterName");
                            return;
                        }
                    }

                    string resultMsg = JoinArenaQueue(session, eventType.ToLower());
                    if (resultMsg != null)
                    {
                        CommandHandlerHelper.WriteOutputInfo(session, resultMsg);
                        return;
                    }
                    break;

                case "cancel":
                    
                    ArenaManager.PlayerCancel(session.Player.Character.Id);

                    break;

                case "forfeit":
                    CommandHandlerHelper.WriteOutputInfo(session, "Forfeit feature not yet supported, check back later");
                    break;

                case "observe":
                case "watch":
                    string eventIdParam = "";

                    if(!PropertyManager.GetBool("arena_allow_observers").Item)
                    {
                        CommandHandlerHelper.WriteOutputInfo(session, $"The arena observer feature is currently disabled");
                        return;
                    }

                    if (parameters.Length != 2)
                    {
                        CommandHandlerHelper.WriteOutputInfo(session, $"Invalid parameters. The {actionType} command requires an EventID parameter to specify which event to join as an observer. Use the \"/arena info\" command to list all active arena events, including their EventID values.\nUsage: To watch an arena event as an observer /arena watch EventID");
                        return;
                    }

                    //Parse EventID param to int and verify it corresponds to an active event
                    int eventID = 0;
                    eventIdParam = parameters[1];
                    try
                    {
                        eventID = int.Parse(eventIdParam);
                    }
                    catch(Exception)
                    {
                        CommandHandlerHelper.WriteOutputInfo(session, $"Invalid parameters. Invalid EventID value {eventIdParam}\nThe {actionType} command requires an EventID parameter to specify which event to join as an observer. Use the \"/arena info\" command to list all active arena events, including their EventID values.\nUsage: To watch an arena event as an observer /arena watch EventID");
                        return;
                    }

                    var arenaEvent = ArenaManager.GetActiveEvents().FirstOrDefault(x => x.Id == eventID);
                    if(arenaEvent != null)
                    {
                        ArenaManager.ObserveEvent(session.Player, eventID);
                    }
                    else
                    {
                        CommandHandlerHelper.WriteOutputInfo(session, $"Invalid parameters. EventID {eventIdParam} does not correspond to an active arena event\nThe {actionType} command requires an EventID parameter to specify which event to join as an observer. Use the \"/arena info\" command to list all active arena events, including their EventID values.\nUsage: To watch an arena event as an observer /arena watch EventID");
                        return;
                    }

                    break;                        

                case "info":

                    var queuedPlayers = ArenaManager.GetQueuedPlayers();
                    var queuedOnes = queuedPlayers.Where(x => x.EventType.ToLower().Equals("1v1"));
                    var queuedTwos = queuedPlayers.Where(x => x.EventType.ToLower().Equals("2v2"));
                    var queuedFFA = queuedPlayers.Where(x => x.EventType.ToLower().Equals("ffa"));
                    var longestOnesWait = queuedOnes.Count() > 0 ? (DateTime.Now - queuedOnes.Min(x => x.CreateDateTime)) : new TimeSpan(0);
                    var longestTwosWait = queuedTwos.Count() > 0 ? (DateTime.Now - queuedTwos.Min(x => x.CreateDateTime)) : new TimeSpan(0);
                    var longestFFAWait = queuedFFA.Count() > 0 ? (DateTime.Now - queuedFFA.Min(x => x.CreateDateTime)) : new TimeSpan(0);

                    string queueInfo = $"Current Arena Queues\n  1v1: {queuedOnes.Count()} players queued with longest wait at {string.Format("{0:%h}h {0:%m}m {0:%s}s", longestOnesWait)}\n  2v2: {queuedTwos.Count()} players queued, with longest wait at {string.Format("{0:%h}h {0:%m}m {0:%s}s", longestTwosWait)}\n  FFA: {queuedFFA.Count()} players queued, with longest wait at {string.Format("{0:%h}h {0:%m}m {0:%s}s", longestFFAWait)}";

                    var activeEvents = ArenaManager.GetActiveEvents();
                    var eventsOnes = activeEvents.Where(x => x.EventType.ToLower().Equals("1v1"));
                    var eventsTwos = activeEvents.Where(x => x.EventType.ToLower().Equals("2v2"));
                    var eventsFFA = activeEvents.Where(x => x.EventType.ToLower().Equals("ffa"));

                    string onesEventInfo = eventsOnes.Count() == 0 ? "No active events" : "";
                    foreach (var ev in eventsOnes)
                    {
                        onesEventInfo += $"\n    EventID: {(ev.Id < 1 ? "Pending" : ev.Id.ToString())}\n" +
                                         $"    Arena: {ArenaManager.GetArenaNameByLandblock(ev.Location)}\n" +
                                         $"    Players:\n    {ev.PlayersDisplay}\n" +
                                         $"    Time Remaining: {ev.TimeRemainingDisplay}\n";
                    }

                    string twosEventInfo = eventsTwos.Count() == 0 ? "No active events" : "";
                    foreach (var ev in eventsTwos)
                    {
                        twosEventInfo += $"\n    EventID: {(ev.Id < 1 ? "Pending" : ev.Id.ToString())}\n" +
                                         $"    Arena: {ArenaManager.GetArenaNameByLandblock(ev.Location)}\n" +
                                         $"    Players:\n    {ev.PlayersDisplay}\n" +
                                         $"    Time Remaining: {ev.TimeRemainingDisplay}\n";
                    }

                    string ffaEventInfo = eventsFFA.Count() == 0 ? "No active events" : "";
                    foreach (var ev in eventsFFA)
                    {
                        ffaEventInfo += $"\n    EventID: {(ev.Id < 1 ? "Pending" : ev.Id.ToString())}\n" +
                                         $"    Arena: {ArenaManager.GetArenaNameByLandblock(ev.Location)}\n" +
                                         $"    Players:\n    {ev.PlayersDisplay}\n" +
                                         $"    Time Remaining: {ev.TimeRemainingDisplay}\n";
                    }

                    string eventInfo = $"Active Arena Matches:\n  1v1: {onesEventInfo}\n  2v2: {twosEventInfo}\n  FFA: {ffaEventInfo}\n";

                    CommandHandlerHelper.WriteOutputInfo(session, $"*********\n{queueInfo}\n\n{eventInfo}\n*********\n");
                    break;

                case "stats":                

                    string returnMsg;
                    if (parameters.Count() >= 2)
                    {
                        string playerParam = "";
                        for(int i = 1; i < parameters.Length; i++)
                        {
                            playerParam += i == 1 ? parameters[i] : $" {parameters[i]}";
                        }

                        var targetPlayer = PlayerManager.GetAllPlayers().FirstOrDefault(x => x.Name.ToLower().Equals(playerParam.ToLower()));
                        if(targetPlayer != null)
                        {
                            var targetOnlinePlayer = PlayerManager.GetOnlinePlayer(targetPlayer.Guid);
                            var targetOfflinePlayer = PlayerManager.GetOfflinePlayer(targetPlayer.Guid);

                            returnMsg = GetArenaStats(targetOnlinePlayer != null ? targetOnlinePlayer.Character.Id : (targetOfflinePlayer != null ? targetOfflinePlayer.Biota.Id : 0), targetPlayer.Name);
                        }
                        else
                        {
                            returnMsg = $"Unable to find a player named {playerParam}";
                        }
                    }
                    else
                    {
                        returnMsg = GetArenaStats(session.Player.Character.Id, session.Player.Character.Name);
                    }
                    
                    CommandHandlerHelper.WriteOutputInfo(session, returnMsg);
                    break;

                case "rank":

                    StringBuilder rankReturnMsg = new StringBuilder();
                    string eventTypeParam = "";
                    if (parameters.Count() >= 2)
                    {
                        eventTypeParam = parameters[1];
                    }

                    bool validParam = false;
                    if(eventTypeParam.ToLower().Equals("1v1") ||
                        eventTypeParam.ToLower().Equals("2v2") ||
                        eventTypeParam.ToLower().Equals("ffa"))
                    {
                        validParam = true;
                    }

                    if (!validParam)
                    {
                        CommandHandlerHelper.WriteOutputInfo(session, "Invalid Event Type Parameter\nUsage: /arena rank {eventType}\nExample: /arena rank 1v1");
                        break;
                    }

                    List<ArenaCharacterStats> topTen = DatabaseManager.Log.GetArenaTopRankedByEventType(eventTypeParam.ToLower());
                   
                    rankReturnMsg.Append($"***** Top Ten {eventTypeParam.ToLower()} Players *****\n\n");
                    for (int i = 0; i < topTen.Count(); i++)
                    {
                        var currStats = topTen[i];
                        rankReturnMsg.Append($"  Rank #{i + 1} - {currStats.CharacterName}\n  Rank Points: {currStats.RankPoints}\n  Total Matches: {currStats.TotalMatches}\n  Total Wins: {currStats.TotalWins}\n  Total Draws: {currStats.TotalDraws}\n  Total Losses: {currStats.TotalLosses}\n\n");
                    }

                    rankReturnMsg.Append($"**********\n");
                    CommandHandlerHelper.WriteOutputInfo(session, rankReturnMsg.ToString());

                    break;

                default:
                    CommandHandlerHelper.WriteOutputInfo(session, $"Arena Commands...\n\n  To join a 1v1 arena match: /arena join\n\n  To join a specific type of arena match: /arena join eventType\n  (replace eventType with the string code for the type of match you want to join; 1v1, 2v2 or FFA)\n\n  To leave an arena queue or stop observing a match: /arena cancel\n\n  To get info about players in an arena queue and active arena matches: /arena info\n\n  To get your current character's stats: /arena stats\n\n  To get a named character's stats: /arena stats characterName\n  (replace characterName with the target character's name)\n\n  To get rank leaderboard by event type: /arena rank eventType\n  (replace eventType with the string code for the type of match you want ranking for; 1v1, 2v2 or FFA)\n\n  To watch a match as a silent observer: /arena watch EventID\n  (use /arena info to get the EventID of an active arena match and use that value in the command)\n\n    To get this help file: /arena help\n");
                    return;
            }
        }

        private static string JoinArenaQueue(Session session, string eventType)
        {
            //Whitelist specific clans to participate
            uint? monarchId = session.Player.MonarchId;
            string monarchName = session.Player.Name;
            var playerAllegiance = AllegianceManager.GetAllegiance(session.Player);
            if (playerAllegiance != null && playerAllegiance.MonarchId.HasValue)
            {
                monarchId = playerAllegiance.MonarchId;
                monarchName = playerAllegiance.Monarch.Player.Name;
            }

            var whiteListId = monarchId.HasValue ? (int)monarchId.Value : (int)session.Player.Character.Id;
            var isWhitelisted = TownControlAllegiances.IsAllowedAllegiance(whiteListId);

            if (!isWhitelisted)
            {
                return "To participate in an Arena match your monarch must be whitelisted.  Please reach out to an admin to get whitelisted.  This helps prevent abuse, apologies for the inconvenience.";
            }

            //Blacklist specific players
            var blacklistString = PropertyManager.GetString("arenas_blacklist").Item;
            if (!string.IsNullOrEmpty(blacklistString))
            {
                var blacklist = blacklistString.Split(',');
                foreach (var charIdString in blacklist)
                {
                    if (uint.TryParse(charIdString, out uint charId) && session.Player.Character.Id == charId)
                    {
                        return "You are blacklisted from joining Arena events, probably because you're a cunt who tried to abuse it or some shit.  Fuck yourself.  Or ask forgiveness from Doc Z.  Whatever, I don't care.";
                    }
                }
            }            

            var minLevel = PropertyManager.GetLong("arenas_min_level").Item;
            if (session.Player.Level < minLevel)
            {
                return $"You must be at least level {minLevel} to join an arena match";
            }

            if(session.Player.IsArenaObserver ||
                session.Player.IsPendingArenaObserver ||
                session.Player.CloakStatus == CloakStatus.On)
                return $"You cannot join an arena queue while you're watching an arena event. Use /arena cancel to stop watching the current event before you queue.";

            if (!session.Player.IsPK)
                return $"You cannot join an arena queue until you are in a PK state";

            if(session.Player.PKTimerActive)
                return $"You cannot join an arena queue while you are PK tagged";

            string returnMsg;
            if(!ArenaManager.AddPlayerToQueue(
                session.Player.Character.Id,
                session.Player.Character.Name,
                session.Player.Level,
                eventType,
                monarchId.HasValue ? monarchId.Value : session.Player.Character.Id,
                monarchName,
                session.EndPointC2S?.Address?.ToString(),
                out returnMsg))
            {
                return returnMsg;
            }

            return $"You have successfully joined the {eventType} arena queue";
        }

        private static string GetArenaStats(uint characterId, string characterName)
        {
            return DatabaseManager.Log.GetArenaStatsByCharacterId(characterId, characterName);
        }

        

        [CommandHandler("ForceLogoffStuckCharacter", AccessLevel.Player, CommandHandlerFlag.RequiresWorld, "Force log off of character that's stuck in game.  Is only allowed when initiated from a character that is on the same account as the target character.")]
        public static void HandleForceLogoffStuckCharacter(Session session, params string[] parameters)
        {
            var playerName = "";
            if (parameters.Length > 0)
                playerName = string.Join(" ", parameters);

            Player target = null;

            if (!string.IsNullOrEmpty(playerName))
            {
                var plr = PlayerManager.FindByName(playerName);
                if (plr != null)
                {
                    target = PlayerManager.GetOnlinePlayer(plr.Guid);

                    if (target == null)
                    {
                        CommandHandlerHelper.WriteOutputInfo(session, $"Unable to force log off for {plr.Name}: Player is not online.");
                        return;
                    }

                    //Verify the target is not the current player
                    if(session.Player.Guid == target.Guid)
                    {
                        CommandHandlerHelper.WriteOutputInfo(session, $"Unable to force log off for {plr.Name}: You cannot target yourself, please try with a different character on same account.");
                        return;
                    }

                    //Verify the target is on the same account as the current player
                    if (session.AccountId != target.Account.AccountId)
                    {
                        CommandHandlerHelper.WriteOutputInfo(session, $"Unable to force log off for {plr.Name}: Target must be within same account as the player who issues the logoff command. Please reach out for admin support.");
                        return;
                    }

                    DeveloperCommands.HandleForceLogoff(session, parameters);
                }
                else
                {
                    CommandHandlerHelper.WriteOutputInfo(session, $"Unable to force log off for {playerName}: Player not found.");
                    return;
                }
            }
            else
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Invalid parameters, please provide a player name for the player that needs to be logged off.");
                return;
            }            
        }

        // rename <Current Name> <New Name>
        [CommandHandler("buyrename", AccessLevel.Player, CommandHandlerFlag.None, 1,
            "Purchase a character rename for 200 PK trophies",
            "< New Name >")]
        public static void HandleBuyRename(Session session, params string[] parameters)
        {
            if(parameters.Length < 1)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Invalid parameters: please provide a new character name. Usage: /BuyRename <NewCharacterName>", ChatMessageType.Broadcast);
                return;
            }

            var isFirstRenameUsed = session.Player.CharacterRenameCount > 0;            
            var numPkTrophiesInInventory = session.Player.GetNumInventoryItemsOfWCID(1000002);
            if(isFirstRenameUsed && numPkTrophiesInInventory < 200)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Renaming your character costs 200 PK trophies. You don't have enough PK trophies in your inventory to cover the cost.", ChatMessageType.Broadcast);
                return;
            }

            var newName = string.Join(" ", parameters).Trim();
            var oldName = session.Player.Name;            

            if (oldName.StartsWith("+"))
                oldName = oldName.Substring(1);
            if (newName.StartsWith("+"))
                newName = newName.Substring(1);

            newName = newName.First().ToString().ToUpper() + newName.Substring(1);

            //Verify the new name is not in the taboo table
            if (PropertyManager.GetBool("taboo_table").Item && DatManager.PortalDat.TabooTable.ContainsBadWord(newName.ToLowerInvariant()))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Error, unable to rename your character to \"{newName}\" as that name is not allowed per the taboo table.", ChatMessageType.Broadcast);
                return;
            }

            if (PropertyManager.GetBool("creature_name_check").Item && DatabaseManager.World.IsCreatureNameInWorldDatabase(newName))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Error, unable to rename your character to \"{newName}\" as that name matches a creature name.", ChatMessageType.Broadcast);
                return;
            }

            //Verify the new name has only alpha characters, apostrophies or dashes, and isn't more than 32 characters
            if(newName.Length > 32)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Error, unable to rename your character to \"{newName}\" as that name exceeds the maximum 32 character limit.", ChatMessageType.Broadcast);
                return;
            }

            var hasInvalidChars = false;
            foreach(Char c in newName)
            {
                if(!Char.IsLetter(c) && c != '\'' && c != '-' && c != ' ')
                {
                    hasInvalidChars = true;
                }
            }

            if (hasInvalidChars)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Error, unable to rename your character to \"{newName}\" as that name contains invalid characters for a player name.  Player names may only contain characters A-Z, spaces, apostrophes or dashes.", ChatMessageType.Broadcast);
                return;
            }

            var onlinePlayer = PlayerManager.GetOnlinePlayer(oldName);
            if (onlinePlayer != null)
            {
                DatabaseManager.Shard.IsCharacterNameAvailable(newName, isAvailable =>
                {
                    if (!isAvailable)
                    {
                        CommandHandlerHelper.WriteOutputInfo(session, $"Error, a player named \"{newName}\" already exists.", ChatMessageType.Broadcast);
                        return;
                    }

                    //Check if the player has sufficient funds to purchase the rename
                    numPkTrophiesInInventory = session.Player.GetNumInventoryItemsOfWCID(1000002);
                    if (isFirstRenameUsed && numPkTrophiesInInventory < 200)
                    {
                        CommandHandlerHelper.WriteOutputInfo(session, $"Renaming your character costs 200 PK trophies. You don't have enough PK trophies in your inventory to cover the cost.", ChatMessageType.Broadcast);
                        return;
                    }
                    else
                    {
                        if (isFirstRenameUsed)
                        {
                            if (session.Player.TryConsumeFromInventoryWithNetworking(1000002, 200))
                            {
                                CommandHandlerHelper.WriteOutputInfo(session, $"200 PK trophies have been removed from your inventory", ChatMessageType.Broadcast);
                            }
                            else
                            {
                                CommandHandlerHelper.WriteOutputInfo(session, $"Error: failed consuming 200 PK trophies from your inventory. Please try again or contact an admin for support.", ChatMessageType.Broadcast);

                                //Log this failure to the audit log
                                PlayerManager.BroadcastToAuditChannel(session.Player, $"Error: player {session.Player.Name} used /BuyRename command, and was verified to have enough PK trophies, but failed to consume the PK trophies with TryConsumeFromInventoryWithNetworking.");
                                return;
                            }                            
                        }                        
                    }

                    onlinePlayer.Character.Name = newName;
                    onlinePlayer.CharacterChangesDetected = true;
                    onlinePlayer.Name = newName;
                    onlinePlayer.CharacterRenameCount += 1;
                    onlinePlayer.SavePlayerToDatabase();

                    CommandHandlerHelper.WriteOutputInfo(session, $"Player named \"{oldName}\" renamed to \"{newName}\" successfully!", ChatMessageType.Broadcast);

                    PlayerManager.BroadcastToAuditChannel(session.Player, $"Player {oldName} used /BuyRename command to rename themselves to {newName}.");

                    onlinePlayer.Session.LogOffPlayer();
                });
            }
        }

        #region Town Control

        private static string _townOwnerMessageHeader = "Town Owners:\n";
        private static string _townOwnerRecordTemplate = "{0} is owned by {1}\n";

        [CommandHandler("town-owners", AccessLevel.Player, CommandHandlerFlag.None, 0,
            "Show owners of each town",
            "")]
        public static void HandleTownOwnersQuery(Session session, params string[] parameters)
        {
            try
            {
                StringBuilder townOwnerMsg = new StringBuilder(_townOwnerMessageHeader);

                var townList = DatabaseManager.TownControl.GetAllTowns();

                foreach (var town in townList)
                {
                    string townOwner = string.Empty;

                    if (town.CurrentOwnerID.HasValue)
                    {
                        var monarch = PlayerManager.FindByGuid(town.CurrentOwnerID.Value);
                        townOwner = monarch.Name;
                    }
                    else
                    {
                        townOwner = "nobody";
                    }

                    townOwnerMsg.Append(String.Format(_townOwnerRecordTemplate, town.TownName, townOwner));
                }

                CommandHandlerHelper.WriteOutputInfo(session, townOwnerMsg.ToString(), ChatMessageType.Broadcast);
            }
            catch (Exception ex)
            {
                CommandHandlerHelper.WriteOutputInfo(session, "Server Error checking town owner list.  Pls report this to the admins with a timestamp of when it happened.", ChatMessageType.Broadcast);
                log.ErrorFormat("Error in PlayerCommands.HandleTownOwnersQuery. Ex: {0}", ex);
            }
        }


        [CommandHandler("town-control-respite", AccessLevel.Player, CommandHandlerFlag.None, 1,
            "Show the remaining respite time for a given town for your clan",
            "")]
        public static void HandleTownRespiteQuery(Session session, params string[] parameters)
        {
            try
            {
                if (parameters != null && parameters.Length > 0)
                {
                    string townName = "";

                    foreach (string param in parameters)
                    {
                        townName = String.Concat(townName, param, " ");
                    }

                    townName = townName.Trim();

                    var townList = DatabaseManager.TownControl.GetAllTowns();

                    var town = townList.Find(x => x.TownName.Equals(townName, StringComparison.OrdinalIgnoreCase));
                    if (town == null)
                    {
                        CommandHandlerHelper.WriteOutputInfo(session, $"{townName} is not a valid town that is provisioned for Town Control", ChatMessageType.Broadcast);
                        return;
                    }

                    if (!session.Player.MonarchId.HasValue)
                    {
                        CommandHandlerHelper.WriteOutputInfo(session, $"You are not part of a valid allegiance, unable to check respite for {townName}", ChatMessageType.Broadcast);
                        return;
                    }

                    if (session.Player.MonarchId == town.CurrentOwnerID)
                    {
                        CommandHandlerHelper.WriteOutputInfo(session, $"Your allegiance already owns {townName}", ChatMessageType.Broadcast);
                        return;
                    }

                    var latestTcEvent = DatabaseManager.TownControl.GetLatestTownControlEventByAttackingMonarchId(session.Player.MonarchId.Value, town.TownId);

                    if (latestTcEvent != null)
                    {
                        var respiteExpirationDate = latestTcEvent.EventStartDateTime?.AddSeconds(town.ConflictRespiteLength.HasValue ? town.ConflictRespiteLength.Value : 0);
                        if (respiteExpirationDate.HasValue && respiteExpirationDate.Value > DateTime.UtcNow)
                        {
                            TimeSpan timeLeft = respiteExpirationDate.Value - DateTime.UtcNow;
                            CommandHandlerHelper.WriteOutputInfo(session, $"Your clan can attack {townName} again in {timeLeft.TotalMinutes} minutes", ChatMessageType.Broadcast);
                            return;
                        }
                    }

                    CommandHandlerHelper.WriteOutputInfo(session, $"Your clan is not currently limited by a respite and can attack {townName} any time", ChatMessageType.Broadcast);
                    return;
                }
                else
                {
                    CommandHandlerHelper.WriteOutputInfo(session, "Invalid Parameters.  Expected Syntax: /town-control-respite TownName", ChatMessageType.Broadcast);
                    return;
                }
            }
            catch (Exception ex)
            {
                CommandHandlerHelper.WriteOutputInfo(session, "Server Error checking town respite timer.  Pls report this to the admins with a timestamp of when it happened.", ChatMessageType.Broadcast);
                log.ErrorFormat("Error in PlayerCommands.HandleTownRespiteQuery. Ex: {0}", ex);
            }
        }

        #endregion
    }
}
