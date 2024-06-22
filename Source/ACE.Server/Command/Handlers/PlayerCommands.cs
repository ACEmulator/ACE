using System;
using System.Collections.Generic;

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
using System.Linq;
using ACE.DatLoader;
using ACE.Database.Models.Auth;
using log4net.Core;
using ACE.Server.Network.Enum;
using ACE.Database.Models.Shard;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.EntityFrameworkCore.Migrations;


namespace ACE.Server.Command.Handlers
{
    public static class PlayerCommands
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Position HavenDrop = (new Position(0x01AC010C, 10.28f, -19.9f, 0.005f, 0, 0, -0.707107f, 0.707107f));
        private static readonly Position TNDrop = (new Position(0x00070143, 70.000000f, - 60.000000f, 0.003500f, 0.000000f, 0.000000f, - 1.000000f, 0.000000f));
        private static readonly Position SubDrop = (new Position(0x01C9022D, 72.900002f, - 30.200001f, 0.003500f, 0.000000f, 0.000000f, - 0.990268f, 0.139173f));
        private static readonly Position FacHubDrop = (new Position(0x8A020212, 58.639099f, - 89.923103f, 6.003500f, 0.000000f, 0.000000f, - 0.099833f, 0.995004f));

        // pop
        [CommandHandler("pop", AccessLevel.Player, CommandHandlerFlag.None, 0,
            "Show current world population",
            "")]
        public static void HandlePop(Session session, params string[] parameters)
        {
            CommandHandlerHelper.WriteOutputInfo(session, $"Current world population: {PlayerManager.GetOnlineCount():N0}", ChatMessageType.Broadcast);
        }

        // recruit
        [CommandHandler("recruit", AccessLevel.Player,CommandHandlerFlag.RequiresWorld,"Sends a fellowship recruitment invite to player.")]
        public static void HandleRecruit(Session session, params string[] parameters)
        {

            if (DateTime.UtcNow - session.Player.PrevControlledCommandLine < TimeSpan.FromSeconds(10))
            {
                session.Player.SendTransientError("You have used this command too recently! Please wait at least 10 seconds between intensive commands.");
                return;
            }

            session.Player.PrevControlledCommandLine = DateTime.UtcNow;

            if (parameters == null || parameters.Count() == 0 || session.Player.Fellowship == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Use /recruit <name of player> to invite a player to your fellowship. You must be in a fellowship.", ChatMessageType.Broadcast));
                return;
            }

            var invitedPlayer = PlayerManager.GetOnlinePlayer(string.Join(" ", parameters));

            if (invitedPlayer != null)
            {
                session.Player.FellowshipRecruit(invitedPlayer);
                session.Network.EnqueueSend(new GameMessageSystemChat($"Recruit message sent to {invitedPlayer.Name}.", ChatMessageType.Broadcast));
                return;
            }

            session.Network.EnqueueSend(new GameMessageSystemChat("Could not find player to invite.", ChatMessageType.Broadcast));

        }

        [CommandHandler("recruitme", AccessLevel.Player, CommandHandlerFlag.RequiresWorld, "Sends a fellowship recruitment invite to player.")]
        public static void HandleRecruitMe(Session session, params string[] parameters)
        {


            if (DateTime.UtcNow - session.Player.PrevControlledCommandLine < TimeSpan.FromSeconds(10))
            {
                session.Player.SendTransientError("You have used this command too recently! Please wait at least 10 seconds between intensive commands.");
                return;
            }


            if (parameters == null || parameters.Length == 0 || session.Player.Fellowship != null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat("Use /recruitme <name of player> to request an invite from the player. You must NOT already be in a fellowship.\nYou can also use /recruit to send an invite request. Type /recruit for more info.", ChatMessageType.Broadcast));
                return;
            }

            var requestedPlayer = PlayerManager.GetOnlinePlayer(string.Join(" ", parameters));

            if (requestedPlayer == null) // Check null player
                { session.Network.EnqueueSend(new GameMessageSystemChat("Could not find player to request invite.", ChatMessageType.Broadcast)); return; }

