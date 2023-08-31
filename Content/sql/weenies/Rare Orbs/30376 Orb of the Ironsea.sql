DELETE FROM `weenie` WHERE `class_Id` = 30376;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (30376, 'wandrareorbironsea', 35, '2021-11-17 16:56:08') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (30376,   1,      32768) /* ItemType - Caster */
     , (30376,   3,          4) /* PaletteTemplate - Brown */
     , (30376,   5,        100) /* EncumbranceVal */
     , (30376,   8,         90) /* Mass */
     , (30376,   9,   16777216) /* ValidLocations - Held */
     , (30376,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (30376,  17,        187) /* RareId */
     , (30376,  19,      50000) /* Value */
     , (30376,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (30376,  45,          2) /* DamageType - Pierce */
     , (30376,  46,        512) /* DefaultCombatStyle - Magic */
     , (30376,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (30376,  94,         16) /* TargetType - Creature */
     , (30376, 106,        350) /* ItemSpellcraft */
     , (30376, 107,       6000) /* ItemCurMana */
     , (30376, 108,       6000) /* ItemMaxMana */
     , (30376, 109,          0) /* ItemDifficulty */
     , (30376, 110,          0) /* ItemAllegianceRankLimit */
     , (30376, 117,         30) /* ItemManaCost */
     , (30376, 151,          2) /* HookType - Wall */
     , (30376, 166,         31) /* SlayerCreatureType - Human */
     , (30376, 169,  118162702) /* TsysMutationData */
     , (30376, 179,         16) /* ImbuedEffect - PierceRending */
     , (30376, 265,         41) /* EquipmentSetId - RareDamageBoost */
     , (30376, 319,         50) /* ItemMaxLevel */
     , (30376, 320,          1) /* ItemXpStyle - Fixed */
     , (30376, 353,          0) /* WeaponType - Undef */;

INSERT INTO `weenie_properties_int64` (`object_Id`, `type`, `value`)
VALUES (30376,   4,          0) /* ItemTotalXp */
     , (30376,   5, 2000000000) /* ItemBaseXp */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (30376,  11, True ) /* IgnoreCollisions */
     , (30376,  13, True ) /* Ethereal */
     , (30376,  14, True ) /* GravityStatus */
     , (30376,  19, True ) /* Attackable */
     , (30376,  22, True ) /* Inscribable */
     , (30376, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (30376,   5,  -0.033) /* ManaRate */
     , (30376,  12,    0.66) /* Shade */
     , (30376,  29,    1.18) /* WeaponDefense */
     , (30376,  39,       1) /* DefaultScale */
     , (30376, 138,    1.25) /* SlayerDamageBonus */
     , (30376, 144,     0.2) /* ManaConversionMod */
     , (30376, 147,    0.3) /* CriticalFrequency */
     , (30376, 152,    1.22) /* ElementalDamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (30376,   1, 'Orb of the Ironsea') /* Name */
     , (30376,  16, 'Although this jewel looks solid, one has only to touch it to realize otherwise. The surface ripples like water when disturbed and yet somehow still manages to hold its spherical shape. Legend has it that this water comes from the deepest parts of the Ironsea and can only be retrieved by coaxing the denizens that live there to the surface. Such water is highly sought after by mages as it seems to help them cast their spells with more power and efficiency.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (30376,   1, 0x02001380) /* Setup */
     , (30376,   3, 0x20000014) /* SoundTable */
     , (30376,   6, 0x04000BEF) /* PaletteBase */
     , (30376,   8, 0x06005C03) /* Icon */
     , (30376,  22, 0x3400002B) /* PhysicsEffectTable */
     , (30376,  27, 0x400000E1) /* UseUserAnimation - UseMagicWand */
	 , (30376,  28,       2132) /* Spell - Nether Bolt VII */
     , (30376,  36, 0x0E000012) /* MutateFilter */
     , (30376,  46, 0x38000032) /* TsysMutationFilter */
     , (30376,  52, 0x06005B0C) /* IconUnderlay */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (30376,  4305,      2)  /* Incantation of Focus Self */
     , (30376,  4329,      2)  /* Incantation of Willpower Self */
     , (30376,  4602,      2)  /* Incantation of Mana Conversion Mastery Self */
     , (30376,  4670,      2)  /* Epic Spirit Thirst */
     , (30376,  4705,      2)  /* Epic Mana Conversion Prowess */;
