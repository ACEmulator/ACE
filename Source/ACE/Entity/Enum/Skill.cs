using System;
using System.Collections.Generic;

namespace ACE.Entity.Enum
{
    /// <summary>
    /// note: even though these are unnumbered, order is very important.  values of "none" or commented
    /// as retired or unused CANNOT be removed
    /// </summary>
    public enum Skill
    {
        None,
        Axe,                 /* Retired */
        Bow,                 /* Retired */
        Crossbow,            /* Retired */
        Dagger,              /* Retired */
        Mace,                /* Retired */

        [AbilityFormula(Ability.Quickness | Ability.Coordination, 3)]
        [SkillCost(10, 10)]
        [SkillUsableUntrained(true)]
        MeleeDefense,

        [AbilityFormula(Ability.Quickness | Ability.Coordination, 5)]
        [SkillCost(6, 4)]
        [SkillUsableUntrained(true)]
        MissileDefense,

        Sling,               /* Retired */
        Spear,               /* Retired */
        Staff,               /* Retired */
        Sword,               /* Retired */
        ThrownWeapon,        /* Retired */
        UnarmedCombat,       /* Retired */

        [AbilityFormula(Ability.Focus, 3)]
        [SkillCost(0, 2, TrainsFree = true)]
        [SkillUsableUntrained(true)]
        ArcaneLore,

        [AbilityFormula(Ability.Focus | Ability.Self, 7)]
        [SkillCost(0, 12, TrainsFree = true)]
        [SkillUsableUntrained(true)]
        MagicDefense,

        [AbilityFormula(Ability.Focus | Ability.Self, 6)]
        [SkillCost(6, 6)]
        [SkillUsableUntrained(false)]
        ManaConversion,

        Spellcraft,          /* Unimplemented */

        [AbilityFormula(Ability.Focus | Ability.Self, 6)]
        [SkillCost(2, CanSpecialize = false)]
        [SkillUsableUntrained(false)]
        ItemTinkering,

        [AbilityFormula(Ability.None)]
        [SkillCost(2, 2)]
        [SkillUsableUntrained(false)]
        AssessPerson,

        [AbilityFormula(Ability.None)]
        [SkillCost(4, 2)]
        [SkillUsableUntrained(false)]
        Deception,

        [AbilityFormula(Ability.Focus | Ability.Coordination, 3)]
        [SkillCost(6, 4)]
        [SkillUsableUntrained(false)]
        Healing,

        [AbilityFormula(Ability.Strength | Ability.Coordination, 2)]
        [SkillCost(0, 4, TrainsFree = true)]
        [SkillUsableUntrained(true)]
        Jump,

        [AbilityFormula(Ability.Focus | Ability.Coordination, 3)]
        [SkillCost(6, 4)]
        [SkillUsableUntrained(false)]
        Lockpick,

        [AbilityFormula(Ability.Quickness)]
        [SkillCost(0, 4, TrainsFree = true)]
        [SkillUsableUntrained(true)]
        Run,

        Awareness,           /* Unimplemented */
        ArmsAndArmorRepair,  /* Unimplemented */

        [AbilityFormula(Ability.None)]
        [SkillCost(4, 2)]
        [SkillUsableUntrained(false)]
        AssessCreature,

        [AbilityFormula(Ability.Focus | Ability.Strength, 2)]
        [SkillCost(4, CanSpecialize = false)]
        [SkillUsableUntrained(false)]
        WeaponTinkering,

        [AbilityFormula(Ability.Focus | Ability.Endurance, 2)]
        [SkillCost(4, CanSpecialize = false)]
        [SkillUsableUntrained(false)]
        ArmorTinkering,

        [AbilityFormula(Ability.Focus)]
        [SkillCost(4, CanSpecialize = false)]
        [SkillUsableUntrained(false)]
        MagicItemTinkering,

        [AbilityFormula(Ability.Focus | Ability.Self, 4)]
        [SkillCost(8, 8)]
        [SkillUsableUntrained(false)]
        CreatureEnchantment,

        [AbilityFormula(Ability.Focus | Ability.Self, 4)]
        [SkillCost(8, 8)]
        [SkillUsableUntrained(false)]
        ItemEnchantment,

        [AbilityFormula(Ability.Focus | Ability.Self, 4)]
        [SkillCost(12, 8)]
        [SkillUsableUntrained(false)]
        LifeMagic,

        [AbilityFormula(Ability.Focus | Ability.Self, 4)]
        [SkillCost(16, 12)]
        [SkillUsableUntrained(false)]
        WarMagic,

        [AbilityFormula(Ability.None)]
        [SkillCost(4, 2)]
        [SkillUsableUntrained(true)]
        Leadership,

        [AbilityFormula(Ability.None)]
        [SkillCost(0, 2, TrainsFree = true)]
        [SkillUsableUntrained(true)]
        Loyalty,

        [AbilityFormula(Ability.Coordination | Ability.Focus, 3)]
        [SkillCost(4, 4)]
        [SkillUsableUntrained(false)]
        Fletching,

        [AbilityFormula(Ability.Coordination | Ability.Focus, 3)]
        [SkillCost(6, 6)]
        [SkillUsableUntrained(false)]
        Alchemy,

        [AbilityFormula(Ability.Coordination | Ability.Focus, 3)]
        [SkillCost(4, 4)]
        [SkillUsableUntrained(false)]
        Cooking,

