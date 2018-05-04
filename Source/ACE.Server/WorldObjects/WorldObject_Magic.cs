using System;
using System.Numerics;

using ACE.Database;
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
using ACE.Server.Network.Motion;
using ACE.Server.Network.Structure;
using ACE.Server.WorldObjects.Entity;
using ACE.Server.Managers;
using ACE.Server.Factories;
using ACE.Server.Physics;

namespace ACE.Server.WorldObjects
{
    partial class WorldObject
    {
        private enum SpellLevel
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

        private static SpellLevel CalculateSpellLevel(uint spellPower)
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

            return false;
        }

        /// <summary>
        /// Method used for handling creature targeted spell casts
        /// </summary>
        public void CreateCreatureSpell(ObjectGuid guidTarget, uint spellId)
        {
            Creature creature = CurrentLandblock.GetObject(Guid) as Creature;

            if (creature.IsBusy == true)
                return;
            else
                creature.IsBusy = true;

            SpellTable spellTable = DatManager.PortalDat.SpellTable;
            if (!spellTable.Spells.ContainsKey(spellId))
            {
                creature.IsBusy = false;
                return;
            }

            SpellBase spell = spellTable.Spells[spellId];

            float scale = SpellAttributes(null, spellId, out float castingDelay, out MotionCommand windUpMotion, out MotionCommand spellGesture);

            creature.IsBusy = false;
            return;
        }

        /// <summary>
        /// Method used for handling creature untargeted spell casts
        /// </summary>
        public void CreateCreatureSpell(uint spellId)
        {
            Creature creature = CurrentLandblock.GetObject(Guid) as Creature;

            if (creature.IsBusy == true)
                return;
            else
                creature.IsBusy = true;

            SpellTable spellTable = DatManager.PortalDat.SpellTable;
            if (!spellTable.Spells.ContainsKey(spellId))
            {
                creature.IsBusy = false;
                return;
            }

            SpellBase spell = spellTable.Spells[spellId];

            float scale = SpellAttributes(null, spellId, out float castingDelay, out MotionCommand windUpMotion, out MotionCommand spellGesture);

            creature.IsBusy = false;
            return;
        }

        public static bool MagicDefenseCheck(uint casterMagicSkill, uint targetMagicDefenseSkill)
        {
            return Physics.Common.Random.RollDice(0.0f, 1.0f) < (1.0f - SkillCheck.GetMagicSkillChance((int)casterMagicSkill, (int)targetMagicDefenseSkill));
        }

