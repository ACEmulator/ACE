DELETE FROM `weenie` WHERE `class_Id` = 10021155;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10021155, 'EGgreavescovenant', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10021155,   1,          2) /* ItemType - Armor */
     , (10021155,   3,         19) /* PaletteTemplate - Copper */
     , (10021155,   4,        512) /* ClothingPriority - OuterwearLowerLegs */
     , (10021155,   5,        919) /* EncumbranceVal */
     , (10021155,   8,        460) /* Mass */
     , (10021155,   9,      16384) /* ValidLocations - LowerLegArmor */
     , (10021155,  16,          1) /* ItemUseable - No */
     , (10021155,  18,          64) /* UiEffects - Magical */
     , (10021155,  19,        653) /* Value */
     , (10021155,  27,         32) /* ArmorType - Metal */
     , (10021155,  28,        509) /* ArmorLevel */
     , (10021155,  33,          1) /* Bonded - Bonded */
     , (10021155,  36,       9999) /* ResistMagic */
     , (10021155,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (10021155, 105,         10) /* ItemWorkmanship */
     , (10021155, 106,        590) /* ItemSpellcraft */
     , (10021155, 107,       2000) /* ItemCurMana */
     , (10021155, 108,       1000) /* ItemMaxMana */
     , (10021155, 109,          1) /* ItemDifficulty */
     , (10021155, 124,          3) /* Version */
     , (10021155, 169,  252313860) /* TsysMutationData */
     , (10021155, 171,         10) /* NumTimesTinkered */
     , (10021155, 179,       4096) /* ImbuedEffect - MagicDefense */
     , (10021155, 374,          2) /* GearCritDamage */
     , (10021155, 375,          1) /* GearCritDamageResist */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10021155,  22, True ) /* Inscribable */
     , (10021155, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (10021155,  12,   0.857) /* Shade */
     , (10021155,  13,       2) /* ArmorModVsSlash */
     , (10021155,  14,       2) /* ArmorModVsPierce */
     , (10021155,  15,       2) /* ArmorModVsBludgeon */
     , (10021155,  16,       2) /* ArmorModVsCold */
     , (10021155,  17,       2) /* ArmorModVsFire */
     , (10021155,  18,       2) /* ArmorModVsAcid */
     , (10021155,  19,       2) /* ArmorModVsElectric */
     , (10021155,  39,    1.33) /* DefaultScale */
     , (10021155, 110,       1) /* BulkMod */
     , (10021155, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10021155,   1, 'Carbon Steel Covenant Greaves') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10021155,   1, 0x020000D1) /* Setup */
     , (10021155,   3, 0x20000014) /* SoundTable */
     , (10021155,   6, 0x0400007E) /* PaletteBase */
     , (10021155,   7, 0x100003DF) /* ClothingBase */
     , (10021155,   8, 0x06001307) /* Icon */
     , (10021155,  22, 0x3400002B) /* PhysicsEffectTable */
     , (10021155,  36, 0x0E000016) /* MutateFilter */
     , (10021155,  46, 0x38000022) /* TsysMutationFilter */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (10021155,  6095,      2)  /* Legendary Impenetrability */
     , (10021155,  2349,      2)  /* Hieromancer's Ward */
     , (10021155,  4018,      2)  /* Permafrost */;
