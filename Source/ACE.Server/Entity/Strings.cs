using System.Collections.Generic;
using ACE.Entity.Enum;

namespace ACE.Server.Entity
{
    public class Strings
    {
        public static Dictionary<DamageType, List<string>> DeathMessages;

        public static List<string> Slashing = new List<string>()
        {
            "You split {0} apart!",
            "You cleave {0} in twain!",
            "{0} is torn to ribbons by your assault!",
            "Your killing blow nearly turns {0} inside-out!",
        };

        public static List<string> Piercing = new List<string>()
        {
            "You run {0} through!",
            "{0}'s death is preceded by a sharp, stabbing pain!",
            "{0} is fatally punctured!",
            "{0}'s perforated corpse falls before you!"
        };

        public static List<string> Bludgeoning = new List<string>()
        {
            "You beat {0} to a lifeless pulp!",
            "{0} is shattered by your assault!",
            "You flatten {0}'s body with the force of your assault!",
            "The thunder of crushing {0} is followed by the deafening silence of death!",
        };

        public static List<string> Fire = new List<string>()
        {
            "You bring {0} to a fiery end!",
            "{0} is reduced to cinders!",
            "{0}'s seared corpse smolders before you!",
            "{0} is incinerated by your assault!"
        };

        public static List<string> Ice = new List<string>()
        {
            "Your attack stops {0} cold!",
            "Your assault sends {0} to an icy death!",
            "{0} suffers a frozen fate!"
        };

        public static List<string> Acid = new List<string>()
        {
            "{0}'s last strength dissolves before you!",
            "{0} is liquified by your attack!",
            "You reduce {0} to a sizzling, oozing mass!",
        };

        public static List<string> Lightning = new List<string>()
        {
            "Blistered by lightning, {0} falls!",
            "Electricity tears {0} apart!",
            "Your lightning coruscates over {0}'s mortal remains!",
        };

        public static List<string> Void = new List<string>()
        {
            "{0} is dessicated by your attack!",
            "{0}'s last strength withers before you!",
            "You reduce {0} to a drained, twisted corpse!"
        };

        public static List<string> Critical = new List<string>()
        {
            "{0} catches your attack, with dire consequences!",
            "You obliterate {0}!",
            "{0} is utterly destroyed by your attack!",
            "You knock {0} into next Morningthaw!",
            "You slay {0} viciously enough to impart death several times over!",
            "The deadly force of your attack is so strong that {0}'s ancestors feel it!",
            "You smite {0} mightily!",
        };

        public static List<string> PKCritical = new List<string>()
        {
            "You send {0} to death so violently that even the lifestone flinches!"
        };

        public static List<string> General = new List<string>()
        {
            "You killed {0}!",
        };

        static Strings()
        {
            DeathMessages = new Dictionary<DamageType, List<string>>();
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
    }
}
