DELETE FROM `weenie` WHERE `class_Id` = 10021152;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10021152, 'EGbreastplatecovenant', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10021152,   1,          2) /* ItemType - Armor */
     , (10021152,   3,         19) /* PaletteTemplate - Copper */
     , (10021152,   4,       1024) /* ClothingPriority - OuterwearChest */
     , (10021152,   5,       2200) /* EncumbranceVal */
     , (10021152,   8,       1100) /* Mass */
     , (10021152,   9,        512) /* ValidLocations - ChestArmor */
     , (10021152,  16,          1) /* ItemUseable - No */
     , (10021152,  18,          64) /* UiEffects - Magical */
     , (10021152,  19,       1631) /* Value */
     , (10021152,  27,         32) /* ArmorType - Metal */
     , (10021152,  28,        509) /* ArmorLevel */
     , (10021152,  33,          1) /* Bonded - Bonded */
     , (10021152,  36,       9999) /* ResistMagic */
     , (10021152,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (10021152, 105,         10) /* ItemWorkmanship */
     , (10021152, 106,        590) /* ItemSpellcraft */
     , (10021152, 107,       2000) /* ItemCurMana */
     , (10021152, 108,       1000) /* ItemMaxMana */
     , (10021152, 109,          1) /* ItemDifficulty */
     , (10021152, 124,          3) /* Version */
     , (10021152, 169,  118097668) /* TsysMutationData */
     , (10021152, 171,         10) /* NumTimesTinkered */
     , (10021152, 179,       4096) /* ImbuedEffect - MagicDefense */
     , (10021152, 240,         10) /* AugmentationResistanceSlash */
     , (10021152, 371,          1) /* GearDamageResist */
     , (10021152, 374,          2) /* GearCritDamage */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10021152,  22, True ) /* Inscribable */
     , (10021152, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (10021152,  12,   0.857) /* Shade */
     , (10021152,  13,       2) /* ArmorModVsSlash */
     , (10021152,  14,       2) /* ArmorModVsPierce */
     , (10021152,  15,       2) /* ArmorModVsBludgeon */
     , (10021152,  16,       2) /* ArmorModVsCold */
     , (10021152,  17,       2) /* ArmorModVsFire */
     , (10021152,  18,       2) /* ArmorModVsAcid */
     , (10021152,  19,       2) /* ArmorModVsElectric */
     , (10021152, 110,       1) /* BulkMod */
     , (10021152, 111,     2.5) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10021152,   1, 'Carbon Steel Covenant Breastplate') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10021152,   1, 0x020000D2) /* Setup */
     , (10021152,   3, 0x20000014) /* SoundTable */
     , (10021152,   6, 0x0400007E) /* PaletteBase */
     , (10021152,   7, 0x100003E4) /* ClothingBase */
     , (10021152,   8, 0x06000FDA) /* Icon */
     , (10021152,  22, 0x3400002B) /* PhysicsEffectTable */
     , (10021152,  36, 0x0E000016) /* MutateFilter */
     , (10021152,  46, 0x38000022) /* TsysMutationFilter */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (10021152,  6095,      2)  /* Legendary Impenetrability */
     , (10021152,  2349,      2)  /* Hieromancer's Ward */
     , (10021152,  4018,      2)  /* Permafrost */;
