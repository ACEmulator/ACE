DELETE FROM `weenie` WHERE `class_Id` = 450510;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450510, 'helmsimulacratailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450510,   1,          2) /* ItemType - Armor */
     , (450510,   3,          2) /* PaletteTemplate - Blue */
     , (450510,   4,      16384) /* ClothingPriority - Head */
     , (450510,   5,        0) /* EncumbranceVal */
     , (450510,   8,        300) /* Mass */
     , (450510,   9,          1) /* ValidLocations - HeadWear */
     , (450510,  16,          1) /* ItemUseable - No */
     , (450510,  18,          1) /* UiEffects - Magical */
     , (450510,  19,       20) /* Value */
     , (450510,  27,         32) /* ArmorType - Metal */
     , (450510,  28,        0) /* ArmorLevel */
     , (450510,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450510, 150,        103) /* HookPlacement - Hook */
     , (450510, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450510,  22, True ) /* Inscribable */
     , (450510,  23, True ) /* DestroyOnSell */
     , (450510,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450510,   5,  -0.033) /* ManaRate */
     , (450510,  13,     1.3) /* ArmorModVsSlash */
     , (450510,  14,       1) /* ArmorModVsPierce */
     , (450510,  15,       1) /* ArmorModVsBludgeon */
     , (450510,  16,     0.4) /* ArmorModVsCold */
     , (450510,  17,     0.4) /* ArmorModVsFire */
     , (450510,  18,     0.6) /* ArmorModVsAcid */
     , (450510,  19,     0.4) /* ArmorModVsElectric */
     , (450510, 110,       1) /* BulkMod */
     , (450510, 111,       1) /* SizeMod */
     , (450510, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450510,   1, 'Helm of the Simulacra') /* Name */
     , (450510,  16, 'A helm enchanted with powerful magic, taken from the Southern Infiltrator Keep dungeon.') /* LongDesc */
     , (450510,  33, 'HelmSimulacra') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450510,   1, 0x02000993) /* Setup */
     , (450510,   3, 0x20000014) /* SoundTable */
     , (450510,   6, 0x0400007E) /* PaletteBase */
     , (450510,   7, 0x10000325) /* ClothingBase */
     , (450510,   8, 0x06002286) /* Icon */
     , (450510,  22, 0x3400002B) /* PhysicsEffectTable */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (450510,   249,      2)  /* Invulnerability Self VI */
     , (450510,   261,      2)  /* Impregnability Self VI */
     , (450510,  1486,      2)  /* Impenetrability VI */;
