using System;
using System.Collections.Generic;

using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class ConsumeWcids
    {
        private static readonly ChanceTable<WeenieClassName> T1_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.healthdraught,   0.25f ),
            ( WeenieClassName.manadraught,     0.25f ),
            ( WeenieClassName.apple,           0.01f ),
            ( WeenieClassName.bread,           0.01f ),
            ( WeenieClassName.cabbage,         0.01f ),
            ( WeenieClassName.cheese,          0.01f ),
            ( WeenieClassName.chicken,         0.01f ),
            ( WeenieClassName.egg,             0.01f ),
            ( WeenieClassName.fish,            0.01f ),
            ( WeenieClassName.grapes,          0.01f ),
            ( WeenieClassName.beefside,        0.01f ),
            ( WeenieClassName.mushroom,        0.01f ),
            ( WeenieClassName.staminapotion,   0.16f ),
            ( WeenieClassName.healthpotion,    0.08f ),
            ( WeenieClassName.manapotion,      0.08f ),
            ( WeenieClassName.staminatincture, 0.08f ),
            ( WeenieClassName.healthtincture,  0.00f ),
            ( WeenieClassName.manatincture,    0.00f ),
            ( WeenieClassName.staminaelixir,   0.00f ),
            ( WeenieClassName.healthelixir,    0.00f ),
            ( WeenieClassName.manaelixir,      0.00f ),
            ( WeenieClassName.staminabrew,     0.00f ),
            ( WeenieClassName.healthtonic,     0.00f ),
            ( WeenieClassName.manatonic,       0.00f ),
            ( WeenieClassName.staminatonic,    0.00f ),
            ( WeenieClassName.healthphiltre,   0.00f ),
            ( WeenieClassName.manaphiltre,     0.00f ),
            ( WeenieClassName.staminaphiltre,  0.00f ),
        };

        private static readonly ChanceTable<WeenieClassName> T2_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.healthdraught,   0.08f ),
            ( WeenieClassName.manadraught,     0.08f ),
            ( WeenieClassName.apple,           0.00f ),
            ( WeenieClassName.bread,           0.00f ),
            ( WeenieClassName.cabbage,         0.00f ),
            ( WeenieClassName.cheese,          0.00f ),
            ( WeenieClassName.chicken,         0.00f ),
            ( WeenieClassName.egg,             0.00f ),
            ( WeenieClassName.fish,            0.00f ),
            ( WeenieClassName.grapes,          0.00f ),
            ( WeenieClassName.beefside,        0.00f ),
            ( WeenieClassName.mushroom,        0.00f ),
            ( WeenieClassName.staminapotion,   0.08f ),
            ( WeenieClassName.healthpotion,    0.17f ),
            ( WeenieClassName.manapotion,      0.17f ),
            ( WeenieClassName.staminatincture, 0.18f ),
            ( WeenieClassName.healthtincture,  0.08f ),
            ( WeenieClassName.manatincture,    0.08f ),
            ( WeenieClassName.staminaelixir,   0.08f ),
            ( WeenieClassName.healthelixir,    0.00f ),
            ( WeenieClassName.manaelixir,      0.00f ),
            ( WeenieClassName.staminabrew,     0.00f ),
            ( WeenieClassName.healthtonic,     0.00f ),
            ( WeenieClassName.manatonic,       0.00f ),
            ( WeenieClassName.staminatonic,    0.00f ),
            ( WeenieClassName.healthphiltre,   0.00f ),
            ( WeenieClassName.manaphiltre,     0.00f ),
            ( WeenieClassName.staminaphiltre,  0.00f ),
        };

        private static readonly ChanceTable<WeenieClassName> T3_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.healthdraught,   0.00f ),
            ( WeenieClassName.manadraught,     0.00f ),
            ( WeenieClassName.apple,           0.00f ),
            ( WeenieClassName.bread,           0.00f ),
            ( WeenieClassName.cabbage,         0.00f ),
            ( WeenieClassName.cheese,          0.00f ),
            ( WeenieClassName.chicken,         0.00f ),
            ( WeenieClassName.egg,             0.00f ),
            ( WeenieClassName.fish,            0.00f ),
            ( WeenieClassName.grapes,          0.00f ),
            ( WeenieClassName.beefside,        0.00f ),
            ( WeenieClassName.mushroom,        0.00f ),
            ( WeenieClassName.staminapotion,   0.00f ),
            ( WeenieClassName.healthpotion,    0.08f ),
            ( WeenieClassName.manapotion,      0.08f ),
            ( WeenieClassName.staminatincture, 0.08f ),
            ( WeenieClassName.healthtincture,  0.17f ),
            ( WeenieClassName.manatincture,    0.17f ),
            ( WeenieClassName.staminaelixir,   0.18f ),
            ( WeenieClassName.healthelixir,    0.08f ),
            ( WeenieClassName.manaelixir,      0.08f ),
            ( WeenieClassName.staminabrew,     0.08f ),
            ( WeenieClassName.healthtonic,     0.00f ),
            ( WeenieClassName.manatonic,       0.00f ),
            ( WeenieClassName.staminatonic,    0.00f ),
            ( WeenieClassName.healthphiltre,   0.00f ),
            ( WeenieClassName.manaphiltre,     0.00f ),
            ( WeenieClassName.staminaphiltre,  0.00f ),
        };

        private static readonly ChanceTable<WeenieClassName> T4_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.healthdraught,   0.00f ),
            ( WeenieClassName.manadraught,     0.00f ),
            ( WeenieClassName.apple,           0.00f ),
            ( WeenieClassName.bread,           0.00f ),
            ( WeenieClassName.cabbage,         0.00f ),
            ( WeenieClassName.cheese,          0.00f ),
            ( WeenieClassName.chicken,         0.00f ),
            ( WeenieClassName.egg,             0.00f ),
            ( WeenieClassName.fish,            0.00f ),
            ( WeenieClassName.grapes,          0.00f ),
            ( WeenieClassName.beefside,        0.00f ),
            ( WeenieClassName.mushroom,        0.00f ),
            ( WeenieClassName.staminapotion,   0.00f ),
            ( WeenieClassName.healthpotion,    0.00f ),
            ( WeenieClassName.manapotion,      0.00f ),
            ( WeenieClassName.staminatincture, 0.00f ),
            ( WeenieClassName.healthtincture,  0.08f ),
            ( WeenieClassName.manatincture,    0.08f ),
            ( WeenieClassName.staminaelixir,   0.08f ),
            ( WeenieClassName.healthelixir,    0.17f ),
            ( WeenieClassName.manaelixir,      0.17f ),
            ( WeenieClassName.staminabrew,     0.18f ),
            ( WeenieClassName.healthtonic,     0.08f ),
            ( WeenieClassName.manatonic,       0.08f ),
            ( WeenieClassName.staminatonic,    0.08f ),
            ( WeenieClassName.healthphiltre,   0.00f ),
            ( WeenieClassName.manaphiltre,     0.00f ),
            ( WeenieClassName.staminaphiltre,  0.00f ),
        };

        private static readonly ChanceTable<WeenieClassName> T5_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.healthdraught,   0.00f ),
            ( WeenieClassName.manadraught,     0.00f ),
            ( WeenieClassName.apple,           0.00f ),
            ( WeenieClassName.bread,           0.00f ),
            ( WeenieClassName.cabbage,         0.00f ),
            ( WeenieClassName.cheese,          0.00f ),
            ( WeenieClassName.chicken,         0.00f ),
            ( WeenieClassName.egg,             0.00f ),
            ( WeenieClassName.fish,            0.00f ),
            ( WeenieClassName.grapes,          0.00f ),
            ( WeenieClassName.beefside,        0.00f ),
            ( WeenieClassName.mushroom,        0.00f ),
            ( WeenieClassName.staminapotion,   0.00f ),
            ( WeenieClassName.healthpotion,    0.00f ),
            ( WeenieClassName.manapotion,      0.00f ),
            ( WeenieClassName.staminatincture, 0.00f ),
            ( WeenieClassName.healthtincture,  0.00f ),
            ( WeenieClassName.manatincture,    0.00f ),
            ( WeenieClassName.staminaelixir,   0.00f ),
            ( WeenieClassName.healthelixir,    0.08f ),
            ( WeenieClassName.manaelixir,      0.08f ),
            ( WeenieClassName.staminabrew,     0.08f ),
            ( WeenieClassName.healthtonic,     0.17f ),
            ( WeenieClassName.manatonic,       0.17f ),
            ( WeenieClassName.staminatonic,    0.18f ),
            ( WeenieClassName.healthphiltre,   0.08f ),
            ( WeenieClassName.manaphiltre,     0.08f ),
            ( WeenieClassName.staminaphiltre,  0.08f ),
        };

        private static readonly ChanceTable<WeenieClassName> T6_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.healthdraught,   0.00f ),
            ( WeenieClassName.manadraught,     0.00f ),
            ( WeenieClassName.apple,           0.00f ),
            ( WeenieClassName.bread,           0.00f ),
            ( WeenieClassName.cabbage,         0.00f ),
            ( WeenieClassName.cheese,          0.00f ),
            ( WeenieClassName.chicken,         0.00f ),
            ( WeenieClassName.egg,             0.00f ),
            ( WeenieClassName.fish,            0.00f ),
            ( WeenieClassName.grapes,          0.00f ),
            ( WeenieClassName.beefside,        0.00f ),
            ( WeenieClassName.mushroom,        0.00f ),
            ( WeenieClassName.staminapotion,   0.00f ),
            ( WeenieClassName.healthpotion,    0.00f ),
            ( WeenieClassName.manapotion,      0.00f ),
            ( WeenieClassName.staminatincture, 0.00f ),
            ( WeenieClassName.healthtincture,  0.00f ),
            ( WeenieClassName.manatincture,    0.00f ),
            ( WeenieClassName.staminaelixir,   0.00f ),
            ( WeenieClassName.healthelixir,    0.00f ),
            ( WeenieClassName.manaelixir,      0.00f ),
            ( WeenieClassName.staminabrew,     0.00f ),
            ( WeenieClassName.healthtonic,     0.08f ),
            ( WeenieClassName.manatonic,       0.08f ),
            ( WeenieClassName.staminatonic,    0.09f ),
            ( WeenieClassName.healthphiltre,   0.25f ),
            ( WeenieClassName.manaphiltre,     0.25f ),
            ( WeenieClassName.staminaphiltre,  0.25f ),
        };

        private static readonly List<ChanceTable<WeenieClassName>> consumeTiers = new List<ChanceTable<WeenieClassName>>()
        {
            T1_Chances,
            T2_Chances,
            T3_Chances,
            T4_Chances,
            T5_Chances,
            T6_Chances,
        };

        public static WeenieClassName Roll(int tier)
        {
            // todo: add unique profiles for t7 / t8?
            tier = Math.Clamp(tier, 1, 6);

            return consumeTiers[tier - 1].Roll();
        }
    }
}
