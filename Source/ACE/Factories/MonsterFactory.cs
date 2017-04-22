using ACE.Entity;
using ACE.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Network;
using ACE.Entity.Objects;

namespace ACE.Factories
{
    public class MonsterFactory
    {
        /// <summary>
        /// Create a new creature at the specified position
        /// </summary>
        /// <param name="saveAsStatic">If set to true, it saves the spawned creature in the DB as a static spawn</param>
        public static Creature SpawnCreature(uint weenieClassId, bool saveAsStatic, Position position)
        {
            AceCreatureObject aco = DatabaseManager.World.GetCreatureDataByWeenie(weenieClassId);
            if (aco == null)
                return null;
            
            AceCreatureStaticLocation acsl = new AceCreatureStaticLocation();
            acsl.WeenieClassId = (ushort)weenieClassId;
            acsl.Landblock = (ushort)position.LandblockId.Landblock;
            acsl.Cell = (ushort)position.Cell;
            acsl.PosX = position.PositionX;
            acsl.PosY = position.PositionY;
            acsl.PosZ = position.PositionZ;
            acsl.QW = position.RotationW;
            acsl.QX = position.RotationX;
            acsl.QY = position.RotationY;
            acsl.QZ = position.RotationZ;
            acsl.CreatureData = aco;

            Creature newCreature = new Creature(acsl);

            if (saveAsStatic) {
                bool success = DatabaseManager.World.InsertStaticCreatureLocation(acsl);
                if (!success)
                    return null;
            }

            return newCreature;
        }

        /// <summary>
        /// Create a number of new creatures at the specified position as defined by the generator
        /// </summary>
        public static List<Creature> SpawnCreaturesFromGenerator(AceCreatureGeneratorLocation acgl)
        {
            List<Creature> creatureList = new List<Creature>();
            Random rnd = new Random();

            // Try to spawn #quantity amount of cratures
            for (var count = 0; count < acgl.Quantity; count++)
            {
                int roll = rnd.Next(1, 100);
                ushort wcid = 0;

                // Check the probability of a given weenie in this generator and accept the fact there might be no weenie rolled
                foreach (var weenieData in acgl.CreatureGeneratorData.OrderBy(d => d.Probability))
                {
                    if (roll < weenieData.Probability)
                    {
                        wcid = weenieData.WeenieClassId;
                        break;
                    }
                }

                if (wcid != 0)
                {
                    Creature c = SpawnCreature(wcid, false, acgl.Position);
                    if (c != null)
                        creatureList.Add(c);
                }                    
            }
            return creatureList;
        }
    }
}