        [AbilityFormula(Ability.None)]
        [SkillCost(0, CanSpecialize = false, TrainsFree = true)]
        [SkillUsableUntrained(true)]
        Salvaging,

        [AbilityFormula(Ability.Strength | Ability.Coordination, 3)]
        [SkillCost(8, 8)]
        [SkillUsableUntrained(true)]
        TwoHandedCombat,

        Gearcraft,           /* Retired */

        [AbilityFormula(Ability.Focus | Ability.Self, 4)]
        [SkillCost(16, 12)]
        [SkillUsableUntrained(false)]
        VoidMagic,

        [AbilityFormula(Ability.Coordination | Ability.Strength, 3)]
        [SkillCost(6, 6)]
        [SkillUsableUntrained(true)]
        HeavyWeapons,

        [AbilityFormula(Ability.Coordination | Ability.Strength, 3)]
        [SkillCost(4, 4)]
        [SkillUsableUntrained(true)]
        LightWeapons,

        [AbilityFormula(Ability.Coordination | Ability.Quickness, 3)]
        [SkillCost(4, 4)]
        [SkillUsableUntrained(true)]
        FinesseWeapons,

        [AbilityFormula(Ability.Coordination, 2)]
        [SkillCost(6, 6)]
        [SkillUsableUntrained(true)]
        MissileWeapons,

        [AbilityFormula(Ability.Coordination | Ability.Strength, 2)]
        [SkillCost(2, 2)]
        [SkillUsableUntrained(true)]
        Shield,

        [AbilityFormula(Ability.Coordination, 3, AbilityMultiplier = 2)]
        [SkillCost(2, 2)]
        [SkillUsableUntrained(false)]
        DualWield,

        [AbilityFormula(Ability.Strength | Ability.Quickness, 3)]
        [SkillCost(4, 2)]
        [SkillUsableUntrained(false)]
        Recklessness,

        [AbilityFormula(Ability.Coordination | Ability.Quickness, 3)]
        [SkillCost(4, 2)]
        [SkillUsableUntrained(false)]
        SneakAttack,

        [AbilityFormula(Ability.Strength | Ability.Coordination, 3)]
        [SkillCost(2, 2)]
        [SkillUsableUntrained(false)]
        DirtyFighting,

        Challenge,          /* Unimplemented */

        [AbilityFormula(Ability.Endurance | Ability.Self, 3)]
        [SkillCost(8, 4)]
        [SkillUsableUntrained(false)]
        Summoning
    }

    public static class SkillExtensions
    {
        public static AbilityFormulaAttribute GetFormula(this Skill skill)
        {
            return Enum.EnumHelper.GetAttributeOfType<AbilityFormulaAttribute>(skill);
        }

        public static SkillCostAttribute GetCost(this Skill skill)
        {
            return Enum.EnumHelper.GetAttributeOfType<SkillCostAttribute>(skill);
        }

        public static SkillUsableUntrainedAttribute GetUsability(this Skill skill)
        {
            return Enum.EnumHelper.GetAttributeOfType<SkillUsableUntrainedAttribute>(skill);
        }

