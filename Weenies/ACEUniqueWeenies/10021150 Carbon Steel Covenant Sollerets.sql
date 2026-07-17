DELETE FROM `weenie` WHERE `class_Id` = 10021150;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10021150, 'EGbootscovenant', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10021150,   1,          2) /* ItemType - Armor */
     , (10021150,   3,         19) /* PaletteTemplate - Copper */
     , (10021150,   4,      65536) /* ClothingPriority - Feet */
     , (10021150,   5,        540) /* EncumbranceVal */
     , (10021150,   8,        360) /* Mass */
     , (10021150,   9,        256) /* ValidLocations - FootWear */
     , (10021150,  16,          1) /* ItemUseable - No */
     , (10021150,  18,          64) /* UiEffects - Magical */
     , (10021150,  19,        653) /* Value */
     , (10021150,  27,         32) /* ArmorType - Metal */
     , (10021150,  28,        509) /* ArmorLevel */
     , (10021150,  33,          1) /* Bonded - Bonded */
     , (10021150,  36,       9999) /* ResistMagic */
     , (10021150,  44,          3) /* Damage */
     , (10021150,  45,          4) /* DamageType - Bludgeon */
     , (10021150,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (10021150, 105,         10) /* ItemWorkmanship */
     , (10021150, 106,        590) /* ItemSpellcraft */
     , (10021150, 107,       2000) /* ItemCurMana */
     , (10021150, 108,       1000) /* ItemMaxMana */
     , (10021150, 109,          1) /* ItemDifficulty */
     , (10021150, 124,          3) /* Version */
     , (10021150, 169,  185204996) /* TsysMutationData */
     , (10021150, 171,         10) /* NumTimesTinkered */
     , (10021150, 179,       4096) /* ImbuedEffect - MagicDefense */
     , (10021150, 374,          2) /* GearCritDamage */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10021150,  22, True ) /* Inscribable */
     , (10021150, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (10021150,  12,   0.871) /* Shade */
     , (10021150,  13,       2) /* ArmorModVsSlash */
     , (10021150,  14,       2) /* ArmorModVsPierce */
     , (10021150,  15,       2) /* ArmorModVsBludgeon */
     , (10021150,  16,       2) /* ArmorModVsCold */
     , (10021150,  17,       2) /* ArmorModVsFire */
     , (10021150,  18,       2) /* ArmorModVsAcid */
     , (10021150,  19,       2) /* ArmorModVsElectric */
     , (10021150,  22,    0.75) /* DamageVariance */
     , (10021150, 110,       1) /* BulkMod */
     , (10021150, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10021150,   1, 'Carbon Steel Covenant Sollerets') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10021150,   1, 0x020000DE) /* Setup */
     , (10021150,   3, 0x20000014) /* SoundTable */
     , (10021150,   6, 0x0400007E) /* PaletteBase */
     , (10021150,   7, 0x100003E2) /* ClothingBase */
     , (10021150,   8, 0x06000FAD) /* Icon */
     , (10021150,  22, 0x3400002B) /* PhysicsEffectTable */
     , (10021150,  36, 0x0E000016) /* MutateFilter */
     , (10021150,  46, 0x38000022) /* TsysMutationFilter */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (10021150,  6095,      2)  /* Legendary Impenetrability */
     , (10021150,  2349,      2)  /* Hieromancer's Ward */
     , (10021150,  4018,      2)  /* Permafrost */;
