using System;
using System.Linq;

using ACE.Common;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Physics.Animation;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        public enum DebugDamageType
        {
            None     = 0x0,
            Attacker = 0x1,
            Defender = 0x2,
            All      = Attacker | Defender
        };

        public DebugDamageType DebugDamage;

        public ObjectGuid DebugDamageTarget;

        /// <summary>
        /// The list of combat maneuvers performable by this creature
        /// </summary>
        public DatLoader.FileTypes.CombatManeuverTable CombatTable { get; set; }

        public CombatMode CombatMode { get; protected set; }

        public AttackType AttackType { get; set; }

        public DamageHistory DamageHistory { get; private set; }

        /// <summary>
        /// Handles queueing up multiple animation sequences between packets
        /// ie., when a player switches from bow to sword combat,
        /// the client will send an unwield item packet for the bow first,
        /// queueing up a switch to peace mode, and then unarmed combat mode.
        /// next the client will send a wield item packet for the sword,
        /// queueing up the switch from unarmed combat -> peace mode -> bow combat
        /// </summary>
        public double LastWeaponSwap;

        public float SetCombatMode(CombatMode combatMode)
        {
            return SetCombatMode(combatMode, out var _);
        }

        /// <summary>
        /// Switches a player or creature to a new combat stance
        /// </summary>
        public float SetCombatMode(CombatMode combatMode, out float queueTime, bool forceHandCombat = false)
        {
            // check if combat stance actually needs switching
            var combatStance = forceHandCombat ? MotionStance.HandCombat : GetCombatStance();

            //Console.WriteLine($"{Name}.SetCombatMode({combatMode}), CombatStance: {combatStance}");

            if (combatMode != CombatMode.NonCombat && CurrentMotionState.Stance == combatStance)
            {
                queueTime = 0.0f;
                return 0.0f;
            }

            if (CombatMode == CombatMode.Missile)
                HideAmmo();

            CombatMode = combatMode;

            var animLength = 0.0f;

            switch (CombatMode)
            {
                case CombatMode.NonCombat:
                    animLength = HandleSwitchToPeaceMode();
                    break;
                case CombatMode.Melee:
                    animLength = HandleSwitchToMeleeCombatMode(forceHandCombat);
                    break;
                case CombatMode.Magic:
                    animLength = HandleSwitchToMagicCombatMode();
                    break;
                case CombatMode.Missile:
                    animLength = HandleSwitchToMissileCombatMode();
                    break;
                default:
                    log.InfoFormat($"Unknown combat mode {CombatMode} for {Name}");
                    break;
            }

            queueTime = HandleStanceQueue(animLength);

            //Console.WriteLine($"SetCombatMode(): queueTime({queueTime}) + animLength({animLength})");
            return queueTime + animLength;
        }

        /// <summary>
        /// Switches a player or creature to non-combat mode
        /// </summary>
        public float HandleSwitchToPeaceMode()
        {
            var animLength = MotionTable.GetAnimationLength(MotionTableId, CurrentMotionState.Stance, MotionCommand.Ready, MotionCommand.NonCombat);

            var motion = new Motion(MotionStance.NonCombat);
            ExecuteMotionPersist(motion);

            var player = this as Player;
            if (player != null)
            {
                player.stance = MotionStance.NonCombat;
                player.Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.CombatMode, (int)CombatMode.NonCombat));
            }

            //Console.WriteLine("HandleSwitchToPeaceMode() - animLength: " + animLength);
            return animLength;
        }

        /// <summary>
        /// Handles switching between combat stances:
        /// old style -> peace mode -> hand combat (weapon swap) -> peace mode -> new style
        /// </summary>
        public float SwitchCombatStyles()
        {
            if (CurrentMotionState.Stance == MotionStance.NonCombat || CurrentMotionState.Stance == MotionStance.Invalid || IsMonster)
                return 0.0f;

            var combatStance = GetCombatStance();

            float peace1 = 0.0f, unarmed = 0.0f, peace2 = 0.0f;

            // this is now handled as a proper 2-step process in HandleActionChangeCombatMode / NextUseTime

            // FIXME: just call generic method to switch to HandCombat first
            peace1 = MotionTable.GetAnimationLength(MotionTableId, CurrentMotionState.Stance, MotionCommand.Ready, MotionCommand.NonCombat);
            /*if (CurrentMotionState.Stance != MotionStance.HandCombat && combatStance != MotionStance.HandCombat)
            {
                unarmed = MotionTable.GetAnimationLength(MotionTableId, MotionStance.NonCombat, MotionCommand.Ready, MotionCommand.HandCombat);
                peace2 = MotionTable.GetAnimationLength(MotionTableId, MotionStance.HandCombat, MotionCommand.Ready, MotionCommand.NonCombat);
            }*/

            SetStance(MotionStance.NonCombat, false);

            //Console.WriteLine($"SwitchCombatStyle() - animLength: {animLength}");
            //Console.WriteLine($"SwitchCombatStyle() - peace1({peace1}) + unarmed({unarmed}) + peace2({peace2})");
            var animLength = peace1 + unarmed + peace2;
            return animLength;
        }

        /// <summary>
        /// Switches a player or creature to melee attack stance
        /// </summary>
        public float HandleSwitchToMeleeCombatMode(bool forceHandCombat = false)
        {
            // get appropriate combat stance for currently wielded items
            var combatStance = forceHandCombat ? MotionStance.HandCombat : GetCombatStance();

            var animLength = SwitchCombatStyles();
            animLength += MotionTable.GetAnimationLength(MotionTableId, CurrentMotionState.Stance, MotionCommand.Ready, (MotionCommand)combatStance);

            var motion = new Motion(combatStance);
            ExecuteMotionPersist(motion);

            var player = this as Player;
            if (player != null)
            {
                player.HandleActionTradeSwitchToCombatMode(player.Session);
                player.Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.CombatMode, (int)CombatMode.Melee));
            }

            //Console.WriteLine("HandleSwitchToMeleeCombatMode() - animLength: " + animLength);
            return animLength;
        }

        /// <summary>
        /// Switches a player or creature to magic casting stance
        /// </summary>
        public float HandleSwitchToMagicCombatMode()
        {
            var wand = GetEquippedWand();
            if (wand == null) return 0.0f;

            var animLength = SwitchCombatStyles();
            animLength += MotionTable.GetAnimationLength(MotionTableId, CurrentMotionState.Stance, MotionCommand.Ready, MotionCommand.Magic);

            var motion = new Motion(MotionStance.Magic);
            ExecuteMotionPersist(motion);

            var player = this as Player;
            if (player != null)
            {
                player.HandleActionTradeSwitchToCombatMode(player.Session);
                player.Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.CombatMode, (int)CombatMode.Magic));
            }

            //Console.WriteLine("HandleSwitchToMagicCombatMode() - animLength: " + animLength);
            return animLength;
        }

        /// <summary>
        /// Switches a player or creature to a missile combat stance
        /// </summary>
        public float HandleSwitchToMissileCombatMode()
        {
            // get appropriate combat stance for currently wielded items
            var weapon = GetEquippedMissileWeapon();
            if (weapon == null) return 0.0f;

            var combatStance = GetCombatStance();

            var swapTime = SwitchCombatStyles();

            var motion = new Motion(combatStance);
            var stanceTime = ExecuteMotionPersist(motion);

            var ammo = GetEquippedAmmo();
            var reloadTime = 0.0f;
            if (ammo != null && weapon.IsAmmoLauncher)
            {
                // bug for bow-wielding skeletons starting from decomposed state:
                // sleep -> wakeup anim time must be passed in here
                var actionChain = new ActionChain();

                var currentTime = Time.GetUnixTime();
                var queueTime = 0.0f;
                if (currentTime < LastWeaponSwap)
                    queueTime += (float)(LastWeaponSwap - currentTime);

                actionChain.AddDelaySeconds(queueTime + swapTime + stanceTime);
                reloadTime = ReloadMissileAmmo(actionChain);
                actionChain.EnqueueChain();
            }

            var player = this as Player;
            if (player != null)
            {
                player.HandleActionTradeSwitchToCombatMode(player.Session);
                player.Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.CombatMode, (int)CombatMode.Missile));
            }
            //Console.WriteLine("HandleSwitchToMissileCombatMode() - animLength: " + animLength);
            return swapTime + stanceTime + reloadTime;
        }

        /// <summary>
        /// Sends the message to hide the current equipped ammo
        /// </summary>
        public void HideAmmo()
        {
            var ammo = GetEquippedAmmo();
            if (ammo != null)
                EnqueueBroadcast(new GameMessagePickupEvent(ammo));
        }

        /// <summary>
        /// Returns the combat stance for the currently wielded items
        /// </summary>
        public MotionStance GetCombatStance()
        {
            var caster = GetEquippedWand();

            if (caster != null)
                return MotionStance.Magic;

            var weapon = GetEquippedWeapon(true);
            var dualWield = GetDualWieldWeapon();
            var shield = GetEquippedShield();

            var combatStance = MotionStance.HandCombat;

            if (weapon != null)
                combatStance = GetWeaponStance(weapon);

            if (dualWield != null)
                combatStance = MotionStance.DualWieldCombat;

            if (shield != null)
                combatStance = AddShieldStance(combatStance);

            return combatStance;
        }

        /// <summary>
        /// Translates the default combat style for a weapon
        /// into a combat motion stance
        /// </summary>
        public MotionStance GetWeaponStance(WorldObject weapon)
        {
            var combatStance = MotionStance.HandCombat;

            switch (weapon.DefaultCombatStyle)
            {
                case CombatStyle.Atlatl:
                    combatStance = MotionStance.AtlatlCombat;
                    break;
                case CombatStyle.Bow:
                    combatStance = MotionStance.BowCombat;
                    break;
                case CombatStyle.Crossbow:
                    combatStance = MotionStance.CrossbowCombat;
                    break;
                case CombatStyle.DualWield:
                    combatStance = MotionStance.DualWieldCombat;
                    break;
                case CombatStyle.Magic:
                    combatStance = MotionStance.Magic;
                    break;
                case CombatStyle.OneHanded:
                    combatStance = MotionStance.SwordCombat;
                    break;
                case CombatStyle.OneHandedAndShield:
                    combatStance = MotionStance.SwordShieldCombat;
                    break;
                case CombatStyle.Sling:
                    combatStance = MotionStance.SlingCombat;
                    break;
                case CombatStyle.ThrownShield:
                    combatStance = MotionStance.ThrownShieldCombat;
                    break;
                case CombatStyle.ThrownWeapon:
                    combatStance = MotionStance.ThrownWeaponCombat;
                    break;
                case CombatStyle.TwoHanded:
                    // MotionStance.TwoHandedStaffCombat doesn't appear to do anything
                    // Additionally, PropertyInt.WeaponType isn't always included, and the 2handed weapons that do appear to use WeaponType.TwoHanded
                    combatStance = MotionStance.TwoHandedSwordCombat;
                    break;
                case CombatStyle.Unarmed:
                    combatStance = MotionStance.HandCombat;
                    break;
                default:
                    Console.WriteLine($"{Name}.GetCombatStance() - {weapon.DefaultCombatStyle}");
                    break;
            }
            return combatStance;
        }

        /// <summary>
        /// Adds the shield stance to an existing combat stance
        /// </summary>
        public MotionStance AddShieldStance(MotionStance combatStance)
        {
            switch (combatStance)
            {
                case MotionStance.SwordCombat:
                    combatStance = MotionStance.SwordShieldCombat;
                    break;
                case MotionStance.ThrownWeaponCombat:
                    combatStance = MotionStance.ThrownShieldCombat;
                    break;
            }
            return combatStance;
        }

        /// <summary>
        /// Adds queued weapon swaps to the current animation time
        /// </summary>
        public float HandleStanceQueue(float animLength)
        {
            var currentTime = Time.GetUnixTime();
            if (currentTime >= LastWeaponSwap)
            {
                LastWeaponSwap = currentTime + animLength;
                return 0.0f;
            }
            else
            {
                LastWeaponSwap += animLength;
                return (float)(LastWeaponSwap - currentTime - animLength);
            }
        }

        /// <summary>
        /// Returns the attack type for non-player creatures
        /// </summary>
        public virtual CombatType GetCombatType()
        {
            return CurrentAttack ?? CombatType.Melee;
        }

        /// <summary>
        /// Returns a value between 0.5-1.5 for non-bow attacks,
        /// depending on the power bar meter
        /// </summary>
        public virtual float GetPowerMod(WorldObject weapon)
        {
            // doesn't apply for non-player creatures?
            return 1.0f;
        }

        /// <summary>
        /// Returns a value between 0.6-1.6 for bow attacks,
        /// depending on the accuracy meter
        /// </summary>
        public virtual float GetAccuracyMod(WorldObject weapon)
        {
            // doesn't apply for non-player creatures?
            return 1.0f;
        }

        /// <summary>
        /// Returns the attribute damage bonus for a physical attack
        /// </summary>
        /// <param name="attackType">Uses strength for melee, coordination for missile</param>
        public float GetAttributeMod(WorldObject weapon)
        {
            var isBow = weapon != null && weapon.IsBow;

            //var attribute = isBow || GetCurrentWeaponSkill() == Skill.FinesseWeapons ? Coordination : Strength;
            var attribute = isBow || weapon?.WeaponSkill == Skill.FinesseWeapons ? Coordination : Strength;

            return SkillFormula.GetAttributeMod((int)attribute.Current, isBow);
        }

        /// <summary>
        /// Returns the current attack skill for this monster,
        /// given their stance and wielded weapon
        /// </summary>
        public virtual Skill GetCurrentAttackSkill()
        {
            return GetCurrentWeaponSkill();
        }

        /// <summary>
        /// Returns the current weapon skill for non-player creatures
        /// </summary>
        public virtual Skill GetCurrentWeaponSkill()
        {
            var weapon = GetEquippedWeapon();

            var skill = weapon != null ? weapon.WeaponSkill : Skill.UnarmedCombat;

            var creatureSkill = GetCreatureSkill(skill);

            if (creatureSkill.InitLevel == 0)
            {
                // convert to post-MoA skill
                if (weapon != null && weapon.IsRanged)
                    skill = Skill.MissileWeapons;
                else if (skill == Skill.Sword)
                    skill = Skill.HeavyWeapons;
                else if (skill == Skill.Dagger)
                    skill = Skill.FinesseWeapons;
                else
                    skill = Skill.LightWeapons;
            }

            //Console.WriteLine("Monster weapon skill: " + skill);

            return skill;
        }

        /// <summary>
        /// Returns the effective attack skill for a non-player creature,
        /// ie. with Heart Seeker bonus
        /// </summary>
        public virtual uint GetEffectiveAttackSkill()
        {
            var attackSkill = GetCreatureSkill(GetCurrentAttackSkill()).Current;

            // TODO: don't use for bow?
            // https://asheron.fandom.com/wiki/Developer_Chat_-_2002/09/23
            var offenseMod = GetWeaponOffenseModifier(this);

            // monsters don't use accuracy mod?

            return (uint)Math.Round(attackSkill * offenseMod);
        }

        /// <summary>
        /// Returns the effective defense skill for a player or creature,
        /// ie. with Defender bonus and imbues
        /// </summary>
        public uint GetEffectiveDefenseSkill(CombatType combatType)
        {
            var defenseSkill = combatType == CombatType.Missile ? Skill.MissileDefense : Skill.MeleeDefense;
            var defenseMod = defenseSkill == Skill.MissileDefense ? GetWeaponMissileDefenseModifier(this) : GetWeaponMeleeDefenseModifier(this);
            var burdenMod = GetBurdenMod();

            var imbuedEffectType = defenseSkill == Skill.MissileDefense ? ImbuedEffectType.MissileDefense : ImbuedEffectType.MeleeDefense;
            var defenseImbues = GetDefenseImbues(imbuedEffectType);

            var stanceMod = this is Player player ? player.GetDefenseStanceMod() : 1.0f;

            //if (this is Player)
                //Console.WriteLine($"StanceMod: {stanceMod}");

            var effectiveDefense = (uint)Math.Round(GetCreatureSkill(defenseSkill).Current * defenseMod * burdenMod * stanceMod + defenseImbues);

            if (IsExhausted) effectiveDefense = 0;

            return effectiveDefense;
        }


        private static double MinAttackSpeed = 0.5;
        private static double MaxAttackSpeed = 2.0;

        /// <summary>
        /// Returns the animation speed for an attack,
        /// based on the current quickness and weapon speed
        /// </summary>
        public float GetAnimSpeed()
        {
            var quickness = Quickness.Current;
            var weaponSpeed = GetWeaponSpeed(this);

            var divisor = 1.0 - (quickness / 300.0) + (weaponSpeed / 150.0);
            if (divisor <= 0)
                return (float)MaxAttackSpeed;

            var animSpeed = (float)Math.Clamp((1.0 / divisor), MinAttackSpeed, MaxAttackSpeed);

            return animSpeed;
        }

        /// <summary>
        /// Called when a creature evades an attack
        /// </summary>
        public virtual void OnEvade(WorldObject attacker, CombatType attackType)
        {
            // empty base for non-player creatures?
        }

        /// <summary>
        /// Called when a creature hits a target
        /// </summary>
        public virtual void OnDamageTarget(WorldObject target, CombatType attackType, bool critical)
        {
            // empty base for non-player creatures?
        }

        /// <summary>
        /// Returns the current attack height as an enumerable string
        /// </summary>
        public string GetAttackHeight()
        {
            return AttackHeight?.GetString();
        }

        /// <summary>
        /// Returns the splatter height for the current attack height
        /// </summary>
        public string GetSplatterHeight()
        {
            if (AttackHeight == null) return "Mid";

            switch (AttackHeight.Value)
            {
                case ACE.Entity.Enum.AttackHeight.Low: return "Low";
                case ACE.Entity.Enum.AttackHeight.Medium: return "Mid";
                case ACE.Entity.Enum.AttackHeight.High: default: return "Up";
            }
        }

        /// <summary>
        /// Returns the splatter direction quadrant string
        /// </summary>
        public string GetSplatterDir(WorldObject target)
        {
            var quadrant = GetRelativeDir(target);

            var splatterDir = quadrant.HasFlag(Quadrant.Left) ? "Left" : "Right";
            splatterDir += quadrant.HasFlag(Quadrant.Front) ? "Front" : "Back";

            return splatterDir;
        }

        public double GetLifeResistance(DamageType damageType)
        {
            double resistance = 1.0;

            switch (damageType)
            {
                case DamageType.Slash:
                    resistance = ResistSlashMod;
                    break;

                case DamageType.Pierce:
                    resistance = ResistPierceMod;
                    break;

                case DamageType.Bludgeon:
                    resistance = ResistBludgeonMod;
                    break;

                case DamageType.Fire:
                    resistance = ResistFireMod;
                    break;

                case DamageType.Cold:
                    resistance = ResistColdMod;
                    break;

                case DamageType.Acid:
                    resistance = ResistAcidMod;
                    break;

                case DamageType.Electric:
                    resistance = ResistElectricMod;
                    break;

                case DamageType.Nether:
                    resistance = ResistNetherMod;
                    break;
            }

            return resistance;
        }

        /// <summary>
        /// Reduces a creatures's attack skill while exhausted
        /// </summary>
        public uint GetExhaustedSkill(uint attackSkill)
        {
            var halfSkill = (uint)Math.Round(attackSkill / 2.0f);

            uint maxPenalty = 50;
            var reducedSkill = attackSkill >= maxPenalty ? attackSkill - maxPenalty : 0;

            return Math.Max(reducedSkill, halfSkill);
        }

        /// <summary>
        /// Returns a divisor for the target height
        /// for aiming projectiles
        /// </summary>
        public virtual float GetAimHeight(WorldObject target)
        {
            return 2.0f;
        }

        /// <summary>
        /// Return the scalar damage absorbed by a shield
        /// </summary>
        public float GetShieldMod(WorldObject attacker, DamageType damageType, WorldObject weapon)
        {
            // ensure combat stance
            if (CombatMode == CombatMode.NonCombat)
                return 1.0f;

            // does the player have a shield equipped?
            var shield = GetEquippedShield();
            if (shield == null) return 1.0f;

            // phantom weapons ignore all armor and shields
            if (weapon != null && weapon.HasImbuedEffect(ImbuedEffectType.IgnoreAllArmor))
                return 1.0f;

            // is monster in front of player,
            // within shield effectiveness area?
            var effectiveAngle = 180.0f;
            var angle = GetAngle(attacker);
            if (Math.Abs(angle) > effectiveAngle / 2.0f)
                return 1.0f;

            // get base shield AL
            var baseSL = shield.GetProperty(PropertyInt.ArmorLevel) ?? 0.0f;

            // shield AL item enchantment additives:
            // impenetrability, brittlemail
            var ignoreMagicArmor = (weapon?.IgnoreMagicArmor ?? false) || (attacker?.IgnoreMagicArmor ?? false);

            var modSL = shield.EnchantmentManager.GetArmorMod();

            if (ignoreMagicArmor)
                modSL = attacker is Player ? (int)Math.Round(IgnoreMagicArmorScaled(modSL)) : 0;

            var effectiveSL = baseSL + modSL;

            // get shield RL against damage type
            var baseRL = GetResistance(shield, damageType);

            // shield RL item enchantment additives:
            // banes, lures
            var modRL = shield.EnchantmentManager.GetArmorModVsType(damageType);

            if (ignoreMagicArmor)
                modRL = attacker is Player ? IgnoreMagicArmorScaled(modRL) : 0.0f;

            var effectiveRL = (float)(baseRL + modRL);

            // resistance clamp
            effectiveRL = Math.Clamp(effectiveRL, -2.0f, 2.0f);

            // handle negative SL
            //if (effectiveSL < 0 && effectiveRL != 0)
                //effectiveRL = 1.0f / effectiveRL;

            var effectiveLevel = effectiveSL * effectiveRL;

            // SL cap:
            // Trained / untrained: 1/2 shield skill
            // Spec: shield skill
            // SL cap is applied *after* item enchantments
            var shieldSkill = GetCreatureSkill(Skill.Shield);
            var shieldCap = shieldSkill.Current;
            if (shieldSkill.AdvancementClass != SkillAdvancementClass.Specialized)
                shieldCap = (uint)Math.Round(shieldCap / 2.0f);

            effectiveLevel = Math.Min(effectiveLevel, shieldCap);

            var ignoreShieldMod = attacker.GetIgnoreShieldMod(weapon);
            //Console.WriteLine($"IgnoreShieldMod: {ignoreShieldMod}");

            effectiveLevel *= ignoreShieldMod;

            // SL is multiplied by existing AL
            var shieldMod = SkillFormula.CalcArmorMod(effectiveLevel);
            //Console.WriteLine("ShieldMod: " + shieldMod);
            return shieldMod;
        }

        /// <summary>
        /// Returns the total applicable Recklessness modifier,
        /// taking into account both attacker and defender players
        /// </summary>
        public static float GetRecklessnessMod(Creature attacker, Creature defender)
        {
            var playerAttacker = attacker as Player;
            var playerDefender = defender as Player;

            var recklessnessMod = 1.0f;

            // multiplicative or additive?
            // defender is a negative Damage Reduction Rating
            // 20 DR combined with 20 DRR = 1.2 * 0.8333... = 1.0
            // 20 DR combined with -20 DRR = 1.2 * 1.2 = 1.44
            if (playerAttacker != null)
                recklessnessMod *= playerAttacker.GetRecklessnessMod();

            if (playerDefender != null)
                recklessnessMod *= playerDefender.GetRecklessnessMod();

            return recklessnessMod;
        }

        public float GetSneakAttackMod(WorldObject target)
        {
            // ensure trained
            var sneakAttack = GetCreatureSkill(Skill.SneakAttack);
            if (sneakAttack.AdvancementClass < SkillAdvancementClass.Trained)
                return 1.0f;

            // ensure creature target
            var creatureTarget = target as Creature;
            if (creatureTarget == null)
                return 1.0f;

            // Effects:
            // General Sneak Attack effects:
            //   - 100% chance to sneak attack from behind an opponent.
            //   - Deception trained: 10% chance to sneak attack from the front of an opponent
            //   - Deception specialized: 15% chance to sneak attack from the front of an opponent
            var angle = creatureTarget.GetAngle(this);
            var behind = Math.Abs(angle) > 90.0f;
            var chance = 0.0f;
            if (behind)
            {
                chance = 1.0f;
            }
            else
            {
                var deception = GetCreatureSkill(Skill.Deception);
                if (deception.AdvancementClass == SkillAdvancementClass.Trained)
                    chance = 0.1f;
                else if (deception.AdvancementClass == SkillAdvancementClass.Specialized)
                    chance = 0.15f;

                // if Deception is below 306 skill, these chances are reduced proportionately.
                // this is in addition to proprtional reduction if your Sneak Attack skill is below your attack skill.
                var deceptionCap = 306;
                if (deception.Current < deceptionCap)
                    chance *= Math.Min((float)deception.Current / deceptionCap, 1.0f);
            }
            //Console.WriteLine($"Sneak attack {(behind ? "behind" : "front")}, chance {Math.Round(chance * 100)}%");

            var rng = ThreadSafeRandom.Next(0.0f, 1.0f);
            if (rng >= chance)
                return 1.0f;

            // Damage Rating:
            // Sneak Attack Trained:
            //   + 10 Damage Rating when Sneak Attack activates
            // Sneak Attack Specialized:
            //   + 20 Damage Rating when Sneak Attack activates
            var damageRating = sneakAttack.AdvancementClass == SkillAdvancementClass.Specialized ? 20.0f : 10.0f;

            // Sneak Attack works for melee, missile, and magic attacks.

            // if the Sneak Attack skill is lower than your attack skill (as determined by your equipped weapon)
            // then the damage rating is reduced proportionately. Because the damage rating caps at 10 for trained
            // and 20 for specialized, there is no reason to raise the skill above your attack skill
            var attackSkill = GetCreatureSkill(GetCurrentAttackSkill());
            if (sneakAttack.Current < attackSkill.Current)
            {
                if (attackSkill.Current > 0)
                    damageRating *= (float)sneakAttack.Current / attackSkill.Current;
                else
                    damageRating = 0;
            }

            // if the defender has Assess Person, they reduce the extra Sneak Attack damage Deception can add
            // from the front by up to 100%.
            // this percent is reduced proportionately if your buffed Assess Person skill is below the deception cap.
            // this reduction does not apply to attacks from behind.
            if (!behind)
            {
                // compare to assess person or deception??
                // wiki info is confusing here, it says 'your buffed Assess Person'
                // which sounds like its scaling sourceAssess / targetAssess,
                // but i think it should be targetAssess / deceptionCap?
                var targetAssess = creatureTarget.GetCreatureSkill(Skill.AssessPerson).Current;

                var deceptionCap = 306;
                damageRating *= 1.0f - Math.Min((float)targetAssess / deceptionCap, 1.0f);
            }

            var sneakAttackMod = (100 + damageRating) / 100.0f;
            //Console.WriteLine("SneakAttackMod: " + sneakAttackMod);
            return sneakAttackMod;
        }

        public void FightDirty(WorldObject target, WorldObject weapon)
        {
            // Skill description:
            // Your melee and missile attacks have a chance to weaken your opponent.
            // - Low attacks can reduce the defense skills of the opponent.
            // - Medium attacks can cause small amounts of bleeding damage.
            // - High attacks can reduce opponents' attack and healing skills

            // Effects:
            // Low: reduces the defense skills of the opponent by -10
            // Medium: bleed ticks for 60 damage per 20 seconds
            // High: reduces the attack skills of the opponent by -10, and
            //       the healing effects of the opponent by -15 rating
            //
            // these damage #s are doubled for dirty fighting specialized.

            // Notes:
            // - Dirty fighting works for melee and missile attacks.
            // - Has a 25% chance to activate on any melee of missile attack.
            //   - This activation is reduced proportionally if Dirty Fighting is lower
            //     than your active weapon skill as determined by your equipped weapon.
            // - All activate effects last 20 seconds.
            // - Although a specific effect won't stack with itself,
            //   you can stack all 3 effects on the opponent at the same time. This means
            //   when a skill activates at one attack height, you can move to another attack height
            //   to try to land an additional effect.
            // - Successfully landing a Dirty Fighting effect is mentioned in chat. Additionally,
            //   the medium height effect results in 'floating glyphs' around the target:

            //   "Dirty Fighting! <Player> delivers a Bleeding Assault to <target>!"
            //   "Dirty Fighting! <Player> delivers a Traumatic Assault to <target>!"

            // dirty fighting skill must be at least trained
            var dirtySkill = GetCreatureSkill(Skill.DirtyFighting);
            if (dirtySkill.AdvancementClass < SkillAdvancementClass.Trained)
                return;

            // ensure creature target
            var creatureTarget = target as Creature;
            if (creatureTarget == null)
                return;

            var chance = 0.25f;

            var attackSkill = GetCreatureSkill(GetCurrentWeaponSkill());
            if (dirtySkill.Current < attackSkill.Current)
            {
                chance *= (float)dirtySkill.Current / attackSkill.Current;
            }

            var rng = ThreadSafeRandom.Next(0.0f, 1.0f);
            if (rng >= chance)
                return;

            switch (AttackHeight)
            {
                case ACE.Entity.Enum.AttackHeight.Low:
                    FightDirty_ApplyLowAttack(creatureTarget, weapon);
                    break;
                case ACE.Entity.Enum.AttackHeight.Medium:
                    FightDirty_ApplyMediumAttack(creatureTarget, weapon);
                    break;
                case ACE.Entity.Enum.AttackHeight.High:
                    FightDirty_ApplyHighAttack(creatureTarget, weapon);
                    break;
            }
        }

        /// <summary>
        /// Reduces the defense skills of the opponent by
        /// -10 if trained, or -20 if specialized
        /// </summary>
        public void FightDirty_ApplyLowAttack(Creature target, WorldObject weapon)
        {
            var spellID = GetCreatureSkill(Skill.DirtyFighting).AdvancementClass == SkillAdvancementClass.Specialized ?
                SpellId.DF_Specialized_DefenseDebuff : SpellId.DF_Trained_DefenseDebuff;

            var spell = new Spell(spellID);
            if (spell.NotFound) return;  // TODO: friendly message to install DF patch

            target.EnchantmentManager.Add(spell, this, weapon);
            target.EnqueueBroadcast(new GameMessageScript(target.Guid, PlayScript.DirtyFightingDefenseDebuff));

            FightDirty_SendMessage(target, spell);
        }

        /// <summary>
        /// Applies bleed ticks for 60 damage per 20 seconds if trained,
        /// 120 damage per 20 seconds if specialized
        /// </summary>
        /// <returns></returns>
        public void FightDirty_ApplyMediumAttack(Creature target, WorldObject weapon)
        {
            var spellID = GetCreatureSkill(Skill.DirtyFighting).AdvancementClass == SkillAdvancementClass.Specialized ?
                SpellId.DF_Specialized_Bleed : SpellId.DF_Trained_Bleed;

            var spell = new Spell(spellID);
            if (spell.NotFound) return;  // TODO: friendly message to install DF patch

            target.EnchantmentManager.Add(spell, this, weapon);

            // only send if not already applied?
            target.EnqueueBroadcast(new GameMessageScript(target.Guid, PlayScript.DirtyFightingDamageOverTime));

            FightDirty_SendMessage(target, spell);
        }

        /// <summary>
        /// Reduces the attack skills and healing rating for opponent
        /// by -10 if trained, or -20 if specialized
        /// </summary>
        public void FightDirty_ApplyHighAttack(Creature target, WorldObject weapon)
        {
            // attack debuff
            var spellID = GetCreatureSkill(Skill.DirtyFighting).AdvancementClass == SkillAdvancementClass.Specialized ?
                SpellId.DF_Specialized_AttackDebuff : SpellId.DF_Trained_AttackDebuff;

            var spell = new Spell(spellID);
            if (spell.NotFound) return;  // TODO: friendly message to install DF patch

            target.EnchantmentManager.Add(spell, this, weapon);
            target.EnqueueBroadcast(new GameMessageScript(target.Guid, PlayScript.DirtyFightingAttackDebuff));

            FightDirty_SendMessage(target, spell);

            // healing resistance rating
            spellID = GetCreatureSkill(Skill.DirtyFighting).AdvancementClass == SkillAdvancementClass.Specialized ?
                SpellId.DF_Specialized_HealingDebuff : SpellId.DF_Trained_HealingDebuff;

            spell = new Spell(spellID);
            if (spell.NotFound) return;  // TODO: friendly message to install DF patch

            target.EnchantmentManager.Add(spell, this, weapon);
            target.EnqueueBroadcast(new GameMessageScript(target.Guid, PlayScript.DirtyFightingHealDebuff));

            FightDirty_SendMessage(target, spell);
        }

        public void FightDirty_SendMessage(Creature target, Spell spell)
        {
            // Dirty Fighting! <Player> delivers a <sic> Unbalancing Blow to <target>!
            //var article = spellBase.Name.StartsWithVowel() ? "an" : "a";

            var msg = $"Dirty Fighting! {Name} delivers a {spell.Name} to {target.Name}!";

            var playerSource = this as Player;
            var playerTarget = target as Player;

            if (playerSource != null)
                playerSource.SendMessage(msg, ChatMessageType.Combat, this);
            if (playerTarget != null)
                playerTarget.SendMessage(msg, ChatMessageType.Combat, this);
        }

        /// <summary>
        /// Returns TRUE if the creature receives a +5 DR bonus for this weapon type
        /// </summary>
        public virtual bool GetHeritageBonus(WorldObject weapon)
        {
            // only for players
            return false;
        }

        /// <summary>
        /// Returns a ResistanceType for a DamageType
        /// </summary>
        public static ResistanceType GetResistanceType(DamageType damageType)
        {
            switch (damageType)
            {
                case DamageType.Slash:
                    return ResistanceType.Slash;
                case DamageType.Pierce:
                    return ResistanceType.Pierce;
                case DamageType.Bludgeon:
                    return ResistanceType.Bludgeon;
                case DamageType.Fire:
                    return ResistanceType.Fire;
                case DamageType.Cold:
                    return ResistanceType.Cold;
                case DamageType.Acid:
                    return ResistanceType.Acid;
                case DamageType.Electric:
                    return ResistanceType.Electric;
                case DamageType.Nether:
                    return ResistanceType.Nether;
                case DamageType.Health:
                    return ResistanceType.HealthDrain;
                case DamageType.Stamina:
                    return ResistanceType.StaminaDrain;
                case DamageType.Mana:
                    return ResistanceType.ManaDrain;
                default:
                    return ResistanceType.Undef;
            }
        }

        public virtual bool CanDamage(Creature target)
        {
            if (target is Player)
            {
                // monster attacking player
                return true;    // other checks handled elsewhere
            }
            else
            {
                // monster attacking monster
                var sourcePet = this is CombatPet;
                var targetPet = target is CombatPet;

                if (sourcePet || targetPet)
                {
                    if (sourcePet && targetPet)     // combat pets can't damage other pets
                        return false;
                    else
                        return true;
                }

                // faction mobs
                if (Faction1Bits != null || target.Faction1Bits != null)
                {
                    if (AllowFactionCombat(target))
                        return true;
                }

                // handle FoeType
                if (PotentialFoe(target))
                    return true;

                return false;
            }
        }

        public static Skill GetDefenseSkill(CombatType combatType)
        {
            switch (combatType)
            {
                case CombatType.Melee:
                    return Skill.MeleeDefense;
                case CombatType.Missile:
                    return Skill.MissileDefense;
                case CombatType.Magic:
                    return Skill.MagicDefense;
                default:
                    return Skill.None;
            }
        }

        /// <summary>
        /// If one of these fields is set, potential aggro from Player or CombatPet movement terminates immediately
        /// </summary>
        protected static readonly Tolerance PlayerCombatPet_MoveExclude = Tolerance.NoAttack | Tolerance.Appraise | Tolerance.Provoke | Tolerance.Retaliate | Tolerance.Monster;

        /// <summary>
        /// If one of these fields is set, potential aggro from other monster movement terminates immediately
        /// </summary>
        protected static readonly Tolerance Monster_MoveExclude = Tolerance.NoAttack | Tolerance.Appraise | Tolerance.Provoke | Tolerance.Retaliate;

        /// <summary>
        /// If one of these fields is set, potential aggro from Player or CombatPet attacks terminates immediately
        /// </summary>
        protected static readonly Tolerance PlayerCombatPet_RetaliateExclude = Tolerance.NoAttack | Tolerance.Monster;

        /// <summary>
        /// Wakes up a monster if it can be alerted
        /// </summary>
        public bool AlertMonster(Creature monster)
        {
            // currently used for proximity checking exclusively:

            // Player_Monster.CheckMonsters() - player movement
            // Monster_Awareness.CheckTargets_Inner() - monster spawning in
            // Monster_Awareness.FactionMob_CheckMonsters() - faction mob scanning

            // non-attackable creatures do not get aggroed,
            // unless they have a TargetingTactic, such as the invisible archers in Oswald's Dirk Quest
            if (!monster.Attackable && monster.TargetingTactic == TargetingTactic.None)
                return false;

            // ensure monster is currently in idle state to wake up,
            // and it has no tolerance to players running nearby
            // TODO: investigate usage for tolerance
            var tolerance = this is Player ? PlayerCombatPet_MoveExclude : Monster_MoveExclude;

            if (monster.MonsterState != State.Idle || (monster.Tolerance & tolerance) != 0)
                return false;

            // for faction mobs, ensure alerter doesn't belong to same faction
            if (SameFaction(monster) && !PotentialFoe(monster))
                return false;

            // add to retaliate targets?

            //Console.WriteLine($"[{Timers.RunningTime}] - {monster.Name} ({monster.Guid}) - waking up");
            monster.AttackTarget = this;
            monster.WakeUp();

            return true;
        }

        /// <summary>
        /// Returns the damage type for the currently equipped weapon / ammo
        /// </summary>
        /// <param name="multiple">If true, returns all of the damage types for the weapon</param>
        public virtual DamageType GetDamageType(bool multiple = false, CombatType? combatType = null)
        {
            // old method, keeping intact for monsters
            var weapon = GetEquippedWeapon();
            var ammo = GetEquippedAmmo();

            if (weapon == null)
                return DamageType.Bludgeon;

            if (combatType == null)
                combatType = GetCombatType();

            var damageSource = combatType == CombatType.Melee || ammo == null || !weapon.IsAmmoLauncher ? weapon : ammo;

            var damageTypes = damageSource.W_DamageType;

            // returning multiple damage types
            if (multiple) return damageTypes;

            // get single damage type
            var motion = CurrentMotionState.MotionState.ForwardCommand.ToString();
            foreach (DamageType damageType in Enum.GetValues(typeof(DamageType)))
            {
                if ((damageTypes & damageType) != 0)
                {
                    // handle multiple damage types
                    if (damageType == DamageType.Slash && motion.Contains("Thrust"))
                        continue;

                    return damageType;
                }
            }
            return damageTypes;
        }

        /// <summary>
        /// Flag indicates which overpower formula is used
        /// True  = Formula A / ratings method
        /// False = Formula B / critical defense method
        /// </summary>
        public static bool OverpowerMethod = false;

        public static bool GetOverpower(Creature attacker, Creature defender)
        {
            if (OverpowerMethod)
                return GetOverpower_Method_A(attacker, defender);
            else
                return GetOverpower_Method_B(attacker, defender);
        }

        public static bool GetOverpower_Method_A(Creature attacker, Creature defender)
        {
            // implemented similar to ratings
            if (attacker.Overpower == null)
                return false;

            var overpowerChance = attacker.Overpower.Value;
            if (defender.OverpowerResist != null)
                overpowerChance -= defender.OverpowerResist.Value;

            //Console.WriteLine($"Overpower chance: {GetOverpowerChance_Method_A(attacker, defender)}");

            if (overpowerChance <= 0)
                return false;

            var rng = ThreadSafeRandom.Next(0.0f, 1.0f);

            return rng < overpowerChance * 0.01f;
        }

        public static bool GetOverpower_Method_B(Creature attacker, Creature defender)
        {
            // implemented similar to critical defense
            if (attacker.Overpower == null)
                return false;

            var overpowerChance = attacker.Overpower.Value;

            //Console.WriteLine($"Overpower chance: {GetOverpowerChance_Method_B(attacker, defender)}");

            var rng = ThreadSafeRandom.Next(0.0f, 1.0f);

            if (rng >= overpowerChance * 0.01f)
                return false;

            if (defender.OverpowerResist == null)
                return true;

            var resistChance = defender.OverpowerResist.Value;

            rng = ThreadSafeRandom.Next(0.0f, 1.0f);

            return rng >= resistChance * 0.01f;
        }

        public static float GetOverpowerChance(Creature attacker, Creature defender)
        {
            if (OverpowerMethod)
                return GetOverpowerChance_Method_A(attacker, defender);
            else
                return GetOverpowerChance_Method_B(attacker, defender);
        }

        public static float GetOverpowerChance_Method_A(Creature attacker, Creature defender)
        {
            if (attacker.Overpower == null)
                return 0.0f;

            var overpowerChance = attacker.Overpower.Value;
            if (defender.OverpowerResist != null)
                overpowerChance -= defender.OverpowerResist.Value;

            if (overpowerChance <= 0)
                return 0.0f;

            return overpowerChance * 0.01f;
        }

        public static float GetOverpowerChance_Method_B(Creature attacker, Creature defender)
        {
            if (attacker.Overpower == null)
                return 0.0f;

            var overpowerChance = (attacker.Overpower ?? 0) * 0.01f;
            var overpowerResistChance = (defender.OverpowerResist ?? 0) * 0.01f;

            return overpowerChance * (1.0f - overpowerResistChance);
        }

        /// <summary>
        /// Returns the number of equipped items with a particular imbue type
        /// </summary>
        public int GetDefenseImbues(ImbuedEffectType imbuedEffectType)
        {
            return EquippedObjects.Values.Count(i => i.GetImbuedEffects().HasFlag(imbuedEffectType));
        }

        /// <summary>
        /// Returns the cloak the creature has equipped,
        /// or 'null' if no cloak is equipped
        /// </summary>
        public WorldObject EquippedCloak => EquippedObjects.Values.FirstOrDefault(i => i.ValidLocations == EquipMask.Cloak);

        /// <summary>
        /// Returns TRUE if creature has cloak equipped
        /// </summary>
        public bool HasCloakEquipped => EquippedCloak != null;

        /// <summary>
        /// Called when a monster attacks another monster
        /// This should only happen between mobs of differing factions, or from FoeType
        /// </summary>
        public void MonsterOnAttackMonster(Creature monster)
        {
            /*Console.WriteLine($"{Name}.MonsterOnAttackMonster({monster.Name})");
            Console.WriteLine($"Attackable: {monster.Attackable}");
            Console.WriteLine($"Tolerance: {monster.Tolerance}");*/

            // when a faction mob attacks a regular mob, the regular mob will retaliate against the faction mob
            if (Faction1Bits != null && (monster.Faction1Bits == null || (Faction1Bits & monster.Faction1Bits) == 0))
                monster.AddRetaliateTarget(this);

            // when a monster with a FoeType attacks a foe, the foe will retaliate
            if (FoeType != null && (monster.FoeType == null || monster.FoeType != CreatureType))
                monster.AddRetaliateTarget(this);

            if (monster.MonsterState == State.Idle && !monster.Tolerance.HasFlag(Tolerance.NoAttack))
            {
                monster.AttackTarget = this;
                monster.WakeUp();
            }
        }

        /// <summary>
        /// Returns TRUE if creatures are both in the same faction
        /// </summary>
        public bool SameFaction(Creature creature)
        {
            return Faction1Bits != null && creature.Faction1Bits != null && (Faction1Bits & creature.Faction1Bits) != 0;
        }

        /// <summary>
        /// Returns TRUE is this creature has a FoeType that matches the input creature's CreatureType,
        /// or if the input creature has a FoeType that matches this creature's CreatureType
        /// </summary>
        public bool PotentialFoe(Creature creature)
        {
            return FoeType != null && FoeType == creature.CreatureType || creature.FoeType != null && creature.FoeType == CreatureType;
        }

        public bool AllowFactionCombat(Creature creature)
        {
            if (Faction1Bits == null && creature.Faction1Bits == null)
                return false;

            var factionSelf = Faction1Bits ?? FactionBits.None;
            var factionOther = creature.Faction1Bits ?? FactionBits.None;

            return (factionSelf & factionOther) == 0;
        }

        public void AddRetaliateTarget(WorldObject wo)
        {
            PhysicsObj.ObjMaint.AddRetaliateTarget(wo.PhysicsObj);
        }

        public bool HasRetaliateTarget(WorldObject wo)
        {
            if (wo != null)
                return PhysicsObj.ObjMaint.RetaliateTargetsContainsKey(wo.Guid.Full);
            else
                return false;
        }

        public void ClearRetaliateTargets()
        {
            PhysicsObj.ObjMaint.ClearRetaliateTargets();
        }
    }
}
