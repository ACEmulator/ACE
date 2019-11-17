using System;
using System.Collections.Generic;

using ACE.Database;
using ACE.DatLoader;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects.Entity;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        /// <summary>
        /// Sets the skill to trained status for a character
        /// </summary>
        public bool TrainSkill(Skill skill, int creditsSpent)
        {
            var cs = GetCreatureSkill(skill);

            if (cs.AdvancementClass != SkillAdvancementClass.Trained && cs.AdvancementClass != SkillAdvancementClass.Specialized)
            {
                if (AvailableSkillCredits >= creditsSpent)
                {
                    cs.AdvancementClass = SkillAdvancementClass.Trained;
                    cs.Ranks = 0;
                    cs.ExperienceSpent = 0;
                    cs.InitLevel += 5;
                    AvailableSkillCredits -= creditsSpent;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Public method for adding a new skill by spending skill credits.
        /// </summary>
        /// <remarks>
        ///  The client will throw up more then one train skill dialog and the user has the chance to spend twice.
        /// </remarks>
        public void HandleActionTrainSkill(Skill skill, int creditsSpent)
        {
            if (AvailableSkillCredits >= creditsSpent)
            {
                // attempt to train the specified skill
                bool trainNewSkill = TrainSkill(skill, creditsSpent);

                // create an update to send to the client
                var currentCredits = new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.AvailableSkillCredits, AvailableSkillCredits ?? 0);

                // as long as the skill is sent, the train new triangle button on the client will not lock up.
                // Sending Skill.None with status untrained worked in test
                var trainSkillUpdate = new GameMessagePrivateUpdateSkill(this, Skill.None, SkillAdvancementClass.Untrained, 0, 0, 0);
                // create a string placeholder for the correct after
                string trainSkillMessageText;

                // if the skill has already been trained or we do not have enough credits, then trainNewSkill be set false
                if (trainNewSkill)
                {
                    // replace the trainSkillUpdate message with the correct skill assignment:
                    var creatureSkill = GetCreatureSkill(skill);
                    trainSkillUpdate = new GameMessagePrivateUpdateSkill(this, creatureSkill);
                    trainSkillMessageText = $"{skill.ToSentence()} trained. You now have {AvailableSkillCredits} credits available.";
                }
                else
                {
                    trainSkillMessageText = $"Failed to train {skill.ToSentence()}! You now have {AvailableSkillCredits} credits available.";
                }

                // create the final game message and send to the client
                var message = new GameMessageSystemChat(trainSkillMessageText, ChatMessageType.Advancement);
                Session.Network.EnqueueSend(trainSkillUpdate, currentCredits, message);
            }
        }

        /// <summary>
        /// Sets the skill to specialized status
        /// </summary>
        /// <param name="resetSkill">only set to TRUE during character creation. set to FALSE during temple / asheron's castle</param>
        public bool SpecializeSkill(Skill skill, int creditsSpent, bool resetSkill = true)
        {
            var cs = GetCreatureSkill(skill);

            if (cs.AdvancementClass == SkillAdvancementClass.Trained)
            {
                if (AvailableSkillCredits >= creditsSpent)
                {
                    if (resetSkill)
                    {
                        // this path for char creation?
                        cs.Ranks = 0;
                        cs.ExperienceSpent = 0;
                    }
                    else
                    {
                        // this path for temple spec?
                        cs.Ranks = (ushort)CalcSkillRank(SkillAdvancementClass.Specialized, cs.ExperienceSpent);
                    }

                    cs.InitLevel += 5;
                    cs.AdvancementClass = SkillAdvancementClass.Specialized;
                    AvailableSkillCredits -= creditsSpent;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Sets the skill to untrained status
        /// </summary>
        public bool UntrainSkill(Skill skill, int creditsSpent)
        {
            var cs = GetCreatureSkill(skill);

            if (cs == null)
                return false;

            if (cs.AdvancementClass != SkillAdvancementClass.Trained && cs.AdvancementClass != SkillAdvancementClass.Specialized)
            {
                // only used to initialize untrained skills for character creation?
                cs.AdvancementClass = SkillAdvancementClass.Untrained;
                cs.InitLevel = 0;
                cs.Ranks = 0;
                cs.ExperienceSpent = 0;
                return true;
            }

            if (cs.AdvancementClass == SkillAdvancementClass.Trained) 
            {
                //Perform refund of XP and credits
                RefundXP(cs.ExperienceSpent);

                // temple untraining heritage skills:
                // heritage skills cannot be untrained, but skill XP can be recovered
                if (IsSkillUntrainable(skill))
                {
                    cs.AdvancementClass = SkillAdvancementClass.Untrained;
                    cs.InitLevel -= 5;
                    AvailableSkillCredits += creditsSpent;
                }

                cs.Ranks = 0;
                cs.ExperienceSpent = 0;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Lowers skill from Specialized to Trained and returns both skill credits and invested XP
        /// </summary>
        public bool UnspecializeSkill(Skill skill, int creditsSpent)
        {
            var cs = GetCreatureSkill(skill);

            if (cs == null)
                return false;

            if (cs.AdvancementClass == SkillAdvancementClass.Specialized)
            {
                //Perform refund of XP and credits
                RefundXP(cs.ExperienceSpent);
                AvailableSkillCredits += creditsSpent;

                cs.AdvancementClass = SkillAdvancementClass.Trained;
                cs.InitLevel -= 5;
                cs.ExperienceSpent = 0;
                cs.Ranks = 0;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Increases a skill by some amount of points
        /// </summary>
        public void AwardSkillPoints(Skill skill, uint amount)
        {
            var creatureSkill = GetCreatureSkill(skill);

            for (var i = 0; i < amount; i++)
            {
                if (IsSkillMaxRank(creatureSkill.Ranks, creatureSkill.AdvancementClass))
                    return;

                // get skill xp required for next rank
                var xpToRank = GetXpToNextRank(creatureSkill);
                if (xpToRank == uint.MaxValue)
                    return;

                AwardSkillXP(skill, xpToRank);
            }
        }

        /// <summary>
        /// Wrapper method used for increasing totalXP and then using the amount granted by RaiseSkillGameAction
        /// </summary>
        /// <param name="skill"></param>
        /// <param name="amount"></param>
        public void AwardSkillXP(Skill skill, uint amount)
        {
            var playerSkill = GetCreatureSkill(skill);

            if (!IsSkillMaxRank(playerSkill.Ranks, playerSkill.AdvancementClass))
            {
                GrantXP(amount, XpType.Emote, ShareType.None);
                RaiseSkillGameAction(skill, amount);
            }
        }

        /// <summary>
        /// Increases a skill from the 'Raise skill' buttons, or through natural usage
        /// </summary>
        public void RaiseSkillGameAction(Skill skill, uint amount)
        {
            var creatureSkill = GetCreatureSkill(skill);

            var prevRank = creatureSkill.Ranks;
            var prevXP = creatureSkill.ExperienceSpent;

            uint result = SpendSkillXp(creatureSkill, amount);

            string messageText;

            if (prevRank != creatureSkill.Ranks)
            {
                // if the skill ranks out at the top of our xp chart
                // then we will start fireworks effects and have special text!
                if (IsSkillMaxRank(creatureSkill.Ranks, creatureSkill.AdvancementClass))
                {
                    // fireworks on rank up is 0x8D
                    PlayParticleEffect(ACE.Entity.Enum.PlayScript.WeddingBliss, Guid);
                    messageText = $"Your base {skill.ToSentence()} is now {creatureSkill.Base} and has reached its upper limit!";
                }
                else
                {
                    messageText = $"Your base {skill.ToSentence()} is now {creatureSkill.Base}!";
                }
                Session.Network.EnqueueSend(new GameMessagePrivateUpdateSkill(this, creatureSkill));
                Session.Network.EnqueueSend(new GameMessageSound(Guid, Sound.RaiseTrait, 1f));
                Session.Network.EnqueueSend(new GameMessageSystemChat(messageText, ChatMessageType.Advancement));

                // retail was missing the 'raise skill' runrate hook here
                if (skill == Skill.Run && PropertyManager.GetBool("runrate_add_hooks").Item)
                    HandleRunRateUpdate();
            }
            else if (prevXP != creatureSkill.ExperienceSpent)
            {
                // skill usage
                Session.Network.EnqueueSend(new GameMessagePrivateUpdateSkill(this, creatureSkill));
            }
        }

        /// <summary>
        /// Adds experience points to a skill
        /// </summary>
        /// <returns>0 if it failed, total skill experience if successful</returns>
        private uint SpendSkillXp(CreatureSkill skill, uint amount, bool sendNetworkPropertyUpdate = true)
        {
            uint result = 0u;

            var xpList = GetXPTable(skill.AdvancementClass);
            if (xpList == null) return result;

            // do not advance if we cannot spend xp to rank up our skill by 1 point
            if (skill.Ranks >= (xpList.Count - 1))
                return result;

            ushort rankUps = (ushort)(Player.CalcSkillRank(skill.AdvancementClass, skill.ExperienceSpent + amount) - skill.Ranks);

            if (SpendXP(amount, sendNetworkPropertyUpdate))
            {
                if (rankUps > 0)
                    skill.Ranks += rankUps;

                skill.ExperienceSpent += amount;
                result = skill.ExperienceSpent;
            }

            return result;
        }

        public void SpendAllAvailableSkillXp(CreatureSkill skill, bool sendNetworkPropertyUpdate = true)
        {
            var xpList = GetXPTable(skill.AdvancementClass);

            if (xpList == null)
                return;

            while (true)
            {
                uint currentRankXp = xpList[Convert.ToInt32(skill.Ranks)];
                uint rank10;

                if (skill.Ranks + 10 >= (xpList.Count))
                {
                    var rank10Offset = 10 - (Convert.ToInt32(skill.Ranks + 10) - (xpList.Count - 1));
                    rank10 = xpList[Convert.ToInt32(skill.Ranks) + rank10Offset] - currentRankXp;
                }
                else
                {
                    rank10 = xpList[Convert.ToInt32(skill.Ranks) + 10] - currentRankXp;
                }

                if (SpendSkillXp(skill, rank10, sendNetworkPropertyUpdate) == 0)
                    break;
            }
        }

        /// <summary>
        /// Grants skill XP proportional to the player's skill level
        /// </summary>
        public void GrantLevelProportionalSkillXP(Skill skill, double percent, ulong max)
        {
            var creatureSkill = GetCreatureSkill(skill);
            if (IsSkillMaxRank(creatureSkill.Ranks, creatureSkill.AdvancementClass))
                return;

            var nextLevelXP = GetXPBetweenSkillLevels(creatureSkill.AdvancementClass, creatureSkill.Ranks, creatureSkill.Ranks + 1).Value;
            var amount = (uint)Math.Min(nextLevelXP * percent, max);

            AwardSkillXP(skill, amount);
        }

        /// <summary>
        /// Returns the remaining XP required to the next skill level
        /// </summary>
        public uint GetXpToNextRank(CreatureSkill skill)
        {
            var xpList = GetXPTable(skill.AdvancementClass);
            if (xpList != null)
                return xpList[Convert.ToInt32(skill.Ranks) + 1] - skill.ExperienceSpent;
            else
                return uint.MaxValue;
        }

        /// <summary>
        /// Returns the XP curve table based on trained or specialized skill
        /// </summary>
        public static List<uint> GetXPTable(SkillAdvancementClass status)
        {
            var xpTable = DatManager.PortalDat.XpTable;
            if (status == SkillAdvancementClass.Trained)
                return xpTable.TrainedSkillXpList;
            if (status == SkillAdvancementClass.Specialized)
                return xpTable.SpecializedSkillXpList;
            return null;
        }

        /// <summary>
        /// Returns the XP required to go between skill level A and skill level B
        /// </summary>
        public ulong? GetXPBetweenSkillLevels(SkillAdvancementClass status, int levelA, int levelB)
        {
            var xpTable = GetXPTable(status);
            if (xpTable == null) return null;
            return xpTable[levelB + 1] - xpTable[levelA + 1];
        }

        /// <summary>
        /// Check a rank against the skill charts too determine if the skill is at max
        /// </summary>
        /// <returns>Returns true if skill is max rank; false if skill is below max rank</returns>
        private bool IsSkillMaxRank(uint rank, SkillAdvancementClass status)
        {
            var xpList = GetXPTable(status);
            if (xpList == null)
                throw new Exception();  // return false?

            if (rank == (xpList.Count - 1))
                return true;

            return false;
        }

        /// <summary>
        /// Returns the maximum rank that can be purchased with an xp amount
        /// </summary>
        /// <param name="sac">Trained or specialized skill</param>
        /// <param name="xpAmount">The amount of xp used to make the purchase</param>
        public static int CalcSkillRank(SkillAdvancementClass sac, uint xpAmount)
        {
            var rankXpTable = GetXPTable(sac);
            for (var i = rankXpTable.Count - 1; i >= 0; i--)
            {
                var rankAmount = rankXpTable[i];
                if (xpAmount >= rankAmount)
                    return i;
            }
            return -1;
        }

        private const uint magicSkillCheckMargin = 50;

        public bool CanReadScroll(Scroll scroll)
        {
            var power = scroll.Spell.Power;

            // level 1/7/8 scrolls can be learned by anyone?
            if (power < 50 || power >= 300) return true;

            var magicSkill = scroll.Spell.GetMagicSkill();
            var playerSkill = GetCreatureSkill(magicSkill);

            var minSkill = power - magicSkillCheckMargin;

            return playerSkill.AdvancementClass >= SkillAdvancementClass.Trained && playerSkill.Current >= minSkill;
        }

        public void AddSkillCredits(int amount, bool showText)
        {
            TotalSkillCredits += amount;
            AvailableSkillCredits += amount;

            Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.AvailableSkillCredits, AvailableSkillCredits ?? 0));

            if (showText)
            {
                var message = string.Format("You have earned {0} skill credit{1}!", amount, amount == 1 ? "" : "s");
                Session.Network.EnqueueSend(new GameMessageSystemChat(message, ChatMessageType.Advancement));
                Session.Network.EnqueueSend(new GameMessageSound(Guid, Sound.RaiseTrait, 1f));
            }
        }

        /// <summary>
        /// Called on player login
        /// If a player has any skills trained that require updates from ACE-World-16-Patches,
        /// ensure these updates are installed, and if they aren't, send a helpful message to player with instructions for installation
        /// </summary>
        public void HandleDBUpdates()
        {
            // dirty fighting
            var dfSkill = GetCreatureSkill(Skill.DirtyFighting);
            if (dfSkill.AdvancementClass >= SkillAdvancementClass.Trained)
            {
                foreach (var spellID in SpellExtensions.DirtyFightingSpells)
                {
                    var spell = new Server.Entity.Spell(spellID);
                    if (spell.NotFound)
                    {
                        var actionChain = new ActionChain();
                        actionChain.AddDelaySeconds(3.0f);
                        actionChain.AddAction(this, () =>
                        {
                            Session.Network.EnqueueSend(new GameMessageSystemChat("To install Dirty Fighting, please apply the latest patches from https://github.com/ACEmulator/ACE-World-16PY-Patches", ChatMessageType.Broadcast));
                        });
                        actionChain.EnqueueChain();
                    }
                    break;  // performance improvement: only check first spell
                }
            }

            // void magic
            var voidSkill = GetCreatureSkill(Skill.VoidMagic);
            if (voidSkill.AdvancementClass >= SkillAdvancementClass.Trained)
            {
                foreach (var spellID in SpellExtensions.VoidMagicSpells)
                {
                    var spell = new Server.Entity.Spell(spellID);
                    if (spell.NotFound)
                    {
                        var actionChain = new ActionChain();
                        actionChain.AddDelaySeconds(3.0f);
                        actionChain.AddAction(this, () =>
                        {
                            Session.Network.EnqueueSend(new GameMessageSystemChat("To install Void Magic, please apply the latest patches from https://github.com/ACEmulator/ACE-World-16PY-Patches", ChatMessageType.Broadcast));
                        });
                        actionChain.EnqueueChain();
                    }
                    break;  // performance improvement: only check first spell (measured 102ms to check 75 uncached void spells)
                }
            }

            // summoning
            var summoning = GetCreatureSkill(Skill.Summoning);
            if (summoning.AdvancementClass >= SkillAdvancementClass.Trained)
            {
                uint essenceWCID = 48878;
                var weenie = DatabaseManager.World.GetCachedWeenie(essenceWCID);
                if (weenie == null)
                {
                    var actionChain = new ActionChain();
                    actionChain.AddDelaySeconds(3.0f);
                    actionChain.AddAction(this, () =>
                    {
                        Session.Network.EnqueueSend(new GameMessageSystemChat("To install Summoning, please apply the latest patches from https://github.com/ACEmulator/ACE-World-16PY-Patches", ChatMessageType.Broadcast));
                    });
                    actionChain.EnqueueChain();
                }
            }
        }

        public static HashSet<Skill> MeleeSkills = new HashSet<Skill>()
        {
            Skill.LightWeapons,
            Skill.HeavyWeapons,
            Skill.FinesseWeapons,
            Skill.DualWield,
            Skill.TwoHandedCombat,

            // legacy
            Skill.Axe,
            Skill.Dagger,
            Skill.Mace,
            Skill.Spear,
            Skill.Staff,
            Skill.Sword,
            Skill.UnarmedCombat
        };

        public static HashSet<Skill> MissileSkills = new HashSet<Skill>()
        {
            Skill.MissileWeapons,

            // legacy
            Skill.Bow,
            Skill.Crossbow,
            Skill.Sling,
            Skill.ThrownWeapon
        };

        public static HashSet<Skill> MagicSkills = new HashSet<Skill>()
        {
            Skill.CreatureEnchantment,
            Skill.ItemEnchantment,
            Skill.LifeMagic,
            Skill.VoidMagic,
            Skill.WarMagic
        };

        public static List<Skill> AlwaysTrained = new List<Skill>()
        {
            Skill.ArcaneLore,
            Skill.Jump,
            Skill.Loyalty,
            Skill.MagicDefense,
            Skill.Run,
            Skill.Salvaging
        };

        public static bool IsSkillUntrainable(Skill skill)
        {
            return !AlwaysTrained.Contains(skill);
        }


        public override bool GetHeritageBonus(WorldObject weapon)
        {
            if (weapon == null || !weapon.IsMasterable)
                return false;

            if (PropertyManager.GetBool("universal_masteries").Item)
            {
                // https://asheron.fandom.com/wiki/Spring_2014_Update
                // end of retail - universal masteries
                return true;
            }
            else
                return GetHeritageBonus(GetWeaponType(weapon));
        }

        public bool GetHeritageBonus(WeaponType weaponType)
        {
            switch (HeritageGroup)
            {
                case HeritageGroup.Aluvian:
                    if (weaponType == WeaponType.Dagger || weaponType == WeaponType.Bow)
                        return true;
                    break;
                case HeritageGroup.Gharundim:
                    if (weaponType == WeaponType.Staff || weaponType == WeaponType.Magic)
                        return true;
                    break;
                case HeritageGroup.Sho:
                    if (weaponType == WeaponType.Unarmed || weaponType == WeaponType.Bow)
                        return true;
                    break;
                case HeritageGroup.Viamontian:
                    if (weaponType == WeaponType.Sword || weaponType == WeaponType.Crossbow)
                        return true;
                    break;
                case HeritageGroup.Shadowbound: // umbraen
                case HeritageGroup.Penumbraen:
                    if (weaponType == WeaponType.Unarmed || weaponType == WeaponType.Crossbow)
                        return true;
                    break;
                case HeritageGroup.Gearknight:
                    if (weaponType == WeaponType.Mace || weaponType == WeaponType.Crossbow)
                        return true;
                    break;
                case HeritageGroup.Undead:
                    if (weaponType == WeaponType.Axe || weaponType == WeaponType.Thrown)
                        return true;
                    break;
                case HeritageGroup.Empyrean:
                    if (weaponType == WeaponType.Sword || weaponType == WeaponType.Magic)
                        return true;
                    break;
                case HeritageGroup.Tumerok:
                    if (weaponType == WeaponType.Spear || weaponType == WeaponType.Thrown)
                        return true;
                    break;
                case HeritageGroup.Lugian:
                    if (weaponType == WeaponType.Axe || weaponType == WeaponType.Thrown)
                        return true;
                    break;
                case HeritageGroup.Olthoi:
                case HeritageGroup.OlthoiAcid:
                    break;
            }
            return false;
        }

        /// <summary>
        /// If the WeaponType is missing from a weapon, tries to convert from WeaponSkill (for old data)
        /// </summary>
        public WeaponType GetWeaponType(WorldObject weapon)
        {
            if (weapon == null)
                return WeaponType.Undef;    // unarmed?

            if (weapon is Caster)
                return WeaponType.Magic;

            var weaponType = weapon.GetProperty(PropertyInt.WeaponType);
            if (weaponType != null)
                return (WeaponType)weaponType;

            var weaponSkill = weapon.GetProperty(PropertyInt.WeaponSkill);
            if (weaponSkill != null && SkillToWeaponType.TryGetValue((Skill)weaponSkill, out WeaponType converted))
                return converted;
            else
                return WeaponType.Undef;
        }

        public Dictionary<Skill, WeaponType> SkillToWeaponType = new Dictionary<Skill, WeaponType>()
        {
            { Skill.UnarmedCombat, WeaponType.Unarmed },
            { Skill.Sword, WeaponType.Sword },
            { Skill.Axe, WeaponType.Axe },
            { Skill.Mace, WeaponType.Mace },
            { Skill.Spear, WeaponType.Spear },
            { Skill.Dagger, WeaponType.Dagger },
            { Skill.Staff, WeaponType.Staff },
            { Skill.Bow, WeaponType.Bow },
            { Skill.Crossbow, WeaponType.Crossbow },
            { Skill.ThrownWeapon, WeaponType.Thrown },
            { Skill.TwoHandedCombat, WeaponType.TwoHanded },
            { Skill.CreatureEnchantment, WeaponType.Magic },    // only for war/void?
            { Skill.ItemEnchantment, WeaponType.Magic },
            { Skill.LifeMagic, WeaponType.Magic },
            { Skill.WarMagic, WeaponType.Magic },
            { Skill.VoidMagic, WeaponType.Magic },
        };

        /// <summary>
        /// Resets the skill, refunds all experience and skill credits, if allowed.
        /// </summary>
        public bool ResetSkill(Skill skillToBeReset)
        {
            var cs = GetCreatureSkill(skillToBeReset);

            //Check to make sure we got a valid skill back
            if (cs == null)
                return false;

            //Gather costs associated with manipulating currently selected skill
            DatManager.PortalDat.SkillTable.SkillBaseHash.TryGetValue((uint)cs.Skill, out var skill);

            if (skill == null)
                return false;

            var skillRemoved = false;
            var skillUntrainable = IsSkillUntrainable(skillToBeReset);
            var typeOfSkill = "";

            if (cs.AdvancementClass == SkillAdvancementClass.Untrained)
            {
                if (cs.Ranks == 0 && cs.ExperienceSpent == 0)
                    return false;
            }

            // salvage / tinkering skills specialized via augmentations
            // cannot be untrained or unspecialized
            bool specAug = false;

            switch (cs.Skill)
            {
                case Skill.ArmorTinkering:
                    specAug = AugmentationSpecializeArmorTinkering > 0;
                    break;

                case Skill.ItemTinkering:
                    specAug = AugmentationSpecializeItemTinkering > 0;
                    break;

                case Skill.MagicItemTinkering:
                    specAug = AugmentationSpecializeMagicItemTinkering > 0;
                    break;

                case Skill.WeaponTinkering:
                    specAug = AugmentationSpecializeWeaponTinkering > 0;
                    break;

                case Skill.Salvaging:
                    specAug = AugmentationSpecializeSalvaging > 0;
                    break;
            }

            if (specAug)
                return false;

            if (cs.AdvancementClass == SkillAdvancementClass.Trained || cs.AdvancementClass == SkillAdvancementClass.Specialized)
            {
                if (cs.AdvancementClass == SkillAdvancementClass.Specialized)
                {
                    typeOfSkill = cs.AdvancementClass.ToString().ToLower() + " ";
                    skillRemoved = true;
                    cs.AdvancementClass = SkillAdvancementClass.Trained;
                    cs.InitLevel -= 5;
                    AvailableSkillCredits += skill.UpgradeCostFromTrainedToSpecialized;
                }

                // temple untraining heritage skills:
                // heritage skills cannot be untrained, but skill XP can be recovered
                if (skillUntrainable)
                {
                    typeOfSkill = cs.AdvancementClass.ToString().ToLower() + " ";
                    skillRemoved = true;
                    cs.AdvancementClass = SkillAdvancementClass.Untrained;
                    cs.InitLevel -= 5;
                    AvailableSkillCredits += skill.TrainedCost;
                }

                //Perform refund of XP and credits
                RefundXP(cs.ExperienceSpent);

                cs.ExperienceSpent = 0;
                cs.Ranks = 0;

                Session.Network.EnqueueSend(new GameMessagePrivateUpdateSkill(this, cs));

                Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.AvailableSkillCredits, AvailableSkillCredits ?? 0));

                Session.Network.EnqueueSend(new GameMessageSystemChat($"Your {typeOfSkill}{cs.Skill.ToSentence()} skill has been {(skillRemoved ? "removed" : "reset")}. All the experience {(skillRemoved ? "and skill credits " : "")}that you spent on this skill have been refunded to you.", ChatMessageType.Broadcast));

                return true;
            }

            return false;
        }
    }
}
