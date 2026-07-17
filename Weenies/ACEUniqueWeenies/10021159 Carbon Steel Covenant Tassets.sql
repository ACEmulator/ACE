DELETE FROM `weenie` WHERE `class_Id` = 10021159;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10021159, 'EGtassetscovenant', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10021159,   1,          2) /* ItemType - Armor */
     , (10021159,   3,         19) /* PaletteTemplate - Copper */
     , (10021159,   4,        256) /* ClothingPriority - OuterwearUpperLegs */
     , (10021159,   5,        919) /* EncumbranceVal */
     , (10021159,   8,        460) /* Mass */
     , (10021159,   9,       8192) /* ValidLocations - UpperLegArmor */
     , (10021159,  16,          1) /* ItemUseable - No */
     , (10021159,  18,          64) /* UiEffects - Magical */
     , (10021159,  19,        653) /* Value */
     , (10021159,  27,         32) /* ArmorType - Metal */
     , (10021159,  28,        509) /* ArmorLevel */
     , (10021159,  33,          1) /* Bonded - Bonded */
     , (10021159,  36,       9999) /* ResistMagic */
     , (10021159,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (10021159, 105,         10) /* ItemWorkmanship */
     , (10021159, 106,        590) /* ItemSpellcraft */
     , (10021159, 107,       2000) /* ItemCurMana */
     , (10021159, 108,       1000) /* ItemMaxMana */
     , (10021159, 109,          1) /* ItemDifficulty */
     , (10021159, 124,          3) /* Version */
     , (10021159, 169,  252313860) /* TsysMutationData */
     , (10021159, 179,       4096) /* ImbuedEffect - MagicDefense */
     , (10021159, 374,          2) /* GearCritDamage */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10021159,  22, True ) /* Inscribable */
     , (10021159, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (10021159,  12,   0.871) /* Shade */
     , (10021159,  13,       2) /* ArmorModVsSlash */
     , (10021159,  14,       2) /* ArmorModVsPierce */
     , (10021159,  15,       2) /* ArmorModVsBludgeon */
     , (10021159,  16,       2) /* ArmorModVsCold */
     , (10021159,  17,       2) /* ArmorModVsFire */
     , (10021159,  18,       2) /* ArmorModVsAcid */
     , (10021159,  19,       2) /* ArmorModVsElectric */
     , (10021159,  39,    1.33) /* DefaultScale */
     , (10021159, 110,       1) /* BulkMod */
     , (10021159, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10021159,   1, 'Carbon Steel Covenant Tassets') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10021159,   1, 0x020000E0) /* Setup */
     , (10021159,   3, 0x20000014) /* SoundTable */
     , (10021159,   6, 0x0400007E) /* PaletteBase */
     , (10021159,   7, 0x100003E3) /* ClothingBase */
     , (10021159,   8, 0x0600275C) /* Icon */
     , (10021159,  22, 0x3400002B) /* PhysicsEffectTable */
     , (10021159,  36, 0x0E000016) /* MutateFilter */
     , (10021159,  46, 0x38000022) /* TsysMutationFilter */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (10021159,  6095,      2)  /* Legendary Impenetrability */
     , (10021159,  2349,      2)  /* Hieromancer's Ward */
     , (10021159,  4018,      2)  /* Permafrost */;
