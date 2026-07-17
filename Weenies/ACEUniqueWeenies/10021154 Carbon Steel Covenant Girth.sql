DELETE FROM `weenie` WHERE `class_Id` = 10021154;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10021154, 'EGgirthcovenant', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10021154,   1,          2) /* ItemType - Armor */
     , (10021154,   3,         19) /* PaletteTemplate - Copper */
     , (10021154,   4,       2048) /* ClothingPriority - OuterwearAbdomen */
     , (10021154,   5,       1099) /* EncumbranceVal */
     , (10021154,   8,        550) /* Mass */
     , (10021154,   9,       1024) /* ValidLocations - AbdomenArmor */
     , (10021154,  16,          1) /* ItemUseable - No */
     , (10021154,  18,          64) /* UiEffects - Magical */
     , (10021154,  19,        980) /* Value */
     , (10021154,  27,         32) /* ArmorType - Metal */
     , (10021154,  28,        509) /* ArmorLevel */
     , (10021154,  33,          1) /* Bonded - Bonded */
     , (10021154,  36,       9999) /* ResistMagic */
     , (10021154,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (10021154, 105,         10) /* ItemWorkmanship */
     , (10021154, 106,        590) /* ItemSpellcraft */
     , (10021154, 107,       2000) /* ItemCurMana */
     , (10021154, 108,       1000) /* ItemMaxMana */
     , (10021154, 109,          1) /* ItemDifficulty */
     , (10021154, 124,          3) /* Version */
     , (10021154, 169,  118096132) /* TsysMutationData */
     , (10021154, 171,         10) /* NumTimesTinkered */
     , (10021154, 179,       4096) /* ImbuedEffect - MagicDefense */
     , (10021154, 371,          1) /* GearDamageResist */
     , (10021154, 374,          2) /* GearCritDamage */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10021154,  22, True ) /* Inscribable */
     , (10021154, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (10021154,  12,   0.871) /* Shade */
     , (10021154,  13,       2) /* ArmorModVsSlash */
     , (10021154,  14,       2) /* ArmorModVsPierce */
     , (10021154,  15,       2) /* ArmorModVsBludgeon */
     , (10021154,  16,       2) /* ArmorModVsCold */
     , (10021154,  17,       2) /* ArmorModVsFire */
     , (10021154,  18,       2) /* ArmorModVsAcid */
     , (10021154,  19,       2) /* ArmorModVsElectric */
     , (10021154, 110,       1) /* BulkMod */
     , (10021154, 111,     1.5) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10021154,   1, 'Carbon Steel Covenant Girth') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10021154,   1, 0x020000D7) /* Setup */
     , (10021154,   3, 0x20000014) /* SoundTable */
     , (10021154,   6, 0x0400007E) /* PaletteBase */
     , (10021154,   7, 0x100003DE) /* ClothingBase */
     , (10021154,   8, 0x060012F0) /* Icon */
     , (10021154,  22, 0x3400002B) /* PhysicsEffectTable */
     , (10021154,  36, 0x0E000016) /* MutateFilter */
     , (10021154,  46, 0x38000022) /* TsysMutationFilter */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (10021154,  6095,      2)  /* Legendary Impenetrability */
     , (10021154,  2349,      2)  /* Hieromancer's Ward */
     , (10021154,  4018,      2)  /* Permafrost */;
