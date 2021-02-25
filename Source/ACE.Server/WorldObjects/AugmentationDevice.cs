using System;
using System.Collections.Generic;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    public class AugmentationDevice: WorldObject
    {
        public long? AugmentationCost
        {
            get => GetProperty(PropertyInt64.AugmentationCost);
            set { if (!value.HasValue) RemoveProperty(PropertyInt64.AugmentationCost); else SetProperty(PropertyInt64.AugmentationCost, value.Value); }
        }

        public int? AugmentationStat
        {
            get => GetProperty(PropertyInt.AugmentationStat);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.AugmentationStat); else SetProperty(PropertyInt.AugmentationStat, value.Value); }
        }

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public AugmentationDevice(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public AugmentationDevice(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
        }

        public override void ActOnUse(WorldObject activator)
        {
            ActOnUse(activator, false);
        }

        public void ActOnUse(WorldObject activator, bool confirmed = false)
        {
            if (!(activator is Player player))
                return;

            if (!VerifyRequirements(player))
                return;

            if (!confirmed)
            {
                player.ConfirmationManager.EnqueueSend(new Confirmation_Augmentation(player.Guid, Guid),
                    $"This action will augment your character with {Name} and will cost {AugmentationCost:N0} available experience.");

                return;
            }
            DoAugmentation(player);
        }

        public void DoAugmentation(Player player)
        {
            //Console.WriteLine($"{Name}.DoAugmentation({player.Name})");

            // set augmentation props for player
            var type = (AugmentationType)(AugmentationStat ?? 0);
            var augProp = AugProps[type];
            var curVal = player.GetProperty(augProp) ?? 0;
            var newVal = curVal + 1;
            player.SetProperty(augProp, newVal);

            if (AugTypeHelper.IsAttribute(type))
            {
                player.AugmentationInnateFamily++;

                var attr = AugTypeHelper.GetAttribute(type);
                var playerAttr = player.Attributes[attr];
                playerAttr.StartingValue += 5;
                player.Session.Network.EnqueueSend(new GameMessagePrivateUpdateAttribute(player, playerAttr));
            }
            else if (AugTypeHelper.IsResist(type))
                player.AugmentationResistanceFamily++;

            else if (AugTypeHelper.IsSkill(type))
            {
                var playerSkill = player.GetCreatureSkill(AugTypeHelper.GetSkill(type));
                playerSkill.AdvancementClass = SkillAdvancementClass.Specialized;
                playerSkill.InitLevel = 10;
                // adjust rank?
                // handle overages?
                // if trained skill is maxed, there will be a ~103m xp overage...
                var specRank = Player.CalcSkillRank(SkillAdvancementClass.Specialized, playerSkill.ExperienceSpent);
                playerSkill.Ranks = (ushort)specRank;
                player.Session.Network.EnqueueSend(new GameMessagePrivateUpdateSkill(player, playerSkill));
            }

            else if (type == AugmentationType.PackSlot)
            {
                // still seems to require the client to relog
                player.ContainerCapacity++;
                player.Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(player, PropertyInt.ContainersCapacity, (int)player.ContainerCapacity));
            }

            else if (type == AugmentationType.BurdenLimit)
            {
                var capacity = player.GetEncumbranceCapacity();
                player.Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(player, PropertyInt.EncumbranceCapacity, capacity));
            }

            // consume xp
            player.AvailableExperience -= AugmentationCost;

            // consume augmentation gem
            player.TryConsumeFromInventoryWithNetworking(this, 1);

            // send network messages
            var updateProp = new GameMessagePrivateUpdatePropertyInt(player, augProp, newVal);
            var updateXP = new GameMessagePrivateUpdatePropertyInt64(player, PropertyInt64.AvailableExperience, player.AvailableExperience ?? 0);

            player.Session.Network.EnqueueSend(updateProp, updateXP);
            player.SendWeenieErrorWithString(WeenieErrorWithString.YouSuccededAcquiringAugmentation, Name);

            // also broadcast to nearby players
            player.EnqueueBroadcast(new GameMessageScript(player.Guid, AugTypeHelper.GetEffect(type)));
            player.EnqueueBroadcast(new GameMessageSystemChat($"{player.Name} has acquired the {Name} augmentation!", ChatMessageType.Broadcast));

            player.SaveBiotaToDatabase();
        }

        public bool VerifyRequirements(Player player)
        {
            var availableXP = player.AvailableExperience ?? 0;
            var augCost = AugmentationCost ?? 0;

            if (AugmentationCost == null)
            {
                player.EnqueueBroadcast(new GameMessageSystemChat($"{Name} is missing AugmentationCost", ChatMessageType.System));
                return false;
            }

            if (availableXP < augCost)
            {
                player.SendWeenieError(WeenieError.AugmentationNotEnoughExperience);
                return false;
            }

            var type = (AugmentationType)(AugmentationStat ?? 0);

            // per-type checks
            if (AugTypeHelper.IsAttribute(type))
            {
                // innate attributes shared cap
                if (player.AugmentationInnateFamily >= MaxAugs[type])
                {
                    player.SendWeenieError(WeenieError.AugmentationTypeUsedTooManyTimes);
                    return false;
                }

                var playerAttribute = player.Attributes[AugTypeHelper.GetAttribute(type)];

                // check InitLevel
                if (playerAttribute.StartingValue >= 100)
                {
                    player.SendWeenieErrorWithString(WeenieErrorWithString.AugmentationSkillNotTrained, $"You are not able to purchase this augmentation because your {playerAttribute.Attribute.ToString()} is already at the maximum innate level!");
                    return false;
                }
            }
            else if (AugTypeHelper.IsSkill(type))
            {
                var playerSkill = player.GetCreatureSkill(AugTypeHelper.GetSkill(type));

                if (playerSkill.AdvancementClass != SkillAdvancementClass.Trained)
                {
                    player.SendWeenieErrorWithString(WeenieErrorWithString.AugmentationSkillNotTrained, $"You are not able to purchase this augmentation because you are not trained in {playerSkill.Skill.ToSentence()}!");
                    return false;
                }
            }
            else if (AugTypeHelper.IsResist(type))
            {
                // resistance shared cap
                if (player.AugmentationResistanceFamily >= MaxAugs[type])
                {
                    player.SendWeenieError(WeenieError.AugmentationTypeUsedTooManyTimes);
                    return false;
                }
            }

            // common checks
            var augProp = player.GetProperty(AugProps[type]) ?? 0;

            if (augProp >= MaxAugs[type])
            {
                player.SendWeenieError(WeenieError.AugmentationUsedTooManyTimes);
                return false;
            }

            return true;
        }

        public static Dictionary<AugmentationType, int> MaxAugs = new Dictionary<AugmentationType, int>()
        {
            { AugmentationType.Strength, 10 },          // attributes in shared group
            { AugmentationType.Endurance, 10 },
            { AugmentationType.Coordination, 10 },
            { AugmentationType.Quickness, 10 },
            { AugmentationType.Focus, 10 },
            { AugmentationType.Self, 10 },
            { AugmentationType.Salvage, 1 },
            { AugmentationType.ItemTinkering, 1 },
            { AugmentationType.ArmorTinkering, 1 },
            { AugmentationType.MagicItemTinkering, 1 },
            { AugmentationType.WeaponTinkering, 1 },
            { AugmentationType.PackSlot, 1 },
            { AugmentationType.BurdenLimit, 5 },
            { AugmentationType.DeathItemLoss, 3 },
            { AugmentationType.DeathSpellLoss, 1 },
            { AugmentationType.CritProtect, 1 },
            { AugmentationType.BonusXP, 1 },
            { AugmentationType.BonusSalvage, 4 },
            { AugmentationType.ImbueChance, 1 },
            { AugmentationType.RegenBonus, 2 },
            { AugmentationType.SpellDuration, 5 },
            { AugmentationType.ResistSlash, 2 },        // resistances in shared group
            { AugmentationType.ResistPierce, 2 },
            { AugmentationType.ResistBludgeon, 2 },
            { AugmentationType.ResistAcid, 2 },
            { AugmentationType.ResistFire, 2 },
            { AugmentationType.ResistCold, 2 },
            { AugmentationType.ResistElectric, 2 },
            { AugmentationType.FociCreature, 1 },
            { AugmentationType.FociItem, 1 },
            { AugmentationType.FociLife, 1 },
            { AugmentationType.FociWar, 1 },
            { AugmentationType.CritChance, 1 },
            { AugmentationType.CritDamage, 1 },
            { AugmentationType.Melee, 1 },
            { AugmentationType.Missile, 1 },
            { AugmentationType.Magic, 1 },
            { AugmentationType.Damage, 1 },
            { AugmentationType.DamageResist, 1 },
            { AugmentationType.AllStats, 1 },
            { AugmentationType.FociVoid, 1 },
        };

        public static Dictionary<AugmentationType, PropertyInt> AugProps = new Dictionary<AugmentationType, PropertyInt>()
        {
            { AugmentationType.Strength, PropertyInt.AugmentationInnateStrength },
            { AugmentationType.Endurance, PropertyInt.AugmentationInnateEndurance },
            { AugmentationType.Coordination, PropertyInt.AugmentationInnateCoordination },
            { AugmentationType.Quickness, PropertyInt.AugmentationInnateQuickness },
            { AugmentationType.Focus, PropertyInt.AugmentationInnateFocus },
            { AugmentationType.Self, PropertyInt.AugmentationInnateSelf },
            { AugmentationType.Salvage, PropertyInt.AugmentationSpecializeSalvaging },
            { AugmentationType.ItemTinkering, PropertyInt.AugmentationSpecializeItemTinkering },
            { AugmentationType.ArmorTinkering, PropertyInt.AugmentationSpecializeArmorTinkering },
            { AugmentationType.MagicItemTinkering, PropertyInt.AugmentationSpecializeMagicItemTinkering },
            { AugmentationType.WeaponTinkering, PropertyInt.AugmentationSpecializeWeaponTinkering },
            { AugmentationType.PackSlot, PropertyInt.AugmentationExtraPackSlot },
            { AugmentationType.BurdenLimit, PropertyInt.AugmentationIncreasedCarryingCapacity },
            { AugmentationType.DeathItemLoss, PropertyInt.AugmentationLessDeathItemLoss },
            { AugmentationType.DeathSpellLoss, PropertyInt.AugmentationSpellsRemainPastDeath },
            { AugmentationType.CritProtect, PropertyInt.AugmentationCriticalDefense },
            { AugmentationType.BonusXP, PropertyInt.AugmentationBonusXp },
            { AugmentationType.BonusSalvage, PropertyInt.AugmentationBonusSalvage },
            { AugmentationType.ImbueChance, PropertyInt.AugmentationBonusImbueChance },
            { AugmentationType.RegenBonus, PropertyInt.AugmentationFasterRegen },
            { AugmentationType.SpellDuration, PropertyInt.AugmentationIncreasedSpellDuration },
            { AugmentationType.ResistSlash, PropertyInt.AugmentationResistanceSlash },
            { AugmentationType.ResistPierce, PropertyInt.AugmentationResistancePierce },
            { AugmentationType.ResistBludgeon, PropertyInt.AugmentationResistanceBlunt },
            { AugmentationType.ResistAcid, PropertyInt.AugmentationResistanceAcid },
            { AugmentationType.ResistFire, PropertyInt.AugmentationResistanceFire },
            { AugmentationType.ResistCold, PropertyInt.AugmentationResistanceFrost },
            { AugmentationType.ResistElectric, PropertyInt.AugmentationResistanceLightning },
            { AugmentationType.FociCreature, PropertyInt.AugmentationInfusedCreatureMagic },
            { AugmentationType.FociItem, PropertyInt.AugmentationInfusedItemMagic },
            { AugmentationType.FociLife, PropertyInt.AugmentationInfusedLifeMagic },
            { AugmentationType.FociWar, PropertyInt.AugmentationInfusedWarMagic },
            { AugmentationType.CritChance, PropertyInt.AugmentationCriticalExpertise },
            { AugmentationType.CritDamage, PropertyInt.AugmentationCriticalPower },
            { AugmentationType.Melee, PropertyInt.AugmentationSkilledMelee },
            { AugmentationType.Missile, PropertyInt.AugmentationSkilledMissile },
            { AugmentationType.Magic, PropertyInt.AugmentationSkilledMagic },
            { AugmentationType.Damage, PropertyInt.AugmentationDamageBonus },
            { AugmentationType.DamageResist, PropertyInt.AugmentationDamageReduction },
            { AugmentationType.AllStats, PropertyInt.AugmentationJackOfAllTrades },
            { AugmentationType.FociVoid, PropertyInt.AugmentationInfusedVoidMagic },
        };
    }
}
