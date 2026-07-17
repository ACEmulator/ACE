DELETE FROM `weenie` WHERE `class_Id` = 10021157;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10021157, 'EGpauldronscovenant', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10021157,   1,          2) /* ItemType - Armor */
     , (10021157,   3,         19) /* PaletteTemplate - Copper */
     , (10021157,   4,       4096) /* ClothingPriority - OuterwearUpperArms */
     , (10021157,   5,        720) /* EncumbranceVal */
     , (10021157,   8,        360) /* Mass */
     , (10021157,   9,       2048) /* ValidLocations - UpperArmArmor */
     , (10021157,  16,          1) /* ItemUseable - No */
     , (10021157,  18,          64) /* UiEffects - Magical */
     , (10021157,  19,        653) /* Value */
     , (10021157,  27,         32) /* ArmorType - Metal */
     , (10021157,  28,        509) /* ArmorLevel */
     , (10021157,  33,          1) /* Bonded - Bonded */
     , (10021157,  36,       9999) /* ResistMagic */
     , (10021157,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (10021157, 105,         10) /* ItemWorkmanship */
     , (10021157, 106,        590) /* ItemSpellcraft */
     , (10021157, 107,       2000) /* ItemCurMana */
     , (10021157, 108,       1000) /* ItemMaxMana */
     , (10021157, 109,          1) /* ItemDifficulty */
     , (10021157, 124,          3) /* Version */
     , (10021157, 169,  118096132) /* TsysMutationData */
     , (10021157, 171,         10) /* NumTimesTinkered */
     , (10021157, 179,       4096) /* ImbuedEffect - MagicDefense */
     , (10021157, 374,          3) /* GearCritDamage */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10021157,  22, True ) /* Inscribable */
     , (10021157, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (10021157,  12,   0.871) /* Shade */
     , (10021157,  13,       2) /* ArmorModVsSlash */
     , (10021157,  14,       2) /* ArmorModVsPierce */
     , (10021157,  15,       2) /* ArmorModVsBludgeon */
     , (10021157,  16,       2) /* ArmorModVsCold */
     , (10021157,  17,       2) /* ArmorModVsFire */
     , (10021157,  18,       2) /* ArmorModVsAcid */
     , (10021157,  19,       2) /* ArmorModVsElectric */
     , (10021157,  39,     1.1) /* DefaultScale */
     , (10021157, 110,       1) /* BulkMod */
     , (10021157, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10021157,   1, 'Carbon Steel Covenant Pauldrons') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10021157,   1, 0x020000D1) /* Setup */
     , (10021157,   3, 0x20000014) /* SoundTable */
     , (10021157,   6, 0x0400007E) /* PaletteBase */
     , (10021157,   7, 0x100003E1) /* ClothingBase */
     , (10021157,   8, 0x0600130C) /* Icon */
     , (10021157,  22, 0x3400002B) /* PhysicsEffectTable */
     , (10021157,  36, 0x0E000016) /* MutateFilter */
     , (10021157,  46, 0x38000022) /* TsysMutationFilter */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (10021157,  6095,      2)  /* Legendary Impenetrability */
     , (10021157,  2349,      2)  /* Hieromancer's Ward */
     , (10021157,  4018,      2)  /* Permafrost */;
