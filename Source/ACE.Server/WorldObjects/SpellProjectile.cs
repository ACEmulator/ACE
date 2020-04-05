using System;
using System.Numerics;

using ACE.Common;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects.Entity;

namespace ACE.Server.WorldObjects
{
    public class SpellProjectile : WorldObject
    {
        public Spell Spell;
        public ProjectileSpellType SpellType { get; set; }

        public Position SpawnPos { get; set; }
        public float DistanceToTarget { get; set; }
        public uint LifeProjectileDamage { get; set; }

        /// <summary>
        /// If a spell projectile has been cast from a built-in weapon spell,
        /// this will point to the item instead of the Creature
        /// </summary>
        public WorldObject Caster { get; set; }

        public SpellProjectileInfo Info { get; set; }

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public SpellProjectile(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public SpellProjectile(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            // Override weenie description defaults
            ValidLocations = null;
            DefaultScriptId = null;
        }

        /// <summary>
        /// Perfroms additional set up of the spell projectile based on the spell id or its derived type.
        /// </summary>
        public void Setup(Spell spell, ProjectileSpellType spellType)
        {
            Spell = spell;
            SpellType = spellType;

            InitPhysicsObj();

            // Runtime changes to default state
            ReportCollisions = true;
            Missile = true;
            AlignPath = true;
            PathClipped = true;
            IgnoreCollisions = false;

            // FIXME: use data here
            if (!Spell.Name.Equals("Rolling Death"))
                Ethereal = false;

            if (SpellType == ProjectileSpellType.Bolt || SpellType == ProjectileSpellType.Streak
                || SpellType == ProjectileSpellType.Arc || SpellType == ProjectileSpellType.Volley || SpellType == ProjectileSpellType.Blast
                || WeenieClassId == 7276 || WeenieClassId == 7277 || WeenieClassId == 7279 || WeenieClassId == 7280)
            {
                DefaultScriptId = (uint)PlayScript.ProjectileCollision;
                DefaultScriptIntensity = 1.0f;
            }

            // Some wall spells don't have scripted collisions
            if (WeenieClassId == 7278 || WeenieClassId == 7281 || WeenieClassId == 7282 || WeenieClassId == 23144)
            {
                ScriptedCollision = false;
            }

            AllowEdgeSlide = false;

            // No need to send an ObjScale of 1.0f over the wire since that is the default value
            if (ObjScale == 1.0f)
                ObjScale = null;

            if (SpellType == ProjectileSpellType.Ring)
            {
                if (spell.Id == 3818)
                {
                    DefaultScriptId = (uint)PlayScript.Explode;
                    DefaultScriptIntensity = 1.0f;
                    ScriptedCollision = true;
                }
                else
                {
                    ScriptedCollision = false;
                }
            }
                
            // Whirling Blade spells get omega values and "align path" turned off which
            // creates the nice swirling animation
            if (WeenieClassId == 1636 || WeenieClassId == 7268 || WeenieClassId == 20979)
            {
                AlignPath = false;
                PhysicsObj.Omega = new Vector3(12.56637f, 0, 0);
            }
        }

        public static ProjectileSpellType GetProjectileSpellType(uint spellID)
        {
            var spell = new Spell(spellID);

            if (spell.Wcid == 0)
                return ProjectileSpellType.Undef;

            if (spell.NumProjectiles == 1)
            {
                if (spell.Category >= SpellCategory.AcidStreak && spell.Category <= SpellCategory.SlashingStreak ||
                         spell.Category == SpellCategory.NetherStreak || spell.Category == SpellCategory.Fireworks)
                    return ProjectileSpellType.Streak;

                else if (spell.NonTracking)
                    return ProjectileSpellType.Arc;

                else
                    return ProjectileSpellType.Bolt;
            }

            if (spell.Category >= SpellCategory.AcidRing && spell.Category <= SpellCategory.SlashingRing || spell.SpreadAngle == 360)
                return ProjectileSpellType.Ring;

            if (spell.Category >= SpellCategory.AcidBurst && spell.Category <= SpellCategory.SlashingBurst ||
                spell.Category == SpellCategory.NetherDamageOverTimeRaising3)
                return ProjectileSpellType.Blast;

            // 1481 - Flaming Missile Volley
            if (spell.Category >= SpellCategory.AcidVolley && spell.Category <= SpellCategory.BladeVolley || spell.Name.Contains("Volley"))
                return ProjectileSpellType.Volley;

            if (spell.Category >= SpellCategory.AcidWall && spell.Category <= SpellCategory.SlashingWall)
                return ProjectileSpellType.Wall;

            if (spell.Category >= SpellCategory.AcidStrike && spell.Category <= SpellCategory.SlashingStrike)
                return ProjectileSpellType.Strike;

            return ProjectileSpellType.Undef;
        }

        public float GetProjectileScriptIntensity(ProjectileSpellType spellType)
        {
            if (spellType == ProjectileSpellType.Wall)
            {
                return 0.4f;
            }
            if (spellType == ProjectileSpellType.Ring)
            {
                if (Spell.Level == 6 || Spell.Id == 3818)
                    return 0.4f;
                if (Spell.Level == 7)
                    return 1.0f;
            }

            // Bolt, Blast, Volley, Streak and Arc all seem to use this scale
            // TODO: should this be based on spell level, or power of first scarab?
            // ie. can this use Spell.Formula.ScarabScale?
            switch (Spell.Level)
            {
                case 1:
                    return 0f;
                case 2:
                    return 0.2f;
                case 3:
                    return 0.4f;
                case 4:
                    return 0.6f;
                case 5:
                    return 0.8f;
                case 6:
                case 7:
                case 8:
                    return 1.0f;
                default:
                    return 0f;
            }
        }

        public bool WorldEntryCollision { get; set; }

        public void ProjectileImpact()
        {
            //Console.WriteLine($"{Name}.ProjectileImpact()");

            ReportCollisions = false;
            Ethereal = true;
            IgnoreCollisions = true;
            NoDraw = true;
            Cloaked = true;
            LightsStatus = false;

            PhysicsObj.set_active(false);

            if (PhysicsObj.entering_world)
            {
                // this path should only happen if spell_projectile_ethereal = false
                EnqueueBroadcast(new GameMessageScript(Guid, PlayScript.Launch, GetProjectileScriptIntensity(SpellType)));
                WorldEntryCollision = true;
            }

            EnqueueBroadcast(new GameMessageSetState(this, PhysicsObj.State));
            EnqueueBroadcast(new GameMessageScript(Guid, PlayScript.Explode, GetProjectileScriptIntensity(SpellType)));

            // this should only be needed for spell_projectile_ethereal = true,
            // however it can also fix a display issue on client in default mode,
            // where GameMessageSetState updates projectile to ethereal before it has actually collided on client,
            // causing a 'ghost' projectile to continue to sail through the target

            PhysicsObj.Velocity = Vector3.Zero;
            EnqueueBroadcast(new GameMessageVectorUpdate(this));

            ActionChain selfDestructChain = new ActionChain();
            selfDestructChain.AddDelaySeconds(5.0);
            selfDestructChain.AddAction(this, () => Destroy());
            selfDestructChain.EnqueueChain();
        }

        /// <summary>
        /// Handles collision with scenery or other static objects that would block a projectile from reaching its target,
        /// in which case the projectile should be removed with no further processing.
        /// </summary>
        public override void OnCollideEnvironment()
        {
            //Console.WriteLine($"{Name}.OnCollideEnvironment()");

            if (Info != null && ProjectileSource is Player player && player.DebugSpell)
            {
                player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{Name}.OnCollideEnvironment()", ChatMessageType.Broadcast));
                player.Session.Network.EnqueueSend(new GameMessageSystemChat(Info.ToString(), ChatMessageType.Broadcast));
            }

            ProjectileImpact();
        }

        public override void OnCollideObject(WorldObject _target)
        {
            //Console.WriteLine($"{Name}.OnCollideObject({_target.Name})");

            var player = ProjectileSource as Player;

            if (Info != null && player != null && player.DebugSpell)
            {
                player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{Name}.OnCollideObject({_target?.Name} ({_target?.Guid}))", ChatMessageType.Broadcast));
                player.Session.Network.EnqueueSend(new GameMessageSystemChat(Info.ToString(), ChatMessageType.Broadcast));
            }

            if (player != null)
                player.LastHitSpellProjectile = Spell;

            // ensure valid creature target
            // non-target objects will be excluded beforehand from collision detection
            var target = _target as Creature;
            if (target == null || player == target)
            {
                OnCollideEnvironment();
                return;
            }

            ProjectileImpact();

            // for untargeted multi-projectile war spells launched by monsters,
            // ensure monster can damage target
            var sourceCreature = ProjectileSource as Creature;
            if (sourceCreature != null && !sourceCreature.CanDamage(target))
                return;

            // if player target, ensure matching PK status
            var targetPlayer = target as Player;

            var pkError = CheckPKStatusVsTarget(player, targetPlayer, Spell);
            if (pkError != null)
            {
                if (player != null)
                    player.Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(player.Session, pkError[0], target.Name));

                if (targetPlayer != null)
                    targetPlayer.Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(targetPlayer.Session, pkError[1], ProjectileSource.Name));

                return;
            }

