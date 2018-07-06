using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.DatLoader;
using ACE.Server.Entity.Actions;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.Structure;
using ACE.Server.Physics;
using ACE.Server.WorldObjects;
using ACE.Database.Models.World;

namespace ACE.Server.Managers
{

    public class QuestManager
    {
        public WorldObject WorldObject { get; }
        public ICollection<CharacterPropertiesQuestRegistry> Quests { get; }

        /// <summary>
        /// Constructs a new QuestManager for a WorldObject
        /// </summary>
        public QuestManager(WorldObject obj)
        {
            WorldObject = obj;
            Quests = WorldObject.Biota.CharacterPropertiesQuestRegistry;
        }

        /// <summary>
        /// Add a quest in this object's registry
        /// </summary>
        public void Add(String qName, WorldObject wo)
        {
            var result = (wo.Biota.CharacterPropertiesQuestRegistry.Where(quest => quest.QuestName == qName));
            if (result.Count() != 0)
            {
                result.FirstOrDefault().NumTimesCompleted += 1;
                Console.WriteLine("You already have this quest.");
            }
            else
            {
                CharacterPropertiesQuestRegistry quest2 = new CharacterPropertiesQuestRegistry
                {
                    QuestName = qName,
                    ObjectId = (uint)wo.Guid.Full,
                    LastTimeCompleted = 0,//Need to determine how to get accurate server time;
                    NumTimesCompleted = 1
                };
                Console.WriteLine("Adding quest to DB");
                wo.Biota.CharacterPropertiesQuestRegistry.Add(quest2);
                SaveDatabase(wo);
            }
        }

        public void Update(String qName, WorldObject wo)
        {
            Add(qName, wo);
            SaveDatabase(wo);
            //var result = (wo.Biota.BiotaPropertiesQuestRegistry.Where(quest => quest.QuestName == qName));
            //if (result.ElementAt(0).QuestName == null)
            //{
            //    Console.WriteLine("You do not have this quest.");
            //}
            //else
            //{
            //    result.ElementAt(0).NumTimesCompleted++;
            //}
        }

        public void Erase(String qName, WorldObject wo)
        {

            foreach(CharacterPropertiesQuestRegistry q in wo.Biota.CharacterPropertiesQuestRegistry.ToList())
            {
                if (q.QuestName.Equals(qName))
                {
                    wo.Biota.CharacterPropertiesQuestRegistry.Remove(q);
                }
            }

                //SaveDatabase(wo);
        }

        public static void PrintQuests(Player player)
        {
            Console.WriteLine("inside print quests");
            if (player.Biota.CharacterPropertiesQuestRegistry.Count() == 0)
            {
                Console.WriteLine("Quests are empty for " + player.Name);
            }
            for (int i = 0; i < player.Biota.CharacterPropertiesQuestRegistry.Count(); i++)
            {
                Console.WriteLine("Quest Name: " + player.Biota.CharacterPropertiesQuestRegistry.ElementAt(i).QuestName);
                Console.WriteLine("Times Completed" + player.Biota.CharacterPropertiesQuestRegistry.ElementAt(i).NumTimesCompleted);
                Console.WriteLine("Last Time Completed" + player.Biota.CharacterPropertiesQuestRegistry.ElementAt(i).LastTimeCompleted);
                Console.WriteLine("ID" + player.Biota.CharacterPropertiesQuestRegistry.ElementAt(i).Id);
                Console.WriteLine("Object ID" + player.Biota.CharacterPropertiesQuestRegistry.ElementAt(i).ObjectId);
            }
        }

        public bool Inquiry(String qName, WorldObject wo)
        {
            Console.WriteLine("Looking for quest name: " + qName + "  inside of " + wo.Name);

            var quest = (wo.Biota.CharacterPropertiesQuestRegistry.Where(quest2 => quest2.QuestName.Equals(qName)));
            Console.WriteLine("Quest count: " + quest.Count());
            if (quest.Count() == 0)
            {
                ///Should be returning NPC's inquiry action for not present/fail
                Console.WriteLine("Quest not present");
                return false;
            }
            else
            {
                ///Should be returning NPC's inquiry action for present/success
                return true;
            }
        }

        public void SaveDatabase(WorldObject wo)
        {
            var player = wo as Player;
            var saveChain = player.GetSaveChain();
            saveChain.EnqueueChain();
        }

    }
}
