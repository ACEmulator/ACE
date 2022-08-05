using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ACE.Server.Entity;
using ACE.Entity.Enum.Properties;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.Structure
{
    /// <summary>
    /// Handles the per-body part AL display for the character panel
    /// </summary>
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
            // get base + enchanted AL per body part
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
            var layers = GetArmorClothing(creature, bodyPart);
            if (layers == null) return 0;

            // get total AL
            var totalAL = 0;

            foreach (var layer in layers)
            {
                var baseAL = layer.GetProperty(PropertyInt.ArmorLevel) ?? 0;

                // impen / brittlemail
                var modAL = layer.EnchantmentManager.GetArmorMod();

                totalAL += baseAL + modAL;
            }

            // doesn't handle negatives?
            totalAL = Math.Max(0, totalAL);

            // if all layers for this body part are unenchantable,
            // send totalAL + 9999 for client to display *
            if (layers.Count > 0 && !layers.Any(i => i.IsEnchantable))
                totalAL += 9999;

            return (uint)totalAL;
        }

        /// <summary>
        /// Returns the armor + clothing for a body part
        /// </summary>
        public List<WorldObject> GetArmorClothing(Creature creature, BodyPart bodyPart)
        {
            var bodyLocation = BodyParts.GetFlags(BodyParts.GetCoverageMask(bodyPart));

            var equipped = creature.EquippedObjects.Values.Where(e => e is Clothing && BodyParts.HasAny(e.ClothingPriority, bodyLocation)).ToList();

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
