using ACE.Common;
using ACE.Database;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Managers;
using System;
using System.Collections.Generic;
using System.Linq;

// TODO: Refactor into Generator.cs?
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
            DerethDateTime currentTime = new DerethDateTime(WorldManager.PortalYearTicks);
            Position pos = null;
            Random random = new Random((int)DateTime.UtcNow.Ticks);

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
            for (int i = 0; i < generator.MaxGeneratedObjects; i++)
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

                // If this generator has linked generators use those for spawning objects
                if (generator.ActivationCreateClass == 0)
                {
                    // Spawn this generator if it's not the top-level generator
                    if (generator.GeneratorIID != null)
                    {
                        results.Add(new Generator(GuidManager.NewGeneratorGuid(), generator));
                        generator.GeneratorEnteredWorld = true;
                    }

                    // Get a random generator from the weighted list of linked generators and read it's AceObject from the DB
                    if (generator.GeneratorLinks.Count == 0)
                        return null;
                    uint linkId = GetRandomGeneratorIdFromGeneratorList(random, generator.GeneratorLinks);
                    AceObject newGen = DatabaseManager.World.GetAceObjectByWeenie(linkId);

                    // The linked generator is at the same location as the top generator and references its parent
                    newGen.Location = pos;
                    newGen.GeneratorIID = generator.AceObjectId;
                    newGen.GeneratorEnteredWorld = true;

                    // Recursively call this method again with the just read generatorObject
                    List<WorldObject> objectList = CreateWorldObjectsFromGenerator(newGen);
                    objectList?.ForEach(o => results.Add(o));
                }
                // else spawn the objects directly from this generator
                else
                {
                    // TODO: Re-evaluate guid assignment here, is it a bad thing items and creatures are in the same pool? Probably.
                    WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)generator.ActivationCreateClass);

                    if (wo != null)
                    {
                        wo.Location = pos;
                        if (wo.WeenieType == WeenieType.Creature || wo.WeenieType == WeenieType.Cow)
                            wo.Guid = GuidManager.NewNonStaticGuid();
                         else
                            wo.Guid = GuidManager.NewItemGuid();
                        wo.GeneratorId = generator.AceObjectId;

                        results.Add(wo);
                    }
                }
            }

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

            byte x = (byte)(cellX * 24 + random.Next(0, 23));
            byte y = (byte)(cellY * 24 + random.Next(0, 23));
            byte z = 0; // TODO: load z from cell.dat, also when specifying 0 the gravity seems to pull the creature to the right z value

            Position pos = new Position(landblock, x, y, z, 0f, 0f, 0f, 0f);

            return pos;
        }

        private static uint GetRandomGeneratorIdFromGeneratorList(Random random, List<AceObjectGeneratorLink> generatorObjects)
        {
            int[] cumulativeValues = new int[generatorObjects.Count];
            
            // Build the cumulative values of the generatorWeights
            cumulativeValues[0] = generatorObjects[0].GeneratorWeight;

            for (int i = 1; i < generatorObjects.Count; i++)
            {
                cumulativeValues[i] = cumulativeValues[i - 1] + generatorObjects[i].GeneratorWeight;
            }

            // Generate a random weight value between 0 and the maximum cumulative weight
            int value = (int)(random.NextDouble() * cumulativeValues[generatorObjects.Count - 1]);

            // Search the index of the cumulative weight that's bigger than the generated random weight
            int index = Array.BinarySearch(cumulativeValues, value);
            if (index < 0)
                index = ~index; // bitwise compliment of the index returned from BinarySearch for the real index

            return generatorObjects[index].GeneratorWeenieClassId;
        }
    }
}