            var critical = false;
            var critDefended = false;
            var overpower = false;

            var damage = CalculateDamage(ProjectileSource, Caster, target, ref critical, ref critDefended, ref overpower);

            // null damage -> target resisted; damage of -1 -> target already dead
            if (damage != null && damage != -1)
            {
                // handle void magic DoTs:
                // instead of instant damage, add DoT to target's enchantment registry
                if (Spell.School == MagicSchool.VoidMagic && Spell.Duration > 0)
                {
                    var dot = ProjectileSource.CreateEnchantment(target, ProjectileSource, Spell);
                    if (dot.Message != null && player != null)
                        player.Session.Network.EnqueueSend(dot.Message);

                    // corruption / corrosion playscript?
                    //target.EnqueueBroadcast(new GameMessageScript(target.Guid, PlayScript.HealthDownVoid));
                    //target.EnqueueBroadcast(new GameMessageScript(target.Guid, PlayScript.DirtyFightingDefenseDebuff));
                }
                else
                {
                    DamageTarget(target, damage, critical, critDefended, overpower);
                }

                if (player != null)
                    Proficiency.OnSuccessUse(player, player.GetCreatureSkill(Spell.School), Spell.PowerMod);

                // handle target procs
                // note that for untargeted multi-projectile spells,
                // ProjectileTarget will be null here, so procs will not apply
                if (sourceCreature != null && ProjectileTarget != null)
                    sourceCreature.TryProcEquippedItems(target, false);
            }

