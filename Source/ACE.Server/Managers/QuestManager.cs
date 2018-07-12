using System;
using System.Collections.Generic;
using System.Linq;
using ACE.Database.Models.Shard;
using ACE.Server.WorldObjects;

namespace ACE.Server.Managers
{
    public class QuestManager
    {
        public Player Player { get; }
        public ICollection<CharacterPropertiesQuestRegistry> Quests { get => Player.Biota.CharacterPropertiesQuestRegistry; }

        /// <summary>
        /// Constructs a new QuestManager for a Player
        /// </summary>
        public QuestManager(Player player)
        {
            Player = player;
        }

        /// <summary>
        /// Add a new quest to this Player's registry
        /// </summary>
        public void Add(string questName)
        {
            var inProgress = Quests.FirstOrDefault(q => q.QuestName == questName);
            if (inProgress != null)
            {
                //Console.WriteLine("QuestManager.Add: quest already in progress!");
                inProgress.NumTimesCompleted++;  // ??
                return;
            }
            var quest = new CharacterPropertiesQuestRegistry
            {
                QuestName = questName,
                ObjectId = Player.Guid.Full,
                LastTimeCompleted = 0,  // TODO: get accurate server time?
                NumTimesCompleted = 1   // ??
            };
            //Console.WriteLine("QuestManager.Add: Adding quest to DB");
            Quests.Add(quest);
        }

        /// <summary>
        /// Update an existing quest in this Player's registry
        /// </summary>
        public void Update(string questName)
        {
            var inProgress = Quests.FirstOrDefault(q => q.QuestName == questName);
            if (inProgress == null)
            {
                //Console.WriteLine("QuestManager.Update: quest doesn't exist!");
                return;
            }
            inProgress.NumTimesCompleted++;     // only called for completion?
        }

        /// <summary>
        /// Increment the number of times completed for a quest
        /// </summary>
        public void Increment(string questName)
        {
            var quest = Quests.FirstOrDefault(q => q.QuestName == questName);
            if (quest == null)
            {
                //Console.WriteLine("QuestManager.Increment: You do not have this quest.");
                return;
            }
            quest.NumTimesCompleted++;
        }

        /// <summary>
        /// Removes an exisitng quest from the Player's registry
        /// </summary>
        public void Erase(string questName)
        {
            //Console.WriteLine("QuestManager.Erase: " + questName);

            var quests = Quests.Where(q => q.QuestName.Equals(questName)).ToList();
            foreach (var quest in quests)
                Quests.Remove(quest);
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
                Console.WriteLine("Player ID: " + quest.ObjectId.ToString("X8"));
                Console.WriteLine("----");
            }
        }

        /// <summary>
        /// Searches for a particular quest in progress for a player
        /// </summary>
        public bool HasQuest(String questName)
        {
            var quests = Quests.Where(q => q.QuestName.Equals(questName)).ToList();

            var hasQuest = quests.Count > 0;
            //Console.WriteLine($"QuestManager.HasQuest - checking if {Player.Name} has quest {questName} ({hasQuest})");
            return hasQuest;
        }
    }
}