            session.Player.PrevControlledCommandLine = DateTime.UtcNow; // If they got the name right and this check needs to be made to time out the command.

            if ( requestedPlayer.Fellowship == null) // Check null fellowship
                { session.Network.EnqueueSend(new GameMessageSystemChat($"The player {requestedPlayer} must be in a fellowship to send you an invite.", ChatMessageType.Broadcast)); return; }

            string fShipName = requestedPlayer.Fellowship.FellowshipName;
            uint fShipLeader = requestedPlayer.Fellowship.FellowshipLeaderGuid;
            string fShipLeaderName = PlayerManager.GetOnlinePlayer(fShipLeader).Name;
            int fShipCount = requestedPlayer.Fellowship.GetFellowshipMembers().Count();
            bool fShipLocked = requestedPlayer.Fellowship.IsLocked;
            bool fShipOpen = requestedPlayer.Fellowship.Open;
            string msg;

            if ((!fShipOpen && requestedPlayer.Guid.Full != fShipLeader) || fShipLocked || fShipCount == Fellowship.MaxFellows)
            {
                msg = $"Fellowship {fShipName} lock status is: {(fShipLocked ? "locked" : "unlocked")}. And the open status is: {(fShipOpen ? "open" : "closed")}. Please see the party leader: {fShipLeaderName} for recruitment!";
                session.Network.EnqueueSend(new GameMessageSystemChat(msg, ChatMessageType.Broadcast));
                return;
            }
            else if (!requestedPlayer.Fellowship.Open && requestedPlayer.Guid.Full == requestedPlayer.Fellowship.FellowshipLeaderGuid)
            {
                msg = $"{fShipName} is a closed fellowship. Please talk with the leader: {fShipLeaderName} to discuss joining.";
                session.Network.EnqueueSend(new GameMessageSystemChat(msg, ChatMessageType.Broadcast));
                return;
            }
            else
            {
                msg = $"Recruit request message sent to {requestedPlayer.Name}.";
                session.Network.EnqueueSend(new GameMessageSystemChat(msg, ChatMessageType.Broadcast));
                requestedPlayer.FellowshipRecruit(session.Player);
                return;
            }
        }

        [CommandHandler("myshare", AccessLevel.Player, CommandHandlerFlag.None, "Calculates your fellowship share portion from your current location in relation to your fellows.")]
        public static void HandleMyShare(Session session, params string[] parameters)
        {

            if (DateTime.UtcNow - session.Player.PrevMyShare < TimeSpan.FromSeconds(10))
            {
                session.Player.SendTransientError("You have used this command too recently! Please wait at least 10 seconds between checks.");
                return;
            }

            if (session.Player.Fellowship == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"You are not in a fellowship. You are getting 100% of your rewards.", ChatMessageType.Broadcast));
                return;
            }

            session.Player.PrevMyShare = DateTime.UtcNow;
            var msg = "Please input a valid number for xp and lum arguments or leave them blank.";
            int param1 = 0;
            int param2 = 0;

            switch (parameters.Count())
            {
                case 0:
                    param1 = 1;
                    break;
                case 1:
                    try { param1 = int.Parse(parameters[0]); } catch { session.Network.EnqueueSend(new GameMessageSystemChat(msg, ChatMessageType.Broadcast)); return; }
                    break;
                case 2:
                    try
                    {
                        param1 = int.Parse(parameters[0]);
                        param2 = int.Parse(parameters[1]);
                    }
                    catch { param1 = 0; param2 = 0; break; }
                    break;
                default:
                    break;
            }

            ShowMyProjectedShare( session, param1, param2 );

        }

        public static void ShowMyProjectedShare(Session session, int xPAmount, int lumAmount)
        {
            var player = session.Player;
            List<Player> shareableMembers = player.Fellowship.GetFellowshipMembers().Values.ToList();
            List<Player> inRange = shareableMembers.Intersect(player.Fellowship.WithinRange(player, true)).ToList();
            int iRCount = inRange.Count;
            double sharePerc = player.Fellowship.GetMemberSharePercent(iRCount);

            var msg = $"======== Start Report ========\nPlayers in your fellowship within share range: {iRCount}";

            foreach (var member in shareableMembers)
            {
                double distScalar = player.Fellowship.GetDistanceScalar(player, member, XpType.Fellowship);
                float dist2D = player.Location.Distance2D(member.Location);

                msg += $"\nPlayer: {member.Name} -- range from you: {Math.Round(dist2D,1):#,###0}\n   - DistanceScalar: {Math.Round(distScalar,2):0.##0}";

                if (xPAmount <= 1)
                {
                    msg += $"\n   - EXP Shared Modifier: {Math.Round((1 * sharePerc * distScalar) * 100,3):0.##0}%";
                }
                else
                {
                    msg += $"\n   - EXP Share Modified: {Math.Round(xPAmount * sharePerc * distScalar):#,###0}";
                }

                if (lumAmount <= 1)
                {
                    msg += $"\n   - Lumin Shared Modifier: {Math.Round((1 * sharePerc * distScalar) * 100, 3):0.##0}%";
                }
                else
                {
                    msg += $"\n   - Lumin Share Modified: {Math.Round(lumAmount * sharePerc * distScalar):#,###0}";
                }
            }

            msg += $"\n======== End Report ========";

            session.Network.EnqueueSend(new GameMessageSystemChat(msg, ChatMessageType.Broadcast));
        }

        [CommandHandler("haven", AccessLevel.Player, CommandHandlerFlag.None, 0, "Recalls to the Adventurer's Haven.")]
        public static void HandleHavenRecall(Session session, params string[] parameters)
        {
            session.Player.HandleActionTeleToDefinedPlace(HavenDrop, "Adventurer's Haven");
            return;
        }

        [CommandHandler("tn", AccessLevel.Player, CommandHandlerFlag.None, 0, "Recalls to the Town Network.")]
        public static void HandleTownNetworkRecall(Session session, params string[] parameters)
        {
            session.Player.HandleActionTeleToDefinedPlace(TNDrop, "Town Network");
            return;
        }

        [CommandHandler("sub", AccessLevel.Player, CommandHandlerFlag.None, 0, "Recalls to the Abandoned Mine.")]
        [CommandHandler("hub", AccessLevel.Player, CommandHandlerFlag.None, 0, "Recalls to the Abandoned Mine.")]
        public static void HandleAbandonedMineRecall(Session session, params string[] parameters)
        {
            session.Player.HandleActionTeleToDefinedPlace(SubDrop, "Abandoned Mine");
            return;
        }

        [CommandHandler("fachub", AccessLevel.Player, CommandHandlerFlag.None, 0, "Recalls to the Facility Hub.")]
        public static void HandleFacilityHubRecall(Session session, params string[] parameters)
        {
            if (session.Player.Level < 10)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat("You must be level 10 to use this command.", ChatMessageType.Broadcast));
                return;
            }
            session.Player.HandleActionTeleToDefinedPlace(FacHubDrop, "Facility Hub");
            return;
        }

        //You need {(xpTable.CharacterLevelXPList[(Level ?? 0) + 1] - (ulong)(TotalExperience ?? 0)):#,###0} more experience to reach your next level!
        [CommandHandler("nextlevel", AccessLevel.Player, CommandHandlerFlag.None, 0, "Recalls to the Adventurer's Haven.")]
        [CommandHandler("lvl", AccessLevel.Player, CommandHandlerFlag.None, 0, "Recalls to the Adventurer's Haven.")]
        public static void HandleNextLevel(Session session, params string[] parameters)
        {

            if (DateTime.UtcNow - session.Player.PrevControlledCommandLine < TimeSpan.FromSeconds(10))
            {
                session.Player.SendTransientError("You have used this command too recently! Please wait at least 10 seconds between intensive commands.");
                return;
            }

            session.Player.PrevControlledCommandLine = DateTime.UtcNow;
            var xpTable = DatManager.PortalDat.XpTable;
            ulong totalExperience = (ulong)(session.Player.TotalExperience ?? 0);
            string msg = "";

            if (session.Player.Level != Player.GetMaxLevel())
            {

                ulong nextLevelExperience = xpTable.CharacterLevelXPList[(session.Player.Level ?? 0) + 1];
                ulong currLevelExperience = xpTable.CharacterLevelXPList[(session.Player.Level ?? 0)];
                double dividend = totalExperience - currLevelExperience;
                double divisor = nextLevelExperience - currLevelExperience;
                msg += $"You need {(nextLevelExperience - totalExperience):#,###0} more experience to reach your next level!"
                    + $"\n   -- You are {Math.Round((dividend / divisor) * 100, 3):#,##0.##0}% of the way there!";


            }
            else
            {
                msg = $"You are currently at the maximum level of {session.Player.Level}!"
                    + $"\n   -- You have {totalExperience:#,###0} total experience points gained towards your level."
                    + $"\n   -- You have {session.Player.AvailableExperience:#,###0} available experience points to spend!";

            }

                session.Network.EnqueueSend(new GameMessageSystemChat(msg, ChatMessageType.Broadcast));

        }

        [CommandHandler("lum", AccessLevel.Player, CommandHandlerFlag.None, 0, "Recalls to the Adventurer's Haven.")]
        public static void HandleLum(Session session, params string[] parameters)
        {

            if (DateTime.UtcNow - session.Player.PrevControlledCommandLine < TimeSpan.FromSeconds(10))
            {
                session.Player.SendTransientError("You have used this command too recently! Please wait at least 10 seconds between intensive commands.");
                return;
            }

            session.Player.PrevControlledCommandLine = DateTime.UtcNow;
            // Ensure player actually has lum first.
            if(session.Player.MaximumLuminance == null) { session.Network.EnqueueSend(new GameMessageSystemChat("You do not have the ability to gain luminence at this time.",ChatMessageType.Broadcast)); return; }

            double playerMaxLumin = (ulong)(session.Player.MaximumLuminance ?? 0);
            double playerAvailLumin = (ulong)(session.Player.AvailableLuminance ?? 0);
            // Avoid div by 0
            if (playerMaxLumin == 0 || playerAvailLumin == 0) { session.Network.EnqueueSend(new GameMessageSystemChat("Cannot divide by zero.", ChatMessageType.Broadcast)); return; }

            string msg = $"You currently have {playerAvailLumin:#,###0} luminance with a capacity of {playerMaxLumin:#,###0}!";
            msg += $"\n   -- You are {Math.Round((playerAvailLumin / playerMaxLumin) * 100, 3):#,##0.##0}% of the way full!";

            session.Network.EnqueueSend(new GameMessageSystemChat(msg, ChatMessageType.Broadcast));
        }

        [CommandHandler("skillcredits", AccessLevel.Player, CommandHandlerFlag.None, 0, "Recalls to the Adventurer's Haven.")]
        public static void HandleSkillCredits(Session session, params string[] parameters)
        {
            var tStamp = session.Player.PrevControlledCommandLine.Add(TimeSpan.FromMinutes(0));
            if (tStamp - DateTime.UtcNow > TimeSpan.Zero)
            {
                session.Player.SendTransientError($"You have used this command too recently! Please wait at least {(tStamp - DateTime.UtcNow):mm':'ss} longer until using the next command.");
                return;
            }

            session.Player.PrevControlledCommandLine = DateTime.UtcNow;

            HashSet<uint> oswaldSkillCredit = null;
            HashSet<uint> ralireaSkillCredit = null;
            Dictionary<uint, int> lumAugSkillCredits = null;
            using (var ctx = new ShardDbContext())
            {
                oswaldSkillCredit = ctx.CharacterPropertiesQuestRegistry.Where(i => i.QuestName.Equals("OswaldManualCompleted")).Select(i => i.CharacterId).ToHashSet();
                ralireaSkillCredit = ctx.CharacterPropertiesQuestRegistry.Where(i => i.QuestName.Equals("ArantahKill1")).Select(i => i.CharacterId).ToHashSet();
                lumAugSkillCredits = ctx.CharacterPropertiesQuestRegistry.Where(i => i.QuestName.Equals("LumAugSkillQuest")).ToDictionary(i => i.CharacterId, i => i.NumTimesCompleted);
            }

            var scHeritage = DatManager.PortalDat.CharGen.HeritageGroups[(uint)session.Player.HeritageGroup].SkillCredits;
            var scLevel = GetAdditionalCredits((int)session.Player.Level) + 2;
            var scOswald = oswaldSkillCredit.Contains(session.Player.Guid.Full)? 1 : 0;
            var scRalirea = ralireaSkillCredit.Contains(session.Player.Guid.Full)? 1 : 0;
            var scLumin = lumAugSkillCredits.TryGetValue(session.Player.Guid.Full, out var lumSkillCredits)? lumSkillCredits : 0;
            var scAscen = session.Player.Enlightenment * 2;
            var scTotalCalc = scHeritage + scLevel + scOswald + scRalirea + scLumin + scAscen;
            var scDiffCalc = scTotalCalc - session.Player.TotalSkillCredits;

            string strMsg = $"You have available {session.Player.AvailableSkillCredits} of your {session.Player.TotalSkillCredits} total skill credits."
                + "\n==== Calculating Correct Numbers ===="
                + $"\n - Heritage: {scHeritage} (from character creation)"
                + $"\n - Level: {scLevel} (earned from levels)"
                + $"\n - Chasing Oswald: {scOswald} (from quest)"
                + $"\n - Aun Ralirea: {scRalirea} (from quest)"
                + $"\n - Luminance: {scLumin} (from Nalicana)"
                + $"\n - Ascension: {scAscen} (from enlightenment (2 per))"
                + $"\n - Your total should be {scTotalCalc}, {((scDiffCalc) == 0? "you are not missing any skill credits." : $"you have {(scDiffCalc <0? $"{-scDiffCalc} more" : $"{scDiffCalc} less")} than you are supposed to." )}"
                + "\n==== Calculations Complete ====";
            session.Network.EnqueueSend(new GameMessageSystemChat(strMsg, ChatMessageType.Broadcast));
        }

        public static int GetAdditionalCredits(int level)
        {
            foreach (var kvp in AdditionalCredits.Reverse())
                if (level >= kvp.Key)
                    return kvp.Value;

            return 0;
        }

        public static SortedDictionary<int, int> AdditionalCredits = new SortedDictionary<int, int>()
        {
            { 2, 1 },
            { 3, 2 },
            { 4, 3 },
            { 5, 4 },
            { 6, 5 },
            { 7, 6 },
            { 8, 7 },
            { 9, 8 },
            { 10, 9 },
            { 12, 10 },
            { 14, 11 },
            { 16, 12 },
            { 18, 13 },
            { 20, 14 },
            { 23, 15 },
            { 26, 16 },
            { 29, 17 },
            { 32, 18 },
            { 35, 19 },
            { 40, 20 },
            { 45, 21 },
            { 50, 22 },
            { 55, 23 },
            { 60, 24 },
            { 65, 25 },
            { 70, 26 },
            { 75, 27 },
            { 80, 28 },
            { 85, 29 },
            { 90, 30 },
            { 95, 31 },
            { 100, 32 },
            { 105, 33 },
            { 110, 34 },
            { 115, 35 },
            { 120, 36 },
            { 125, 37 },
            { 130, 38 },
            { 140, 39 },
            { 150, 40 },
            { 160, 41 },
            { 180, 42 },
            { 200, 43 },
            { 225, 44 },
            { 250, 45 },
            { 275, 46 },
            { 300, 47 }, // Level 1k mod additional credits
            { 330, 48 },
            { 360, 49 },
            { 390, 50 },
            { 420, 51 },
            { 450, 52 },
            { 480, 53 },
            { 510, 54 },
            { 540, 55 },
            { 570, 56 },
            { 600, 57 },
            { 630, 58 },
            { 660, 59 },
            { 690, 60 },
            { 720, 61 },
            { 750, 62 },
            { 780, 63 },
            { 810, 64 },
            { 840, 65 },
            { 870, 66 },
            { 900, 67 },
            { 930, 68 },
            { 960, 69 },
            { 990, 70 }
        };

        [CommandHandler("blink", AccessLevel.Player, CommandHandlerFlag.RequiresWorld, "Command utilized by a banned program.")]
        public static void HandleBlink(Session session, params string[] parameters)
        {

            if(session.AccessLevel >= AccessLevel.Sentinel)
            {
                return;
            }

            Account thisAccount = DatabaseManager.Authentication.GetAccountByName(session.Account);
            string banText = $"Player {session.Player.Name} used the Blink Plugin to exploit functions of VTank. This is a zero-tolerance policy violation. Account ({thisAccount.AccountName}) has been autobanned for 365 days.";
            thisAccount.BannedTime = DateTime.UtcNow;
            thisAccount.BanExpireTime = DateTime.UtcNow + TimeSpan.FromDays(365);
            thisAccount.BanReason = banText;
            thisAccount.BannedByAccountId = 1; // The admin account.

            log.Info(banText);
            DatabaseManager.Authentication.UpdateAccount(thisAccount);
            CommandHandlerHelper.WriteOutputInfo(session, banText, ChatMessageType.Broadcast);
            PlayerManager.BroadcastToAuditChannel(session.Player, banText);

            session.Terminate(SessionTerminationReason.AccountBooted, new GameMessageBootAccount(banText));

        }

        // quest info (uses GDLe formatting to match plugin expectations)
        [CommandHandler("myquests", AccessLevel.Player, CommandHandlerFlag.RequiresWorld, "Shows your quest log")]
        public static void HandleQuests(Session session, params string[] parameters)
        {
            if (DateTime.UtcNow - session.Player.PrevControlledCommandLine < TimeSpan.FromSeconds(10))
            {
                session.Player.SendTransientError("You have used this command too recently! Please wait at least 10 seconds between intensive commands.");
                return;
            }

            session.Player.PrevControlledCommandLine = DateTime.UtcNow;

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

        [CommandHandler("checkquest", AccessLevel.Player, CommandHandlerFlag.RequiresWorld, "Shows your quest log entry for a sepecific quest")]
        public static void HandleCheckQuest(Session session, params string[] parameters)
        {
            var msg = "@checkquest - Check information on your quest.\n";
            msg += "Usage: @checkquest [questName] - List the quest flags for specified quest name.\n";
            msg += "  ---  @checkquest fellow [questName] - List the quest flags for your Fellowship. You must be in a fellowship to use this.";

            if(parameters.Length == 0)
            {
                session.Player.SendMessage(msg);
                return;
            }

            CharacterPropertiesQuestRegistry quest;

            if (parameters[0].Equals("fellow"))
            {
                if (session.Player.Fellowship == null || parameters.Length < 2)
                {
                    session.Player.SendMessage(msg);
                    return;
                }

                quest = session.Player.Fellowship.QuestManager.GetQuest(parameters[1]);

                if (quest == null)
                {
                    session.Player.SendMessage("No quests found.");
                    return;
                }
            }
            else
            {
                quest = session.Player.QuestManager.GetQuest(parameters[1]);

                if (quest == null)
                {
                    session.Player.SendMessage("No quests found.");
                    return;
                }
            }

            msg = $"Quest Name: {quest.QuestName}\nCompletions: {quest.NumTimesCompleted} | Last Completion: {quest.LastTimeCompleted} ({Common.Time.GetDateTimeFromTimestamp(quest.LastTimeCompleted).ToLocalTime()})\n";
            var nextSolve = session.Player.Fellowship.QuestManager.GetNextSolveTime(quest.QuestName);

            if (nextSolve == TimeSpan.MinValue)
                msg += "Can Solve: Immediately\n";
            else if (nextSolve == TimeSpan.MaxValue)
                msg += "Can Solve: Never again\n";
            else
                msg += $"Can Solve: In {nextSolve:%d} days, {nextSolve:%h} hours, {nextSolve:%m} minutes and, {nextSolve:%s} seconds. ({(DateTime.UtcNow + nextSolve).ToLocalTime()})\n";

            session.Player.SendMessage(msg);

            return;
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

            if (DateTime.UtcNow - session.Player.PrevObjSend < TimeSpan.FromMinutes(2))// Psyber Edit: Normally 5 min, changed to 2.
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
    }
}
