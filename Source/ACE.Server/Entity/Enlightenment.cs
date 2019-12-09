using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.WorldObjects;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.Entity
{
    public class Enlightenment
    {
        // https://asheron.fandom.com/wiki/Enlightenment

        // Reset your character to level 1, losing all experience and luminance but gaining a title, two points in vitality and one point in all of your skills.
        // In order to be eligible for enlightenment, you must be level 275, Master rank in a Society, and have all luminance auras with the exception of the skill credit auras.

        // As stated in the Spring 2014 patch notes, Enlightenment is a process for the most devoted players of Asheron's Call to continue enhancing characters which have been "maxed out" in terms of experience and abilities.
        // It was not intended to be a quest that every player would undertake or be interested in.

        // Requirements:
        // - Level 275
        // - Have all luminance auras (crafting aura included) except the 2 skill credit auras. (20 million total luminance)
        // - Have mastery rank in a society
        // - Have 25 unused pack spaces
        // - Max # of times for enlightenment: 5

        // You lose:
        // - All experience, reverting to level 1.
        // - All luminance, and luminance auras with the exception of the skill credit auras.
        // - The ability to use aetheria (until you attain sufficient level and re-open aetheria slots).
        // - The ability to gain luminance (until you attain level 200 and re-complete Nalicana's Test).
        // - The ability to equip and use items which have skill and level requirements beyond those of a level 1 character.
        //   Any equipped items are moved to your pack automatically.

        // You keep:
        // - All augmentations obtained through Augmentation Gems.
        // - Skill credits from luminance auras, Aun Ralirea, and Chasing Oswald quests.
        // - All quest flags with the exception of aetheria and luminance.

        // You gain:
        // - A new title each time you enlighten
        // - +2 to vitality
        // - +1 to all of your skills
        // - An attribute reset certificate

        public static void HandleEnlightenment(Player player)
        {
            if (!VerifyRequirements(player))
                return;

            DequipAllItems(player);

            RemoveAbility(player);

            AddPerks(player);
        }

        public static bool VerifyRequirements(Player player)
        {
            if (player.Level < 275)
            {
                player.Session.Network.EnqueueSend(new GameMessageSystemChat($"You must be level 275 for enlightenment.", ChatMessageType.Broadcast));
                return false;
            }

            if (!VerifyLumAugs(player))
            {
                player.Session.Network.EnqueueSend(new GameMessageSystemChat($"You must have all luminance auras for enlightenment.", ChatMessageType.Broadcast));
                return false;
            }

            // TODO: society masteries

            if (player.GetFreeInventorySlots() < 25)
            {
                player.Session.Network.EnqueueSend(new GameMessageSystemChat($"You must have at least 25 free inventory slots for enlightenment.", ChatMessageType.Broadcast));
                return false;
            }

            if (player.Enlightenment >= 5)
            {
                player.Session.Network.EnqueueSend(new GameMessageSystemChat($"You have already reached the maximum enlightenment level!", ChatMessageType.Broadcast));
                return false;
            }
            return true;
        }

        public static bool VerifyLumAugs(Player player)
        {
            return player.LumAugAllSkills != 0
                && player.LumAugCritDamageRating != 0
                && player.LumAugCritReductionRating != 0
                && player.LumAugDamageRating != 0
                && player.LumAugDamageReductionRating != 0
                && player.LumAugHealingRating != 0
                && player.LumAugItemManaGain != 0
                && player.LumAugItemManaUsage != 0
                && player.LumAugSkilledCraft != 0
                && player.LumAugSkilledSpec != 0
                && player.LumAugSurgeChanceRating != 0;
        }

        public static void DequipAllItems(Player player)
        {
            // how to handle burden after strength adjustment?
            foreach (var item in player.EquippedObjects.Values)
                player.TryDequipObjectWithNetworking(item.Guid.Full, out var _, Player.DequipObjectAction.DequipToPack);
        }

        public static void RemoveAbility(Player player)
        {
            RemoveLevel(player);
            RemoveAetheria(player);
            RemoveLuminance(player);
        }

        public static void RemoveLevel(Player player)
        {
            player.UpdateProperty(player, PropertyInt.Level, 1);
            player.UpdateProperty(player, PropertyInt64.TotalExperience, 0);
            player.UpdateProperty(player, PropertyInt64.AvailableExperience, 0);

            // reset skills
            foreach (var skill in player.Skills.Values)
            {
                skill.ExperienceSpent = 0;
                skill.Ranks = 0;

                player.Session.Network.EnqueueSend(new GameMessagePrivateUpdateSkill(player, skill));
            }

            // remove skill credits except for those from:
            // todo: luminance auras, Aun Ralirea, and Chasing Oswald quests.
            player.UpdateProperty(player, PropertyInt.AvailableSkillCredits, 0);
        }

        public static void RemoveAetheria(Player player)
        {
            // todo: remove aetheria quest flag
            player.UpdateProperty(player, PropertyInt.AetheriaBitfield, 0);
        }

        public static void RemoveLuminance(Player player)
        {
            player.UpdateProperty(player, PropertyInt64.AvailableLuminance, 0);
            player.UpdateProperty(player, PropertyInt64.MaximumLuminance, 0);

            player.UpdateProperty(player, PropertyInt.LumAugAllSkills, 0);
            player.UpdateProperty(player, PropertyInt.LumAugCritDamageRating, 0);
            player.UpdateProperty(player, PropertyInt.LumAugCritReductionRating, 0);
            player.UpdateProperty(player, PropertyInt.LumAugDamageRating, 0);

            player.UpdateProperty(player, PropertyInt.LumAugDamageReductionRating, 0);
            player.UpdateProperty(player, PropertyInt.LumAugHealingRating, 0);
            player.UpdateProperty(player, PropertyInt.LumAugItemManaGain, 0);
            player.UpdateProperty(player, PropertyInt.LumAugItemManaUsage, 0);

            player.UpdateProperty(player, PropertyInt.LumAugSkilledCraft, 0);
            player.UpdateProperty(player, PropertyInt.LumAugSkilledSpec, 0);
            player.UpdateProperty(player, PropertyInt.LumAugSurgeChanceRating, 0);

            // LumAugNoDestroyCraft?
            // LumAugSurgeEffectRating?
            // LumAugVitality?

            // todo: remove luminance quest flag
        }

        public static void AddPerks(Player player)
        {
            // +1 to all skills
            // this could be handled through InitLevel, since we are always using deltas when modifying that field
            // (ie. +5/-5, instead of specifically setting to 5 trained / 10 specialized in SkillAlterationDevice)
            // however, it just feels safer to handle this dynamically in CreatureSkill, based on Enlightenment (similar to augs)
            var enlightenment = player.Enlightenment + 1;
            player.UpdateProperty(player, PropertyInt.Enlightenment, enlightenment);

            // add title
            switch (enlightenment)
            {
                case 1:
                    player.AddTitle(CharacterTitle.Awakened);
                    break;
                case 2:
                    player.AddTitle(CharacterTitle.Enlightened);
                    break;
                case 3:
                    player.AddTitle(CharacterTitle.Illuminated);
                    break;
                case 4:
                    player.AddTitle(CharacterTitle.Transcended);
                    break;
                case 5:
                    player.AddTitle(CharacterTitle.CosmicConscious);
                    break;
            }

            // todo: attribute reset certificate

            // +2 vitality
            // handled automatically via PropertyInt.Enlightenment * 2

            /*var vitality = player.LumAugVitality + 2;
            player.UpdateProperty(player, PropertyInt.LumAugVitality, vitality);*/
        }
    }
}
