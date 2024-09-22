using System;

using ACE.Common;
using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    public class Cloak
    {
        /// <summary>
        /// Opt out of the standarad ACE formula and proc at a custom rate
        /// This setting will affect cloaks with any proc, including -200
        /// </summary>
        private static bool UseCustomMath => PropertyManager.GetBool("use_cloak_proc_custom_scale", false).Item;

        /// <summary>
        /// The maximum frequency of cloak procs, in seconds
        /// </summary>
        private static double MinDelay
        {
            get
            {
                if (!UseCustomMath)
                {
                    return 5.0;
                }
                return PropertyManager.GetDouble("cloak_cooldown_seconds").Item;
            }
        }

        /// <summary>
        /// The base maximum percentage at which cloaks will proc
        /// </summary>
        private static float MaxProcBase
        {
            get
            {
                if (!UseCustomMath)
                {
                    return 0.25f;
                }
                return Convert.ToSingle(PropertyManager.GetDouble("cloak_max_proc_base").Item);
            }
        }

        /// <summary>
        /// The minimum proc chance of a cloak
        /// </summary>
        private static float MinProc
        {
            get
            {
                if (!UseCustomMath)
                {
                    return 0f;
                }
                return Convert.ToSingle(PropertyManager.GetDouble("cloak_min_proc_base", 0f).Item);
            }
        }

        /// <summary>
        /// The base maxiumum percentage at which cloaks with
        /// -200 will proc
        /// </summary>
        private static float MaxProcBase200
        {
            get
            {
                if (!UseCustomMath)
                {
                    return 0.15f;
                }
                return Convert.ToSingle(PropertyManager.GetDouble("cloak_max_proc_base").Item);
            }
        }

        private const float TwoThirds = 2.0f / 3.0f;

        /// <summary>
        /// Rolls for a chance at procing a cloak spell
        /// If successful, casts the spell
        /// </summary>
        public static bool TryProcSpell(Creature defender, WorldObject attacker, WorldObject cloak, float damage_percent)
        {
            if (cloak == null) return false;

            if (!RollProc(cloak, damage_percent))
                return false;

            return HandleProcSpell(defender, attacker, cloak);
        }

        /// <summary>
        /// Rolls for a chance at procing a cloak spell
        /// </summary>
        /// <param name="damage_percent">The percent of MaxHealth inflicted by an enemy's hit</param>
        /// <returns></returns>
        public static bool RollProc(WorldObject cloak, float damage_percent)
        {
            // TODO: find retail formula

            var currentTime = Time.GetUnixTime();

            if (currentTime - cloak.UseTimestamp < MinDelay)
                return false;

            var itemLevel = cloak.ItemLevel ?? 0;

            if (itemLevel < 1) return false;

            var maxProcBase = MaxProcBase;

            if (HasDamageProc(cloak))
            {
                maxProcBase = MaxProcBase200;
                damage_percent *= TwoThirds;
            }

            var maxProcRate = maxProcBase + (itemLevel - 1) * 0.0125f;

            if (UseCustomMath)
            {
                // The proc chance should only plateau for damage above a certain configured percentage
                var maxProcAtDamagePercent = Convert.ToSingle(PropertyManager.GetDouble("cloak_max_proc_damage_percentage", 30.0).Item);
                // Reduce the damage percent for the calculation if necessary to a fraction of the percentage based on the configuration
                damage_percent = maxProcRate * (damage_percent / maxProcAtDamagePercent);
            }

            // take the lowest of the chance between damage and proc rate, then override
            // with min proc if necessary
            var chance = Math.Max(Math.Min(damage_percent, maxProcRate), MinProc);

            var rng = ThreadSafeRandom.Next(0.0f, 1.0f);

            if (rng < chance)
            {
                cloak.UseTimestamp = currentTime;
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Casts the cloak proc spell
        /// </summary>
        public static bool HandleProcSpell(Creature defender, WorldObject attacker, WorldObject cloak)
        {
            if (cloak.ProcSpell == null) return false;

            var spell = new Spell(cloak.ProcSpell.Value);

            if (spell.NotFound)
            {
                if (defender is Player player)
                {
                    if (spell._spellBase == null)
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat($"SpellId {cloak.ProcSpell} Invalid.", ChatMessageType.System));
                    else
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{spell.Name} spell not implemented, yet!", ChatMessageType.System));

                }
                return false;
            }

            var targetSelf = spell.Flags.HasFlag(SpellFlags.SelfTargeted);
            var untargeted = spell.NonComponentTargetType == ItemType.None;

            var target = attacker;
            if (untargeted)
                target = null;
            else if (targetSelf)
                target = defender;

            // cloak range?

            var msg = new GameMessageSystemChat($"The cloak of {defender.Name} weaves the magic of {spell.Name}!", ChatMessageType.Spellcasting);

            defender.EnqueueBroadcast(msg, WorldObject.LocalBroadcastRange, ChatMessageType.Spellcasting);

            defender.TryCastSpell(spell, target, cloak, cloak, true, true, false);

            return true;
        }

        /// <summary>
        /// Returns TRUE if object is cloak
        /// </summary>
        public static bool IsCloak(WorldObject wo)
        {
            return wo.ValidLocations == EquipMask.Cloak;
        }

        /// <summary>
        /// The amount of damage reduced by a cloak proced with PropertyInt.CloakWeaveProc=2
        /// </summary>
        public const int DamageReductionAmount = 200;

        public static int GetDamageReductionAmount(WorldObject source)
        {
            var damageReductionAmount = DamageReductionAmount;

            // https://asheron.fandom.com/wiki/Master_of_Arms
            // Cloaks with the chance to reduce incoming damage by 200 have been reduced to 100 for PvP circumstances.
            if (source is Player)
                damageReductionAmount /= 2;

            return damageReductionAmount;
        }

        /// <summary>
        /// Returns the reduced damage amount when a cloak procs
        /// with PropertyInt.CloakWeaveProc=2
        /// </summary>
        public static uint GetReducedAmount(WorldObject source, uint damage)
        {
            var damageReductionAmount = GetDamageReductionAmount(source);

            if (damage > damageReductionAmount)
                return (uint)(damage - damageReductionAmount);
            else
                return 0;
        }

        public static int GetReducedAmount(WorldObject source, int damage)
        {
            var damageReductionAmount = GetDamageReductionAmount(source);

            return Math.Max(0, damage - damageReductionAmount);
        }

        public static float GetReducedAmount(WorldObject source, float damage)
        {
            var damageReductionAmount = GetDamageReductionAmount(source);

            return Math.Max(0, damage - damageReductionAmount);
        }

        /// <summary>
        /// Sends the message to attacker and defender when cloak is proced with PropertyInt.CloakWeaveProc=2
        /// </summary>
        public static void ShowMessage(Creature defender, WorldObject attacker, int origDamage, int reducedDamage)
        {
            var suffix = $"reduced the damage from {origDamage} down to {reducedDamage}!";

            if (defender is Player playerDefender)
                playerDefender.Session.Network.EnqueueSend(new GameMessageSystemChat($"Your cloak {suffix}", ChatMessageType.Magic));

            // send message to attacker?
            if (attacker is Player playerAttacker)
                playerAttacker.Session.Network.EnqueueSend(new GameMessageSystemChat($"The cloak of {defender.Name} {suffix}", ChatMessageType.Magic));
        }

        public static void ShowMessage(Creature defender, WorldObject attacker, float origDamage, float reducedDamage)
        {
            ShowMessage(defender, attacker, (int)Math.Round(origDamage), (int)Math.Round(reducedDamage));
        }

        /// <summary>
        /// Returns TRUE If cloak has a damage reduction proc
        /// Matches client logic
        /// </summary>
        public static bool HasDamageProc(WorldObject cloak)
        {
            return cloak?.CloakWeaveProc == 2;
        }

        /// <summary>
        /// Returns TRUE if cloak has a spell proc
        /// </summary>
        public static bool HasProcSpell(WorldObject cloak)
        {
            return cloak?.ProcSpell != null;
        }
    }
}