            // also called on resist
            if (player != null && targetPlayer == null)
                player.OnAttackMonster(target);
        }

        /// <summary>
        /// Calculates the damage for a spell projectile
        /// Used by war magic, void magic, and life magic projectiles
        /// </summary>
        public double? CalculateDamage(WorldObject source, WorldObject caster, Creature target, ref bool criticalHit, ref bool critDefended, ref bool overpower)
        {
            var sourcePlayer = source as Player;
            var targetPlayer = target as Player;

            if (source == null || targetPlayer != null && targetPlayer.Invincible == true)
                return null;

            // target already dead?
            if (target.Health.Current <= 0)
                return -1;

            // check lifestone protection
            if (targetPlayer != null && targetPlayer.UnderLifestoneProtection)
            {
                if (sourcePlayer != null)
                    sourcePlayer.Session.Network.EnqueueSend(new GameMessageSystemChat($"The Lifestone's magic protects {targetPlayer.Name} from the attack!", ChatMessageType.Magic));

                targetPlayer.HandleLifestoneProtection();
                return null;
            }

            double damageBonus = 0.0f, warSkillBonus = 0.0f, finalDamage = 0.0f;

            var resistanceType = Creature.GetResistanceType(Spell.DamageType);

            var sourceCreature = source as Creature;
            if (sourceCreature?.Overpower != null)
                overpower = Creature.GetOverpower(sourceCreature, target);

            var resisted = source.TryResistSpell(target, Spell, caster, true);
            if (resisted && !overpower)
                return null;

            CreatureSkill attackSkill = null;
            if (sourceCreature != null)
                attackSkill = sourceCreature.GetCreatureSkill(Spell.School);

            // critical hit
            var critical = GetWeaponMagicCritFrequency(sourceCreature, attackSkill, target);
            if (ThreadSafeRandom.Next(0.0f, 1.0f) < critical)
            {
                if (targetPlayer != null && targetPlayer.AugmentationCriticalDefense > 0)
                {
                    var criticalDefenseMod = sourcePlayer != null ? 0.05f : 0.25f;
                    var criticalDefenseChance = targetPlayer.AugmentationCriticalDefense * criticalDefenseMod;

                    if (criticalDefenseChance > ThreadSafeRandom.Next(0.0f, 1.0f))
                        critDefended = true;
                }

                if (!critDefended)
                    criticalHit = true;
            }

            var absorbMod = GetAbsorbMod(target);

            bool isPVP = sourcePlayer != null && targetPlayer != null;

            if (isPVP && Spell.IsHarmful)
                Player.UpdatePKTimers(sourcePlayer, targetPlayer);

            var elementalDmgBonus = GetCasterElementalDamageModifier(sourceCreature, target, Spell.DamageType);

            // Possible 2x + damage bonus for the slayer property
            var slayerBonus = GetWeaponCreatureSlayerModifier(sourceCreature, target);

            // life magic projectiles: ie., martyr's hecatomb
            if (Spell.School == MagicSchool.LifeMagic)
            {
                var lifeMagicDamage = LifeProjectileDamage * Spell.DamageRatio;

                // could life magic projectiles crit?
                // if so, did they use the same 1.5x formula as war magic, instead of 2.0x?
                if (criticalHit)
                    damageBonus = lifeMagicDamage * 0.5f * GetWeaponCritDamageMod(sourceCreature, attackSkill, target);

                var weaponResistanceMod = GetWeaponResistanceModifier(sourceCreature, attackSkill, Spell.DamageType);

                // if attacker/weapon has IgnoreMagicResist directly, do not transfer to spell projectile
                // only pass if SpellProjectile has it directly, such as 2637 - Invoking Aun Tanua

                var resistanceMod = Math.Max(0.0f, target.GetResistanceMod(resistanceType, this, null, weaponResistanceMod));

                finalDamage = (lifeMagicDamage + damageBonus) * elementalDmgBonus * slayerBonus * resistanceMod * absorbMod;
                return finalDamage;
            }
            // war/void magic projectiles
            else
            {
                if (criticalHit)
                {
                    // Original:
                    // http://acpedia.org/wiki/Announcements_-_2002/08_-_Atonement#Letter_to_the_Players

                    // Critical Strikes: In addition to the skill-based damage bonus, each projectile spell has a 2% chance of causing a critical hit on the target and doing increased damage.
                    // A magical critical hit is similar in some respects to melee critical hits (although the damage calculation is handled differently).
                    // While a melee critical hit automatically does twice the maximum damage of the weapon, a magical critical hit will do an additional half the minimum damage of the spell.
                    // For instance, a magical critical hit from a level 7 spell, which does 110-180 points of damage, would add an additional 55 points of damage to the spell.

                    // Later updated for PvE only:

                    // http://acpedia.org/wiki/Announcements_-_2004/07_-_Treaties_in_Stone#Letter_to_the_Players

                    // Currently when a War Magic spell scores a critical hit, it adds a multiple of the base damage of the spell to a normal damage roll.
                    // Starting in July, War Magic critical hits will instead add a multiple of the maximum damage of the spell.
                    // No more crits that do less damage than non-crits!

                    if (isPVP) // PvP: 50% of the MIN damage added to normal damage roll
                        damageBonus = Spell.MinDamage * 0.5f;
                    else   // PvE: 50% of the MAX damage added to normal damage roll
                        damageBonus = Spell.MaxDamage * 0.5f;

                    var critDamageMod = GetWeaponCritDamageMod(sourceCreature, attackSkill, target);

                    damageBonus *= critDamageMod;
                }

                /* War Magic skill-based damage bonus
                 * http://acpedia.org/wiki/Announcements_-_2002/08_-_Atonement#Letter_to_the_Players
                 */
                if (sourcePlayer != null)
                {
                    // per retail stats, level 8 difficulty is capped to 350 instead of 400
                    // without this, level 7s have the potential to deal more damage than level 8s
                    var difficulty = Math.Min(Spell.Power, 350);
                    var magicSkill = sourcePlayer.GetCreatureSkill(Spell.School).Current;

                    if (magicSkill > difficulty)
                    {
                        // Bonus clamped to a maximum of 50%
                        //var percentageBonus = Math.Clamp((magicSkill - Spell.Power) / 100.0f, 0.0f, 0.5f);
                        var percentageBonus = (magicSkill - difficulty) / 1000.0f;

                        warSkillBonus = Spell.MinDamage * percentageBonus;
                    }
                }
                var baseDamage = ThreadSafeRandom.Next(Spell.MinDamage, Spell.MaxDamage);

                var weaponResistanceMod = GetWeaponResistanceModifier(sourceCreature, attackSkill, Spell.DamageType);

                // if attacker/weapon has IgnoreMagicResist directly, do not transfer to spell projectile
                // only pass if SpellProjectile has it directly, such as 2637 - Invoking Aun Tanua

                var resistanceMod = Math.Max(0.0f, target.GetResistanceMod(resistanceType, this, null, weaponResistanceMod));

                finalDamage = baseDamage + damageBonus + warSkillBonus;

                finalDamage *= resistanceMod * elementalDmgBonus * slayerBonus * absorbMod;

                return finalDamage;
            }
        }

