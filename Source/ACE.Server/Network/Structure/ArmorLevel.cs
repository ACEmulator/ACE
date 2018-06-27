using System.Collections.Generic;
using System.IO;
using System.Linq;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.Structure
{
    public class ArmorLevel
    {
        public uint Head;
        public uint Chest;
        public uint Abdomen;
        public uint UpperArm;
        public uint LowerArm;
        public uint Hand;
        public uint UpperLeg;
        public uint LowerLeg;
        public uint Foot;

        public ArmorLevel(Creature creature)
        {
            // get all body parts
            Head = GetArmorLevel(creature, BodyPart.Head);
            Chest = GetArmorLevel(creature, BodyPart.Chest);
            Abdomen = GetArmorLevel(creature, BodyPart.Abdomen);
            UpperArm = GetArmorLevel(creature, BodyPart.UpperArm);
            LowerArm = GetArmorLevel(creature, BodyPart.LowerArm);
            Hand = GetArmorLevel(creature, BodyPart.Hand);
            UpperLeg = GetArmorLevel(creature, BodyPart.UpperLeg);
            LowerLeg = GetArmorLevel(creature, BodyPart.LowerLeg);
            Foot = GetArmorLevel(creature, BodyPart.Foot);

        }

        public uint GetArmorLevel(Creature creature, BodyPart bodyPart)
        {
            // get armor pieces covering this body part
            var layers = GetArmor(creature, bodyPart);
            if (layers == null) return 0;

            // get total AL?
            var totalAL = 0;

            foreach (var layer in layers)
            {
                var armorLevel = layer.GetProperty(PropertyInt.ArmorLevel) ?? 0;
                totalAL += armorLevel;

                // impen?
                // body armor / life spells?
            }
            // doesn't handle negatives?
            return (uint)totalAL;
        }

        /// <summary>
        /// Returns the player armor for a body part
        /// </summary>
        public List<WorldObject> GetArmor(Creature creature, BodyPart bodyPart)
        {
            var bodyLocation = BodyParts.GetFlags(BodyParts.GetEquipMask(bodyPart));

            var equipped = creature.EquippedObjects.Values.Where(e => e is Clothing && BodyParts.HasAny(e.CurrentWieldedLocation, bodyLocation)).ToList();

            return equipped;
        }
    }

    public static class ArmorLevelExtensions
    {
        public static void Write(this BinaryWriter writer, ArmorLevel armorLevel)
        {
            writer.Write(armorLevel.Head);
            writer.Write(armorLevel.Chest);
            writer.Write(armorLevel.Abdomen);
            writer.Write(armorLevel.UpperArm);
            writer.Write(armorLevel.LowerArm);
            writer.Write(armorLevel.Hand);
            writer.Write(armorLevel.UpperLeg);
            writer.Write(armorLevel.LowerLeg);
            writer.Write(armorLevel.Foot);
        }
    }
}
