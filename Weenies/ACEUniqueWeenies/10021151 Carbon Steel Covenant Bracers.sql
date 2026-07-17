DELETE FROM `weenie` WHERE `class_Id` = 10021151;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10021151, 'EGbracerscovenant', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10021151,   1,          2) /* ItemType - Armor */
     , (10021151,   3,         19) /* PaletteTemplate - Copper */
     , (10021151,   4,       8192) /* ClothingPriority - OuterwearLowerArms */
     , (10021151,   5,        540) /* EncumbranceVal */
     , (10021151,   8,        270) /* Mass */
     , (10021151,   9,       4096) /* ValidLocations - LowerArmArmor */
     , (10021151,  16,          1) /* ItemUseable - No */
     , (10021151,  18,          64) /* UiEffects - Magical */
     , (10021151,  19,        653) /* Value */
     , (10021151,  27,         32) /* ArmorType - Metal */
     , (10021151,  28,        509) /* ArmorLevel */
     , (10021151,  33,          1) /* Bonded - Bonded */
     , (10021151,  36,       9999) /* ResistMagic */
     , (10021151,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (10021151, 105,         10) /* ItemWorkmanship */
     , (10021151, 106,        590) /* ItemSpellcraft */
     , (10021151, 107,       2000) /* ItemCurMana */
     , (10021151, 108,       1000) /* ItemMaxMana */
     , (10021151, 109,          1) /* ItemDifficulty */
     , (10021151, 124,          3) /* Version */
     , (10021151, 169,  118097156) /* TsysMutationData */
     , (10021151, 171,         10) /* NumTimesTinkered */
     , (10021151, 179,       4096) /* ImbuedEffect - MagicDefense */
     , (10021151, 374,          3) /* GearCritDamage */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10021151,  22, True ) /* Inscribable */
     , (10021151, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (10021151,  12,   0.871) /* Shade */
     , (10021151,  13,       2) /* ArmorModVsSlash */
     , (10021151,  14,       2) /* ArmorModVsPierce */
     , (10021151,  15,       2) /* ArmorModVsBludgeon */
     , (10021151,  16,       2) /* ArmorModVsCold */
     , (10021151,  17,       2) /* ArmorModVsFire */
     , (10021151,  18,       2) /* ArmorModVsAcid */
     , (10021151,  19,       2) /* ArmorModVsElectric */
     , (10021151, 110,       1) /* BulkMod */
     , (10021151, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10021151,   1, 'Carbon Steel Covenant Bracers') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10021151,   1, 0x020000D1) /* Setup */
     , (10021151,   3, 0x20000014) /* SoundTable */
     , (10021151,   6, 0x0400007E) /* PaletteBase */
     , (10021151,   7, 0x100003DC) /* ClothingBase */
     , (10021151,   8, 0x06000FC3) /* Icon */
     , (10021151,  22, 0x3400002B) /* PhysicsEffectTable */
     , (10021151,  36, 0x0E000016) /* MutateFilter */
     , (10021151,  46, 0x38000022) /* TsysMutationFilter */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (10021151,  6095,      2)  /* Legendary Impenetrability */
     , (10021151,  2349,      2)  /* Hieromancer's Ward */
     , (10021151,  4018,      2)  /* Permafrost */;
