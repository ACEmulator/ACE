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

        /// <summary>
        /// This is almost always a Player
        /// 
        /// however there are some rare cases of Creatures having quests
        /// such as 'chickencrossingroad'
        /// </summary>
        public Creature Creature { get; }

        public Fellowship Fellowship { get; }

        private ICollection<CharacterPropertiesQuestRegistry> runtimeQuests { get; set; } = new HashSet<CharacterPropertiesQuestRegistry>();

        public string Name
        {
            get
            {
                if (Creature != null)
                    return Creature.Name;
                else
                    return $"Fellowship({Fellowship.FellowshipName})";
            }

        }
        public uint IDtoUseForQuestRegistry
        {
            get
            {
                if (Creature != null)
                    return Creature.Guid.Full;
                else
                    return 1; 
                    //return Fellowship.FellowshipLeaderGuid;
            }
        }

        public static bool Debug = false;

        /// <summary>
        /// Constructs a new QuestManager for a Player / Creature
        /// </summary>
        public QuestManager(Creature creature)
        {
            Creature = creature;
        }

        /// <summary>
        /// Constructs a new QuestManager for a Fellowship
        /// </summary>
        public QuestManager(Fellowship fellowship)
        {
            Fellowship = fellowship;
        }

        /// <summary>
        /// This will return a clone of the quests collection. You should not mutate the results.
        /// This is mostly used for information/debugging
        /// </summary>
        /// <returns></returns>
        public ICollection<CharacterPropertiesQuestRegistry> GetQuests()
        {
            if (Creature is Player player)
                return player.Character.GetQuests(player.CharacterDatabaseLock);

            // Not a player
            return runtimeQuests;
        }

        /// <summary>
        /// Returns TRUE if a player has started a particular quest
        /// </summary>
        public bool HasQuest(string questFormat)
        {
            var questName = GetQuestName(questFormat);
            var hasQuest = GetQuest(questName) != null;

            if (Debug)
                Console.WriteLine($"{Name}.QuestManager.HasQuest({questFormat}): {hasQuest}");

            return hasQuest;
        }

        public bool HasQuestCompletes(string questName)
        {
            if (Debug) Console.WriteLine($"{Name}.QuestManager.HasQuestCompletes({questName})");

            if (!questName.Contains("@"))
                return HasQuest(questName);

            var pieces = questName.Split('@');
            if (pieces.Length != 2)
            {
                Console.WriteLine($"{Name}.QuestManager.HasQuestCompletes({questName}): error parsing quest name");
                return false;
            }
            var name = pieces[0];
            if (!Int32.TryParse(pieces[1], out var numCompletes))
            {
                Console.WriteLine($"{Name}.QuestManager.HasQuestCompletes({questName}): unknown quest format");
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
            if (Creature is Player player)
                return player.Character.GetQuest(questName, player.CharacterDatabaseLock);

            // Not a player
            return runtimeQuests.FirstOrDefault(q => q.QuestName.Equals(questName, StringComparison.OrdinalIgnoreCase));
        }

        private CharacterPropertiesQuestRegistry GetOrCreateQuest(string questName, out bool questRegistryWasCreated)
        {
            if (Creature is Player player)
                return player.Character.GetOrCreateQuest(questName, player.CharacterDatabaseLock, out questRegistryWasCreated);

            // Not a player
            var existing = runtimeQuests.FirstOrDefault(q => q.QuestName.Equals(questName, StringComparison.OrdinalIgnoreCase));

            if (existing == null)
            {
                existing = new CharacterPropertiesQuestRegistry
                {
                    QuestName = questName,
                };

                runtimeQuests.Add(existing);

                questRegistryWasCreated = true;
            }
            else
                questRegistryWasCreated = false;

            return existing;
        }

        /// <summary>
        /// Adds or updates a quest completion to the player's registry
        /// </summary>
        public void Update(string questFormat)
        {
            var questName = GetQuestName(questFormat);

            var quest = GetOrCreateQuest(questName, out var questRegistryWasCreated);

            if (questRegistryWasCreated)
            {
                quest.LastTimeCompleted = (uint) Time.GetUnixTime();
                quest.NumTimesCompleted = 1; // initial add / first solve

                quest.CharacterId = IDtoUseForQuestRegistry;

                if (Debug) Console.WriteLine($"{Name}.QuestManager.Update({quest}): added quest");

                if (Creature is Player player)
                {
                    player.CharacterChangesDetected = true;

                    player.ContractManager.NotifyOfQuestUpdate(quest.QuestName);
                }
            }
            else
            {
                if (IsMaxSolves(questName))
                {
                    if (Debug) Console.WriteLine($"{Name}.QuestManager.Update({quest}): can not update existing quest. IsMaxSolves({questName}) is true.");
                    return;
                }

                // update existing quest
                quest.LastTimeCompleted = (uint)Time.GetUnixTime();
                quest.NumTimesCompleted++;

                if (Debug) Console.WriteLine($"{Name}.QuestManager.Update({quest}): updated quest ({quest.NumTimesCompleted})");

                if (Creature is Player player)
                {
                    player.CharacterChangesDetected = true;

                    player.ContractManager.NotifyOfQuestUpdate(quest.QuestName);
                }
            }
        }

        /// <summary>
        /// Initialize a quest completion with the provided number to the player's registry
        /// </summary>
        public void SetQuestCompletions(string questFormat, int questCompletions = 0)
        {
            var questName = GetQuestName(questFormat);

            var maxSolves = GetMaxSolves(questName);

            var numTimesCompleted = maxSolves > -1 ? Math.Min(questCompletions, maxSolves) : Math.Abs(questCompletions);

            var quest = GetOrCreateQuest(questName, out var questRegistryWasCreated);

            if (questRegistryWasCreated)
            {
                quest.LastTimeCompleted = (uint) Time.GetUnixTime();
                quest.NumTimesCompleted = numTimesCompleted; // initialize the quest to the given completions

                quest.CharacterId = IDtoUseForQuestRegistry;

                if (Debug) Console.WriteLine($"{Name}.QuestManager.SetQuestCompletions({questFormat}): initialized quest to {quest.NumTimesCompleted}");

                if (Creature is Player player)
                {
                    player.CharacterChangesDetected = true;

                    player.ContractManager.NotifyOfQuestUpdate(quest.QuestName);
                }
            }
            else
            {
                // update existing quest
                quest.LastTimeCompleted = (uint)Time.GetUnixTime();
                quest.NumTimesCompleted = numTimesCompleted;

                if (Debug) Console.WriteLine($"{Name}.QuestManager.SetQuestCompletions({questFormat}): initialized quest to {quest.NumTimesCompleted}");

                if (Creature is Player player)
                {
                    player.CharacterChangesDetected = true;

                    player.ContractManager.NotifyOfQuestUpdate(quest.QuestName);
                }
            }
        }

        /// <summary>
        /// Returns TRUE if player can solve this quest now
        /// </summary>
        public bool CanSolve(string questFormat)
        {
            var questName = GetQuestName(questFormat);

            // verify max solves / quest timer
            var nextSolveTime = GetNextSolveTime(questName);

            var canSolve = nextSolveTime == TimeSpan.MinValue;
            if (Debug) Console.WriteLine($"{Name}.QuestManager.CanSolve({questName}): {canSolve}");
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
        /// Returns the maximum # of solves for this quest
        /// </summary>
        public int GetMaxSolves(string questFormat)
        {
            var questName = GetQuestName(questFormat);

            var quest = DatabaseManager.World.GetCachedQuest(questName);
            if (quest == null) return 0;

            return quest.MaxSolves;
        }

        /// <summary>
        /// Returns the current # of solves for this quest
        /// </summary>
        public int GetCurrentSolves(string questFormat)
        {
            var questName = GetQuestName(questFormat);

            var quest = GetQuest(questName);
            if (quest == null) return 0;

            return quest.NumTimesCompleted;
        }

        /// <summary>
        /// Some quests we do not want to scale MinDelta if "quest_mindelta_rate" has been set.
        /// They may be things that are races against time, like Colo
        /// </summary>
        public static bool CanScaleQuestMinDelta(Database.Models.World.Quest quest)
        {
            if (quest.Name.StartsWith("ColoArena"))
                return false;

            return true;
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
            uint nextSolveTime;

            if (CanScaleQuestMinDelta(quest))
                nextSolveTime = playerQuest.LastTimeCompleted + (uint)(quest.MinDelta * PropertyManager.GetDouble("quest_mindelta_rate", 1).Item);
            else
                nextSolveTime = playerQuest.LastTimeCompleted + quest.MinDelta;

            if (currentTime >= nextSolveTime)
                return TimeSpan.MinValue;   // can solve again now - next solve time expired

            // return the time remaining on the player's quest timer
            return TimeSpan.FromSeconds(nextSolveTime - currentTime);
        }

        /// <summary>
        /// Increment the number of times completed for a quest
        /// </summary>
        public void Increment(string questName, int amount = 1)
        {
            for (var i = 0; i < amount; i++)
                Update(questName);
        }

        /// <summary>
        /// Decrement the number of times completed for a quest
        /// </summary>
        public void Decrement(string quest, int amount = 1)
        {
            var questName = GetQuestName(quest);

            var existing = GetQuest(questName);

            if (existing != null)
            {
                //if (existing.NumTimesCompleted == 0)
                //{
                //    if (Debug) Console.WriteLine($"{Name}.QuestManager.Decrement({quest}): can not Decrement existing quest. {questName}.NumTimesCompleted is already 0.");
                //    return;
                //}

                // update existing quest
                existing.LastTimeCompleted = (uint)Time.GetUnixTime();
                existing.NumTimesCompleted -= amount;

                if (Debug) Console.WriteLine($"{Name}.QuestManager.Decrement({quest}): updated quest ({existing.NumTimesCompleted})");

                if (Creature is Player player)
                {
                    player.CharacterChangesDetected = true;
                    player.ContractManager.NotifyOfQuestUpdate(existing.QuestName);
                }
            }
        }

        /// <summary>
        /// Removes an existing quest from the Player's registry
        /// </summary>
        public void Erase(string questFormat)
        {
            if (Debug)
                Console.WriteLine($"{Name}.QuestManager.Erase({questFormat})");

            var questName = GetQuestName(questFormat);

            if (Creature is Player player)
            {
                if (player.Character.EraseQuest(questName, player.CharacterDatabaseLock))
                {
                    player.CharacterChangesDetected = true;

                    player.ContractManager.NotifyOfQuestUpdate(questName);
                }
            }
            else
            {
                // Not a player
                var quests = runtimeQuests.Where(q => q.QuestName.Equals(questName, StringComparison.OrdinalIgnoreCase)).ToList();
                foreach (var quest in quests)
                    runtimeQuests.Remove(quest);
            }
        }

        /// <summary>
        /// Removes an all quests from registry
        /// </summary>
        public void EraseAll()
        {
            if (Debug)
                Console.WriteLine($"{Name}.QuestManager.EraseAll");

            if (Creature is Player player)
            {
                player.Character.EraseAllQuests(out var questNamesErased, player.CharacterDatabaseLock);

                if (questNamesErased.Count > 0)
                {
                    player.CharacterChangesDetected = true;

                    foreach (var questName in questNamesErased)
                        player.ContractManager.NotifyOfQuestUpdate(questName);
                }
            }
            else
            {
                // Not a player
                runtimeQuests.Clear();
            }
        }

        /// <summary>
        /// Shows the current quests in progress for a Player
        /// </summary>
        public void ShowQuests(Player player)
        {
            Console.WriteLine("ShowQuests");

            var quests = GetQuests();

            if (quests.Count == 0)
            {
                Console.WriteLine("No quests in progress for " + Name);
                return;
            }

            foreach (var quest in quests)
            {
                Console.WriteLine("Quest Name: " + quest.QuestName);
                Console.WriteLine("Times Completed: " + quest.NumTimesCompleted);
                Console.WriteLine("Last Time Completed: " + quest.LastTimeCompleted);
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

            int min = _min ?? int.MinValue;    // use defaults?
            int max = _max ?? int.MaxValue;

            var hasQuestSolves = numSolves >= min && numSolves <= max;    // verify: can either of these be -1?
            if (Debug)
                Console.WriteLine($"{Name}.QuestManager.HasQuestSolves({questFormat}, {_min}, {_max}): {hasQuestSolves}");

            return hasQuestSolves;
        }

        /// <summary>
        /// Called when a player hasn't started a quest yet
        /// </summary>
        public void HandleNoQuestError(WorldObject wo)
        {
            var player = Creature as Player;

            if (player == null) return;

            var error = new GameEventInventoryServerSaveFailed(player.Session, wo.Guid.Full, WeenieError.ItemRequiresQuestToBePickedUp);
            player.Session.Network.EnqueueSend(error);
        }

        public void HandlePortalQuestError(string questName)
        {
            var player = Creature as Player;

            if (player == null) return;

            if (!HasQuest(questName))
            {
                player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.YouMustCompleteQuestToUsePortal));
            }
            else if (CanSolve(questName))
            {
                var error = new GameEventWeenieError(player.Session, WeenieError.QuestSolvedTooLongAgo);
                var text = new GameMessageSystemChat("You completed the quest this portal requires too long ago!", ChatMessageType.Magic); // This msg wasn't sent in retail PCAP, leading to a completely silent fail when using the portal with an expired flag.
                player.Session.Network.EnqueueSend(text, error);
            }
        }

        /// <summary>
        /// Called when either the player has completed the quest too recently, or max solves has been reached.
        /// </summary>
        public void HandleSolveError(string questName)
        {
            var player = Creature as Player;
            if (player == null) return;

            if (IsMaxSolves(questName))
            {
                var error = new GameEventInventoryServerSaveFailed(player.Session, 0, WeenieError.YouHaveSolvedThisQuestTooManyTimes);
                var text = new GameMessageSystemChat("You have solved this quest too many times!", ChatMessageType.Broadcast);
                player.Session.Network.EnqueueSend(text, error);
            }
            else
            {
                var error = new GameEventInventoryServerSaveFailed(player.Session, 0, WeenieError.YouHaveSolvedThisQuestTooRecently);
                var text = new GameMessageSystemChat("You have solved this quest too recently!", ChatMessageType.Broadcast);

                var remainStr = GetNextSolveTime(questName).GetFriendlyString();
                var remain = new GameMessageSystemChat($"You may complete this quest again in {remainStr}.", ChatMessageType.Broadcast);
                player.Session.Network.EnqueueSend(text, remain, error);
            }
        }

        /// <summary>
        /// Increments the counter for a kill task for a player
        /// </summary>
        public void HandleKillTask(string killQuestName, WorldObject killedCreature)
        {
            var player = Creature as Player;
            if (player == null) return;

            // http://acpedia.org/wiki/Announcements_-_2012/12_-_A_Growing_Twilight#Release_Notes

            if (killedCreature == null)
            {
                log.Error($"{Name}.QuestManager.HandleKillTask({killQuestName}): input object is null!");
                return;
            }

            var questName = GetQuestName(killQuestName);
            var quest = DatabaseManager.World.GetCachedQuest(questName);

            if (quest == null)
            {
                log.Error($"{Name}.QuestManager.HandleKillTask({killQuestName}): couldn't find kill task {questName} in database");
                return;
            }

            if (!HasQuest(questName))
                return;

            Stamp(killQuestName);

            var playerQuest = GetQuest(questName);

            if (playerQuest == null)
            {
                // this should be impossible
                log.Error($"{Name}.QuestManager.HandleKillTask({killQuestName}): couldn't find kill task {questName} in player quests");
                return;
            }

            var msg = $"You have killed {playerQuest.NumTimesCompleted} {killedCreature.GetPluralName()}!";

            if (IsMaxSolves(questName))
                msg += $" Your task is complete!";
            else
                msg += $" You must kill {quest.MaxSolves} to complete your task.";

            player.Session.Network.EnqueueSend(new GameMessageSystemChat(msg, ChatMessageType.Broadcast));
        }

        /// <summary>
        /// Called when a player kills Creature
        /// </summary>
        public void OnDeath(WorldObject killer)
        {
        }

        public bool HasQuestBits(string questFormat, int bits)
        {
            var questName = GetQuestName(questFormat);

            var quest = GetQuest(questName);
            if (quest == null) return false;

            var hasQuestBits = (quest.NumTimesCompleted & bits) == bits;

            if (Debug)
                Console.WriteLine($"{Name}.QuestManager.HasQuestBits({questFormat}, 0x{bits:X}): {hasQuestBits}");

            return hasQuestBits;
        }

        public bool HasNoQuestBits(string questFormat, int bits)
        {
            var questName = GetQuestName(questFormat);

            var quest = GetQuest(questName);
            if (quest == null) return true;

            var hasNoQuestBits = (quest.NumTimesCompleted & bits) == 0;

            if (Debug)
                Console.WriteLine($"{Name}.QuestManager.HasNoQuestBits({questFormat}, 0x{bits:X}): {hasNoQuestBits}");

            return hasNoQuestBits;
        }

        public void SetQuestBits(string questFormat, int bits, bool on = true)
        {
            var questName = GetQuestName(questFormat);

            var quest = GetQuest(questName);

            var questBits = 0;

            if (quest != null) questBits = quest.NumTimesCompleted;

            if (on)
                questBits |= bits;
            else
                questBits &= ~bits;

            if (Debug)
                Console.WriteLine($"{Name}.QuestManager.SetQuestBits({questFormat}, 0x{bits:X}): {on}");

            SetQuestCompletions(questFormat, questBits);
        }
    }
}
