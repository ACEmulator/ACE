using System;

using ACE.DatLoader;
using ACE.DatLoader.Entity;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects.Entity;

namespace ACE.Server.WorldObjects
{
    public class SkillAlterationDevice : WorldObject
    {
        public enum SkillAlterationType
        {
            Undef      = 0,
            Specialize = 1,
            Lower      = 2,
        }

        public SkillAlterationType TypeOfAlteration
        {
            get => (SkillAlterationType)(GetProperty(PropertyInt.TypeOfAlteration) ?? 0);
            set { if (value == 0) RemoveProperty(PropertyInt.TypeOfAlteration); else SetProperty(PropertyInt.TypeOfAlteration, (int)value); }
        }

        public Skill SkillToBeAltered
        {
            get => (Skill)(GetProperty(PropertyInt.SkillToBeAltered) ?? 0);
            set { if (value == 0) RemoveProperty(PropertyInt.SkillToBeAltered); else SetProperty(PropertyInt.SkillToBeAltered, (int)value); }
        }

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public SkillAlterationDevice(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public SkillAlterationDevice(Biota biota) : base(biota)
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

        public void ActOnUse(WorldObject activator, bool confirmed)
        {
            if (!(activator is Player player))
                return;

            // verify skill
            var skill = player.GetCreatureSkill(SkillToBeAltered);

            if (skill == null)
            {
                player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.YouFailToAlterSkill));
                return;
            }

            // get skill training / specialization costs
            var skillBase = DatManager.PortalDat.SkillTable.SkillBaseHash[(uint)skill.Skill];

            if (!VerifyRequirements(player, skill, skillBase))
                return;

            // confirmation dialog only for spec?
            if (!confirmed && TypeOfAlteration == SkillAlterationType.Specialize)
            {
                player.ConfirmationManager.EnqueueSend(new Confirmation_AlterSkill(player.Guid, Guid), $"This action will specialize your {skill.Skill.ToSentence()} skill and cost {skillBase.UpgradeCostFromTrainedToSpecialized} credits.");
                return;
            }

            AlterSkill(player, skill, skillBase);
        }

        public bool VerifyRequirements(Player player, CreatureSkill skill, SkillBase skillBase)
        {
            switch (TypeOfAlteration)
            {
                // Gem of Enlightenment
                case SkillAlterationType.Specialize:

                    // ensure skill is trained
                    if (skill.AdvancementClass != SkillAdvancementClass.Trained)
                    {
                        player.Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(player.Session, WeenieErrorWithString.Your_SkillMustBeTrained, skill.Skill.ToSentence()));
                        return false;
                    }

                    // ensure player has enough available skill credits
                    if (player.AvailableSkillCredits < skillBase.UpgradeCostFromTrainedToSpecialized)
                    {
                        player.Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(player.Session, WeenieErrorWithString.NotEnoughSkillCreditsToSpecialize, skill.Skill.ToSentence()));
                        return false;
                    }

