using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.DatLoader;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    public class SkillAlterationDevice : WorldObject
    {
        public SkillAlterationType TypeOfAlteration { get; set; }
        public Skill SkillToBeAltered { get; set; }

        public enum SkillAlterationType
        {
            Undef = 0,
            Specialize = 1,
            Lower = 2,
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
            //Copy values to properties
            TypeOfAlteration = (SkillAlterationType)(GetProperty(PropertyInt.TypeOfAlteration) ?? 1);
            SkillToBeAltered = (Skill)(GetProperty(PropertyInt.SkillToBeAltered) ?? 0);
        }

        public override void ActOnUse(WorldObject activator)
        {
            var player = activator as Player;
            if (player == null) return;

            var currentSkill = player.GetCreatureSkill(SkillToBeAltered);

            //Check to make sure we got a valid skill back
            if (currentSkill == null)
            {
                player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.YouFailToAlterSkill));
                return;
            }

            //Gather costs associated with manipulating currently selected skill
            var skill = DatManager.PortalDat.SkillTable.SkillBaseHash[(uint)currentSkill.Skill];

            switch (TypeOfAlteration)
            {
                case SkillAlterationType.Specialize:
                    //Check to make sure player won't exceed limit of 70 specialized credits after operation
                    if (skill.UpgradeCostFromTrainedToSpecialized + GetTotalSpecializedCredits(player) > 70)
                    {
                        player.Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(player.Session, WeenieErrorWithString.TooManyCreditsInSpecializedSkills, currentSkill.Skill.ToSentence()));
                        break;
                    }

                    //Check to see if the skill is ripe for specializing
                    if (currentSkill.AdvancementClass == SkillAdvancementClass.Trained)
                    {
                        if (player.AvailableSkillCredits >= skill.UpgradeCostFromTrainedToSpecialized)
                        {
                            if (player.SpecializeSkill(currentSkill.Skill, skill.UpgradeCostFromTrainedToSpecialized, false))
                            {
                                //Specialization was successful, notify the client
                                player.Session.Network.EnqueueSend(new GameMessagePrivateUpdateSkill(player, currentSkill));
                                player.Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(player, PropertyInt.AvailableSkillCredits, player.AvailableSkillCredits ?? 0));
                                player.Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(player.Session, WeenieErrorWithString.YouHaveSucceededSpecializing_Skill, currentSkill.Skill.ToSentence()));

                                //Destroy the gem we used successfully
                                player.TryConsumeFromInventoryWithNetworking(this, 1);

                                break;
                            }
                        }
                        else
                        {
                            player.Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(player.Session, WeenieErrorWithString.NotEnoughSkillCreditsToSpecialize, currentSkill.Skill.ToSentence()));
                            break;
                        }
                    }

                    //Tried to use a specialization gem on a skill that is either already specialized, or untrained
                    player.Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(player.Session, WeenieErrorWithString.Your_SkillMustBeTrained, currentSkill.Skill.ToSentence()));
                    break;

                case SkillAlterationType.Lower:

                    //We're using a Gem of Forgetfullness

                    //Check for equipped items that have requirements in the skill we're lowering
                    if (CheckWieldedItems(player))
                    {
                        //Items are wielded which might be affected by a lowering operation
                        player.Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(player.Session, WeenieErrorWithString.CannotLowerSkillWhileWieldingItem, currentSkill.Skill.ToSentence()));
                        break;
                    }

                    if (currentSkill.AdvancementClass == SkillAdvancementClass.Specialized)
                    {
                        if (player.UnspecializeSkill(currentSkill.Skill, skill.UpgradeCostFromTrainedToSpecialized))
                        {
                            //Unspecialization was successful, notify the client
                            player.Session.Network.EnqueueSend(new GameMessagePrivateUpdateSkill(player, currentSkill));
                            player.Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(player, PropertyInt.AvailableSkillCredits, player.AvailableSkillCredits ?? 0));
                            player.Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(player.Session, WeenieErrorWithString.YouHaveSucceededUnspecializing_Skill, currentSkill.Skill.ToSentence()));

                            //Destroy the gem we used successfully
                            player.TryConsumeFromInventoryWithNetworking(this, 1);

                            break;
                        }
                    }
                    else if (currentSkill.AdvancementClass == SkillAdvancementClass.Trained)
                    {
                        var untrainable = Player.IsSkillUntrainable(currentSkill.Skill);

                        if (player.UntrainSkill(currentSkill.Skill, skill.TrainedCost))
                        {
                            //Untraining was successful, notify the client
                            player.Session.Network.EnqueueSend(new GameMessagePrivateUpdateSkill(player, currentSkill));

                            if (untrainable)
                            {
                                player.Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(player, PropertyInt.AvailableSkillCredits, player.AvailableSkillCredits ?? 0));
                                player.Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(player.Session, WeenieErrorWithString.YouHaveSucceededUntraining_Skill, currentSkill.Skill.ToSentence()));
                            }
                            else
                            {
                                player.Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(player.Session, WeenieErrorWithString.CannotUntrain_SkillButRecoveredXP, currentSkill.Skill.ToSentence()));
                            }

                            //Destroy the gem we used successfully
                            player.TryConsumeFromInventoryWithNetworking(this, 1);

                            break;
                        }
                    }
                    else
                        player.Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(player.Session, WeenieErrorWithString.Your_SkillIsAlreadyUntrained, currentSkill.Skill.ToSentence()));

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
                    var skill = DatManager.PortalDat.SkillTable.SkillBaseHash[(uint)kvp.Key];

                    specializedCreditsTotal += skill.UpgradeCostFromTrainedToSpecialized;
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
                var itemWieldReq = (WieldRequirement)(equippedItem.GetProperty(PropertyInt.WieldRequirements) ?? 0);

                if (itemWieldReq == WieldRequirement.RawSkill || itemWieldReq == WieldRequirement.Skill)
                {
                    // Check WieldDifficulty property against player's Skill level, defined by item's WieldSkillType property
                    var itemSkillReq = player.ConvertToMoASkill((Skill)(equippedItem.GetProperty(PropertyInt.WieldSkillType) ?? 0));

                    if (itemSkillReq == SkillToBeAltered)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