        // TODO: move this to be loaded from a DAT
        public static List<Tuple<uint, uint, uint>> SpecializedChart = new List<Tuple<uint, uint, uint>>()
        {
            new Tuple<uint, uint, uint>(0u, 0u, 0u),
            new Tuple<uint, uint, uint>(1u, 23u, 23),
            new Tuple<uint, uint, uint>(2u, 56u, 33),
            new Tuple<uint, uint, uint>(3u, 97u, 41),
            new Tuple<uint, uint, uint>(4u, 149u, 52),
            new Tuple<uint, uint, uint>(5u, 211u, 62),
            new Tuple<uint, uint, uint>(6u, 282u, 71),
            new Tuple<uint, uint, uint>(7u, 364u, 82),
            new Tuple<uint, uint, uint>(8u, 456u, 92),
            new Tuple<uint, uint, uint>(9u, 558u, 102),
            new Tuple<uint, uint, uint>(10u, 671u, 113),
            new Tuple<uint, uint, uint>(11u, 795u, 124),
            new Tuple<uint, uint, uint>(12u, 931u, 136),
            new Tuple<uint, uint, uint>(13u, 1077u, 146),
            new Tuple<uint, uint, uint>(14u, 1236u, 159),
            new Tuple<uint, uint, uint>(15u, 1406u, 170),
            new Tuple<uint, uint, uint>(16u, 1589u, 183),
            new Tuple<uint, uint, uint>(17u, 1784u, 195),
            new Tuple<uint, uint, uint>(18u, 1992u, 208),
            new Tuple<uint, uint, uint>(19u, 2214u, 222),
            new Tuple<uint, uint, uint>(20u, 2449u, 235),
            new Tuple<uint, uint, uint>(21u, 2699u, 250),
            new Tuple<uint, uint, uint>(22u, 2963u, 264),
            new Tuple<uint, uint, uint>(23u, 3243u, 280),
            new Tuple<uint, uint, uint>(24u, 3539u, 296),
            new Tuple<uint, uint, uint>(25u, 3850u, 311),
            new Tuple<uint, uint, uint>(26u, 4180u, 330),
            new Tuple<uint, uint, uint>(27u, 4527u, 347),
            new Tuple<uint, uint, uint>(28u, 4892u, 365),
            new Tuple<uint, uint, uint>(29u, 5277u, 385),
            new Tuple<uint, uint, uint>(30u, 5683u, 406),
            new Tuple<uint, uint, uint>(31u, 6109u, 426),
            new Tuple<uint, uint, uint>(32u, 6559u, 450),
            new Tuple<uint, uint, uint>(33u, 7031u, 472),
            new Tuple<uint, uint, uint>(34u, 7529u, 498),
            new Tuple<uint, uint, uint>(35u, 8052u, 523),
            new Tuple<uint, uint, uint>(36u, 8603u, 551),
            new Tuple<uint, uint, uint>(37u, 9183u, 580),
            new Tuple<uint, uint, uint>(38u, 9794u, 611),
            new Tuple<uint, uint, uint>(39u, 10437u, 643),
            new Tuple<uint, uint, uint>(40u, 11115u, 678),
            new Tuple<uint, uint, uint>(41u, 11829u, 714),
            new Tuple<uint, uint, uint>(42u, 12582u, 753),
            new Tuple<uint, uint, uint>(43u, 13376u, 794),
            new Tuple<uint, uint, uint>(44u, 14213u, 837),
            new Tuple<uint, uint, uint>(45u, 15098u, 885),
            new Tuple<uint, uint, uint>(46u, 16031u, 933),
            new Tuple<uint, uint, uint>(47u, 17018u, 987),
            new Tuple<uint, uint, uint>(48u, 18061u, 1043),
            new Tuple<uint, uint, uint>(49u, 19165u, 1104),
            new Tuple<uint, uint, uint>(50u, 20332u, 1167),
            new Tuple<uint, uint, uint>(51u, 21569u, 1237),
            new Tuple<uint, uint, uint>(52u, 22879u, 1310),
            new Tuple<uint, uint, uint>(53u, 24267u, 1388),
            new Tuple<uint, uint, uint>(54u, 25740u, 1473),
            new Tuple<uint, uint, uint>(55u, 27304u, 1564),
            new Tuple<uint, uint, uint>(56u, 28964u, 1660),
            new Tuple<uint, uint, uint>(57u, 30728u, 1764),
            new Tuple<uint, uint, uint>(58u, 32603u, 1875),
            new Tuple<uint, uint, uint>(59u, 34597u, 1994),
            new Tuple<uint, uint, uint>(60u, 36720u, 2123),
            new Tuple<uint, uint, uint>(61u, 38981u, 2261),
            new Tuple<uint, uint, uint>(62u, 41389u, 2408),
            new Tuple<uint, uint, uint>(63u, 43956u, 2567),
            new Tuple<uint, uint, uint>(64u, 46695u, 2739),
            new Tuple<uint, uint, uint>(65u, 49616u, 2921),
            new Tuple<uint, uint, uint>(66u, 52736u, 3120),
            new Tuple<uint, uint, uint>(67u, 56067u, 3331),
            new Tuple<uint, uint, uint>(68u, 59627u, 3560),
            new Tuple<uint, uint, uint>(69u, 63433u, 3806),
            new Tuple<uint, uint, uint>(70u, 67504u, 4071),
            new Tuple<uint, uint, uint>(71u, 71859u, 4355),
            new Tuple<uint, uint, uint>(72u, 76521u, 4662),
            new Tuple<uint, uint, uint>(73u, 81513u, 4992),
            new Tuple<uint, uint, uint>(74u, 86860u, 5347),
            new Tuple<uint, uint, uint>(75u, 92590u, 5730),
            new Tuple<uint, uint, uint>(76u, 98732u, 6142),
            new Tuple<uint, uint, uint>(77u, 105319u, 6587),
            new Tuple<uint, uint, uint>(78u, 112384u, 7065),
            new Tuple<uint, uint, uint>(79u, 119965u, 7581),
            new Tuple<uint, uint, uint>(80u, 128101u, 8136),
            new Tuple<uint, uint, uint>(81u, 136836u, 8735),
            new Tuple<uint, uint, uint>(82u, 146216u, 9380),
            new Tuple<uint, uint, uint>(83u, 156291u, 10075),
            new Tuple<uint, uint, uint>(84u, 167116u, 10825),
            new Tuple<uint, uint, uint>(85u, 178749u, 11633),
            new Tuple<uint, uint, uint>(86u, 191252u, 12503),
            new Tuple<uint, uint, uint>(87u, 204694u, 13442),
            new Tuple<uint, uint, uint>(88u, 219149u, 14455),
            new Tuple<uint, uint, uint>(89u, 234694u, 15545),
            new Tuple<uint, uint, uint>(90u, 251416u, 16722),
            new Tuple<uint, uint, uint>(91u, 269407u, 17991),
            new Tuple<uint, uint, uint>(92u, 288765u, 19358),
            new Tuple<uint, uint, uint>(93u, 309599u, 20834),
            new Tuple<uint, uint, uint>(94u, 332022u, 22423),
            new Tuple<uint, uint, uint>(95u, 356161u, 24139),
            new Tuple<uint, uint, uint>(96u, 382148u, 25987),
            new Tuple<uint, uint, uint>(97u, 410131u, 27983),
            new Tuple<uint, uint, uint>(98u, 440264u, 30133),
            new Tuple<uint, uint, uint>(99u, 472717u, 32453),
            new Tuple<uint, uint, uint>(100u, 507671u, 34954),
            new Tuple<uint, uint, uint>(101u, 545324u, 37653),
            new Tuple<uint, uint, uint>(102u, 585886u, 40562),
            new Tuple<uint, uint, uint>(103u, 629586u, 43700),
            new Tuple<uint, uint, uint>(104u, 676672u, 47086),
            new Tuple<uint, uint, uint>(105u, 726408u, 49736),
            new Tuple<uint, uint, uint>(106u, 777982u, 51574),
            new Tuple<uint, uint, uint>(107u, 831204u, 53222),
            new Tuple<uint, uint, uint>(108u, 886706u, 55502),
            new Tuple<uint, uint, uint>(109u, 944149u, 57443),
            new Tuple<uint, uint, uint>(110u, 1004623u, 60474),
            new Tuple<uint, uint, uint>(111u, 1068144u, 63521),
            new Tuple<uint, uint, uint>(112u, 1134867u, 66723),
            new Tuple<uint, uint, uint>(113u, 1204278u, 69411),
            new Tuple<uint, uint, uint>(114u, 1276904u, 72626),
            new Tuple<uint, uint, uint>(115u, 1353312u, 76408),
            new Tuple<uint, uint, uint>(116u, 1434114u, 80802),
            new Tuple<uint, uint, uint>(117u, 1518971u, 84857),
            new Tuple<uint, uint, uint>(118u, 1607595u, 88624),
            new Tuple<uint, uint, uint>(119u, 1700755u, 93160),
            new Tuple<uint, uint, uint>(120u, 1799280u, 98525),
            new Tuple<uint, uint, uint>(121u, 1903065u, 103785),
            new Tuple<uint, uint, uint>(122u, 2011073u, 108008),
            new Tuple<uint, uint, uint>(123u, 2124346u, 113273),
            new Tuple<uint, uint, uint>(124u, 2244006u, 119660),
            new Tuple<uint, uint, uint>(125u, 2368266u, 124260),
            new Tuple<uint, uint, uint>(126u, 2497430u, 129164),
            new Tuple<uint, uint, uint>(127u, 2631909u, 134479),
            new Tuple<uint, uint, uint>(128u, 2771224u, 139315),
            new Tuple<uint, uint, uint>(129u, 2917013u, 145789),
            new Tuple<uint, uint, uint>(130u, 3067048u, 150035),
            new Tuple<uint, uint, uint>(131u, 3222235u, 155187),
            new Tuple<uint, uint, uint>(132u, 3383635u, 161400),
            new Tuple<uint, uint, uint>(133u, 3551467u, 167832),
            new Tuple<uint, uint, uint>(134u, 3725130u, 173663),
            new Tuple<uint, uint, uint>(135u, 3904206u, 179076),
            new Tuple<uint, uint, uint>(136u, 4089485u, 185279),
            new Tuple<uint, uint, uint>(137u, 4280974u, 191489),
            new Tuple<uint, uint, uint>(138u, 4478917u, 197943),
            new Tuple<uint, uint, uint>(139u, 4684816u, 205899),
            new Tuple<uint, uint, uint>(140u, 4898446u, 213630),
            new Tuple<uint, uint, uint>(141u, 5119881u, 221435),
            new Tuple<uint, uint, uint>(142u, 5349513u, 229632),
            new Tuple<uint, uint, uint>(143u, 5587084u, 237571),
            new Tuple<uint, uint, uint>(144u, 5832707u, 245623),
            new Tuple<uint, uint, uint>(145u, 6086897u, 254190),
            new Tuple<uint, uint, uint>(146u, 6350606u, 263709),
            new Tuple<uint, uint, uint>(147u, 6623252u, 272646),
            new Tuple<uint, uint, uint>(148u, 6905759u, 282507),
            new Tuple<uint, uint, uint>(149u, 7199598u, 293839),
            new Tuple<uint, uint, uint>(150u, 7510827u, 311229),
            new Tuple<uint, uint, uint>(151u, 7835138u, 324311),
            new Tuple<uint, uint, uint>(152u, 8185908u, 350770),
            new Tuple<uint, uint, uint>(153u, 8566254u, 380346),
            new Tuple<uint, uint, uint>(154u, 8983087u, 416833),
            new Tuple<uint, uint, uint>(155u, 9452180u, 469093),
            new Tuple<uint, uint, uint>(156u, 9978231u, 526051),
            new Tuple<uint, uint, uint>(157u, 10590938u, 612707),
            new Tuple<uint, uint, uint>(158u, 11292080u, 701142),
            new Tuple<uint, uint, uint>(159u, 12080597u, 788517),
            new Tuple<uint, uint, uint>(160u, 12978687u, 898090),
            new Tuple<uint, uint, uint>(161u, 13957900u, 979213),
            new Tuple<uint, uint, uint>(162u, 14971249u, 1013349),
            new Tuple<uint, uint, uint>(163u, 16103320u, 1132071),
            new Tuple<uint, uint, uint>(164u, 17322402u, 1219082),
            new Tuple<uint, uint, uint>(165u, 18634617u, 1312215),
            new Tuple<uint, uint, uint>(166u, 20062065u, 1427448),
            new Tuple<uint, uint, uint>(167u, 21585981u, 1523916),
            new Tuple<uint, uint, uint>(168u, 23214900u, 1628919),
            new Tuple<uint, uint, uint>(169u, 24936844u, 1721944),
            new Tuple<uint, uint, uint>(170u, 26808511u, 1871667),
            new Tuple<uint, uint, uint>(171u, 28810492u, 2001981),
            new Tuple<uint, uint, uint>(172u, 30975492u, 2165000),
            new Tuple<uint, uint, uint>(173u, 33221583u, 2246091),
            new Tuple<uint, uint, uint>(174u, 35528463u, 2306880),
            new Tuple<uint, uint, uint>(175u, 38089744u, 2561281),
            new Tuple<uint, uint, uint>(176u, 40943261u, 2853517),
            new Tuple<uint, uint, uint>(177u, 43951402u, 3008141),
            new Tuple<uint, uint, uint>(178u, 47181470u, 3230068),
            new Tuple<uint, uint, uint>(179u, 50806066u, 3624596),
            new Tuple<uint, uint, uint>(180u, 54703511u, 3897445),
            new Tuple<uint, uint, uint>(181u, 59258291u, 4554780),
            new Tuple<uint, uint, uint>(182u, 64461548u, 5203257),
            new Tuple<uint, uint, uint>(183u, 70511600u, 6050052),
            new Tuple<uint, uint, uint>(184u, 77114508u, 6602908),
            new Tuple<uint, uint, uint>(185u, 84284685u, 7170177),
            new Tuple<uint, uint, uint>(186u, 92045555u, 7760870),
            new Tuple<uint, uint, uint>(187u, 100330262u, 8284707),
            new Tuple<uint, uint, uint>(188u, 109182433u, 8852171),
            new Tuple<uint, uint, uint>(189u, 118957009u, 9774576),
            new Tuple<uint, uint, uint>(190u, 129861131u, 10904122),
            new Tuple<uint, uint, uint>(191u, 141695103u, 11833972),
            new Tuple<uint, uint, uint>(192u, 154193427u, 12498324),
            new Tuple<uint, uint, uint>(193u, 167565923u, 13372496),
            new Tuple<uint, uint, uint>(194u, 183038936u, 15473013),
            new Tuple<uint, uint, uint>(195u, 200856634u, 17817698),
            new Tuple<uint, uint, uint>(196u, 221282414u, 20425780),
            new Tuple<uint, uint, uint>(197u, 244600416u, 23318002),
            new Tuple<uint, uint, uint>(198u, 271117157u, 26516741),
            new Tuple<uint, uint, uint>(199u, 301163291u, 30046134),
            new Tuple<uint, uint, uint>(200u, 336095513u, 34932222),
            new Tuple<uint, uint, uint>(201u, 374298608u, 38203095),
            new Tuple<uint, uint, uint>(202u, 418187661u, 43889053),
            new Tuple<uint, uint, uint>(203u, 466210448u, 48022787),
            new Tuple<uint, uint, uint>(204u, 520850007u, 54639559),
            new Tuple<uint, uint, uint>(205u, 581627417u, 60777410),
            new Tuple<uint, uint, uint>(206u, 648104789u, 66477372),
            new Tuple<uint, uint, uint>(207u, 721888505u, 73783716),
            new Tuple<uint, uint, uint>(208u, 802632699u, 80744194),
            new Tuple<uint, uint, uint>(209u, 890043017u, 87410318),
            new Tuple<uint, uint, uint>(210u, 984880677u, 94837660),
            new Tuple<uint, uint, uint>(211u, 1085966844u, 101086167),
            new Tuple<uint, uint, uint>(212u, 1196187351u, 110220507),
            new Tuple<uint, uint, uint>(213u, 1315497790u, 119310439),
            new Tuple<uint, uint, uint>(214u, 1443929007u, 128431217),
            new Tuple<uint, uint, uint>(215u, 1582593030u, 138664023),
            new Tuple<uint, uint, uint>(216u, 1730689458u, 148096428),
            new Tuple<uint, uint, uint>(217u, 1891512364u, 160822906),
            new Tuple<uint, uint, uint>(218u, 2064457725u, 172945361),
            new Tuple<uint, uint, uint>(219u, 2249031458u, 184573733),
            new Tuple<uint, uint, uint>(220u, 2449858070u, 200826612),
            new Tuple<uint, uint, uint>(221u, 2667631083u, 217773013),
            new Tuple<uint, uint, uint>(222u, 2902448781u, 234817698),
            new Tuple<uint, uint, uint>(223u, 3160874561u, 258425780),
            new Tuple<uint, uint, uint>(224u, 3440192563u, 279318002),
            new Tuple<uint, uint, uint>(225u, 3750444304u, 310251741),
            new Tuple<uint, uint, uint>(226u, 4100490438u, 350046134)
        };