        public float GetAbsorbMod(Creature target)
        {
            switch (target.CombatMode)
            {
                case CombatMode.Melee:

                    // does target have shield equipped?
                    var shield = target.GetEquippedShield();
                    if (shield != null && shield.AbsorbMagicDamage != null)
                        return GetShieldMod(target, shield);

                    break;

                case CombatMode.Missile:

                    var weapon = target.GetEquippedMissileWeapon();
                    if (weapon != null && weapon.AbsorbMagicDamage != null)
                        return AbsorbMagic(target, weapon);

                    break;

                case CombatMode.Magic:

                    weapon = target.GetEquippedWand();
                    if (weapon != null && weapon.AbsorbMagicDamage != null)
                        return AbsorbMagic(target, weapon);

                    break;
            }
            return 1.0f;
        }

        /// <summary>
        /// Calculates the amount of damage a shield absorbs from magic projectile
        /// </summary>
        public float GetShieldMod(Creature target, WorldObject shield)
        {
            // is spell projectile in front of creature target,
            // within shield effectiveness area?
            var effectiveAngle = 180.0f;
            var angle = target.GetAngle(this);
            if (Math.Abs(angle) > effectiveAngle / 2.0f)
                return 1.0f;

            // https://asheron.fandom.com/wiki/Shield
            // The formula to determine magic absorption for shields is:
            // Reduction Percent = (cap * specMod * baseSkill * 0.003f) - (cap * specMod * 0.3f)
            // Cap = Maximum reduction
            // SpecMod = 1.0 for spec, 0.8 for trained
            // BaseSkill = 100 to 433 (above 433 base shield you always achieve the maximum %)

            var shieldSkill = target.GetCreatureSkill(Skill.Shield);
            // ensure trained?
            if (shieldSkill.AdvancementClass < SkillAdvancementClass.Trained || shieldSkill.Base < 100)
                return 1.0f;

            var baseSkill = Math.Min(shieldSkill.Base, 433);
            var specMod = shieldSkill.AdvancementClass == SkillAdvancementClass.Specialized ? 1.0f : 0.8f;
            var cap = (float)(shield.GetProperty(PropertyFloat.AbsorbMagicDamage) ?? 0.0f);

            // speced, 100 skill = 0%
            // trained, 100 skill = 0%
            // speced, 200 skill = 30%
            // trained, 200 skill = 24%
            // speced, 300 skill = 60%
            // trained, 300 skill = 48%
            // speced, 433 skill = 100%
            // trained, 433 skill = 80%

            var reduction = (cap * specMod * baseSkill * 0.003f) - (cap * specMod * 0.3f);

            var shieldMod = Math.Min(1.0f, 1.0f - reduction);
            return shieldMod;
        }

