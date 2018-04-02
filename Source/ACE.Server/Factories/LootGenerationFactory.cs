using System.Collections.Generic;

using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    public class LootGenerationFactory
    {
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

        public static List<WorldObject> CreateRandomObjectsOfType(WeenieType type, int count)
        {
            var weenies = DatabaseManager.World.GetRandomWeeniesOfType((int)type, count);

            var worldObjects = new List<WorldObject>();

            foreach (var weenie in weenies)
            {
                var wo = WorldObjectFactory.CreateNewWorldObject(weenie.ClassId);
                worldObjects.Add(wo);
            }

            return worldObjects;
        }
    }
}