        protected bool LifeMagic(WorldObject target, SpellBase spell, Database.Models.World.Spell spellStatMod, out string message, bool castByItem = false)
        {
            string srcVital, destVital, action;

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
                                    spellTarget.Mana.Current = 0;
                                else
                                    spellTarget.Mana.Current = (uint)newSpellTargetVital;
                            }
                            else
                                spellTarget.Mana.Current = spellTarget.Mana.MaxValue;
                            break;
                        case 256:   // Stamina
                            newSpellTargetVital = (int)(spellTarget.Stamina.Current + boost);
                            srcVital = "stamina";
                            if (newSpellTargetVital < spellTarget.Stamina.MaxValue)
                            {
                                if (newSpellTargetVital <= 0)
                                    spellTarget.Stamina.Current = 0;
                                else
                                    spellTarget.Stamina.Current = (uint)newSpellTargetVital;
                            }
                            else
                                spellTarget.Stamina.Current = spellTarget.Stamina.MaxValue;
                            break;
                        default:   // Health
                            newSpellTargetVital = (int)(spellTarget.Health.Current + boost);
                            srcVital = "health";
                            if (newSpellTargetVital < spellTarget.Health.MaxValue)
                            {
                                if (newSpellTargetVital <= 0)
                                    spellTarget.Health.Current = 0;
                                else
                                    spellTarget.Health.Current = (uint)newSpellTargetVital;
                            }
                            else
                                spellTarget.Health.Current = spellTarget.Health.MaxValue;
                            break;
                    }
                    if (WeenieClassId == 1)
                    {
                        if (spell.BaseRangeConstant == 0)
                            message = $"You {action} {Math.Abs(boost).ToString()} {srcVital}";
                        else
                            message = $"You {action} {Math.Abs(boost).ToString()} of {srcVital} from {spellTarget.Name}";
                    }
                    else
                        message = null;
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
                                spellTarget.Mana.Current = 0;
                            else
                                spellTarget.Mana.Current = (uint)newSpellTargetVital;
                            break;
                        case (int)PropertyAttribute2nd.Stamina:
                            srcVital = "stamina";
                            if (newSpellTargetVital <= 0)
                                spellTarget.Stamina.Current = 0;
                            else
                                spellTarget.Stamina.Current = (uint)newSpellTargetVital;
                            break;
                        default:   // Health
                            srcVital = "health";
                            if (newSpellTargetVital <= 0)
                                spellTarget.Health.Current = 0;
                            else
                                spellTarget.Health.Current = (uint)newSpellTargetVital;
                            break;
                    }

                    // Apply the scaled change in vitals to the caster
                    uint newCasterVital;
                    switch (spellStatMod.Destination)
                    {
                        case (int)PropertyAttribute2nd.Mana:
                            destVital = "mana";
                            newCasterVital = caster.Mana.Current + casterVitalChange;
                            caster.Mana.Current = newCasterVital;
                            if (caster.Mana.Current >= caster.Mana.MaxValue)
                                caster.Mana.Current = caster.Mana.MaxValue;
                            break;
                        case (int)PropertyAttribute2nd.Stamina:
                            destVital = "stamina";
                            newCasterVital = caster.Stamina.Current + casterVitalChange;
                            caster.Stamina.Current = newCasterVital;
                            if (caster.Stamina.Current >= caster.Stamina.MaxValue)
                                caster.Stamina.Current = caster.Stamina.MaxValue;
                            break;
                        default:   // Health
                            destVital = "health";
                            newCasterVital = caster.Mana.Current + casterVitalChange;
                            caster.Health.Current = newCasterVital;
                            if (caster.Health.Current >= caster.Health.MaxValue)
                                caster.Health.Current = caster.Health.MaxValue;
                            break;
                    }

                    if (WeenieClassId == 1)
                    {
                        if (target.Guid == Guid)
                        {
                            message = $"You drain {vitalChange.ToString()} of {srcVital} and apply {casterVitalChange.ToString()} of {destVital} to yourself";
                        }
                        else
                            message = $"You drain {vitalChange.ToString()} of {srcVital} from {spellTarget.Name} and apply {casterVitalChange.ToString()} to yourself";
                    }
                    else
                        message = null;
                    break;
                case SpellType.LifeProjectile:
                    caster = (Creature)this;
                    uint damage;
                    if (spell.Name.Contains("Blight"))
                    {
                        damage = (uint)(caster.GetCurrentCreatureVital(PropertyAttribute2nd.Mana) * caster.GetNaturalResistence(ResistanceType.ManaDrain));
                        newCasterVital = caster.Mana.Current - damage;
                        if (newCasterVital <= 0)
                            caster.Mana.Current = 0;
                        else
                            caster.Mana.Current = (uint)newCasterVital;
                    }
                    else if (spell.Name.Contains("Tenacity"))
                    {
                        damage = (uint)(spellTarget.GetCurrentCreatureVital(PropertyAttribute2nd.Stamina) * spellTarget.GetNaturalResistence(ResistanceType.StaminaDrain));
                        newCasterVital = caster.Stamina.Current - damage;
                        if (newCasterVital <= 0)
                            caster.Stamina.Current = 0;
                        else
                            caster.Stamina.Current = (uint)newCasterVital;
                    }
                    else
                    {
                        damage = (uint)(spellTarget.GetCurrentCreatureVital(PropertyAttribute2nd.Stamina) * spellTarget.GetNaturalResistence(ResistanceType.HealthDrain));
                        newCasterVital = caster.Health.Current - damage;
                        if (newCasterVital <= 0)
                            caster.Health.Current = 0;
                        else
                            caster.Health.Current = (uint)newCasterVital;
                    }

                    CreateSpellProjectile(target, spell.MetaSpellId, (uint)spellStatMod.Wcid, damage);

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

            if (spellTarget.Health.Current == 0)
                return true;
            else
                return false;
        }

        protected string CreatureMagic(WorldObject target, SpellBase spell, Database.Models.World.Spell spellStatMod, bool castByItem = false)
        {
            return CreateEnchantment(target, spell, spellStatMod, castByItem);
        }

        private string CreateEnchantment(WorldObject target, SpellBase spell, Database.Models.World.Spell spellStatMod, bool castByItem)
        {
            if (WeenieClassId == 1)
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

                var message = $"You cast {spell.Name} on {targetName}{suffix}";

                if (target is Player)
                {
                    if (stackType != StackType.Surpassed)
                        playerTarget.Session.Network.EnqueueSend(new GameEventMagicUpdateEnchantment(playerTarget.Session, enchantment));

                    if (player != playerTarget)
                        playerTarget.Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} cast {spell.Name} on you{suffix}", ChatMessageType.Magic));
                }

                return message;
            }

            return "";
        }

        protected void ItemMagic(WorldObject target, SpellBase spell, Database.Models.World.Spell spellStatMod, bool castByItem = false)
        {
            Player player = CurrentLandblock.GetObject(Guid) as Player;

            if (spell.Name.Contains("Portal") || spell.Name.Contains("Lifestone"))
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
                CreateEnchantment(target, spell, spellStatMod, castByItem);
            }
        }

        protected void WarMagic(WorldObject target, SpellBase spell, Database.Models.World.Spell spellStatMod)
        {
            if (spellStatMod.NumProjectiles == 1)
            {
                CreateSpellProjectile(target, spell.MetaSpellId, (uint)spellStatMod.Wcid);
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

        protected void VoidMagic(WorldObject target, SpellBase spell, Database.Models.World.Spell spellStatMod)
        {
            if (WeenieClassId == 1)
            {
                Player player = (Player)this;
                player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, errorType: WeenieError.None),
                    new GameMessageSystemChat($"{spell.Name} spell not implemented, yet!", ChatMessageType.System));
            }
        }

        private void CreateSpellProjectile(WorldObject target, uint spellId, uint projectileWcid, uint lifeProjectileDamage = 0)
        {
            SpellProjectile spellProjectile = WorldObjectFactory.CreateNewWorldObject(projectileWcid) as SpellProjectile;

            var origin = Location.ToGlobal();
            origin.Z += Height / 2.0f;

            var dest = target.Location.ToGlobal();
            dest.Z += target.Height / 2.0f;

            float speed = 35.0f;
            if (DatManager.PortalDat.SpellTable.Spells[spellId].Name.Contains("Streak"))
                speed = 40.0f;

            var dir = Vector3.Normalize(dest - origin);

            origin += dir * 2.0f;

            float time;
            var velocity = dir * speed;
            var dist = (dest - origin).Length();
            if (DatManager.PortalDat.SpellTable.Spells[spellId].Name.Contains("Arc"))
            {
                speed = 20.0f;
                spellProjectile.Velocity = GetSpellProjectileVelocity(origin, dest, speed, out time);
            }
            else
            {
                spellProjectile.Velocity = new AceVector3(velocity.X, velocity.Y, velocity.Z);
                var velocityLength = spellProjectile.Velocity.Get().Length();
                time = dist / velocityLength;
            }

            var loc = Location;
            origin = loc.Pos;
            origin.Z += Height / 2.0f;
            origin += dir * 2.0f;

            spellProjectile.Location = new ACE.Entity.Position(loc.LandblockId.Raw, origin.X, origin.Y, origin.Z, loc.Rotation.X, loc.Rotation.Y, loc.Rotation.Z, loc.RotationW);
            SetSpellProjectilePhysicsState(spellProjectile);

            spellProjectile.ParentWorldObject = (Creature)this;
            spellProjectile.TargetGuid = target.Guid;
            spellProjectile.SpellId = spellId;
            spellProjectile.LifeProjectileDamage = lifeProjectileDamage;

            LandblockManager.AddObject(spellProjectile);
            CurrentLandblock.EnqueueBroadcast(spellProjectile.Location, new GameMessageScript(spellProjectile.Guid, ACE.Entity.Enum.PlayScript.Launch, 1.0f));

            // TODO : removed when real server projectile tracking and collisions are implemented
            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(time);
            actionChain.AddAction(spellProjectile, () => spellProjectile.HandleOnCollide(spellProjectile.TargetGuid));
            actionChain.EnqueueChain();
        }

        /// <summary>
        /// Sets the physics state for a launched projectile
        /// </summary>
        private void SetSpellProjectilePhysicsState(WorldObject obj)
        {
            obj.ReportCollisions = true;
            obj.Missile = true;
            obj.AlignPath = true;
            obj.PathClipped = true;
            obj.Ethereal = false;
            obj.IgnoreCollisions = false;
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