        /// <summary>
        /// Calculates the damage reduction modifier for bows and casters
        /// with 'Magic Absorbing' property
        /// </summary>
        public float AbsorbMagic(Creature target, WorldObject item)
        {
            // https://asheron.fandom.com/wiki/Category:Magic_Absorbing

            // Tomes and Bows
            // The formula to determine magic absorption for Tomes and the Fetish of the Dark Idols:
            // - For a 25% maximum item: (magic absorbing %) = 25 - (0.1 * (319 - base magic defense))
            // - For a 10% maximum item: (magic absorbing %) = 10 - (0.04 * (319 - base magic defense))

            // wiki currently has what is likely a typo for the 10% formula,
            // where it has a factor of 0.4 instead of 0.04
            // with 0.4, the 10% items would not start to become effective until base magic defense 294
            // with 0.04, both formulas start to become effective at base magic defense 69

            // using an equivalent formula that produces the correct results for 10% and 25%,
            // and also produces the correct results for any %

            if (item.AbsorbMagicDamage == null)
                return 1.0f;

            var maxPercent = item.AbsorbMagicDamage.Value;

            var baseCap = 319;
            var magicDefBase = target.GetCreatureSkill(Skill.MagicDefense).Base;
            var diff = Math.Max(0, baseCap - magicDefBase);

            var percent = maxPercent - maxPercent * diff * 0.004f;

            return Math.Min(1.0f, 1.0f - (float)percent);
        }

