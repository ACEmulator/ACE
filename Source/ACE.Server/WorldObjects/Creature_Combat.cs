using System;
using System.Linq;
using System.Numerics;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Motion;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Extensions;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        public CombatMode CombatMode { get; private set; }

        public DamageHistory DamageHistory;

        /// <summary>
        /// Switches a player or creature to a new combat stance
        /// </summary>
        public float SetCombatMode(CombatMode combatMode)
        {
            //Console.WriteLine($"Changing combat mode for {Name} to {combatMode}");

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
                    animLength = HandleSwitchToMeleeCombatMode();
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
            return animLength;
        }

        /// <summary>
        /// Switches a player or creature to non-combat mode
        /// </summary>
        public float HandleSwitchToPeaceMode()
        {
            var animLength = MotionTable.GetAnimationLength(MotionTableId, CurrentMotionState.Stance, MotionCommand.NonCombat, MotionCommand.Ready);

            var motion = new UniversalMotion(MotionStance.NonCombat);
            motion.MovementData.CurrentStyle = (uint)MotionStance.NonCombat;
            SetMotionState(this, motion);

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
        /// Switches a player or creature to melee attack stance
        /// </summary>
        public float HandleSwitchToMeleeCombatMode()
        {
            // get appropriate combat stance for currently wielded items
            var combatStance = GetCombatStance();

            var animLength = MotionTable.GetAnimationLength(MotionTableId, CurrentMotionState.Stance, (MotionCommand)combatStance, MotionCommand.Ready);

            var motion = new UniversalMotion(combatStance);
            motion.MovementData.CurrentStyle = (uint)combatStance;
            SetMotionState(this, motion);

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

            var animLength = MotionTable.GetAnimationLength(MotionTableId, CurrentMotionState.Stance, MotionCommand.Magic, MotionCommand.Ready);

            var motion = new UniversalMotion(MotionStance.Magic);
            motion.MovementData.CurrentStyle = (uint)MotionStance.Magic;
            SetMotionState(this, motion);

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

            var animLength = MotionTable.GetAnimationLength(MotionTableId, CurrentMotionState.Stance, (MotionCommand)combatStance, MotionCommand.Ready);

            var motion = new UniversalMotion(combatStance);
            motion.MovementData.CurrentStyle = (uint)combatStance;
            SetMotionState(this, motion);

            var ammo = GetEquippedAmmo();
            if (ammo != null && weapon.IsAmmoLauncher)
                animLength += ReloadMissileAmmo();

            var player = this as Player;
            if (player != null)
            {
                player.HandleActionTradeSwitchToCombatMode(player.Session);
                player.Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.CombatMode, (int)CombatMode.Missile));
            }
            //Console.WriteLine("HandleSwitchToMissileCombatMode() - animLength: " + animLength);
            return animLength;
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
            var weapon = GetEquippedWeapon();
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
        /// Returns the attribute damage bonus for a physical attack
        /// </summary>
        /// <param name="attackType">Uses strength for melee, coordination for missile</param>
        public float GetAttributeMod(AttackType attackType)
        {
            if (attackType == AttackType.Melee)
                return SkillFormula.GetAttributeMod(PropertyAttribute.Strength, (int)Strength.Current);
            else if (attackType == AttackType.Missile)
                return SkillFormula.GetAttributeMod(PropertyAttribute.Coordination, (int)Coordination.Current);
            else
                return 1.0f;
        }

        /// <summary>
        /// Returns the pre-MoA skill for a non-player creature
        /// </summary>
        public virtual Skill GetCurrentWeaponSkill()
        {
            var weapon = GetEquippedWeapon();
            if (weapon == null) return Skill.UnarmedCombat;

            var skill = (Skill)(weapon.GetProperty(PropertyInt.WieldSkilltype) ?? 0);
            //Console.WriteLine("Monster weapon skill: " + skill);

            return skill == Skill.None ? Skill.UnarmedCombat : skill;
        }


        private static double MinAttackSpeed = 0.5;
        private static double MaxAttackSpeed = 2.0;

        /// <summary>
        /// Returns the animation speed for an attack,
        /// based on the current quickness and weapon speed
        /// </summary>
        public float GetAnimSpeed()
        {
            var quickness = GetCreatureAttribute(PropertyAttribute.Quickness).Current;
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
        public virtual void OnEvade(WorldObject attacker, AttackType attackType)
        {
            // empty base for non-player creatures?
        }

        /// <summary>
        /// Called when a creature hits a target
        /// </summary>
        public virtual void OnDamageTarget(WorldObject target, AttackType attackType)
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
        /// <returns></returns>
        public string GetSplatterHeight()
        {
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
            var sourcePos = new Vector3(Location.PositionX, Location.PositionY, 0);
            var targetPos = new Vector3(target.Location.PositionX, target.Location.PositionY, 0);
            var targetDir = new AFrame(target.Location.Pos, target.Location.Rotation).get_vector_heading();

            targetDir.Z = 0;
            targetDir = targetDir.Normalize();

            var sourceToTarget = (sourcePos - targetPos).Normalize();

            var dir = Vector3.Dot(sourceToTarget, targetDir);
            var angle = Vector3.Cross(sourceToTarget, targetDir);

            var frontBack = dir >= 0 ? "Front" : "Back";
            var leftRight = angle.Z <= 0 ? "Left" : "Right";

            return leftRight + frontBack;
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
        public float GetShieldMod(WorldObject attacker, DamageType damageType)
        {
            // ensure combat stance
            if (CombatMode == CombatMode.NonCombat)
                return 1.0f;

            // does the player have a shield equipped?
            var shield = GetEquippedShield();
            if (shield == null) return 1.0f;

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
            var modSL = shield.EnchantmentManager.GetArmorMod();
            var effectiveSL = baseSL + modSL;

            // get shield RL against damage type
            var baseRL = GetResistance(shield, damageType);

            // shield RL item enchantment additives:
            // banes, lures
            var modRL = shield.EnchantmentManager.GetArmorModVsType(damageType);
            var effectiveRL = (float)(baseRL + modRL);

            // resistance cap
            if (effectiveRL > 2.0f)
                effectiveRL = 2.0f;

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

            // SL is multiplied by existing AL
            var shieldMod = SkillFormula.CalcArmorMod(effectiveLevel);
            //Console.WriteLine("ShieldMod: " + shieldMod);
            return shieldMod;
        }
    }
}