                    // ensure player won't exceed limit of 70 specialized credits after operation
                    if (GetTotalSpecializedCredits(player) + skillBase.SpecializedCost > 70)
                    {
                        player.Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(player.Session, WeenieErrorWithString.TooManyCreditsInSpecializedSkills, skill.Skill.ToSentence()));
                        return false;
                    }
                    break;

                // Gem of Forgetfulness
                case SkillAlterationType.Lower:

                    // ensure skill is trained or specialized
                    if (skill.AdvancementClass < SkillAdvancementClass.Trained)
                    {
                        player.Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(player.Session, WeenieErrorWithString.Your_SkillIsAlreadyUntrained, skill.Skill.ToSentence()));
                        return false;
                    }

                    // salvage / tinkering skills specialized via augmentations
                    // Salvaging cannot be untrained or unspecialized, specialized tinkering skills can be reset at Asheron's Castle only.
                    if (player.IsSkillSpecializedViaAugmentation(skill.Skill, out var playerHasAugmentation) && playerHasAugmentation)
                    {
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat($"You cannot lower your {skill.Skill.ToSentence()} augmented skill.", ChatMessageType.Broadcast));
                        return false;
                    }

                    // Check for equipped items that have requirements in the skill we're lowering
                    if (CheckWieldedItems(player))
                    {
                        // Items are wielded which might be affected by a lowering operation
                        player.Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(player.Session, WeenieErrorWithString.CannotLowerSkillWhileWieldingItem, skill.Skill.ToSentence()));
                        return false;
                    }

                    break;

            }
            return true;
        }

        public void AlterSkill(Player player, CreatureSkill skill, SkillBase skillBase)
        {
            switch (TypeOfAlteration)
            {
                // Gem of Enlightenment
                case SkillAlterationType.Specialize:

                    if (player.SpecializeSkill(skill.Skill, skillBase.UpgradeCostFromTrainedToSpecialized, false))
                    {
                        var updateSkill = new GameMessagePrivateUpdateSkill(player, skill);
                        var availableSkillCredits = new GameMessagePrivateUpdatePropertyInt(player, PropertyInt.AvailableSkillCredits, player.AvailableSkillCredits ?? 0);
                        var msg = new GameEventWeenieErrorWithString(player.Session, WeenieErrorWithString.YouHaveSucceededSpecializing_Skill, skill.Skill.ToSentence());

                        player.Session.Network.EnqueueSend(updateSkill, availableSkillCredits, msg);

                        player.TryConsumeFromInventoryWithNetworking(this, 1);
                    }
                    break;

                // Gem of Forgetfulness
                case SkillAlterationType.Lower:

                    // specialized => trained
                    if (skill.AdvancementClass == SkillAdvancementClass.Specialized)
                    {
                        if (player.UnspecializeSkill(skill.Skill, skillBase.UpgradeCostFromTrainedToSpecialized))
                        {
                            var updateSkill = new GameMessagePrivateUpdateSkill(player, skill);
                            var availableSkillCredits = new GameMessagePrivateUpdatePropertyInt(player, PropertyInt.AvailableSkillCredits, player.AvailableSkillCredits ?? 0);
                            var msg = new GameEventWeenieErrorWithString(player.Session, WeenieErrorWithString.YouHaveSucceededUnspecializing_Skill, skill.Skill.ToSentence());

                            player.Session.Network.EnqueueSend(updateSkill, availableSkillCredits, msg);

                            player.TryConsumeFromInventoryWithNetworking(this, 1);
                        }
                    }

                    // trained => untrained
                    // in the case of skills which can't be untrained,
                    // keep trained, but recover the xp spent
                    else if (skill.AdvancementClass == SkillAdvancementClass.Trained)
                    {
                        var untrainable = Player.IsSkillUntrainable(skill.Skill);

                        if (player.UntrainSkill(skill.Skill, skillBase.TrainedCost))
                        {
                            var updateSkill = new GameMessagePrivateUpdateSkill(player, skill);
                            var availableSkillCredits = new GameMessagePrivateUpdatePropertyInt(player, PropertyInt.AvailableSkillCredits, player.AvailableSkillCredits ?? 0);
                            var msg = untrainable ? WeenieErrorWithString.YouHaveSucceededUntraining_Skill : WeenieErrorWithString.CannotUntrain_SkillButRecoveredXP;
                            var message = new GameEventWeenieErrorWithString(player.Session, msg, skill.Skill.ToSentence());

                            player.Session.Network.EnqueueSend(updateSkill, availableSkillCredits, message);

                            player.TryConsumeFromInventoryWithNetworking(this, 1);
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Calculates and returns the current total number of specialized credits
        /// </summary>
        private int GetTotalSpecializedCredits(Player player)
        {
            var specializedCreditsTotal = 0;

            foreach (var kvp in player.Skills)
            {
                if (kvp.Value.AdvancementClass == SkillAdvancementClass.Specialized)
                {
                    switch (kvp.Key)
                    {
                        // exclude None/Undef skill
                        case Skill.None:

                        // exclude aug specs
                        case Skill.ArmorTinkering:
                        case Skill.ItemTinkering:
                        case Skill.MagicItemTinkering:
                        case Skill.WeaponTinkering:
                        case Skill.Salvaging:
                            continue;
                    }

                    var skill = DatManager.PortalDat.SkillTable.SkillBaseHash[(uint)kvp.Key];

                    specializedCreditsTotal += skill.SpecializedCost;

                    if (kvp.Key == Skill.ArcaneLore) // exclude Arcane Lore TrainedCost
                        specializedCreditsTotal -= skill.TrainedCost;
                }
            }

            return specializedCreditsTotal;
        }

        /// <summary>
        /// Checks wielded items and their requirements to see if they'd be violated by an impending skill lowering operation
        /// </summary>
        private bool CheckWieldedItems(Player player)
        {
            foreach (var equippedItem in player.EquippedObjects.Values)
            {
                if (CheckWieldRequirement(player, equippedItem.WieldRequirements, equippedItem.WieldSkillType) ||
                    CheckWieldRequirement(player, equippedItem.WieldRequirements2, equippedItem.WieldSkillType2) ||
                    CheckWieldRequirement(player, equippedItem.WieldRequirements3, equippedItem.WieldSkillType3) ||
                    CheckWieldRequirement(player, equippedItem.WieldRequirements4, equippedItem.WieldSkillType4))
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckWieldRequirement(Player player, WieldRequirement itemWieldReq, int? wieldSkillType)
        {
            if (itemWieldReq != WieldRequirement.RawSkill && itemWieldReq != WieldRequirement.Skill)
                return false;

            return player.ConvertToMoASkill((Skill)(wieldSkillType ?? 0)) == SkillToBeAltered;
        }
    }
}
