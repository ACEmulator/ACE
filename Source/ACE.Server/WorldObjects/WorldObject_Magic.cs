using System;
using System.Numerics;

using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.DatLoader.Entity;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Structure;
using ACE.Server.Managers;
using ACE.Server.Factories;
using ACE.Server.Physics;

namespace ACE.Server.WorldObjects
{
    partial class WorldObject
    {
        public enum SpellLevel
        {
            One = 1,
            Two = 50,
            Three = 100,
            Four = 150,
            Five = 200,
            Six = 250,
            Seven = 300,
            Eight = 350
        }
        protected static SpellLevel CalculateSpellLevel(uint spellPower)
        {
            if (spellPower < 50)
                return SpellLevel.One;
            else if (spellPower < 100)
                return SpellLevel.Two;
            else if (spellPower < 150)
                return SpellLevel.Three;
            else if (spellPower < 200)
                return SpellLevel.Four;
            else if (spellPower < 250)
                return SpellLevel.Five;
            else if (spellPower < 300)
                return SpellLevel.Six;
            else if (spellPower < 350)
                return SpellLevel.Seven;
            else return SpellLevel.Eight;
        }

        /// <summary>
        /// Method used for the scaling, windup motion, and spell gestures for spell casts
        /// </summary>
        protected static float SpellAttributes(string account, uint spellId, out float castingDelay, out MotionCommand windUpMotion, out MotionCommand spellGesture)
        {
            float scale;

            SpellComponentsTable comps = DatManager.PortalDat.SpellComponentsTable;

            SpellTable spellTable = DatManager.PortalDat.SpellTable;
            if (!spellTable.Spells.ContainsKey(spellId))
            {
                windUpMotion = MotionCommand.Invalid;
                spellGesture = MotionCommand.Invalid;
                castingDelay = 0.0f;
                return -1.0f;
            }

            SpellBase spell = spellTable.Spells[spellId];

            ////Determine scale of the spell effects and windup animation
            SpellLevel spellLevel = CalculateSpellLevel(spell.Power);
            if (account == null)
            {
                switch (spellLevel)
                {
                    case SpellLevel.One:
                        scale = 0.1f;
                        break;
                    case SpellLevel.Two:
                        scale = 0.2f;
                        break;
                    case SpellLevel.Three:
                        scale = 0.4f;
                        break;
                    case SpellLevel.Four:
                        scale = 0.5f;
                        break;
                    case SpellLevel.Five:
                        scale = 0.6f;
                        break;
                    case SpellLevel.Six:
                        scale = 1.0f;
                        break;
                    default:
                        scale = 1.0f;
                        break;
                }

                spellGesture = MotionCommand.Magic;
                windUpMotion = 0;
                castingDelay = 0.0f;
                return scale;
            }

            switch (spellLevel)
            {
                case SpellLevel.One:
                    scale = 0.1f;
                    castingDelay = 1.3f;
                    windUpMotion = MotionCommand.MagicPowerUp01;
                    break;
                case SpellLevel.Two:
                    scale = 0.2f;
                    castingDelay = 1.4f;
                    windUpMotion = MotionCommand.MagicPowerUp02;
                    break;
                case SpellLevel.Three:
                    scale = 0.4f;
                    castingDelay = 1.5f;
                    windUpMotion = MotionCommand.MagicPowerUp03;
                    break;
                case SpellLevel.Four:
                    scale = 0.5f;
                    castingDelay = 1.7f;
                    windUpMotion = MotionCommand.MagicPowerUp04;
                    break;
                case SpellLevel.Five:
                    scale = 0.6f;
                    castingDelay = 1.9f;
                    windUpMotion = MotionCommand.MagicPowerUp05;
                    break;
                case SpellLevel.Six:
                    scale = 1.0f;
                    castingDelay = 2.0f;
                    windUpMotion = MotionCommand.MagicPowerUp06;
                    break;
                default:
                    scale = 1.0f;
                    castingDelay = 2.0f;
                    windUpMotion = MotionCommand.MagicPowerUp07Purple;
                    break;
            }

            var formula = SpellTable.GetSpellFormula(spellTable, spellId, account);
            spellGesture = (MotionCommand)comps.SpellComponents[formula[formula.Count - 1]].Gesture;

            return scale;
        }

