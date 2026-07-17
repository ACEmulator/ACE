DELETE FROM `weenie` WHERE `class_Id` = 10030352;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10030352, 'EGbraceletrarebinding', 1, '2021-11-17 16:56:08') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10030352,   1,          8) /* ItemType - Jewelry */
     , (10030352,   3,          1) /* PaletteTemplate - AquaBlue */
     , (10030352,   5,         60) /* EncumbranceVal */
     , (10030352,   8,         90) /* Mass */
     , (10030352,   9,     196608) /* ValidLocations - WristWear */
     , (10030352,  16,          1) /* ItemUseable - No */
     , (10030352,  17,        276) /* RareId */
     , (10030352,  19,      50000) /* Value */
     , (10030352,  33,          1) /* Bonded - Bonded */
     , (10030352,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (10030352,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (10030352, 106,        580) /* ItemSpellcraft */
     , (10030352, 107,       3000) /* ItemCurMana */
     , (10030352, 108,       3000) /* ItemMaxMana */
     , (10030352,  376,          3) /* healing bonus */
     , (10030352, 109,          0) /* ItemDifficulty */
     , (10030352, 110,          0) /* ItemAllegianceRankLimit */
     , (10030352, 151,          2) /* HookType - Wall */
     , (10030352, 169,  118162702) /* TsysMutationData */;

INSERT INTO `weenie_properties_int64` (`object_Id`, `type`, `value`)
VALUES (10030352,   4,          0) /* ItemTotalXp */
     , (10030352,   5, 2000000000) /* ItemBaseXp */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10030352,  11, True ) /* IgnoreCollisions */
     , (10030352,  13, True ) /* Ethereal */
     , (10030352,  14, True ) /* GravityStatus */
     , (10030352,  19, True ) /* Attackable */
     , (10030352,  22, True ) /* Inscribable */
     , (10030352, 100, False) /* Dyable */
     , (10030352, 112, False) /* cast on self*/;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (10030352,   5,  -0) /* ManaRate */
     , (10030352,  12,    0.66) /* Shade */
     , (10030352,  39,     0.5) /* DefaultScale */
     , (10030352, 156,     .15) /* ProcSpellRate */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10030352,   1, 'Bracelet of Binding') /* Name */
     , (10030352,  16, 'This bracelet grants the wearer a kind of specialized extra sense. Through the mind''s eye, the workmanship of an item can be assessed. Ways to improve an item or repair flaws are made visible, allowing the user to more expertly make modifications.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10030352,   1, 0x020000FB) /* Setup */
     , (10030352,   3, 0x20000014) /* SoundTable */
     , (10030352,   6, 0x04000BEF) /* PaletteBase */
     , (10030352,   8, 0x06005BDE) /* Icon */
     , (10030352,  22, 0x3400002B) /* PhysicsEffectTable */
     , (10030352,  36, 0x0E000012) /* MutateFilter */
     , (10030352,  46, 0x38000032) /* TsysMutationFilter */
     , (10030352,  52, 0x06005B0C) /* IconUnderlay */
     , (10030352,  55,       5402) /* ProcSpell - Incantation of Corruption 5338 */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (10030352,  3964,      2)  /* Epic Focus */
     , (10030352,  4512,      2)  /* Incantation of Armor Tinkering Expertise Self */
     , (10030352,  4566,      2)  /* Incantation of Item Tinkering Expertise Self */
     , (10030352,  4592,      2)  /* Incantation of Magic Item Tinkering Expertise Self */
     , (10030352,  4640,      2)  /* Incantation of Weapon Tinkering Expertise Self */
     , (10030352,  4685,      2)  /* Epic Armor Tinkering Expertise */
     , (10030352,  4698,      2)  /* Epic Item Tinkering Expertise */
     , (10030352,  4703,      2)  /* Epic Magic Item Tinkering Expertise */
     , (10030352,  4912,      2)  /* Epic Weapon Tinkering Expertise */;