        // TODO: move this to be loaded from a DAT
        public static List<Tuple<uint, uint, uint>> TrainedChart = new List<Tuple<uint, uint, uint>>()
        {
            new Tuple<uint, uint, uint>(0u, 0u, 0u),
            new Tuple<uint, uint, uint>(1u, 58u, 58),
            new Tuple<uint, uint, uint>(2u, 138u, 80),
            new Tuple<uint, uint, uint>(3u, 243u, 105),
            new Tuple<uint, uint, uint>(4u, 372u, 129),
            new Tuple<uint, uint, uint>(5u, 526u, 154),
            new Tuple<uint, uint, uint>(6u, 704u, 178),
            new Tuple<uint, uint, uint>(7u, 908u, 204),
            new Tuple<uint, uint, uint>(8u, 1138u, 230),
            new Tuple<uint, uint, uint>(9u, 1395u, 257),
            new Tuple<uint, uint, uint>(10u, 1678u, 283),
            new Tuple<uint, uint, uint>(11u, 1988u, 310),
            new Tuple<uint, uint, uint>(12u, 2326u, 338),
            new Tuple<uint, uint, uint>(13u, 2693u, 367),
            new Tuple<uint, uint, uint>(14u, 3089u, 396),
            new Tuple<uint, uint, uint>(15u, 3515u, 426),
            new Tuple<uint, uint, uint>(16u, 3971u, 456),
            new Tuple<uint, uint, uint>(17u, 4459u, 488),
            new Tuple<uint, uint, uint>(18u, 4980u, 521),
            new Tuple<uint, uint, uint>(19u, 5534u, 554),
            new Tuple<uint, uint, uint>(20u, 6122u, 588),
            new Tuple<uint, uint, uint>(21u, 6747u, 625),
            new Tuple<uint, uint, uint>(22u, 7408u, 661),
            new Tuple<uint, uint, uint>(23u, 8107u, 699),
            new Tuple<uint, uint, uint>(24u, 8846u, 739),
            new Tuple<uint, uint, uint>(25u, 9625u, 779),
            new Tuple<uint, uint, uint>(26u, 10448u, 823),
            new Tuple<uint, uint, uint>(27u, 11316u, 868),
            new Tuple<uint, uint, uint>(28u, 12230u, 914),
            new Tuple<uint, uint, uint>(29u, 13192u, 962),
            new Tuple<uint, uint, uint>(30u, 14206u, 1014),
            new Tuple<uint, uint, uint>(31u, 15273u, 1067),
            new Tuple<uint, uint, uint>(32u, 16396u, 1123),
            new Tuple<uint, uint, uint>(33u, 17578u, 1182),
            new Tuple<uint, uint, uint>(34u, 18821u, 1243),
            new Tuple<uint, uint, uint>(35u, 20130u, 1309),
            new Tuple<uint, uint, uint>(36u, 21508u, 1378),
            new Tuple<uint, uint, uint>(37u, 22958u, 1450),
            new Tuple<uint, uint, uint>(38u, 24485u, 1527),
            new Tuple<uint, uint, uint>(39u, 26092u, 1607),
            new Tuple<uint, uint, uint>(40u, 27786u, 1694),
            new Tuple<uint, uint, uint>(41u, 29572u, 1786),
            new Tuple<uint, uint, uint>(42u, 31454u, 1882),
            new Tuple<uint, uint, uint>(43u, 33438u, 1984),
            new Tuple<uint, uint, uint>(44u, 35533u, 2095),
            new Tuple<uint, uint, uint>(45u, 37743u, 2210),
            new Tuple<uint, uint, uint>(46u, 40078u, 2335),
            new Tuple<uint, uint, uint>(47u, 42545u, 2467),
            new Tuple<uint, uint, uint>(48u, 45152u, 2607),
            new Tuple<uint, uint, uint>(49u, 47911u, 2759),
            new Tuple<uint, uint, uint>(50u, 50830u, 2919),
            new Tuple<uint, uint, uint>(51u, 53921u, 3091),
            new Tuple<uint, uint, uint>(52u, 57196u, 3275),
            new Tuple<uint, uint, uint>(53u, 60668u, 3472),
            new Tuple<uint, uint, uint>(54u, 64350u, 3682),
            new Tuple<uint, uint, uint>(55u, 68259u, 3909),
            new Tuple<uint, uint, uint>(56u, 72409u, 4150),
            new Tuple<uint, uint, uint>(57u, 76818u, 4409),
            new Tuple<uint, uint, uint>(58u, 81506u, 4688),
            new Tuple<uint, uint, uint>(59u, 86493u, 4987),
            new Tuple<uint, uint, uint>(60u, 91800u, 5307),
            new Tuple<uint, uint, uint>(61u, 97451u, 5651),
            new Tuple<uint, uint, uint>(62u, 103472u, 6021),
            new Tuple<uint, uint, uint>(63u, 109890u, 6418),
            new Tuple<uint, uint, uint>(64u, 116736u, 6846),
            new Tuple<uint, uint, uint>(65u, 124040u, 7304),
            new Tuple<uint, uint, uint>(66u, 131838u, 7798),
            new Tuple<uint, uint, uint>(67u, 140167u, 8329),
            new Tuple<uint, uint, uint>(68u, 149067u, 8900),
            new Tuple<uint, uint, uint>(69u, 158582u, 9515),
            new Tuple<uint, uint, uint>(70u, 168758u, 10176),
            new Tuple<uint, uint, uint>(71u, 179646u, 10888),
            new Tuple<uint, uint, uint>(72u, 191301u, 11655),
            new Tuple<uint, uint, uint>(73u, 203781u, 12480),
            new Tuple<uint, uint, uint>(74u, 217149u, 13368),
            new Tuple<uint, uint, uint>(75u, 231474u, 14325),
            new Tuple<uint, uint, uint>(76u, 246830u, 15356),
            new Tuple<uint, uint, uint>(77u, 263297u, 16467),
            new Tuple<uint, uint, uint>(78u, 280959u, 17662),
            new Tuple<uint, uint, uint>(79u, 299911u, 18952),
            new Tuple<uint, uint, uint>(80u, 320252u, 20341),
            new Tuple<uint, uint, uint>(81u, 342089u, 21837),
            new Tuple<uint, uint, uint>(82u, 365539u, 23450),
            new Tuple<uint, uint, uint>(83u, 390727u, 25188),
            new Tuple<uint, uint, uint>(84u, 417789u, 27062),
            new Tuple<uint, uint, uint>(85u, 446871u, 29082),
            new Tuple<uint, uint, uint>(86u, 478129u, 31258),
            new Tuple<uint, uint, uint>(87u, 511735u, 33606),
            new Tuple<uint, uint, uint>(88u, 547871u, 36136),
            new Tuple<uint, uint, uint>(89u, 586735u, 38864),
            new Tuple<uint, uint, uint>(90u, 628540u, 41805),
            new Tuple<uint, uint, uint>(91u, 673517u, 44977),
            new Tuple<uint, uint, uint>(92u, 721913u, 48396),
            new Tuple<uint, uint, uint>(93u, 773996u, 52083),
            new Tuple<uint, uint, uint>(94u, 830054u, 56058),
            new Tuple<uint, uint, uint>(95u, 890401u, 60347),
            new Tuple<uint, uint, uint>(96u, 955370u, 64969),
            new Tuple<uint, uint, uint>(97u, 1025326u, 69956),
            new Tuple<uint, uint, uint>(98u, 1100659u, 75333),
            new Tuple<uint, uint, uint>(99u, 1181791u, 81132),
            new Tuple<uint, uint, uint>(100u, 1269177u, 87386),
            new Tuple<uint, uint, uint>(101u, 1363308u, 94131),
            new Tuple<uint, uint, uint>(102u, 1464714u, 101406),
            new Tuple<uint, uint, uint>(103u, 1573965u, 109251),
            new Tuple<uint, uint, uint>(104u, 1691679u, 117714),
            new Tuple<uint, uint, uint>(105u, 1818520u, 126841),
            new Tuple<uint, uint, uint>(106u, 1955205u, 136685),
            new Tuple<uint, uint, uint>(107u, 2102508u, 147303),
            new Tuple<uint, uint, uint>(108u, 2261264u, 158756),
            new Tuple<uint, uint, uint>(109u, 2432373u, 171109),
            new Tuple<uint, uint, uint>(110u, 2616806u, 184433),
            new Tuple<uint, uint, uint>(111u, 2815610u, 198804),
            new Tuple<uint, uint, uint>(112u, 3029917u, 214307),
            new Tuple<uint, uint, uint>(113u, 3260945u, 231028),
            new Tuple<uint, uint, uint>(114u, 3510009u, 249064),
            new Tuple<uint, uint, uint>(115u, 3778529u, 268520),
            new Tuple<uint, uint, uint>(116u, 4068034u, 289505),
            new Tuple<uint, uint, uint>(117u, 4380177u, 312143),
            new Tuple<uint, uint, uint>(118u, 4716738u, 336561),
            new Tuple<uint, uint, uint>(119u, 5079638u, 362900),
            new Tuple<uint, uint, uint>(120u, 5470950u, 391312),
            new Tuple<uint, uint, uint>(121u, 5892911u, 421961),
            new Tuple<uint, uint, uint>(122u, 6347931u, 455020),
            new Tuple<uint, uint, uint>(123u, 6838614u, 490683),
            new Tuple<uint, uint, uint>(124u, 7367765u, 529151),
            new Tuple<uint, uint, uint>(125u, 7938414u, 570649),
            new Tuple<uint, uint, uint>(126u, 8553825u, 615411),
            new Tuple<uint, uint, uint>(127u, 9217523u, 663698),
            new Tuple<uint, uint, uint>(128u, 9933309u, 715786),
            new Tuple<uint, uint, uint>(129u, 10705283u, 771974),
            new Tuple<uint, uint, uint>(130u, 11537868u, 832585),
            new Tuple<uint, uint, uint>(131u, 12435837u, 897969),
            new Tuple<uint, uint, uint>(132u, 13404336u, 968499),
            new Tuple<uint, uint, uint>(133u, 14448918u, 1044582),
            new Tuple<uint, uint, uint>(134u, 15575574u, 1126656),
            new Tuple<uint, uint, uint>(135u, 16790764u, 1215190),
            new Tuple<uint, uint, uint>(136u, 18101461u, 1310697),
            new Tuple<uint, uint, uint>(137u, 19515183u, 1413722),
            new Tuple<uint, uint, uint>(138u, 21040043u, 1524860),
            new Tuple<uint, uint, uint>(139u, 22684790u, 1644747),
            new Tuple<uint, uint, uint>(140u, 24458865u, 1774075),
            new Tuple<uint, uint, uint>(141u, 26372451u, 1913586),
            new Tuple<uint, uint, uint>(142u, 28436532u, 2064081),
            new Tuple<uint, uint, uint>(143u, 30662960u, 2226428),
            new Tuple<uint, uint, uint>(144u, 33064516u, 2401556),
            new Tuple<uint, uint, uint>(145u, 35654992u, 2590476),
            new Tuple<uint, uint, uint>(146u, 38449264u, 2794272),
            new Tuple<uint, uint, uint>(147u, 41463378u, 3014114),
            new Tuple<uint, uint, uint>(148u, 44714647u, 3251269),
            new Tuple<uint, uint, uint>(149u, 48221744u, 3507097),
            new Tuple<uint, uint, uint>(150u, 52004816u, 3783072),
            new Tuple<uint, uint, uint>(151u, 56085593u, 4080777),
            new Tuple<uint, uint, uint>(152u, 60487519u, 4401926),
            new Tuple<uint, uint, uint>(153u, 65235884u, 4748365),
            new Tuple<uint, uint, uint>(154u, 70357967u, 5122083),
            new Tuple<uint, uint, uint>(155u, 75883199u, 5525232),
            new Tuple<uint, uint, uint>(156u, 81843326u, 5960127),
            new Tuple<uint, uint, uint>(157u, 88272594u, 6429268),
            new Tuple<uint, uint, uint>(158u, 95207949u, 6935355),
            new Tuple<uint, uint, uint>(159u, 102689242u, 7481293),
            new Tuple<uint, uint, uint>(160u, 110759467u, 8070225),
            new Tuple<uint, uint, uint>(161u, 119465000u, 8705533),
            new Tuple<uint, uint, uint>(162u, 128855871u, 9390871),
            new Tuple<uint, uint, uint>(163u, 138986049u, 10130178),
            new Tuple<uint, uint, uint>(164u, 149913755u, 10927706),
            new Tuple<uint, uint, uint>(165u, 161701793u, 11788038),
            new Tuple<uint, uint, uint>(166u, 174417913u, 12716120),
            new Tuple<uint, uint, uint>(167u, 188135201u, 13717288),
            new Tuple<uint, uint, uint>(168u, 202932500u, 14797299),
            new Tuple<uint, uint, uint>(169u, 218894860u, 15962360),
            new Tuple<uint, uint, uint>(170u, 236114028u, 17219168),
            new Tuple<uint, uint, uint>(171u, 254688979u, 18574951),
            new Tuple<uint, uint, uint>(172u, 274726480u, 20037501),
            new Tuple<uint, uint, uint>(173u, 296341707u, 21615227),
            new Tuple<uint, uint, uint>(174u, 319658907u, 23317200),
            new Tuple<uint, uint, uint>(175u, 344812110u, 25153203),
            new Tuple<uint, uint, uint>(176u, 371945902u, 27133792),
            new Tuple<uint, uint, uint>(177u, 401216255u, 29270353),
            new Tuple<uint, uint, uint>(178u, 432791424u, 31575169),
            new Tuple<uint, uint, uint>(179u, 466852915u, 34061491),
            new Tuple<uint, uint, uint>(180u, 503596527u, 36743612),
            new Tuple<uint, uint, uint>(181u, 543233477u, 39636950),
            new Tuple<uint, uint, uint>(182u, 585991620u, 42758143),
            new Tuple<uint, uint, uint>(183u, 632116749u, 46125129),
            new Tuple<uint, uint, uint>(184u, 681874018u, 49757269),
            new Tuple<uint, uint, uint>(185u, 735549461u, 53675443),
            new Tuple<uint, uint, uint>(186u, 793451636u, 57902175),
            new Tuple<uint, uint, uint>(187u, 855913403u, 62461767),
            new Tuple<uint, uint, uint>(188u, 923293832u, 67380429),
            new Tuple<uint, uint, uint>(189u, 995980273u, 72686441),
            new Tuple<uint, uint, uint>(190u, 1074390578u, 78410305),
            new Tuple<uint, uint, uint>(191u, 1158975507u, 84584929),
            new Tuple<uint, uint, uint>(192u, 1250221316u, 91245809),
            new Tuple<uint, uint, uint>(193u, 1348652558u, 98431242),
            new Tuple<uint, uint, uint>(194u, 1454835090u, 106182532),
            new Tuple<uint, uint, uint>(195u, 1569379334u, 114544244),
            new Tuple<uint, uint, uint>(196u, 1692943784u, 123564450),
            new Tuple<uint, uint, uint>(197u, 1826238790u, 133295006),
            new Tuple<uint, uint, uint>(198u, 1970030642u, 143791852),
            new Tuple<uint, uint, uint>(199u, 2125145977u, 155115335),
            new Tuple<uint, uint, uint>(200u, 2292476532u, 167330555),
            new Tuple<uint, uint, uint>(201u, 2472984268u, 180507736),
            new Tuple<uint, uint, uint>(202u, 2667706901u, 194722633),
            new Tuple<uint, uint, uint>(203u, 2877763869u, 210056968),
            new Tuple<uint, uint, uint>(204u, 3104362767u, 226598898),
            new Tuple<uint, uint, uint>(205u, 3348806291u, 244443524),
            new Tuple<uint, uint, uint>(206u, 3612499722u, 263693431),
            new Tuple<uint, uint, uint>(207u, 3896959013u, 284459291),
            new Tuple<uint, uint, uint>(208u, 4203819496u, 306860483)
        };
    }

    public enum SkillStatus : uint
    {
        Inactive,
        Untrained,
        Trained,
        Specialized
    }
}