        /// <summary>
        /// Called for a spell projectile to damage its target
        /// </summary>
        public void DamageTarget(WorldObject _target, double? damage, bool critical, bool critDefended, bool overpower)
        {
            var player = ProjectileSource as Player;

            var target = _target as Creature;
            var targetPlayer = _target as Player;

            if (targetPlayer != null && (targetPlayer.Invincible || targetPlayer.IsDead))
                return;

            uint amount;
            var percent = 0.0f;
            var heritageMod = 1.0f;
            var sneakAttackMod = 1.0f;

            // handle life projectiles for stamina / mana
            if (Spell.Category == SpellCategory.StaminaLowering)
            {
                percent = (float)damage / target.Stamina.MaxValue;
                amount = (uint)-target.UpdateVitalDelta(target.Stamina, (int)-Math.Round(damage.Value));
            }
            else if (Spell.Category == SpellCategory.ManaLowering)
            {
                percent = (float)damage / target.Mana.MaxValue;
                amount = (uint)-target.UpdateVitalDelta(target.Mana, (int)-Math.Round(damage.Value));
            }
            else
            {
                // for possibly applying sneak attack to magic projectiles,
                // only do this for health-damaging projectiles?
                if (player != null)
                {
                    // TODO: use target direction vs. projectile position, instead of player position
                    // could sneak attack be applied to void DoTs?
                    sneakAttackMod = player.GetSneakAttackMod(target);
                    //Console.WriteLine("Magic sneak attack:  + sneakAttackMod);
                    heritageMod = player.GetHeritageBonus(player.GetEquippedWand()) ? 1.05f : 1.0f;
                }

                // DR / DRR applies for magic too?
                var creatureSource = ProjectileSource as Creature;
                var damageRating = creatureSource != null ? creatureSource.GetDamageRating() : 0;
                var damageRatingMod = Creature.AdditiveCombine(Creature.GetPositiveRatingMod(damageRating), heritageMod, sneakAttackMod);
                var damageResistRatingMod = Creature.GetNegativeRatingMod(target.GetDamageResistRating(CombatType.Magic));
                damage *= damageRatingMod * damageResistRatingMod;

                //Console.WriteLine($"Damage rating: " + Creature.ModToRating(damageRatingMod));

                percent = (float)damage / target.Health.MaxValue;
                amount = (uint)-target.UpdateVitalDelta(target.Health, (int)-Math.Round(damage.Value));
                target.DamageHistory.Add(ProjectileSource, Spell.DamageType, amount);

                //if (targetPlayer != null && targetPlayer.Fellowship != null)
                    //targetPlayer.Fellowship.OnVitalUpdate(targetPlayer);
            }

            amount = (uint)Math.Round(damage.Value);    // full amount for debugging

            if (target.IsAlive)
            {
                string verb = null, plural = null;
                Strings.GetAttackVerb(Spell.DamageType, percent, ref verb, ref plural);
                var type = Spell.DamageType.GetName().ToLower();

                var critMsg = critical ? "Critical hit! " : "";
                var sneakMsg = sneakAttackMod > 1.0f ? "Sneak Attack! " : "";
                var overpowerMsg = overpower ? "Overpower! " : "";

                var nonHealth = Spell.Category == SpellCategory.StaminaLowering || Spell.Category == SpellCategory.ManaLowering;

                if (player != null)
                {
                    var critProt = critDefended ? " Your target's Critical Protection augmentation allows them to avoid your critical hit!" : "";

                    var attackerMsg = $"{critMsg}{overpowerMsg}{sneakMsg}You {verb} {target.Name} for {amount} points with {Spell.Name}.{critProt}";

                    // could these crit / sneak attack?
                    if (nonHealth)
                    {
                        var vital = Spell.Category == SpellCategory.StaminaLowering ? "stamina" : "mana";
                        attackerMsg = $"With {Spell.Name} you drain {amount} points of {vital} from {target.Name}.";
                    }

                    if (!player.SquelchManager.Squelches.Contains(target, ChatMessageType.Magic))
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat(attackerMsg, ChatMessageType.Magic));
                }

                if (targetPlayer != null)
                {
                    var critProt = critDefended ? " Your Critical Protection augmentation allows you to avoid a critical hit!" : "";

                    var defenderMsg = $"{critMsg}{overpowerMsg}{sneakMsg}{ProjectileSource.Name} {plural} you for {amount} points with {Spell.Name}.{critProt}";

                    if (nonHealth)
                    {
                        var vital = Spell.Category == SpellCategory.StaminaLowering ? "stamina" : "mana";
                        defenderMsg = $"{ProjectileSource.Name} casts {Spell.Name} and drains {amount} points of your {vital}.";
                    }

                    if (!targetPlayer.SquelchManager.Squelches.Contains(ProjectileSource, ChatMessageType.Magic))
                        targetPlayer.Session.Network.EnqueueSend(new GameMessageSystemChat(defenderMsg, ChatMessageType.Magic));
                }

                if (!nonHealth)
                {
                    if (target.HasCloakEquipped)
                        Cloak.TryProcSpell(target, ProjectileSource, percent);

                    target.EmoteManager.OnDamage(player);

                    if (critical)
                        target.EmoteManager.OnReceiveCritical(player);
                }
            }
            else
            {
                var lastDamager = ProjectileSource != null ? new DamageHistoryInfo(ProjectileSource) : null;
                target.OnDeath(lastDamager, Spell.DamageType, critical);
                target.Die();
            }
        }

        /// <summary>
        /// Sets the physics state for a launched projectile
        /// </summary>
        public void SetProjectilePhysicsState(WorldObject target, bool useGravity)
        {
            if (useGravity)
                GravityStatus = true;

            CurrentMotionState = null;
            Placement = null;

            // TODO: Physics description timestamps (sequence numbers) don't seem to be getting updated

            //Console.WriteLine("SpellProjectile PhysicsState: " + PhysicsObj.State);

            var pos = Location.Pos;
            var rotation = Location.Rotation;
            PhysicsObj.Position.Frame.Origin = pos;
            PhysicsObj.Position.Frame.Orientation = rotation;

            var velocity = Velocity;
            //velocity = Vector3.Transform(velocity, Matrix4x4.Transpose(Matrix4x4.CreateFromQuaternion(rotation)));
            PhysicsObj.Velocity = velocity;

            if (target != null)
                PhysicsObj.ProjectileTarget = target.PhysicsObj;

            PhysicsObj.set_active(true);
        }
    }
}
