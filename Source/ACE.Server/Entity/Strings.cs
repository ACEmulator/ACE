using System;
using System.Collections.Generic;

using ACE.Common;
using ACE.Entity.Enum;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    public class Strings
    {
        public static Dictionary<DamageType, List<DeathMessage>> DeathMessages;

        // all verified in retail pcaps
        // 0 = victim
        // 1 = killer

        public static List<DeathMessage> Slashing = new List<DeathMessage>()
        {
            new DeathMessage(
                "You split {0} apart!",
                "{1} splits you apart!",
                "{1} splits {0} apart!"),

            new DeathMessage(
                "You cleave {0} in twain!",
                "{1} cleaves you in twain!",
                "{1} cleaves {0} in twain!"),

            new DeathMessage(
                "{0} is torn to ribbons by your assault!",
                "You are torn to ribbons by {1}'s assault!",
                "{0} is torn to ribbons by {1}'s assault!"),

            new DeathMessage(
                "Your killing blow nearly turns {0} inside-out!",
                "{1}'s killing blow nearly turns you inside-out!",
                "{1}'s killing blow nearly turns {0} inside-out!")
        };

        public static List<DeathMessage> Piercing = new List<DeathMessage>()
        {
            new DeathMessage(
                "You run {0} through!",
                "{1} runs you through!",
                "{1} runs {0} through!"),

            new DeathMessage(
                "{0} is fatally punctured!",
                "You are fatally punctured by {1}!",
                "{0} is fatally punctured by {1}!"),

            new DeathMessage(
                "{0}'s perforated corpse falls before you!",
                "Your perforated corpse falls before {1}!",
                "{0}'s perforated corpse falls before {1}!"),

            new DeathMessage(
                "{0}'s death is preceded by a sharp, stabbing pain!",
                "Your death is preceded by a sharp, stabbing pain, courtesy of {1}!",
                "{0}'s death is preceded by a sharp, stabbing pain, courtesy of {1}!")
        };

        public static List<DeathMessage> Bludgeoning = new List<DeathMessage>()
        {
            new DeathMessage(
                "You beat {0} to a lifeless pulp!",
                "{1} beats you to a lifeless pulp!",
                "{1} beats {0} to a lifeless pulp!"),

            new DeathMessage(
                "{0} is shattered by your assault!",
                "Your body is shattered by {1}'s attack!",
                "{0}'s body is shattered by {1}'s attack!"),

            new DeathMessage(
                "You flatten {0}'s body with the force of your assault!",
                "The force of {1}'s assault flattens you!",
                "The force of {1}'s assault flattens {0}!"),

            new DeathMessage(
                "The thunder of crushing {0} is followed by the deafening silence of death!",
                "The thunder of {1} crushing {0} is followed by the deafening silence of your death!",
                "The thunder of {1} crushing {0} is followed by the deafening silence of death!")
        };

        public static List<DeathMessage> Fire = new List<DeathMessage>()
        {
            new DeathMessage(
                "You bring {0} to a fiery end!",
                "{1} brings you to a fiery end!",
                "{1} brings {0} to a fiery end!"),

            new DeathMessage(
                "{0} is reduced to cinders!",
                "You are reduced to cinders by {1}!",
                "{1} reduced {0} to cinders!"),

            new DeathMessage(
                "{0} is incinerated by your assault!",
                "You are incinerated by {1}'s assault!",
                "{0} is incinerated by {1}'s assault!"),

            new DeathMessage(
                "{0}'s seared corpse smolders before you!",
                "Your seared corpse smolders before {1}!",
                "{0}'s seared corpse smolders before {1}!")
        };

        public static List<DeathMessage> Ice = new List<DeathMessage>()
        {
            new DeathMessage(
                "Your attack stops {0} cold!",
                "{1}'s attack stops you cold!",
                "{1}'s attack stops {0} cold!"),

            new DeathMessage(
                "Your assault sends {0} to an icy death!",
                "{1}'s assault sends you to an icy death!",
                "{1}'s assault sends {0} to an icy death!"),

            new DeathMessage(
                "{0} suffers a frozen fate!",
                "You suffer a frozen fate at the hands of {1}!",
                "{0} suffers a frozen fate at the hands of {1}!")
        };

        public static List<DeathMessage> Acid = new List<DeathMessage>()
        {
            new DeathMessage(
                "{0} is liquified by your attack!",
                "You are liquified by {1}'s attack!",
                "{0} is liquified by {1}'s attack!"),

            new DeathMessage(
                "{0}'s last strength dissolves before you!",
                "Your last strength dissolves before {1}!",
                "{0}'s last strength dissolves before {1}!"),

            new DeathMessage(
                "You reduce {0} to a sizzling, oozing mass!",
                "{1} reduces you to a sizzling, oozing mass!",
                "{1} reduces {0} to a sizzling, oozing mass!")
        };

        public static List<DeathMessage> Lightning = new List<DeathMessage>()
        {
            new DeathMessage(
                "Blistered by lightning, {0} falls!",
                "Blistered by {1}'s lightning, you die!",
                "Blistered by {1}'s lightning, {0} dies!"),

            new DeathMessage(
                "Electricity tears {0} apart!",
                "Electricity from {1}'s attack tears you apart!",
                "Electricity from {1}'s attack tears {0} apart!"),

            new DeathMessage(
                "Your lightning coruscates over {0}'s mortal remains!",
                "{1}'s lightning coruscates over your mortal remains!",
                "{1}'s lightning coruscates over {0}'s mortal remains!")
        };

        public static List<DeathMessage> Void = new List<DeathMessage>()
        {
            new DeathMessage(
                "{0} is dessicated by your attack!",
                "You are dessicated by {1}'s attack!",
                "{0} is dessicated by {1}'s attack!"),

            new DeathMessage(
                "{0}'s last strength withers before you!",
                "Your last strength withers before {1}!",
                "{0}'s last strength withers before {1}!"),

            new DeathMessage(
                "You reduce {0} to a drained, twisted corpse!",
                "{1} reduces you to a drained, twisted corpse!",
                "{1} reduces {0} to a drained, twisted corpse!")
        };

        public static List<DeathMessage> Critical = new List<DeathMessage>()
        {
            new DeathMessage(
                "You obliterate {0}!",
                "{1} obliterates you!",
                "{1} obliterates {0}!"),

            new DeathMessage(
                "You smite {0} mightily!",
                "{1} smites you mightily!",
                "{1} smites {0} mightily!"),

            new DeathMessage(
                "You knock {0} into next Morningthaw!",
                "{1} knocks you into next Morningthaw!",
                "{1} knocks {0} into next Morningthaw!"),

            new DeathMessage(
                "{0} is utterly destroyed by your attack!",
                "You are utterly destroyed by {1}'s attack!",
                "{0} is utterly destroyed by {1}'s attack!"),

            new DeathMessage(
                "{0} catches your attack, with dire consequences!",
                "You catch {1}'s attack, with dire consequences!",
                "{0} catches {1}'s attack, with dire consequences!"),

            new DeathMessage(
                "You slay {0} viciously enough to impart death several times over!",
                "{1} slays you viciously enough to impart death several times over!",
                "{1} slays {0} viciously enough to impart death several times over!"),

            new DeathMessage(
                "The deadly force of your attack is so strong that {0}'s ancestors feel it!",
                "The deadly force of {1}'s attack is so strong that your ancestors feel it!",
                "The deadly force of {1}'s attack is so strong that {0}'s ancestors feel it!")
        };

        public static List<DeathMessage> PKCritical = new List<DeathMessage>()
        {
            new DeathMessage(
                "You send {0} to death so violently that even the lifestone flinches!",
                "{1} sends you to your death so violently that even the lifestone flinches!",
                "{1} sends {0} to death so violently that even the lifestone flinches!")
        };

        public static List<DeathMessage> General = new List<DeathMessage>()
        {
            new DeathMessage(
                "You killed {0}!",
                "You were killed by {1}!",
                "{0} was killed by {1}!"),

            new DeathMessage(
                "{0} died!",
                "You died!",
                "{0} died!"
            )
        };

        static Strings()
        {
            DeathMessages = new Dictionary<DamageType, List<DeathMessage>>();
            DeathMessages.Add(DamageType.Undef, General);
            DeathMessages.Add(DamageType.Slash, Slashing);
            DeathMessages.Add(DamageType.Pierce, Piercing);
            DeathMessages.Add(DamageType.Bludgeon, Bludgeoning);
            DeathMessages.Add(DamageType.Fire, Fire);
            DeathMessages.Add(DamageType.Cold, Ice);
            DeathMessages.Add(DamageType.Acid, Acid);
            DeathMessages.Add(DamageType.Electric, Lightning);
            DeathMessages.Add(DamageType.Nether, Void);
            DeathMessages.Add(DamageType.Base, General);
            DeathMessages.Add(DamageType.Health, General);
        }

        public static bool GetAttackVerb(DamageType damageType, float percent, ref string single, ref string plural)
        {
            if (percent < 0.0f)
                return false;

            switch (damageType)
            {
                default:
                    single = "hit";
                    plural = "hits";
                    return true;

                case DamageType.Slash:

                    if (percent > 0.5f)
                    {
                        single = "mangle";
                        plural = "mangles";
                    }
                    else if (percent > 0.25f)
                    {
                        single = "slash";
                        plural = "slashes";
                    }
                    else if (percent > 0.1f)
                    {
                        single = "cut";
                        plural = "cuts";
                    }
                    else
                    {
                        single = "scratch";
                        plural = "scratches";
                    }
                    return true;

                case DamageType.Pierce:

                    if (percent > 0.5f)
                    {
                        single = "gore";
                        plural = "gores";
                    }
                    else if (percent > 0.25f)
                    {
                        single = "impale";
                        plural = "impales";
                    }
                    else if (percent > 0.1f)
                    {
                        single = "stab";
                        plural = "stabs";
                    }
                    else
                    {
                        single = "nick";
                        plural = "nicks";
                    }
                    return true;

                case DamageType.Bludgeon:

                    if (percent > 0.5f)
                    {
                        single = "crush";
                        plural = "crushes";
                    }
                    else if (percent > 0.25f)
                    {
                        single = "smash";
                        plural = "smashes";
                    }
                    else if (percent > 0.1f)
                    {
                        single = "bash";
                        plural = "bashes";
                    }
                    else
                    {
                        single = "graze";
                        plural = "grazes";
                    }
                    return true;

                case DamageType.Fire:

                    if (percent > 0.5f)
                    {
                        single = "incinerate";
                        plural = "incinerates";
                    }
                    else if (percent > 0.25f)
                    {
                        single = "burn";
                        plural = "burns";
                    }
                    else if (percent > 0.1f)
                    {
                        single = "scorch";
                        plural = "scorches";
                    }
                    else
                    {
                        single = "singe";
                        plural = "singes";
                    }
                    return true;

                case DamageType.Cold:

                    if (percent > 0.5f)
                    {
                        single = "freeze";
                        plural = "freezes";
                    }
                    else if (percent > 0.25f)
                    {
                        single = "frost";
                        plural = "frosts";
                    }
                    else if (percent > 0.1f)
                    {
                        single = "chill";
                        plural = "chills";
                    }
                    else
                    {
                        single = "numb";
                        plural = "numbs";
                    }
                    return true;

                case DamageType.Acid:

                    if (percent > 0.5f)
                    {
                        single = "dissolve";
                        plural = "dissolves";
                    }
                    else if (percent > 0.25f)
                    {
                        single = "corrode";
                        plural = "corrodes";
                    }
                    else if (percent > 0.1f)
                    {
                        single = "sear";
                        plural = "sears";
                    }
                    else
                    {
                        single = "blister";
                        plural = "blisters";
                    }
                    return true;

                case DamageType.Electric:

                    if (percent > 0.5f)
                    {
                        single = "blast";
                        plural = "blasts";
                    }
                    else if (percent > 0.25f)
                    {
                        single = "jolt";
                        plural = "jolts";
                    }
                    else if (percent > 0.1f)
                    {
                        single = "shock";
                        plural = "shocks";
                    }
                    else
                    {
                        single = "spark";
                        plural = "sparks";
                    }
                    return true;

                case DamageType.Nether:

                    if (percent > 0.5f)
                    {
                        single = "eradicate";
                        plural = "eradicates";
                    }
                    else if (percent > 0.25f)
                    {
                        single = "wither";
                        plural = "withers";
                    }
                    else if (percent > 0.1f)
                    {
                        single = "twist";
                        plural = "twists";
                    }
                    else
                    {
                        single = "scar";
                        plural = "scars";
                    }
                    return true;

                case DamageType.Health:

                    if (percent > 0.5f)
                    {
                        single = "deplete";
                        plural = "depletes";
                    }
                    else if (percent > 0.25f)
                    {
                        single = "siphon";
                        plural = "siphons";
                    }
                    else if (percent > 0.1f)
                    {
                        single = "exhaust";
                        plural = "exhausts";
                    }
                    else
                    {
                        single = "drain";
                        plural = "drains";
                    }
                    return true;
            }
        }

        /// <summary>
        /// Returns the player message for falling impact damage
        /// </summary>
        public static string GetFallMessage(uint damage, uint maxHealth)
        {
            var percent = (float)damage / maxHealth;

            string severity = "";
            if (percent > 0.5f)
                severity = "massive";
            else if (percent > 0.25f)
                severity = "crushing";
            else if (percent > 0.1f)
                severity = "heavy";
            else
                severity = "minor";

            return $"You suffer {damage:N0} points of {severity} impact damage.";
        }

        /// <summary>
        /// Returns a randomized death message based on damage type
        /// </summary>
        public static DeathMessage GetDeathMessage(DamageType damageType, bool criticalHit = false)
        {
            if (!criticalHit)
            {
                //var damageType = killer.GetDamageType();
                DeathMessages.TryGetValue(damageType, out var messages);

                if (messages == null)
                {
                    Console.WriteLine($"GetDeathMessage({damageType}, {criticalHit}) - unknown damage type");
                    return General[1];
                }

                var idx = ThreadSafeRandom.Next(0, messages.Count - 1);
                return messages[idx];
            }
            else
            {
                var messages = Critical;
                var idx = ThreadSafeRandom.Next(0, messages.Count - 1);
                return messages[idx];
            }
        }
    }
}
