using System;
using System.Numerics;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Extensions;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        public DamageHistory DamageHistory;

        public float GetAttributeMod(AttackType attackType)
        {
            if (attackType == AttackType.Melee)
                return SkillFormula.GetAttributeMod(PropertyAttribute.Strength, (int)Strength.Current);
            else if (attackType == AttackType.Missile)
                return SkillFormula.GetAttributeMod(PropertyAttribute.Coordination, (int)Coordination.Current);
            else
                return 1.0f;
        }

        public string GetSplatterHeight()
        {
            switch (AttackHeight.Value)
            {
                case ACE.Entity.Enum.AttackHeight.Low: return "Low";
                case ACE.Entity.Enum.AttackHeight.Medium: return "Mid";
                case ACE.Entity.Enum.AttackHeight.High: default: return "Up";
            }
        }

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


        public virtual float GetAimHeight(WorldObject target)
        {
            return 2.0f;
        }

        public float GetShieldMod(WorldObject attacker, DamageType damageType)
        {
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
