using System;
using System.Collections.Generic;
using System.Diagnostics;

using ACE.Database;
using ACE.DatLoader;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;
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
        public bool SpecializeSkill(Skill skill, int creditsSpent, bool resetSkill = true)
        {
            var cs = GetCreatureSkill(skill);

            if (cs.AdvancementClass == SkillAdvancementClass.Trained)
            {
                if (AvailableSkillCredits >= creditsSpent)
                {
                    if (resetSkill)
                    {
                        cs.Ranks = 0;
                        cs.ExperienceSpent = 0;
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
        public void AwardSkillPoints(Skill skill, uint amount, bool usage = false)
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

                RaiseSkillGameAction(skill, xpToRank, usage);
            }
        }

        /// <summary>
        /// Increases a skill from the 'Raise skill' buttons, or through natural usage
        /// </summary>
        public void RaiseSkillGameAction(Skill skill, uint amount, bool usage = false)
        {
            var creatureSkill = GetCreatureSkill(skill);

            var prevRank = creatureSkill.Ranks;
            var prevXP = creatureSkill.ExperienceSpent;

            uint result = SpendSkillXp(creatureSkill, amount, usage);

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
            }
            else if (prevXP != creatureSkill.ExperienceSpent)
            {
                // skill usage
                Session.Network.EnqueueSend(new GameMessagePrivateUpdateSkill(this, creatureSkill));
            }
            else if (!usage)
            {
                messageText = $"Your attempt to raise {skill} has failed!";
                Session.Network.EnqueueSend(new GameMessageSystemChat(messageText, ChatMessageType.Advancement));
            }
        }

        /// <summary>
        /// Adds experience points to a skill
        /// </summary>
        /// <remarks>
        ///     Known Issues:
        ///         1. Earned XP usage in ranks besides 1 or 10 need to be accounted for.
        /// </remarks>
        /// <returns>0 if it failed, total skill experience if successful</returns>
        private uint SpendSkillXp(CreatureSkill skill, uint amount, bool usage = false, bool sendNetworkPropertyUpdate = true)
        {
            uint result = 0u;

            var xpList = GetXPTable(skill.AdvancementClass);
            if (xpList == null) return result;

            // do not advance if we cannot spend xp to rank up our skill by 1 point
            if (skill.Ranks >= (xpList.Count - 1))
                return result;

            ushort rankUps = 0;
            uint currentRankXp = skill.ExperienceSpent;
            uint rank1 = xpList[Convert.ToInt32(skill.Ranks) + 1] - currentRankXp;
            uint rank10;
            ushort rank10Offset = 0;

            if (skill.Ranks + 10 >= (xpList.Count))
            {
                rank10Offset = (ushort)(10 - ((skill.Ranks + 10) - (xpList.Count - 1)));
                rank10 = xpList[skill.Ranks + rank10Offset] - currentRankXp;
            }
            else
            {
                rank10 = xpList[skill.Ranks + 10] - currentRankXp;
            }

            if (amount >= rank10)
            {
                if (rank10Offset > 0)
                    rankUps = rank10Offset;
                else
                    rankUps = 10;
            }
            else if (amount >= rank1)
                rankUps = 1;
            
            if (!usage)
            {
                if (SpendXP(amount, sendNetworkPropertyUpdate))
                {
                    if (rankUps > 0)
                        skill.Ranks += rankUps;

                    skill.ExperienceSpent += amount;
                    result = skill.ExperienceSpent;
                }
            }
            else
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

                if (SpendSkillXp(skill, rank10, false, sendNetworkPropertyUpdate) == 0)
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

            RaiseSkillGameAction(skill, amount, true);
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
        public List<uint> GetXPTable(SkillAdvancementClass status)
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

        public Skill ConvertToMoASkill(Skill skill)
        {
            if (this is Player player)
            {
                if (SkillExtensions.RetiredMelee.Contains(skill))
                    return player.GetHighestMeleeSkill();
                if (SkillExtensions.RetiredMissile.Contains(skill))
                    return Skill.MissileWeapons;
            }

            return skill;
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

        public static Dictionary<HeritageGroup, List<Skill>> HeritageBonuses = new Dictionary<HeritageGroup, List<Skill>>
        {
            // contains a bunch of outdated skills, according to heritage select screen description?
            { ACE.Entity.Enum.HeritageGroup.Aluvian, new List<Skill> { Skill.Dagger, Skill.Bow } },
            { ACE.Entity.Enum.HeritageGroup.Gharundim, new List<Skill> { Skill.Staff, Skill.WarMagic } }, // magic spells?
            { ACE.Entity.Enum.HeritageGroup.Sho, new List<Skill> { Skill.UnarmedCombat, Skill.Bow } },
            { ACE.Entity.Enum.HeritageGroup.Viamontian, new List<Skill> { Skill.Sword, Skill.Crossbow } },
            { ACE.Entity.Enum.HeritageGroup.Shadowbound, new List<Skill> { Skill.UnarmedCombat, Skill.Crossbow } }, // umbraen?
            { ACE.Entity.Enum.HeritageGroup.Penumbraen, new List<Skill> { Skill.UnarmedCombat, Skill.Crossbow } },
            { ACE.Entity.Enum.HeritageGroup.Gearknight, new List<Skill> { Skill.Mace, Skill.Crossbow } },
            { ACE.Entity.Enum.HeritageGroup.Undead, new List<Skill> { Skill.Axe, Skill.ThrownWeapon } },
            { ACE.Entity.Enum.HeritageGroup.Empyrean, new List<Skill> { Skill.Sword, Skill.WarMagic } },  // magic?
            { ACE.Entity.Enum.HeritageGroup.Tumerok, new List<Skill> { Skill.Spear, Skill.ThrownWeapon } },
            { ACE.Entity.Enum.HeritageGroup.Lugian, new List<Skill> { Skill.Axe, Skill.ThrownWeapon } },
            { ACE.Entity.Enum.HeritageGroup.Olthoi, new List<Skill>() },    // natural claws and pincers?
            { ACE.Entity.Enum.HeritageGroup.OlthoiAcid, new List<Skill>() }    // olthoi spitters acidic spit?
        };
    }
}
