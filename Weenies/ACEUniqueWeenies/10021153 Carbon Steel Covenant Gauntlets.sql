DELETE FROM `weenie` WHERE `class_Id` = 10021153;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10021153, 'EGgauntletscovenant', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10021153,   1,          2) /* ItemType - Armor */
     , (10021153,   3,         19) /* PaletteTemplate - Copper */
     , (10021153,   4,      32768) /* ClothingPriority - Hands */
     , (10021153,   5,        919) /* EncumbranceVal */
     , (10021153,   8,        460) /* Mass */
     , (10021153,   9,         32) /* ValidLocations - HandWear */
     , (10021153,  16,          64) /* ItemUseable - No */
     , (10021153,  18,        64) /* UiEffects - Magical, BoostHealth, BoostMana, Frost, Bludgeoning */
     , (10021153,  19,        653) /* Value */
     , (10021153,  27,         32) /* ArmorType - Metal */
     , (10021153,  28,        509) /* ArmorLevel */
     , (10021153,  33,          1) /* Bonded - Bonded */
     , (10021153,  36,       9999) /* ResistMagic */
     , (10021153,  44,          3) /* Damage */
     , (10021153,  45,          4) /* DamageType - Bludgeon */
     , (10021153,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (10021153, 105,         10) /* ItemWorkmanship */
     , (10021153, 106,        590) /* ItemSpellcraft */
     , (10021153, 107,       2000) /* ItemCurMana */
     , (10021153, 108,       1000) /* ItemMaxMana */
     , (10021153, 109,          1) /* ItemDifficulty */
     , (10021153, 124,          3) /* Version */
     , (10021153, 169,  151651588) /* TsysMutationData */
     , (10021153, 171,         10) /* NumTimesTinkered */
     , (10021153, 179,       4096) /* ImbuedEffect - MagicDefense */
     , (10021153, 370,          1) /* GearDamage */
     , (10021153, 374,          2) /* GearCritDamage */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10021153,  22, True ) /* Inscribable */
     , (10021153, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (10021153,  12,   0.857) /* Shade */
     , (10021153,  13,       2) /* ArmorModVsSlash */
     , (10021153,  14,       2) /* ArmorModVsPierce */
     , (10021153,  15,       2) /* ArmorModVsBludgeon */
     , (10021153,  16,       2) /* ArmorModVsCold */
     , (10021153,  17,       2) /* ArmorModVsFire */
     , (10021153,  18,       2) /* ArmorModVsAcid */
     , (10021153,  19,       2) /* ArmorModVsElectric */
     , (10021153,  22,    0.75) /* DamageVariance */
     , (10021153, 110,       1) /* BulkMod */
     , (10021153, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10021153,   1, 'Carbon Steel Covenant Gauntlets') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10021153,   1, 0x020000D8) /* Setup */
     , (10021153,   3, 0x20000014) /* SoundTable */
     , (10021153,   6, 0x0400007E) /* PaletteBase */
     , (10021153,   7, 0x100003DD) /* ClothingBase */
     , (10021153,   8, 0x06000FCD) /* Icon */
     , (10021153,  22, 0x3400002B) /* PhysicsEffectTable */
     , (10021153,  36, 0x0E000016) /* MutateFilter */
     , (10021153,  46, 0x38000022) /* TsysMutationFilter */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (10021153,  6095,      2)  /* Legendary Impenetrability */
     , (10021153,  2349,      2)  /* Hieromancer's Ward */
     , (10021153,  4018,      2)  /* Permafrost */;
