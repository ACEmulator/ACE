using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using ACE.Common;
using ACE.Common.Extensions;
using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects;
using ACE.Server.Entity;

namespace ACE.Server.Managers
{
    public class QuestManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Player Player { get; }

        public Fellowship Fellowship { get; }
        private ICollection<CharacterPropertiesQuestRegistry> fellowQuests { get; set; } = new HashSet<CharacterPropertiesQuestRegistry>();

        public ICollection<CharacterPropertiesQuestRegistry> Quests
        {
            get
            {
                if (Player != null)
                    return Player.Character.CharacterPropertiesQuestRegistry;
                else
                    return fellowQuests;
            }
        }

        public static bool Debug = false;

        /// <summary>
        /// Constructs a new QuestManager for a Player
        /// </summary>
        public QuestManager(Player player)
        {
            Player = player;
        }

        /// <summary>
        /// Constructs a new QuestManager for a Fellowship
        /// </summary>
        public QuestManager(Fellowship fellowship)
        {
            Fellowship = fellowship;
        }

        /// <summary>
        /// Returns TRUE if a player has started a particular quest
        /// </summary>
        public bool HasQuest(string questFormat)
        {
            var questName = GetQuestName(questFormat);
            var hasQuest = GetQuest(questName) != null;

            if (Debug)
                Console.WriteLine($"{((Player != null) ? Player.Name : $"Fellowship({Fellowship.FellowshipName})")}.HasQuest({questFormat}): {hasQuest}");

            return hasQuest;
        }

        public bool HasQuestCompletes(string questName)
        {
            if (Debug) Console.WriteLine($"{((Player != null) ? Player.Name : $"Fellowship({Fellowship.FellowshipName})")}.HasQuestCompletes({questName})");

            if (!questName.Contains("@"))
                return HasQuest(questName);

            var pieces = questName.Split('@');
            if (pieces.Length != 2)
            {
                Console.WriteLine($"{((Player != null) ? Player.Name : $"Fellowship({Fellowship.FellowshipName})")}.QuestManager.HasQuestCompletes({questName}): error parsing quest name");
                return false;
            }
            var name = pieces[0];
            if (!Int32.TryParse(pieces[1], out var numCompletes))
            {
                Console.WriteLine($"{((Player != null) ? Player.Name : $"Fellowship({Fellowship.FellowshipName})")}.QuestManager.HasQuestCompletes({questName}): unknown quest format");
                return HasQuest(questName);
            }
            var quest = GetQuest(name);
            if (quest == null)
                return false;

            var success = quest.NumTimesCompleted == numCompletes;     // minimum or exact?
            if (Debug) Console.WriteLine(success);
            return success;
        }