        /// <summary>
        /// Determine is the spell being case is harmful or beneficial
        /// </summary>
        /// <param name="spell"></param>
        /// <returns></returns>
        protected bool IsSpellHarmful(SpellBase spell)
        {
            // All War and Void Magic spells are harmful
            if (spell.School == MagicSchool.WarMagic || spell.School == MagicSchool.VoidMagic)
                return true;

            // Life Magic spells that don't have bit three of their bitfield property set are harmful
            if (spell.School == MagicSchool.LifeMagic && (spell.Bitfield & 0x4) == 0)
                return true;

            // Creature Magic spells that don't have bit three of their bitfield property set are harmful
            if (spell.School == MagicSchool.CreatureEnchantment && (spell.Bitfield & 0x4) == 0)
                return true;

            return false;
        }

        /// <summary>
        /// Determines whether the target for the spell being cast is invalid
        /// </summary>
        /// <param name="spell"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        protected bool IsInvalidTarget(SpellBase spell, WorldObject target)
        {
            // Self targeted spells should have a target of self
            if ((int)Math.Floor(spell.BaseRangeConstant) == 0 && target.WeenieClassId != 1)
                return true;

            // Invalidate non Item Enchantment spells cast against non Creatures or Players
            if (spell.School != MagicSchool.ItemEnchantment)
            {
                if (target.WeenieType != WeenieType.Creature)
                {
                    if (target.WeenieClassId != 1)
                        return true;
                }
            }

            // Invalidate beneficial spells against Creature/Non-player targets
            if (target.WeenieType == WeenieType.Creature && IsSpellHarmful(spell) == false)
                return true;

            // Cannot cast Weapon Aura spells on targets that are not players or creatures
            if ((spell.Name.Contains("Aura of")) && (spell.School == MagicSchool.ItemEnchantment))
            {
                if (target.WeenieClassId != 1)
                {
                    if (target.WeenieType != WeenieType.Creature)
                        return true;
                }
            }

            // Cannot cast Weapon Aura spells on targets that are not players or creatures
            if ((spell.MetaSpellType == SpellType.Enchantment) && (spell.School == MagicSchool.ItemEnchantment))
            {
                if ((target.WeenieClassId == 1)
                    || (target.WeenieType == WeenieType.Creature)
                    || (target.WeenieType == WeenieType.Clothing)
                    || (target.WeenieType == WeenieType.Caster)
                    || (target.WeenieType == WeenieType.MeleeWeapon)
                    || (target.WeenieType == WeenieType.MissileLauncher)
                    || (target.WeenieType == WeenieType.Missile)
                    || (target.WeenieType == WeenieType.Door)
                    || (target.WeenieType == WeenieType.Chest))
                    return false;
                else
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether a spell will be evaded, based upon the caster's magic skill vs target's magic defense skill
        /// </summary>
        /// <param name="casterMagicSkill"></param>
        /// <param name="targetMagicDefenseSkill"></param>
        /// <returns></returns>
        public static bool MagicDefenseCheck(uint casterMagicSkill, uint targetMagicDefenseSkill)
        {
            return Physics.Common.Random.RollDice(0.0f, 1.0f) < (1.0f - SkillCheck.GetSkillChance((int)casterMagicSkill, (int)targetMagicDefenseSkill));
        }

        /// <summary>
        /// Creates the Life Magic spell
        /// </summary>
        /// <param name="target"></param>
        /// <param name="spell"></param>
        /// <param name="spellStatMod"></param>
        /// <param name="message"></param>
        /// <param name="castByItem"></param>
        /// <returns></returns>
        protected bool LifeMagic(WorldObject target, SpellBase spell, Database.Models.World.Spell spellStatMod, out string message, bool castByItem = false)
        {
            string srcVital, destVital, action;
            string targetMsg = null;

            Player player = null;
            Creature creature = null;
            if (WeenieClassId == 1)
                player = (Player)this;
            else if (WeenieType == WeenieType.Creature)
                creature = (Creature)this;

            Creature spellTarget;
            if (spell.BaseRangeConstant == 0)
                spellTarget = (Creature)this;
            else
                spellTarget = (Creature)target;

            int newSpellTargetVital;
            switch (spell.MetaSpellType)
            {
                case SpellType.Boost:
                    int minBoostValue, maxBoostValue;
                    if ((spellStatMod.BoostVariance + spellStatMod.Boost) < spellStatMod.Boost)
                    {
                        minBoostValue = (int)(spellStatMod.BoostVariance + spellStatMod.Boost);
                        maxBoostValue = (int)spellStatMod.Boost;
                    }
                    else
                    {
                        minBoostValue = (int)spellStatMod.Boost;
                        maxBoostValue = (int)(spellStatMod.BoostVariance + spellStatMod.Boost);
                    }
                    int boost = Physics.Common.Random.RollDice(minBoostValue, maxBoostValue);
                    if (boost < 0)
                        action = "drain";
                    else
                        action = "restore";
                    switch (spellStatMod.DamageType)
                    {
                        case 512:   // Mana
                            newSpellTargetVital = (int)(spellTarget.Mana.Current + boost);
                            srcVital = "mana";
                            if (newSpellTargetVital < spellTarget.Mana.MaxValue)
                            {
                                if (newSpellTargetVital <= 0)
                                    spellTarget.UpdateVital(spellTarget.Mana, 0);
                                else
                                    spellTarget.UpdateVital(spellTarget.Mana, (uint)newSpellTargetVital);
                            }
                            else
                                spellTarget.UpdateVital(spellTarget.Mana, spellTarget.Mana.MaxValue);
                            break;
                        case 256:   // Stamina
                            newSpellTargetVital = (int)(spellTarget.Stamina.Current + boost);
                            srcVital = "stamina";
                            if (newSpellTargetVital < spellTarget.Stamina.MaxValue)
                            {
                                if (newSpellTargetVital <= 0)
                                    spellTarget.UpdateVital(spellTarget.Stamina, 0);
                                else
                                    spellTarget.UpdateVital(spellTarget.Stamina, (uint)newSpellTargetVital);
                            }
                            else
                                spellTarget.UpdateVital(spellTarget.Stamina, spellTarget.Stamina.MaxValue);
                            break;
                        default:   // Health
                            newSpellTargetVital = (int)(spellTarget.Health.Current + boost);
                            srcVital = "health";
                            if (newSpellTargetVital < spellTarget.Health.MaxValue)
                            {
                                if (newSpellTargetVital <= 0)
                                    spellTarget.UpdateVital(spellTarget.Health, 0);
                                else
                                    spellTarget.UpdateVital(spellTarget.Health, (uint)newSpellTargetVital);
                            }
                            else
                                spellTarget.UpdateVital(spellTarget.Health, spellTarget.Health.MaxValue);
                            break;
                    }
                    if (this is Player)
                    {
                        if (spell.BaseRangeConstant == 0)
                            message = $"You {action} {Math.Abs(boost).ToString()} {srcVital}";
                        else
                            message = $"You {action} {Math.Abs(boost).ToString()} points of {srcVital} from {spellTarget.Name}";
                    }
                    else
                        message = null;

                    if (target is Player && spell.BaseRangeConstant > 0)
                        targetMsg = $"{Name} casts {spell.Name} and {action}s {Math.Abs(boost)} points of your {srcVital}.";

                    break;

                case SpellType.Transfer:
                    // Calculate the change in vitals of the target
                    Creature caster;
                    if (spell.BaseRangeConstant == 0 && spell.BaseRangeMod == 1)
                        caster = spellTarget;
                    else
                        caster = (Creature)this;
                    uint vitalChange, casterVitalChange;
                    ResistanceType resistanceDrain, resistanceBoost;
                    if (spellStatMod.Source == (int)PropertyAttribute2nd.Mana)
                        resistanceDrain = ResistanceType.ManaDrain;
                    else if (spellStatMod.Source == (int)PropertyAttribute2nd.Stamina)
                        resistanceDrain = ResistanceType.StaminaDrain;
                    else
                        resistanceDrain = ResistanceType.HealthDrain;
                    vitalChange = (uint)((spellTarget.GetCurrentCreatureVital((PropertyAttribute2nd)spellStatMod.Source) * spellStatMod.Proportion) * spellTarget.GetNaturalResistence(resistanceDrain));
                    if (spellStatMod.TransferCap != 0)
                    {
                        if (vitalChange > spellStatMod.TransferCap)
                            vitalChange = (uint)spellStatMod.TransferCap;
                    }
                    if (spellStatMod.Destination == (int)PropertyAttribute2nd.Mana)
                        resistanceBoost = ResistanceType.ManaDrain;
                    else if (spellStatMod.Source == (int)PropertyAttribute2nd.Stamina)
                        resistanceBoost = ResistanceType.StaminaDrain;
                    else
                        resistanceBoost = ResistanceType.HealthDrain;
                    casterVitalChange = (uint)((vitalChange * (1.0f - spellStatMod.LossPercent)) * spellTarget.GetNaturalResistence(resistanceBoost));
                    vitalChange = (uint)(casterVitalChange / (1.0f - spellStatMod.LossPercent));
                    newSpellTargetVital = (int)(spellTarget.GetCurrentCreatureVital((PropertyAttribute2nd)spellStatMod.Source) - vitalChange);

                    // Apply the change in vitals to the target
                    switch (spellStatMod.Source)
                    {
                        case (int)PropertyAttribute2nd.Mana:
                            srcVital = "mana";
                            if (newSpellTargetVital <= 0)
                                spellTarget.UpdateVital(spellTarget.Mana, 0);
                            else
                                spellTarget.UpdateVital(spellTarget.Mana, (uint)newSpellTargetVital);
                            break;
                        case (int)PropertyAttribute2nd.Stamina:
                            srcVital = "stamina";
                            if (newSpellTargetVital <= 0)
                                spellTarget.UpdateVital(spellTarget.Stamina, 0);
                            else
                                spellTarget.UpdateVital(spellTarget.Stamina, (uint)newSpellTargetVital);
                            break;
                        default:   // Health
                            srcVital = "health";
                            if (newSpellTargetVital <= 0)
                                spellTarget.UpdateVital(spellTarget.Health, 0);
                            else
                                spellTarget.UpdateVital(spellTarget.Health, (uint)newSpellTargetVital);
                            break;
                    }

                    // Apply the scaled change in vitals to the caster
                    uint newCasterVital;
                    switch (spellStatMod.Destination)
                    {
                        case (int)PropertyAttribute2nd.Mana:
                            destVital = "mana";
                            newCasterVital = caster.Mana.Current + casterVitalChange;
                            caster.UpdateVital(caster.Mana, newCasterVital);
                            break;
                        case (int)PropertyAttribute2nd.Stamina:
                            destVital = "stamina";
                            newCasterVital = caster.Stamina.Current + casterVitalChange;
                            caster.UpdateVital(caster.Stamina, newCasterVital);
                            break;
                        default:   // Health
                            destVital = "health";
                            newCasterVital = caster.Mana.Current + casterVitalChange;
                            caster.UpdateVital(caster.Health, newCasterVital);
                            break;
                    }

                    if (WeenieClassId == 1)
                    {
                        if (target.Guid == Guid)
                        {
                            message = $"You drain {vitalChange.ToString()} points of {srcVital} and apply {casterVitalChange.ToString()} points of {destVital} to yourself";
                        }
                        else
                            message = $"You drain {vitalChange.ToString()} points of {srcVital} from {spellTarget.Name} and apply {casterVitalChange.ToString()} to yourself";
                    }
                    else
                        message = null;

                    if (target is Player && target != this)
                        targetMsg = $"You lose {vitalChange} points of {srcVital} due to {Name} casting {spell.Name} on you";

                    break;

                case SpellType.LifeProjectile:
                    caster = (Creature)this;
                    uint damage;
                    if (spell.Name.Contains("Blight"))
                    {
                        damage = (uint)(caster.GetCurrentCreatureVital(PropertyAttribute2nd.Mana) * caster.GetNaturalResistence(ResistanceType.ManaDrain));
                        newCasterVital = caster.Mana.Current - damage;
                        if (newCasterVital <= 0)
                            caster.UpdateVital(caster.Mana, 0);
                        else
                            caster.UpdateVital(caster.Mana, (uint)newCasterVital);
                    }
                    else if (spell.Name.Contains("Tenacity"))
                    {
                        damage = (uint)(spellTarget.GetCurrentCreatureVital(PropertyAttribute2nd.Stamina) * spellTarget.GetNaturalResistence(ResistanceType.StaminaDrain));
                        newCasterVital = caster.Stamina.Current - damage;
                        if (newCasterVital <= 0)
                            caster.UpdateVital(caster.Stamina, 0);
                        else
                            caster.UpdateVital(caster.Stamina, (uint)newCasterVital);
                    }
                    else
                    {
                        damage = (uint)(spellTarget.GetCurrentCreatureVital(PropertyAttribute2nd.Stamina) * spellTarget.GetNaturalResistence(ResistanceType.HealthDrain));
                        newCasterVital = caster.Health.Current - damage;
                        if (newCasterVital <= 0)
                            caster.UpdateVital(caster.Health, 0);
                        else
                            caster.UpdateVital(caster.Health, (uint)newCasterVital);
                    }

                    CreateSpellProjectile(this, target, spell.MetaSpellId, (uint)spellStatMod.Wcid, damage);

                    if (caster.Health.Current <= 0)
                    {
                        caster.Die();

                        if (caster.WeenieClassId == 1)
                        {
                            Strings.DeathMessages.TryGetValue(DamageType.Base, out var messages);
                            player.Session.Network.EnqueueSend(new GameMessageSystemChat("You have killed yourself", ChatMessageType.Broadcast));
                        }
                    }
                    message = null;
                    break;
                case SpellType.Dispel:
                    message = "Spell not implemented, yet!";
                    break;
                case SpellType.Enchantment:
                    message = CreateEnchantment(target, spell, spellStatMod, castByItem);
                    break;
                default:
                    message = "Spell not implemented, yet!";
                    break;
            }

            if (targetMsg != null)
            {
                var playerTarget = target as Player;
                playerTarget.Session.Network.EnqueueSend(new GameMessageSystemChat(targetMsg, ChatMessageType.Magic));
            }

            if (spellTarget.Health.Current == 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Wrapper around CreateEnchantment for Creature Magic
        /// </summary>
        /// <param name="target"></param>
        /// <param name="spell"></param>
        /// <param name="spellStatMod"></param>
        /// <param name="castByItem"></param>
        /// <returns></returns>
        protected string CreatureMagic(WorldObject target, SpellBase spell, Database.Models.World.Spell spellStatMod, bool castByItem = false)
        {
            return CreateEnchantment(target, spell, spellStatMod, castByItem);
        }

        /// <summary>
        /// Item Magic
        /// </summary>
        /// <param name="target"></param>
        /// <param name="spell"></param>
        /// <param name="spellStatMod"></param>
        /// <param name="castByItem"></param>
        protected string ItemMagic(WorldObject target, SpellBase spell, Database.Models.World.Spell spellStatMod, bool castByItem = false)
        {
            Player player = CurrentLandblock.GetObject(Guid) as Player;

            if ((spell.MetaSpellType == SpellType.PortalLink)
                || (spell.MetaSpellType == SpellType.PortalRecall)
                || (spell.MetaSpellType == SpellType.PortalSending)
                || (spell.MetaSpellType == SpellType.PortalSummon))
            {
                switch (spell.MetaSpellId)
                {
                    case 2645: // Portal Recall
                        if (!player.TeleToPosition(PositionType.LastPortal))
                        {
                            // You must link to a portal to recall it!
                            player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.YouMustLinkToPortalToRecall));
                        }
                        break;
                    case 1635: // Lifestone Recall
                        if (!player.TeleToPosition(PositionType.LinkedLifestone))
                        {
                            // You must link to a lifestone to recall it!
                            player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.YouMustLinkToLifestoneToRecall));
                        }
                        break;
                    case 48: // Primary Portal Recall
                        if (!player.TeleToPosition(PositionType.LinkedPortalOne))
                        {
                            // You must link to a portal to recall it!
                            player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.YouMustLinkToPortalToRecall));
                        }
                        break;
                    case 2647: // Secondary Portal Recall
                        if (!player.TeleToPosition(PositionType.LinkedPortalTwo))
                        {
                            // You must link to a portal to recall it!
                            player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.YouMustLinkToPortalToRecall));
                        }
                        break;
                    default:
                        break;
                }
            }
            else if (spell.MetaSpellType == SpellType.Enchantment)
            {
                return CreateEnchantment(target, spell, spellStatMod, castByItem);
            }

            return "";
        }

        /// <summary>
        /// War Magic
        /// </summary>
        /// <param name="target"></param>
        /// <param name="spell"></param>
        /// <param name="spellStatMod"></param>
        protected void WarMagic(WorldObject target, SpellBase spell, Database.Models.World.Spell spellStatMod)
        {
            if (spellStatMod.NumProjectiles == 1)
            {
                CreateSpellProjectile(this, target, spell.MetaSpellId, (uint)spellStatMod.Wcid);
            }
            else
            {
                if (WeenieClassId == 1)
                {
                    Player player = (Player)this;
                    player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, errorType: WeenieError.None),
                        new GameMessageSystemChat($"{spell.Name} spell not implemented, yet!", ChatMessageType.System));
                }
            }
        }

        /// <summary>
        /// Void Magic
        /// </summary>
        /// <param name="target"></param>
        /// <param name="spell"></param>
        /// <param name="spellStatMod"></param>
        protected void VoidMagic(WorldObject target, SpellBase spell, Database.Models.World.Spell spellStatMod)
        {
            if (WeenieClassId == 1)
            {
                Player player = (Player)this;
                player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, errorType: WeenieError.None),
                    new GameMessageSystemChat($"{spell.Name} spell not implemented, yet!", ChatMessageType.System));
            }
        }

        /// <summary>
        /// Creates an enchantment and interacts with the Enchantment registry.
        /// Used by Life, Creature, Item, and Void magic
        /// </summary>
        /// <param name="target"></param>
        /// <param name="spell"></param>
        /// <param name="spellStatMod"></param>
        /// <param name="castByItem"></param>
        /// <returns></returns>
        private string CreateEnchantment(WorldObject target, SpellBase spell, Database.Models.World.Spell spellStatMod, bool castByItem)
        {
            double duration;
            if (castByItem)
                duration = -1;
            else
                duration = spell.Duration;
            // create enchantment
            var enchantment = new Enchantment(target, spellStatMod.SpellId, duration, 1, (uint)EnchantmentMask.CreatureSpells);
            var stackType = target.EnchantmentManager.Add(enchantment, castByItem);

            var player = this as Player;
            var playerTarget = target as Player;
            var creatureTarget = target as Creature;

            // build message
            var suffix = "";
            switch (stackType)
            {
                case StackType.Refresh:
                    suffix = $", refreshing {spell.Name}";
                    break;
                case StackType.Surpass:
                    suffix = $", surpassing {target.EnchantmentManager.Surpass.Name}";
                    break;
                case StackType.Surpassed:
                    suffix = $", but it is surpassed by {target.EnchantmentManager.Surpass.Name}";
                    break;
            }

            var targetName = this == target ? "yourself" : target.Name;

            string message;
            if (castByItem == true)
                message = $"An item casts {spell.Name} on you";
            else
                message = $"You cast {spell.Name} on {targetName}{suffix}";


            if (target is Player)
            {
                if (stackType != StackType.Surpassed)
                    playerTarget.Session.Network.EnqueueSend(new GameEventMagicUpdateEnchantment(playerTarget.Session, enchantment));

                if (playerTarget != this)
                    playerTarget.Session.Network.EnqueueSend(new GameMessageSystemChat($"{Name} cast {spell.Name} on you{suffix}", ChatMessageType.Magic));
            }

            return message;
        }

        /// <summary>
        /// Creates the Magic projectile spells for Life, War, and Void Magic
        /// </summary>
        /// <param name="caster"></param>
        /// <param name="target"></param>
        /// <param name="spellId"></param>
        /// <param name="projectileWcid"></param>
        /// <param name="lifeProjectileDamage"></param>
        private void CreateSpellProjectile(WorldObject caster, WorldObject target, uint spellId, uint projectileWcid, uint lifeProjectileDamage = 0)
        {
            SpellProjectile spellProjectile = WorldObjectFactory.CreateNewWorldObject(projectileWcid) as SpellProjectile;
            spellProjectile.Setup(spellId);

            var origin = caster.Location.ToGlobal();
            if (spellProjectile.SpellType == SpellProjectile.ProjectileSpellType.Arc)
                origin.Z += caster.Height;
            else
                origin.Z += caster.Height * 2.0f / 3.0f;
                
            var dest = target.Location.ToGlobal();
            dest.Z += target.Height / 2.0f;

            var direction = Vector3.Normalize(dest - origin);
            // This is not perfect but is close to values that retail used. TODO: revisit this later.
            origin += direction * (caster.PhysicsObj.GetRadius() + spellProjectile.PhysicsObj.GetRadius());

            float time;
            var dist = (dest - origin).Length();
            float speed = 15f;
            if (spellProjectile.SpellType == SpellProjectile.ProjectileSpellType.Bolt)
            {
                speed = GetStationaryVelocity(15f, dist);
            }
            else if (spellProjectile.SpellType == SpellProjectile.ProjectileSpellType.Streak)
            {
                speed = GetStationaryVelocity(45f, dist);
            }
            else if (spellProjectile.SpellType == SpellProjectile.ProjectileSpellType.Arc)
            {
                speed = GetStationaryVelocity(40f, dist);
            }

            // TODO: Implement target leading for non arc spells
            // Also: velocity seems to increase when target is moving away from the caster and decrease when
            // the target is moving toward the caster. This still needs more research.

            var velocity = direction * speed;
            
            if (spellProjectile.SpellType == SpellProjectile.ProjectileSpellType.Arc)
            {
                spellProjectile.Velocity = GetSpellProjectileVelocity(origin, dest, speed, out time);
            }
            else
            {
                spellProjectile.Velocity = new AceVector3(velocity.X, velocity.Y, velocity.Z);
                var velocityLength = spellProjectile.Velocity.Get().Length();
                time = dist / velocityLength;
            }
            spellProjectile.FlightTime = time;

            var loc = caster.Location;
            origin = loc.Pos;
            if (spellProjectile.SpellType == SpellProjectile.ProjectileSpellType.Arc)
                origin.Z += caster.Height;
            else
                origin.Z += caster.Height * 2.0f / 3.0f;
            origin += direction * (caster.PhysicsObj.GetRadius() + spellProjectile.PhysicsObj.GetRadius());

            spellProjectile.Location = new ACE.Entity.Position(loc.LandblockId.Raw, origin.X, origin.Y, origin.Z, loc.Rotation.X, loc.Rotation.Y, loc.Rotation.Z, loc.RotationW);
            spellProjectile.ParentWorldObject = (Creature)this;
            spellProjectile.TargetGuid = target.Guid;
            spellProjectile.LifeProjectileDamage = lifeProjectileDamage;

            LandblockManager.AddObject(spellProjectile);
            CurrentLandblock.EnqueueBroadcast(spellProjectile.Location, new GameMessageScript(spellProjectile.Guid, ACE.Entity.Enum.PlayScript.Launch, spellProjectile.PlayscriptIntensity));

            // TODO : removed when real server projectile tracking and collisions are implemented
            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(spellProjectile.FlightTime);
            actionChain.AddAction(spellProjectile, () => spellProjectile.HandleOnCollide(spellProjectile.TargetGuid));
            actionChain.EnqueueChain();
        }

        /// <summary>
        /// Calculates the velocity of a spell projectile based on distance to the target (assuming it is stationary)
        /// </summary>
        /// <param name="defaultVelocity"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        private float GetStationaryVelocity(float defaultVelocity, float distance)
        {
            var velocity = (float)((defaultVelocity * .9998363f) - (defaultVelocity * .62034f) / distance +
                                   (defaultVelocity * .44868f) / Math.Pow(distance, 2f) - (defaultVelocity * .25256f)
                                   / Math.Pow(distance, 3f));

            velocity = Math.Clamp(velocity, 1, 50);

            return velocity;
        }

        /// <summary>
        /// Calculates the velocity to launch the projectile from origin to dest
        /// </summary>
        private AceVector3 GetSpellProjectileVelocity(Vector3 origin, Vector3 dest, float speed, out float time)
        {
            Trajectory.SolveBallisticArc(origin, speed, dest, out Vector3 velocity, out time);

            return new AceVector3(velocity.X, velocity.Y, velocity.Z);
        }
    }
}
