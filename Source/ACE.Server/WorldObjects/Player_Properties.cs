
using ACE.Common;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public override string Name
        {
            get => IsPlussed ? (((CloakStatus ?? ACE.Entity.Enum.CloakStatus.Off) < ACE.Entity.Enum.CloakStatus.Player) ? "+" + base.Name : base.Name) : base.Name;

            set
            {
                var name = value;

                if (name.StartsWith("+"))
                    name = name.Substring(1);

                base.Name = name;
            }
        }

        // ========================================
        // ========= Admin Tier Properties ========
        // ========================================

        public bool IsAdmin
        {
            get => GetProperty(PropertyBool.IsAdmin) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.IsAdmin); else SetProperty(PropertyBool.IsAdmin, value); }
        }

        public bool IsSentinel
        {
            get => GetProperty(PropertyBool.IsSentinel) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.IsSentinel); else SetProperty(PropertyBool.IsSentinel, value); }
        }

        public bool IsEnvoy
        {
            get => GetProperty(PropertyBool.IsSentinel) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.IsSentinel); else SetProperty(PropertyBool.IsSentinel, value); }
        }

        public bool IsArch
        {
            get => GetProperty(PropertyBool.IsArch) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.IsArch); else SetProperty(PropertyBool.IsArch, value); }
        }

        public bool IsPsr
        {
            get => GetProperty(PropertyBool.IsPsr) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.IsPsr); else SetProperty(PropertyBool.IsPsr, value); }
        }

        public bool IsAdvocate
        {
            get => GetProperty(PropertyBool.IsAdvocate) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.IsAdvocate); else SetProperty(PropertyBool.IsAdvocate, value); }
        }

        public bool IsPlussed
        {
            get => Character.IsPlussed || (ConfigManager.Config.Server.Accounts.OverrideCharacterPermissions && Session.AccessLevel > AccessLevel.Advocate);
        }


        // ========================================
        // ========== Account Properties ==========
        // ========================================

        public bool Account15Days
        {
            get => GetProperty(PropertyBool.Account15Days) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.Account15Days); else SetProperty(PropertyBool.Account15Days, value); }
        }


        // ========================================
        // ========= Advocate Properties ==========
        // ========================================

        /// <summary>
        /// Flag indicates if advocate quest has been completed
        /// </summary>
        public bool AdvocateQuest
        {
            get => GetProperty(PropertyBool.AdvocateQuest) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.AdvocateQuest); else SetProperty(PropertyBool.AdvocateQuest, value); }
        }

        /// <summary>
        /// Flag indicates if player is currently in advocate state
        /// </summary>
        public bool AdvocateState
        {
            get => GetProperty(PropertyBool.AdvocateState) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.AdvocateState); else SetProperty(PropertyBool.AdvocateState, value); }
        }

        public int? AdvocateLevel
        {
            get => GetProperty(PropertyInt.AdvocateLevel);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.AdvocateLevel); else SetProperty(PropertyInt.AdvocateLevel, value.Value); }
        }


        // ========================================
        // ========= Channel Properties ===========
        // ========================================

        public Channel? ChannelsActive
        {
            get => (Channel?)GetProperty(PropertyInt.ChannelsActive);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ChannelsActive); else SetProperty(PropertyInt.ChannelsActive, (int)value.Value); }
        }

        public Channel? ChannelsAllowed
        {
            get => (Channel?)GetProperty(PropertyInt.ChannelsAllowed);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ChannelsAllowed); else SetProperty(PropertyInt.ChannelsAllowed, (int)value.Value); }
        }


        // ========================================
        // ========== Player Properties ===========
        // ========================================

        public int? Age
        {
            get => GetProperty(PropertyInt.Age);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.Age); else SetProperty(PropertyInt.Age, value.Value); }
        }

        public long? AvailableExperience
        {
            get => GetProperty(PropertyInt64.AvailableExperience);
            set { if (!value.HasValue) RemoveProperty(PropertyInt64.AvailableExperience); else SetProperty(PropertyInt64.AvailableExperience, value.Value); }
        }

        public long? TotalExperience
        {
            get => GetProperty(PropertyInt64.TotalExperience);
            set { if (!value.HasValue) RemoveProperty(PropertyInt64.TotalExperience); else SetProperty(PropertyInt64.TotalExperience, value.Value); }
        }

        public long? AvailableLuminance
        {
            get => GetProperty(PropertyInt64.AvailableLuminance);
            set { if (!value.HasValue) RemoveProperty(PropertyInt64.AvailableLuminance); else SetProperty(PropertyInt64.AvailableLuminance, value.Value); }
        }

        public long? MaximumLuminance
        {
            get => GetProperty(PropertyInt64.MaximumLuminance);
            set { if (!value.HasValue) RemoveProperty(PropertyInt64.MaximumLuminance); else SetProperty(PropertyInt64.MaximumLuminance, value.Value); }
        }

        public int? AvailableSkillCredits
        {
            get => GetProperty(PropertyInt.AvailableSkillCredits);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.AvailableSkillCredits); else SetProperty(PropertyInt.AvailableSkillCredits, value.Value); }
        }

        public int? TotalSkillCredits
        {
            get => GetProperty(PropertyInt.TotalSkillCredits);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.TotalSkillCredits); else SetProperty(PropertyInt.TotalSkillCredits, value.Value); }
        }

        public int NumDeaths
        {
            get => GetProperty(PropertyInt.NumDeaths) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.NumDeaths); else SetProperty(PropertyInt.NumDeaths, value); }
        }

        public int? DeathLevel
        {
            get => GetProperty(PropertyInt.DeathLevel);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.DeathLevel); else SetProperty(PropertyInt.DeathLevel, value.Value); }
        }

        public int? VitaeCpPool
        {
            get => GetProperty(PropertyInt.VitaeCpPool);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.VitaeCpPool); else SetProperty(PropertyInt.VitaeCpPool, value.Value); }
        }

        public bool HasVitae => EnchantmentManager.HasVitae;

        /// <summary>
        /// Will return 1.0f if no vitae exists.
        /// </summary>
        public float Vitae
        {
            get
            {
                var vitae = EnchantmentManager.GetVitae();

                if (vitae == null)
                    return 1.0f;

                return vitae.StatModValue;
            }
        }

        /// <summary>
        /// The timestamp the player originally purchased house
        /// </summary>
        public int? HousePurchaseTimestamp
        {
            get => GetProperty(PropertyInt.HousePurchaseTimestamp);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.HousePurchaseTimestamp); else SetProperty(PropertyInt.HousePurchaseTimestamp, value.Value); }
        }

        /// <summary>
        /// The timestamp when the current maintenance period ends
        /// </summary>
        public int? HouseRentTimestamp
        {
            get => GetProperty(PropertyInt.HouseRentTimestamp);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.HouseRentTimestamp); else SetProperty(PropertyInt.HouseRentTimestamp, value.Value); }
        }


        // ========================================
        // ===== Player Properties - Titles========
        // ========================================

        public int? CharacterTitleId
        {
            get => GetProperty(PropertyInt.CharacterTitleId);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.CharacterTitleId); else SetProperty(PropertyInt.CharacterTitleId, value.Value); }
        }

        public int? NumCharacterTitles
        {
            get => GetProperty(PropertyInt.NumCharacterTitles);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.NumCharacterTitles); else SetProperty(PropertyInt.NumCharacterTitles, value.Value); }
        }

        // ========================================
        // =========== Augmentations ==============
        // ========================================

        /// <summary>
        /// All of your skills are increased by 5. This augmentation cannot be repeated.
        /// </summary>
        public int AugmentationJackOfAllTrades
        {
            get => GetProperty(PropertyInt.AugmentationJackOfAllTrades) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationJackOfAllTrades); else SetProperty(PropertyInt.AugmentationJackOfAllTrades, value); }
        }

        /// <summary>
        /// Eye of the Remorseless
        /// Increases your chance of critical hits by 1%. This augmentation cannot be repeated.
        /// </summary>
        public int AugmentationCriticalExpertise
        {
            get => GetProperty(PropertyInt.AugmentationCriticalExpertise) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationCriticalExpertise); else SetProperty(PropertyInt.AugmentationCriticalExpertise, value); }
        }

        /// <summary>
        /// Iron Skin of the Invincible
        /// Increases your damage reduction rating by 3. This augmentation cannot be repeated.
        /// </summary>
        public int AugmentationDamageReduction
        {
            get => GetProperty(PropertyInt.AugmentationDamageReduction) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationDamageReduction); else SetProperty(PropertyInt.AugmentationDamageReduction, value); }
        }

        /// <summary>
        /// Critical Protection
        /// Grants you limited protection from critical hits. 25% of critical hits from creatures,
        /// and 5% critical hits from players will strike you for normal damage.
        /// This augmentation cannot be repeated.
        /// </summary>
        public int AugmentationCriticalDefense
        {
            get => GetProperty(PropertyInt.AugmentationCriticalDefense) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationCriticalDefense); else SetProperty(PropertyInt.AugmentationCriticalDefense, value); }
        }

        /// <summary>
        /// Removes the player's need to use a focus for Life Magic (Foci of Verdancy)
        /// This augmentation cannot be repeated.
        /// </summary>
        public int AugmentationInfusedLifeMagic
        {
            get => GetProperty(PropertyInt.AugmentationInfusedLifeMagic) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationInfusedLifeMagic); else SetProperty(PropertyInt.AugmentationInfusedLifeMagic, value); }
        }

        /// <summary>
        /// Hand of the Remorseless
        /// Increases critical damage by 3%. This augmentation cannot be repeated.
        /// </summary>
        public int AugmentationCriticalPower
        {
            get => GetProperty(PropertyInt.AugmentationCriticalPower) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationCriticalPower); else SetProperty(PropertyInt.AugmentationCriticalPower, value); }
        }

        /// <summary>
        /// Might of the Seventh Mule
        /// Gives the player 20% more burden-carrying capacity for each gem used (max x5)
        /// </summary>
        public int AugmentationIncreasedCarryingCapacity
        {
            get => GetProperty(PropertyInt.AugmentationIncreasedCarryingCapacity) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationIncreasedCarryingCapacity); else SetProperty(PropertyInt.AugmentationIncreasedCarryingCapacity, value); }
        }

        /// <summary>
        /// Removes the player's need to use a focus for Creature Magic (Foci of Enchantment)
        /// This augmentation cannot be repeated.
        /// </summary>
        public int AugmentationInfusedCreatureMagic
        {
            get => GetProperty(PropertyInt.AugmentationInfusedCreatureMagic) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationInfusedCreatureMagic); else SetProperty(PropertyInt.AugmentationInfusedCreatureMagic, value); }
        }

        /// <summary>
        /// Removes the player's need to use a focus for Item Magic (Foci of Artifice)
        /// This augmentation cannot be repeated.
        /// </summary>
        public int AugmentationInfusedItemMagic
        {
            get => GetProperty(PropertyInt.AugmentationInfusedItemMagic) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationInfusedItemMagic); else SetProperty(PropertyInt.AugmentationInfusedItemMagic, value); }
        }

        /// <summary>
        /// Removes the player's need to use a focus for Void Magic (Foci of Shadow)
        /// This augmentation cannot be repeated.
        /// </summary>
        public int AugmentationInfusedVoidMagic
        {
            get => GetProperty(PropertyInt.AugmentationInfusedVoidMagic) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationInfusedVoidMagic); else SetProperty(PropertyInt.AugmentationInfusedVoidMagic, value); }
        }

        /// <summary>
        /// Removes the player's need to use a focus for War Magic (Foci of Strife)
        /// This augmentation cannot be repeated.
        /// </summary>
        public int AugmentationInfusedWarMagic
        {
            get => GetProperty(PropertyInt.AugmentationInfusedWarMagic) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationInfusedWarMagic); else SetProperty(PropertyInt.AugmentationInfusedWarMagic, value); }
        }

        /// <summary>
        /// Clutch of the Miser
        /// The player loses 5 fewer items at death. (stacks 3x)
        /// If the player is killed in a PK battle, they still drop items.
        /// </summary>
        public int AugmentationLessDeathItemLoss
        {
            get => GetProperty(PropertyInt.AugmentationLessDeathItemLoss) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationLessDeathItemLoss); else SetProperty(PropertyInt.AugmentationLessDeathItemLoss, value); }
        }

        /// <summary>
        /// Enduring Enchantment
        /// The player keeps their enchantments after dying, unless killed in a PK battle.
        /// </summary>
        public int AugmentationSpellsRemainPastDeath
        {
            get => GetProperty(PropertyInt.AugmentationSpellsRemainPastDeath) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationSpellsRemainPastDeath); else SetProperty(PropertyInt.AugmentationSpellsRemainPastDeath, value); }
        }

        /// <summary>
        /// Quick Learner
        /// The player receives +5% bonus XP earned via hunting / killing creatures.
        /// This augmentation cannot be repeated.
        /// </summary>
        public int AugmentationBonusXp
        {
            get => GetProperty(PropertyInt.AugmentationBonusXp) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationBonusXp); else SetProperty(PropertyInt.AugmentationBonusXp, value); }
        }

        /// <summary>
        /// Innate Renewal
        /// The player receives a 100% bonus to vital regeneration rate
        /// The player can augment themselves twice in this way.
        /// </summary>
        public int AugmentationFasterRegen
        {
            get => GetProperty(PropertyInt.AugmentationFasterRegen) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationFasterRegen); else SetProperty(PropertyInt.AugmentationFasterRegen, value); }
        }

        // "augs" - Asheron's Lesser Benediction, Asheron's Bendiction, Blackmoor's Favor
        // these don't use PropertyInt.Augmentation*, but they are reusable enchantments

        /// <summary>
        /// Shadow of the Seventh Mule
        /// Grants the player an extra, 8th pack slot. This augmentation cannot be repeated.
        /// </summary>
        public int AugmentationExtraPackSlot
        {
            get => GetProperty(PropertyInt.AugmentationExtraPackSlot) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationExtraPackSlot); else SetProperty(PropertyInt.AugmentationExtraPackSlot, value); }
        }

        /// <summary>
        /// Reinforcement of the Lugians
        /// Grants the player 5 extra points to their innate Strength attribute
        /// This augmentation will not increase innate Strength (your Strength at character creatoin) beyond 100.
        /// You can augment each of your attributes in this way, but only ten times in combination.
        /// </summary>
        public int AugmentationInnateStrength
        {
            get => GetProperty(PropertyInt.AugmentationInnateStrength) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationInnateStrength); else SetProperty(PropertyInt.AugmentationInnateStrength, value); }
        }

        /// <summary>
        /// Bleeargh's Fortitude
        /// Grants the player 5 extra points to their innate Endurance attribute
        /// This augmentation will not increase innate Endurance (your Endurance at character creation) beyond 100.
        /// You can augment each of your attributes in this way, but only ten times in combination.
        /// </summary>
        public int AugmentationInnateEndurance
        {
            get => GetProperty(PropertyInt.AugmentationInnateEndurance) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationInnateEndurance); else SetProperty(PropertyInt.AugmentationInnateEndurance, value); }
        }

        /// <summary>
        /// Oswald's Enhancement
        /// Grants the player 5 extra points to their innate Coordination attribute
        /// This augmentation will not increase innate Coordination (your Coordination at character creation) beyond 100.
        /// You can augment each of your attributes in this way, but only ten times in combination.
        /// </summary>
        public int AugmentationInnateCoordination
        {
            get => GetProperty(PropertyInt.AugmentationInnateCoordination) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationInnateCoordination); else SetProperty(PropertyInt.AugmentationInnateCoordination, value); }
        }

        /// <summary>
        /// Siraluun's Blessing
        /// Grants the player 5 extra points to their innate Quickness attribute
        /// This augmentation will not increase innate Quickness (your Quickness at character creation) beyond 100.
        /// You can augment each of your attributes in this way, but only ten times in combination.
        /// </summary>
        public int AugmentationInnateQuickness
        {
            get => GetProperty(PropertyInt.AugmentationInnateQuickness) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationInnateQuickness); else SetProperty(PropertyInt.AugmentationInnateQuickness, value); }
        }

        /// <summary>
        /// Enduring Calm
        /// Grants the player 5 extra points to their innate Focus attribute
        /// This augmentation will not increase innate Focus (your Focus at character creation) beyond 100.
        /// You can augment each of your attributes in this way, but only ten times in combination.
        /// </summary>
        public int AugmentationInnateFocus
        {
            get => GetProperty(PropertyInt.AugmentationInnateFocus) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationInnateFocus); else SetProperty(PropertyInt.AugmentationInnateFocus, value); }
        }

        /// <summary>
        /// Steadfast Will
        /// Grants the player 5 extra points to their innate Self attribute
        /// This augmentation will not increase innate Self (your Strength at Self creation) beyond 100.
        /// You can augment each of your attributes in this way, but only ten times in combination.
        /// </summary>
        public int AugmentationInnateSelf
        {
            get => GetProperty(PropertyInt.AugmentationInnateSelf) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationInnateSelf); else SetProperty(PropertyInt.AugmentationInnateSelf, value); }
        }

        /// <summary>
        /// The number of innate attribute augs that have been applied (max 10)
        /// </summary>
        public int AugmentationInnateFamily
        {
            get => GetProperty(PropertyInt.AugmentationInnateFamily) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationInnateFamily); else SetProperty(PropertyInt.AugmentationInnateFamily, value); }
        }

        /// <summary>
        /// Enhancement of the Blade Turner
        /// Grants the player 10% extra resistance to slashing damage. You may only have 2 resistance augmentations in effect at any time.
        /// </summary>
        public int AugmentationResistanceSlash
        {
            get => GetProperty(PropertyInt.AugmentationResistanceSlash) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationResistanceSlash); else SetProperty(PropertyInt.AugmentationResistanceSlash, value); }
        }

        /// <summary>
        /// Enhancement of the Arrow Turner
        /// Grants the player 10% extra resistance to piercing damage. You may only have 2 resistance augmentations in effect at any time.
        /// </summary>
        public int AugmentationResistancePierce
        {
            get => GetProperty(PropertyInt.AugmentationResistancePierce) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationResistancePierce); else SetProperty(PropertyInt.AugmentationResistancePierce, value); }
        }

        /// <summary>
        /// Enhancement of the Mace Turner
        /// Grants the player 10% extra resistance to bludgeon damage. You may only have 2 resistance augmentations in effect at any time.
        /// </summary>
        public int AugmentationResistanceBlunt
        {
            get => GetProperty(PropertyInt.AugmentationResistanceBlunt) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationResistanceBlunt); else SetProperty(PropertyInt.AugmentationResistanceBlunt, value); }
        }

        /// <summary>
        /// Fiery Enhancement
        /// Grants the player 10% extra resistance to fire damage. You may only have 2 resistance augmentations in effect at any time.
        /// </summary>
        public int AugmentationResistanceFire
        {
            get => GetProperty(PropertyInt.AugmentationResistanceFire) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationResistanceFire); else SetProperty(PropertyInt.AugmentationResistanceFire, value); }
        }

        /// <summary>
        /// Icy Enhancement
        /// Grants the player 10% extra resistance to cold damage. You may only have 2 resistance augmentations in effect at any time.
        /// </summary>
        public int AugmentationResistanceFrost
        {
            get => GetProperty(PropertyInt.AugmentationResistanceFrost) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationResistanceFrost); else SetProperty(PropertyInt.AugmentationResistanceFrost, value); }
        }

        /// <summary>
        /// Caustic Enhancement
        /// Grants the player 10% extra resistance to acid damage. You may only have 2 resistance augmentations in effect at any time.
        /// </summary>
        public int AugmentationResistanceAcid
        {
            get => GetProperty(PropertyInt.AugmentationResistanceAcid) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationResistanceAcid); else SetProperty(PropertyInt.AugmentationResistanceAcid, value); }
        }

        /// <summary>
        /// Storm's Enhancement
        /// Grants the player 10% extra resistance to lightning damage. You may only have 2 resistance augmentations in effect at any time.
        /// </summary>
        public int AugmentationResistanceLightning
        {
            get => GetProperty(PropertyInt.AugmentationResistanceLightning) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationResistanceLightning); else SetProperty(PropertyInt.AugmentationResistanceLightning, value); }
        }

        /// <summary>
        /// The number of resistance augs that have been applied (max 2)
        /// </summary>
        public int AugmentationResistanceFamily
        {
            get => GetProperty(PropertyInt.AugmentationResistanceFamily) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationResistanceFamily); else SetProperty(PropertyInt.AugmentationResistanceFamily, value); }
        }

        /// <summary>
        /// Frenzy of the Slayer
        /// Increases the player's damage rating by 3. This augmentation cannot be repeated.
        /// </summary>
        public int AugmentationDamageBonus
        {
            get => GetProperty(PropertyInt.AugmentationDamageBonus) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationDamageBonus); else SetProperty(PropertyInt.AugmentationDamageBonus, value); }
        }

        /// <summary>
        /// Ciandra's Fortune
        /// The player receives 25% more material from salavaging (max 4x)
        /// </summary>
        public int AugmentationBonusSalvage
        {
            get => GetProperty(PropertyInt.AugmentationBonusSalvage) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationBonusSalvage); else SetProperty(PropertyInt.AugmentationBonusSalvage, value); }
        }

        /// <summary>
        /// Charmed Smith
        /// The player has 5% increased chance to succeed when imbuing items.
        /// This augmentation cannot be repeated.
        /// </summary>
        public int AugmentationBonusImbueChance
        {
            get => GetProperty(PropertyInt.AugmentationBonusImbueChance) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationBonusImbueChance); else SetProperty(PropertyInt.AugmentationBonusImbueChance, value); }
        }

        /// <summary>
        /// Jibril's Essence
        /// Using this gem will specialize your skill in Armor Tinkering and raise your skill points accordingly.
        /// Once specialized, you will not be able to unspecialize or untrain Armor Tinkering.
        /// You must have this skill Trained in order to use this gem.
        /// This augmentation cannot be repeated.
        /// </summary>
        public int AugmentationSpecializeArmorTinkering
        {
            get => GetProperty(PropertyInt.AugmentationSpecializeArmorTinkering) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationSpecializeArmorTinkering); else SetProperty(PropertyInt.AugmentationSpecializeArmorTinkering, value); }
        }

        /// <summary>
        /// Yoshi's Essence
        /// Using this gem will specialize your skill in Item Tinkering and raise your skill points accordingly.
        /// Once specialized, you will not be able to unspecialize or untrain Item Tinkering.
        /// You must have this skill Trained in order to use this gem.
        /// This augmentation cannot be repeated.
        /// </summary>
        public int AugmentationSpecializeItemTinkering
        {
            get => GetProperty(PropertyInt.AugmentationSpecializeItemTinkering) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationSpecializeItemTinkering); else SetProperty(PropertyInt.AugmentationSpecializeItemTinkering, value); }
        }

        /// <summary>
        /// Celdiseth's Essence
        /// Using this gem will specialize your skill in Magic Item Tinkering and raise your skill points accordingly.
        /// Once specialized, you will not be able to unspecialize or untrain Magic Item Tinkering.
        /// You must have this skill Trained in order to use this gem.
        /// This augmentation cannot be repeated.
        /// </summary>
        public int AugmentationSpecializeMagicItemTinkering
        {
            get => GetProperty(PropertyInt.AugmentationSpecializeMagicItemTinkering) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationSpecializeMagicItemTinkering); else SetProperty(PropertyInt.AugmentationSpecializeMagicItemTinkering, value); }
        }

        /// <summary>
        /// Koga's Essence
        /// Using this gem will specialize your skill in Weapon Tinkering and raise your skill points accordingly.
        /// Once specialized, you will not be able to unspecialize or untrain Weapon Tinkering.
        /// You must have this skill Trained in order to use this gem.
        /// This augmentation cannot be repeated.
        /// </summary>
        public int AugmentationSpecializeWeaponTinkering
        {
            get => GetProperty(PropertyInt.AugmentationSpecializeWeaponTinkering) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationSpecializeWeaponTinkering); else SetProperty(PropertyInt.AugmentationSpecializeWeaponTinkering, value); }
        }

        /// <summary>
        /// Ciandra's Essence
        /// Using this gem will specialize your skill in Salvaging and raise your skill points accordingly.
        /// Once specialized, you will not be able to unspecialize or untrain Salvaging.
        /// You must have this skill Trained in order to use this gem.
        /// This augmentation cannot be repeated.
        /// </summary>
        public int AugmentationSpecializeSalvaging
        {
            get => GetProperty(PropertyInt.AugmentationSpecializeSalvaging) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationSpecializeSalvaging); else SetProperty(PropertyInt.AugmentationSpecializeSalvaging, value); }
        }

        /// <summary>
        /// Master of the Steel Circle
        /// Your effective melee skill when using any melee weapon is increased by +10.
        /// This augmentation cannot be repeated.
        /// </summary>
        public int AugmentationSkilledMelee
        {
            get => GetProperty(PropertyInt.AugmentationSkilledMelee) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationSkilledMelee); else SetProperty(PropertyInt.AugmentationSkilledMelee, value); }
        }

        /// <summary>
        /// Master of the Five Fold Path
        /// Your effective magic skill when using any magic weapon is increased by +10.
        /// This augmentation cannot be repeated.
        /// </summary>
        public int AugmentationSkilledMagic
        {
            get => GetProperty(PropertyInt.AugmentationSkilledMagic) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationSkilledMagic); else SetProperty(PropertyInt.AugmentationSkilledMagic, value); }
        }

        /// <summary>
        /// Master of the Focused Eye
        /// Your effective missile skill when using any missile weapon is increased by +10.
        /// This augmentation cannot be repeated.
        /// </summary>
        public int AugmentationSkilledMissile
        {
            get => GetProperty(PropertyInt.AugmentationSkilledMissile) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationSkilledMissile); else SetProperty(PropertyInt.AugmentationSkilledMissile, value); }
        }

        /// <summary>
        /// Archmage's Endurance
        /// +20% spell duration (max 5 stacks)
        /// </summary>
        public int AugmentationIncreasedSpellDuration
        {
            get => GetProperty(PropertyInt.AugmentationIncreasedSpellDuration) ?? 0;
            set { if (value == 0) RemoveProperty(PropertyInt.AugmentationIncreasedSpellDuration); else SetProperty(PropertyInt.AugmentationIncreasedSpellDuration, value); }
        }
    }
}
