using System.Collections.Generic;

using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.WorldObjects;
using ACE.Server.Managers;
using ACE.Server.Network.Sequence;

namespace ACE.Server.Factories
{
    public class LootGenerationFactory
    {
        // This is throw away code to understand the world object creation process.

        public static void Spawn(WorldObject inventoryItem, Position position)
        {
            throw new System.NotImplementedException();
            /* this was commented out because it's setting the phsycisdescriptionflag. Not sure we should be setting that here.
             // if we need to spawn things into the landscape, we should have such a factory that uses does that. LootGen factory doesn't seem appropriate for that
            inventoryItem.Sequences.GetNextSequence(SequenceType.ObjectTeleport);
            inventoryItem.Sequences.GetNextSequence(SequenceType.ObjectVector);
            inventoryItem.Location = position.InFrontOf(1.00f);
            inventoryItem.PhysicsDescriptionFlag |= PhysicsDescriptionFlag.Position;
            LandblockManager.AddObject(inventoryItem);*/
        }

        public static void CreateRandomTestWorldObjects(Player player, uint typeId, uint numItems)
        {
            throw new System.NotImplementedException();/*
            var weenieList = DatabaseManager.World.GetRandomWeeniesOfType(typeId, numItems);
            List<WorldObject> items = new List<WorldObject>();
            for (int i = 0; i < numItems; i++)
            {
                WorldObject wo = WorldObjectFactory.CreateNewWorldObject(weenieList[i].WeenieClassId);
                items.Add(wo);
            }
            player.HandleAddNewWorldObjectsToInventory(items);*/
        }
    }
}