        /// <summary>
        /// Returns an active or completed quest for this player
        /// </summary>
        public CharacterPropertiesQuestRegistry GetQuest(string questName)
        {
            return Quests.FirstOrDefault(q => q.QuestName.Equals(questName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Adds or updates a quest completion to the player's registry
        /// </summary>
        public void Update(string quest)
        {
            var questName = GetQuestName(quest);

            var existing = Quests.FirstOrDefault(q => q.QuestName.Equals(questName, StringComparison.OrdinalIgnoreCase));

            if (existing == null)
            {
                // add new quest entry
                var info = new CharacterPropertiesQuestRegistry
                {
                    QuestName = questName,
                    //CharacterId = Player.Guid.Full,
                    LastTimeCompleted = (uint)Time.GetUnixTime(),
                    NumTimesCompleted = 1   // initial add / first solve
                };
                if (Player != null)
                    info.CharacterId = Player.Guid.Full;
                else
                    info.CharacterId = Fellowship.FellowshipLeaderGuid;
                if (Debug) Console.WriteLine($"{((Player != null) ? Player.Name : $"Fellowship({Fellowship.FellowshipName})")}.QuestManager.Update({quest}): added quest");
                Quests.Add(info);
                if (Player != null)
                {
                    Player.CharacterChangesDetected = true;
                    Player.ContractManager.NotifyOfQuestUpdate(info.QuestName);
                }
            }
            else
            {
                if (IsMaxSolves(questName))
                {
                    if (Debug) Console.WriteLine($"{((Player != null) ? Player.Name : $"Fellowship({Fellowship.FellowshipName})")}.QuestManager.Update({quest}): can not update existing quest. IsMaxSolves({questName}) is true.");
                    return;
                }

                // update existing quest
                existing.LastTimeCompleted = (uint)Time.GetUnixTime();
                existing.NumTimesCompleted++;
                if (Debug) Console.WriteLine($"{((Player != null) ? Player.Name : $"Fellowship({Fellowship.FellowshipName})")}.QuestManager.Update({quest}): updated quest ({existing.NumTimesCompleted})");
                if (Player != null)
                {
                    Player.CharacterChangesDetected = true;
                    Player.ContractManager.NotifyOfQuestUpdate(existing.QuestName);
                }
            }
        }

        /// <summary>
        /// Initialize a quest completion with the provided number to the player's registry
        /// </summary>
        public void SetQuestCompletions(string questFormat, int questCompletions = 0)
        {
            var questName = GetQuestName(questFormat);

            var existing = Quests.FirstOrDefault(q => q.QuestName.Equals(questName, StringComparison.OrdinalIgnoreCase));

            if (existing == null)
            {
                // add new quest entry
                var info = new CharacterPropertiesQuestRegistry
                {
                    QuestName = questName,
                    //CharacterId = Player.Guid.Full,
                    LastTimeCompleted = (uint)Time.GetUnixTime(),
                    NumTimesCompleted = questCompletions   // initialize the quest to the given completions
                };
                if (Player != null)
                    info.CharacterId = Player.Guid.Full;
                else
                    info.CharacterId = Fellowship.FellowshipLeaderGuid;
                if (Debug) Console.WriteLine($"{((Player != null) ? Player.Name : $"Fellowship({Fellowship.FellowshipName})")}.QuestManager.Update({questFormat}): initialized quest to {info.NumTimesCompleted}");
                Quests.Add(info);
                if (Player != null)
                {
                    Player.CharacterChangesDetected = true;
                    Player.ContractManager.NotifyOfQuestUpdate(info.QuestName);
                }
            }
            else
            {
                // update existing quest
                existing.LastTimeCompleted = (uint)Time.GetUnixTime();
                existing.NumTimesCompleted = questCompletions;
                if (Debug) Console.WriteLine($"{((Player != null) ? Player.Name : $"Fellowship({Fellowship.FellowshipName})")}.QuestManager.Update({questFormat}): initialized quest to {existing.NumTimesCompleted}");
                if (Player != null)
                {
                    Player.CharacterChangesDetected = true;
                    Player.ContractManager.NotifyOfQuestUpdate(existing.QuestName);
                }
            }
        }

        /// <summary>
        /// Returns TRUE if player can solve this quest now
        /// </summary>
        public bool CanSolve(string questFormat)
        {
            var questName = GetQuestName(questFormat);

            // Always return false for Kill Task count quests,
            // as their emote logic is backwards from standard quests, and
            // they should always be solvable, once flagged with main kill task quest
            if (questName.EndsWith("count", StringComparison.Ordinal) || questName.EndsWith("counter", StringComparison.Ordinal))
                return false;

            // verify max solves / quest timer
            var nextSolveTime = GetNextSolveTime(questName);

            var canSolve = nextSolveTime == TimeSpan.MinValue;
            if (Debug) Console.WriteLine($"{((Player != null) ? Player.Name : $"Fellowship({Fellowship.FellowshipName})")}.CanSolve({questName}): {canSolve}");
            return canSolve;
        }

        /// <summary>
        /// Returns TRUE if player has reached the maximum # of solves for this quest
        /// </summary>
        public bool IsMaxSolves(string questName)
        {
            var quest = DatabaseManager.World.GetCachedQuest(questName);
            if (quest == null) return false;

            var playerQuest = GetQuest(questName);
            if (playerQuest == null) return false;  // player hasn't completed this quest yet

            // return TRUE if quest has solve limit, and it has been reached
            return quest.MaxSolves > -1 && playerQuest.NumTimesCompleted >= quest.MaxSolves;
        }

        /// <summary>
        /// Returns the time remaining until the player can solve this quest again
        /// </summary>
        public TimeSpan GetNextSolveTime(string questFormat)
        {
            var questName = GetQuestName(questFormat);

            var quest = DatabaseManager.World.GetCachedQuest(questName);
            if (quest == null)
                return TimeSpan.MaxValue;   // world quest not found - cannot solve it

            var playerQuest = GetQuest(questName);
            if (playerQuest == null)
                return TimeSpan.MinValue;   // player hasn't completed this quest yet - can solve immediately

            if (quest.MaxSolves > -1 && playerQuest.NumTimesCompleted >= quest.MaxSolves)
                return TimeSpan.MaxValue;   // cannot solve this quest again - max solves reached / exceeded

            var currentTime = (uint)Time.GetUnixTime();
            var nextSolveTime = playerQuest.LastTimeCompleted + quest.MinDelta;

            if (currentTime >= nextSolveTime)
                return TimeSpan.MinValue;   // can solve again now - next solve time expired

            // return the time remaining on the player's quest timer
            return TimeSpan.FromSeconds(nextSolveTime - currentTime);
        }

        /// <summary>
        /// Increment the number of times completed for a quest
        /// </summary>
        public void Increment(string questName)
        {
            // kill task / append # to quest name?
            Update(questName);
        }

        /// <summary>
        /// Removes an existing quest from the Player's registry
        /// </summary>
        public void Erase(string questFormat)
        {
            if (Debug)
                Console.WriteLine($"{Player.Name}.QuestManager.Erase({questFormat})");

            var questName = GetQuestName(questFormat);

            var quests = Quests.Where(q => q.QuestName.Equals(questName, StringComparison.OrdinalIgnoreCase)).ToList();
            foreach (var quest in quests)
            {
                Quests.Remove(quest);
                if (Player != null)
                {
                    Player.CharacterChangesDetected = true;
                    Player.ContractManager.NotifyOfQuestUpdate(quest.QuestName);
                }
            }
        }

        /// <summary>
        /// Removes an all quests from registry
        /// </summary>
        public void EraseAll()
        {
            if (Debug)
                Console.WriteLine($"{Player.Name}.QuestManager.EraseAll");

            var quests = Quests.ToList();
            foreach (var quest in quests)
            {
                Quests.Remove(quest);
                if (Player != null)
                {
                    Player.CharacterChangesDetected = true;
                    Player.ContractManager.NotifyOfQuestUpdate(quest.QuestName);
                }
            }
        }

        /// <summary>
        /// Shows the current quests in progress for a Player
        /// </summary>
        public void ShowQuests(Player player)
        {
            Console.WriteLine("ShowQuests");

            if (Quests.Count == 0)
            {
                Console.WriteLine("No quests in progress for " + player.Name);
                return;
            }
            foreach (var quest in Quests)
            {
                Console.WriteLine("Quest Name: " + quest.QuestName);
                Console.WriteLine("Times Completed: " + quest.NumTimesCompleted);
                Console.WriteLine("Last Time Completed: " + quest.LastTimeCompleted);
                Console.WriteLine("Quest ID: " + quest.Id.ToString("X8"));
                Console.WriteLine("Player ID: " + quest.CharacterId.ToString("X8"));
                Console.WriteLine("----");
            }
        }

        public void Stamp(string questFormat)
        {
            var questName = GetQuestName(questFormat);
            Update(questName);  // ??
        }

        /// <summary>
        /// Returns the quest name without the @ comment
        /// </summary>
        /// <param name="questFormat">A quest name with an optional @comment on the end</param>
        public static string GetQuestName(string questFormat)
        {
            var idx = questFormat.IndexOf('@');     // strip comment
            if (idx == -1)
                return questFormat;

            var questName = questFormat.Substring(0, idx);
            return questName;
        }

        /// <summary>
        /// Returns TRUE if player has solved this quest between min-max times
        /// </summary>
        public bool HasQuestSolves(string questFormat, int? _min, int? _max)
        {
            var questName = GetQuestName(questFormat);    // strip optional @comment

            var quest = GetQuest(questName);
            var numSolves = quest != null ? quest.NumTimesCompleted : 0;

            int min = _min ?? 0;    // use defaults?
            int max = _max ?? 0;

            var hasQuestSolves = numSolves >= min && numSolves <= max;    // verify: can either of these be -1?
            if (Debug)
                Console.WriteLine($"{((Player != null) ? Player.Name : $"Fellowship({Fellowship.FellowshipName})")}.HasQuestSolves({questFormat}, {_min}, {_max}): {hasQuestSolves}");

            return hasQuestSolves;
        }

        /// <summary>
        /// Called when a player hasn't started a quest yet
        /// </summary>
        public void HandleNoQuestError(WorldObject wo)
        {
            if (wo is Portal)
            {
                Player.Session.Network.EnqueueSend(new GameEventWeenieError(Player.Session, WeenieError.YouMustCompleteQuestToUsePortal));
            }
            else
            {
                var error = new GameEventInventoryServerSaveFailed(Player.Session, wo.Guid.Full, WeenieError.ItemRequiresQuestToBePickedUp);
                Player.Session.Network.EnqueueSend(error);
            }
        }

        /// <summary>
        /// Called when either the player has completed the quest too recently, or max solves has been reached.
        /// </summary>
        public void HandleSolveError(string questName)
        {
            if (IsMaxSolves(questName))
            {
                var error = new GameEventInventoryServerSaveFailed(Player.Session, 0, WeenieError.YouHaveSolvedThisQuestTooManyTimes);
                var text = new GameMessageSystemChat("You have solved this quest too many times!", ChatMessageType.Broadcast);
                Player.Session.Network.EnqueueSend(text, error);
            }
            else
            {
                var error = new GameEventInventoryServerSaveFailed(Player.Session, 0, WeenieError.YouHaveSolvedThisQuestTooRecently);
                var text = new GameMessageSystemChat("You have solved this quest too recently!", ChatMessageType.Broadcast);

                var remainStr = GetNextSolveTime(questName).GetFriendlyString();
                var remain = new GameMessageSystemChat($"You may complete this quest again in {remainStr}.", ChatMessageType.Broadcast);
                Player.Session.Network.EnqueueSend(text, remain, error);
            }
        }

        /// <summary>
        /// Increments the counter for a kill task, and optionally shares with fellowship
        /// </summary>
        public void HandleKillTask(string _questName, WorldObject obj, float shareableRange)
        {
            // http://acpedia.org/wiki/Announcements_-_2012/12_-_A_Growing_Twilight#Release_Notes

            if (HasQuest(_questName))
            {
                Stamp(_questName);

                var questName = GetQuestName(_questName);
                var quest = DatabaseManager.World.GetCachedQuest(questName);

                if (quest == null)
                {
                    log.Error($"{((Player != null) ? Player.Name : $"Fellowship({Fellowship.FellowshipName})")}.HandleKillTask({_questName}): couldn't find kill task {questName} in database");
                    return;
                }

                var playerQuest = Quests.FirstOrDefault(q => q.QuestName.Equals(questName, StringComparison.OrdinalIgnoreCase));

                if (playerQuest == null)
                {
                    log.Error($"{((Player != null) ? Player.Name : $"Fellowship({Fellowship.FellowshipName})")}.HandleKillTask({_questName}): couldn't find kill task {questName} in player quests");
                    return;
                }

                if (obj == null)
                {
                    log.Error($"{((Player != null) ? Player.Name : $"Fellowship({Fellowship.FellowshipName})")}.HandleKillTask({_questName}): input object is null!");
                    return;
                }

                var msg = "";

                if (IsMaxSolves(questName))
                    msg = $"You have killed {quest.MaxSolves} {obj.GetPluralName()}. Your task is complete!";
                else
                    msg = $"You have killed {playerQuest.NumTimesCompleted} {obj.GetPluralName()}. You must kill {quest.MaxSolves} to complete your task!";

                Player.Session.Network.EnqueueSend(new GameMessageSystemChat(msg, ChatMessageType.Broadcast));
            }
            else if (PropertyManager.GetBool("fellow_kt_killer").Item)
            {
                // if this option is enabled (retail default), the killer is required to have kill task
                // for it to share with fellowship
                return;
            }

            // are we in a fellowship? if so, share with fellowship
            if (shareableRange > 0.0f && Player.Fellowship != null)
            {
                var landblockRange = PropertyManager.GetBool("fellow_kt_landblock").Item;

                // killtasks can be shared with all members of a fellowship,
                // they do not use the same "ShareableMembers" as XP sharing
                var fellows = Player.Fellowship.GetFellowshipMembers();

                foreach (var fellow in fellows.Values.Where(f => f != Player))
                {
                    // ensure within shareable distance
                    var shareable = landblockRange ?
                        Player.CurrentLandblock == fellow.CurrentLandblock || Player.Location.DistanceTo(fellow.Location) <= 192.0f :
                        Player.Location.Distance2D(fellow.Location) <= shareableRange && Player.ObjMaint.VisibleObjects.ContainsKey(fellow.Guid.Full);      // 2d visible distance / radar range?

                    if (shareable)
                        fellow.QuestManager.HandleKillTask(_questName, obj, 0.0f);
                }
            }
        }
    }
}
