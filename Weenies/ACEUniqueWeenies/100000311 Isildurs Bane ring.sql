DELETE FROM `weenie` WHERE `class_Id` = 100000311;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (100000311, 'Isldurs Bane3', 1, '2005-02-09 10:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (100000311,   1,          8) /* ItemType - Jewelry */
     , (100000311,   3,         39) /* PaletteTemplate - Black */
     , (100000311,   5,         15) /* EncumbranceVal */
     , (100000311,   8,         10) /* Mass */
     , (100000311,   9,     786432) /* ValidLocations - FingerWear */
     , (100000311,  16,          1) /* ItemUseable - No */
     , (100000311,  18,          1) /* UiEffects - Magical */
     , (100000311,  19,         10) /* Value */
     , (100000311,  33,          1) /* Bonded - Bonded */
     , (100000311,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (100000311, 105,         10) /* ItemWorkmanship */
     , (100000311, 106,        580) /* ItemSpellcraft */
     , (100000311, 107,        200) /* ItemCurMana */
     , (100000311, 108,       1000) /* ItemMaxMana */
     , (100000311, 109,          1) /* ItemDifficulty */
     , (100000311, 376,          3) /* GearHealingBoost */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (100000311,  22, True ) /* Inscribable */
     , (100000311, 112, False) /* ProcSpellSelfTargeted */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (100000311,   5,       0) /* ManaRate */
     , (100000311,  39,     0.5) /* DefaultScale */
     , (100000311, 156,    0.15) /* ProcSpellRate */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (100000311,   1, 'Isildurs Bane') /* Name */
     , (100000311,  16, 'Great Ring of Power') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (100000311,   1, 0x02000103) /* Setup */
     , (100000311,   3, 0x20000014) /* SoundTable */
     , (100000311,   6, 0x04000BEF) /* PaletteBase */
     , (100000311,   7, 0x1000035E) /* ClothingBase */
     , (100000311,   8, 0x06003337) /* Icon */
     , (100000311,  22, 0x3400002B) /* PhysicsEffectTable */
     , (100000311,  36, 0x0E000016) /* MutateFilter */
     , (100000311,  55,       5368) /* ProcSpell - Incantation of Nether Arc */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (100000311,  3864,      2)  /* Zongo's Fist */
     , (100000311,  3530,      2)  /* Ketnan's Eye */
     , (100000311,  3533,      2)  /* Brighteyes' Favor */
     , (100000311,  3863,      2)  /* Hunter's Hardiness */
     , (100000311,  3531,      2)  /* Bobo's Quickening */
     , (100000311,  3862,      2)  /* Duke Raoul's Pride */
     , (100000311,  3378,      2)  /* Vision Beyond the Grave */
     , (100000311,  4291,      2)  /* Incantation of Armor Self */
     , (100000311,  4169,      2)  /* Harbinger Blood Infusion */
     , (100000311,  4462,      2)  /* Incantation of Blade Protection Self */
     , (100000311,  4464,      2)  /* Incantation of Bludgeoning Protection Self */
     , (100000311,  4460,      2)  /* Incantation Acid Protection */
     , (100000311,  4466,      2)  /* Incantation Cold Protection */
     , (100000311,  4468,      2)  /* Incantation Flame Protection */
     , (100000311,  4470,      2)  /* Incantation Lightning Protection */

     /* Harbinger says 80%, but doesn't seem as strong ig as level 8 pros when tested */
          -- , (100000311,  4189,      2)  /* Harbinger Acid Protection */
     -- , (100000311,  4190,      2)  /* Harbinger Cold Protection */
     -- , (100000311,  4191,      2)  /* Harbinger Flame Protection */
     -- , (100000311,  4192,      2)  /* Harbinger Lightning Protection */
     , (100000311,  4472,      2)  /* Incantation of Piercing Protection Self */
     , (100000311,  4086,      2)  /* Icy Shield */
     , (100000311,  4015,      2)  /* Ruschk Skin */
     , (100000311,  3811,      2)  /* Blackmoor's Favor */
     , (100000311,  6079,      2)  /* Legendary Storm Ward */
     , (100000311,  6080,      2)  /* Legendary Acid Ward */
     , (100000311,  6081,      2)  /* Legendary Bludgeoning Ward */
     , (100000311,  6082,      2)  /* Legendary Flame Ward */
     , (100000311,  6083,      2)  /* Legendary Frost Ward */
     , (100000311,  6084,      2)  /* Legendary Piercing Ward */
     , (100000311,  6085,      2)  /* Legendary Slashing Ward */
     , (100000311,  6102,      2)  /* Legendary Armor */
     , (100000311,  4510,      2)  /* Incantation of Arcane Enlightenment Self */
     , (100000311,  3361,      2)  /* The Art of Destruction */
     , (100000311,  5418,      2)  /* Incantation of Void Magic Mastery Self */
     , (100000311,  4596,      2)  /* Incantation of Magic Resistance Self */
     , (100000311,  4556,      2)  /* Incantation of Healing Mastery Self */
     , (100000311,  4616,      2)  /* Incantation of Sprint Self */
     , (100000311,  4564,      2)  /* Incantation of Item Enchantment Mastery Self */
     , (100000311,  4602,      2)  /* Incantation of Mana Conversion Mastery Self */
     , (100000311,  4582,      2)  /* Incantation of Life Magic Mastery Self */
     , (100000311,  4560,      2)  /* Incantation of Invulnerability Self */
     , (100000311,  2959,      2)  /* Mark of the Priestess */
     , (100000311,  6060,      2)  /* Legendary Life Magic Aptitude */
     , (100000311,  6056,      2)  /* Legendary Item Enchantment Aptitude */
     , (100000311,  4895,      2)  /* Master Warlock's War Magic Aptitude */
     , (100000311,  5434,      2)  /* Master Voidlock's Void Magic Aptitude */
     , (100000311,  4827,      2)  /* Master Artifex's Item Aptitude */
     , (100000311,  3802,      2)  /* Shadow Reek */
     , (100000311,  6145,      2)  /* Master Invoker's Summoning Aptitude */
     , (100000311,  6125,      2)  /* Legendary Summoning Prowess */
     , (100000311,  6123,      2)  /* Incantation of Summoning Mastery Self */
     , (100000311,  4861,      2)  /* Master Guardian's Invulnerability */
     , (100000311,  4853,      2)  /* Master Negator's Magic Resistance */
     , (100000311,  4865,      2)  /* Master Wayfarer's Impregnability */
     , (100000311,  4741,      2)  /* Master Sage's Focus */
     , (100000311,  4753,      2)  /* Master Adherent's Willpower */
     , (100000311,  4737,      2)  /* Master Hero's Endurance */
     , (100000311,  4749,      2)  /* Master Brute's Strength */
     , (100000311,  4733,      2)  /* Master Duelist's Coordination */
     , (100000311,  5187,      2)  /* Rare Damage Boost X */
     , (100000311,  5197,      2)  /* Rare Damage Reduction V */
     , (100000311,  6235,      2)  /* Paragon's Sneak Attack Mastery V */
     , (100000311,  6245,      2)  /* Paragon's Void Magic Mastery V */
     , (100000311,  6250,      2)  /* Paragon's War Magic Mastery V */
     , (100000311,  6260,      2)  /* Paragon's Willpower V */
     , (100000311,  6265,      2)  /* Paragon's Coordination V */
     , (100000311,  6270,      2)  /* Paragon's Endurance V */
     , (100000311,  6275,      2)  /* Paragon's Focus V */
     , (100000311,  6280,      2)  /* Paragon Quickness V */
     , (100000311,  6285,      2)  /* Paragon's Strength V */
     , (100000311,  6290,      2)  /* Paragon's Stamina V */
     , (100000311,  6295,      2)  /* Paragon's Critical Damage Boost V */
     , (100000311,  6299,      2)  /* Paragon's Critical Damage Reduction IV */
     , (100000311,  6305,      2)  /* Paragon's Damage Boost V */
     , (100000311,  6310,      2)  /* Paragon's Damage Reduction V */
     , (100000311,  6329,      2)  /* Gauntlet Critical Damage Boost II */
     , (100000311,  6331,      2)  /* Gauntlet Damage Boost II */
     , (100000311,  6333,      2)  /* Gauntlet Damage Reduction II */
     , (100000311,  6335,      2)  /* Gauntlet Critical Damage Reduction II */
     , (100000311,  6337,      2)  /* Gauntlet Healing Boost II */
     , (100000311,  4904,      2)  /* Society Master's Blessing */
     , (100000311,  5753,      2)  /* Cloaked in Skill */
     , (100000311,  3204,      2)  /* Blazing Heart */
     , (100000311,  3800,      2)  /* Burning Spirit */
     , (100000311,  3005,      2)  /* Dispersion */
     , (100000311,  3006,      2)  /* Foresight */
     , (100000311,  3007,      2)  /* Uncanny Dodge */
     , (100000311,  3761,      2)  /* Fiun Resistance */
     , (100000311,  5140,      2)  /* Augmented Damage III */
     , (100000311,  5143,      2)  /* Augmented Damage Reduction III */
     , (100000311,  5146,      2)  /* Augmented Health III */
     , (100000311,  5149,      2)  /* Augmented Mana III */
     , (100000311,  5152,      2)  /* Augmented Stamina III */
     , (100000311,  2997,      2)  /* Splendor of the Firebird */
     , (100000311,  2993,      2)  /* Grace of the Unicorn */
     , (100000311,  2995,      2)  /* Power of the Dragon */
     , (100000311,  3897,      2)  /* Dark Purpose */
     , (100000311,  3894,      2)  /* Dark Persistence */
     , (100000311,  3895,      2)  /* Dark Reflexes */
     , (100000311,  3896,      2)  /* Dark Equilibrium */
     , (100000311,  3801,      2)  /* Shadow Touch */
     , (100000311,  4763,      2)  /* Masterwork Acid Resistance */
     , (100000311,  4767,      2)  /* Masterwork Bludgeoning Resistance */
     , (100000311,  4771,      2)  /* Masterwork Flame Resistance */
     , (100000311,  4775,      2)  /* Masterwork Frost Resistance */
     , (100000311,  4779,      2)  /* Masterwork Lightning Resistance */
     , (100000311,  4783,      2)  /* Masterwork Piercing Resistance */
     , (100000311,  4787,      2)  /* Masterwork Slashing Resistance */
     , (100000311,  6176,      2)  /* Genius */
     , (100000311,  2638,      2)  /* Heart of Oak */
     , (100000311,  3242,      2)  /* Weave of Chorizite */
     , (100000311,  5652,      2)  /* Weave of the Item Enchantment V */
     , (100000311,  5667,      2)  /* Weave of Life Magic V */
     , (100000311,  5682,      2)  /* Weave of the Magic Resistance V */
     , (100000311,  5742,      2)  /* Weave of Void Magic V */
     , (100000311,  6142,      2)  /* Novice Invoker's Summoning Aptitude */
     , (100000311,  5747,      2)  /* Weave of War Magic V */
     , (100000311,  4280,      2)  /* Deck of Hands Favor */
     , (100000311,  4281,      2)  /* Deck of Eyes Favor */
     , (100000311,  4745,      2)  /* Master Rover's Quickness */
     , (100000311,  2428,      2)  /* Timaru's Shelter */
     , (100000311,  2430,      2)  /* Timaru's Shelter */
     , (100000311,  2429,      2)  /* Timaru's Shelter */
     , (100000311,  2440,      2)  /* Greater Stone Cliffs */
     , (100000311,  4974,      2)  /* Life Giver's Boon */
     , (100000311,  5451,      2)  /* Luminous Vitality */
     , (100000311,  5438,      2)  /* Corruptor's Boon */
     , (100000311,  5122,      2)  /* Call of Leadership V */
     , (100000311,  5127,      2)  /* Answer of Loyalty (Mana) V */
     , (100000311,  5132,      2)  /* Answer of Loyalty (Stamina) V */
     , (100000311,  2006,      2)  /* Warrior's Ultimate Vitality */
     , (100000311,  2014,      2)  /* Wizard's Ultimate Intellect */
     , (100000311,  2010,      2)  /* Warrior's Ultimate Vigor */
     , (100000311,  3034,      2)  /* Benediction of Immortality */
     , (100000311,  3810,      2)  /* Asheron's Benediction */
     , (100000311,  6170,      2)  /* Honeyed Life Mead */
     , (100000311,  6171,      2)  /* Honeyed Mana Mead */
     , (100000311,  6172,      2)  /* Honeyed Vigor Mead */
     , (100000311,  2016,      2)  /* Impulse */
     , (100000311,  4025,      2)  /* Cast Iron Stomach */
     , (100000311,  4759,      2)  /* Journeyman Tracker's Stamina */
     , (100000311,  6054,      2)  /* Legendary Impregnability */
     , (100000311,  6055,      2)  /* Legendary Invulnerability */
     , (100000311,  6063,      2)  /* Legendary Magic Resistance */
     , (100000311,  6064,      2)  /* Legendary Mana Conversion Prowess */
     , (100000311,  6074,      2)  /* Legendary Void Magic Aptitude */
     , (100000311,  6075,      2)  /* Legendary War Magic Aptitude */
     , (100000311,  6076,      2)  /* Legendary Stamina Gain */
     , (100000311,  6053,      2)  /* Legendary Healing Prowess */
     , (100000311,  6070,      2)  /* Legendary Sneak Attack Prowess */
     , (100000311,  2642,      2)  /* Consumption */
     , (100000311,  6101,      2)  /* Legendary Willpower */
     , (100000311,  6103,      2)  /* Legendary Coordination */
     , (100000311,  6105,      2)  /* Legendary Focus */
     , (100000311,  6106,      2)  /* Legendary Quickness */
     , (100000311,  6107,      2)  /* Legendary Strength */
     , (100000311,  6104,      2)  /* Legendary Endurance */
     , (100000311,  4498,      2)  /* Incantation of Rejuvenation Self */
     , (100000311,  4072,      2)  /* Aurlanaa's Resolve */
     , (100000311,  4096,      2)  /* Flame Chain */
     , (100000311,  2386,      2)  /* Indomitability */
     , (100000311,  6077,      2)  /* Legendary Health Gain */
     , (100000311,  2446,      2)  /* Greater Growth */
     , (100000311,  3249,      2)  /* Ghostly Chorus */
     , (100000311,  6078,      2)  /* Legendary Mana Gain */
     , (100000311,  4068,      2)  /* Mucor Mana Well */;
