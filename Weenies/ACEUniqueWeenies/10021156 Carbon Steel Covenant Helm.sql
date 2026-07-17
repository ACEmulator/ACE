DELETE FROM `weenie` WHERE `class_Id` = 10021156;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10021156, 'EGhelmcovenant', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10021156,   1,          2) /* ItemType - Armor */
     , (10021156,   3,         19) /* PaletteTemplate - Copper */
     , (10021156,   4,      16384) /* ClothingPriority - Head */
     , (10021156,   5,        600) /* EncumbranceVal */
     , (10021156,   8,        300) /* Mass */
     , (10021156,   9,          1) /* ValidLocations - HeadWear */
     , (10021156,  16,          1) /* ItemUseable - No */
     , (10021156,  18,          64) /* UiEffects - Magical */
     , (10021156,  19,       1187) /* Value */
     , (10021156,  27,         32) /* ArmorType - Metal */
     , (10021156,  28,        509) /* ArmorLevel */
     , (10021156,  33,          1) /* Bonded - Bonded */
     , (10021156,  36,       9999) /* ResistMagic */
     , (10021156,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (10021156, 106,        590) /* ItemSpellcraft */
     , (10021156, 107,       2000) /* ItemCurMana */
     , (10021156, 108,       1000) /* ItemMaxMana */
     , (10021156, 109,          1) /* ItemDifficulty */
     , (10021156, 124,          3) /* Version */
     , (10021156, 150,        103) /* HookPlacement - Hook */
     , (10021156, 151,          2) /* HookType - Wall */
     , (10021156, 169,  168429060) /* TsysMutationData */
     , (10021156, 171,         10) /* NumTimesTinkered */
     , (10021156, 179,       4096) /* ImbuedEffect - MagicDefense */
     , (10021156, 374,          2) /* GearCritDamage */
     , (10021156, 376,          2) /* GearHealingBoost */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10021156,  22, True ) /* Inscribable */
     , (10021156, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (10021156,  12,   0.871) /* Shade */
     , (10021156,  13,       2) /* ArmorModVsSlash */
     , (10021156,  14,       2) /* ArmorModVsPierce */
     , (10021156,  15,       2) /* ArmorModVsBludgeon */
     , (10021156,  16,       2) /* ArmorModVsCold */
     , (10021156,  17,       2) /* ArmorModVsFire */
     , (10021156,  18,       2) /* ArmorModVsAcid */
     , (10021156,  19,       2) /* ArmorModVsElectric */
     , (10021156, 105,      10) /* HotspotCycleTime */
     , (10021156, 110,     0.8) /* BulkMod */
     , (10021156, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10021156,   1, 'Carbon Steel Covenant Helm') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10021156,   1, 0x02000D7C) /* Setup */
     , (10021156,   3, 0x20000014) /* SoundTable */
     , (10021156,   6, 0x0400007E) /* PaletteBase */
     , (10021156,   7, 0x100003E0) /* ClothingBase */
     , (10021156,   8, 0x06000FCF) /* Icon */
     , (10021156,  22, 0x3400002B) /* PhysicsEffectTable */
     , (10021156,  36, 0x0E000016) /* MutateFilter */
     , (10021156,  46, 0x38000022) /* TsysMutationFilter */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (10021156,  6095,      2)  /* Legendary Impenetrability */
     , (10021156,  2349,      2)  /* Hieromancer's Ward */
     , (10021156,  4018,      2)  /* Permafrost */;
