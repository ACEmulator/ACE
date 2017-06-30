using ACE.Common;
using ACE.Database;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Managers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ACE.Factories
{
    /// <summary>
    /// Factory class for creating objects from generators
    /// </summary>
    public class GeneratorFactory
    {
        /// <summary>
        /// Enumerator for acting on the GeneratorTimeType of a generator
        /// </summary>
        private enum GeneratorTimeType : int
        {
            Undef = 0,
            RealTime = 1,
            Defined = 2,
            Event = 3,
            Night = 4,
            Day = 5
        }

        /// <summary>
        /// Enumerator for acting on the GeneratorType of a generator
        /// </summary>
        private enum GeneratorType : int
        {
            Undef = 0,
            Relative = 1,
            Absolute = 2
        }

        /// <summary>
        /// Create WorldObjects by ObjectType from a generator.
        /// </summary>
        /// <param name="generatorObject"></param>
        /// <returns>List of created WorldObjects</returns>
        public static List<WorldObject> CreateWorldObjectsFromGenerator(AceObject generator)
        {
            List<WorldObject> results = new List<WorldObject>();
            var currentTime = new DerethDateTime(WorldManager.PortalYearTicks);
            Position pos = null;
            Random random = new Random((int)WorldManager.PortalYearTicks);

            // Check if the current generator is meant to spawn objects at this time of the day
            switch (generator.GeneratorTimeType)
            {
                case (int)GeneratorTimeType.Day:
                    if (currentTime.IsNight)
                        return null;
                    break;

                case (int)GeneratorTimeType.Night:
                    if (currentTime.IsDaytime)
                        return null;
                    break;
            }

            // Check the probability of this generator spawning something at all
            if (random.Next(1, 100) >= generator.GeneratorProbability)
                return null;

            // Generate objects from this generator #MaxGeneratedObjects times
            for (var i = 0; i < generator.MaxGeneratedObjects; i++)
            {
                switch (generator.GeneratorType)
                {
                    // Use the position of the generator as a static position
                    case (int)GeneratorType.Absolute:
                        pos = generator.Location.InFrontOf(2.0);
                        pos.PositionZ = pos.PositionZ - 0.5f;
                        break;

                    // Generate a random position inside the landblock
                    case (int)GeneratorType.Relative:
                        pos = GetRandomLocInLandblock(random, generator.Location.Cell);
                        break;
                }

                AceObject baseObject = DatabaseManager.World.GetAceObjectByWeenie(generator.ActivationCreateClass);
                baseObject.Generator = generator.AceObjectId;

                // Determine the ObjectType and call the specific Factory
                ObjectType ot = (ObjectType)baseObject.Type;
                switch (ot)
                {
                    case ObjectType.Creature:
                        // TODO: enhance this if we need to spawn NPCs too, else it is just monsters for this tpye
                        results.Add(MonsterFactory.SpawnMonster(baseObject, pos));
                        break;

                    case ObjectType.Portal:
                        // TODO: enable generators to spawn portals, i.e. for Humming Crystal Portal
                        // results.Add();
                        break;

                    case ObjectType.Misc:
                        // TODO: enable generators to spawn misc items: i.e. Campfire
                        // results.Add();
                        break;

                    default:
                        baseObject.Location = pos;
                        if (baseObject.Location != null)
                            results.Add(new DebugObject(new ObjectGuid(baseObject.AceObjectId), baseObject));
                        break;
                }
            }

            // TODO: extend to hierarchical generators: i.e. a Northern Marae Lessel Generator can spawn different Camps of Monsters, etc.

            return results;
        }

        /// <summary>
        /// Create a random position in the given landblock.
        /// </summary>
        /// <param name="random">random device to be used </param>
        /// <param name="landblock"></param>
        /// <returns>randomly generated position</returns>
        private static Position GetRandomLocInLandblock(Random random, uint landblock)
        {
            byte cellX = (byte)random.Next(0, 7);
            byte cellY = (byte)random.Next(0, 7);
            ushort cell = (ushort)(cellX << 3 | cellY);

            uint lblock = (landblock << 16) | cell;
            byte x = (byte)(cellX * 24 + random.Next(0, 23));
            byte y = (byte)(cellY * 24 + random.Next(0, 23));
            byte z = 0; // TODO: load z from cell.dat, also when specifying 0 the gravity seems to pull the creature to the right z value

            Position pos = new Position(lblock, x, y, z, 0f, 0f, 0f, 0f);

            return pos;
        }
    }
}
