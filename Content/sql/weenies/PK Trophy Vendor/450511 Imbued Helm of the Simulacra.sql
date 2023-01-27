DELETE FROM `weenie` WHERE `class_Id` = 450511;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450511, 'helmsimulacraimbuedtailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450511,   1,          2) /* ItemType - Armor */
     , (450511,   3,          5) /* PaletteTemplate - DarkBlue */
     , (450511,   4,      16384) /* ClothingPriority - Head */
     , (450511,   5,        0) /* EncumbranceVal */
     , (450511,   8,        300) /* Mass */
     , (450511,   9,          1) /* ValidLocations - HeadWear */
     , (450511,  16,          1) /* ItemUseable - No */
     , (450511,  18,          1) /* UiEffects - Magical */
     , (450511,  19,       20) /* Value */
     , (450511,  27,         32) /* ArmorType - Metal */
     , (450511,  28,        0) /* ArmorLevel */
     , (450511,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450511, 150,        103) /* HookPlacement - Hook */
     , (450511, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450511,  22, True ) /* Inscribable */
     , (450511,  23, True ) /* DestroyOnSell */
     , (450511,  69, False) /* IsSellable */
     , (450511,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450511,   5,   -0.05) /* ManaRate */
     , (450511,  12,    0.66) /* Shade */
     , (450511,  13,     1.3) /* ArmorModVsSlash */
     , (450511,  14,       1) /* ArmorModVsPierce */
     , (450511,  15,       1) /* ArmorModVsBludgeon */
     , (450511,  16,     0.4) /* ArmorModVsCold */
     , (450511,  17,     0.4) /* ArmorModVsFire */
     , (450511,  18,     0.6) /* ArmorModVsAcid */
     , (450511,  19,     0.4) /* ArmorModVsElectric */
     , (450511, 110,       1) /* BulkMod */
     , (450511, 111,       1) /* SizeMod */
     , (450511, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450511,   1, 'Imbued Helm of the Simulacra') /* Name */
     , (450511,  16, 'A helm imbued with the power of the Asteliary Gem.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450511,   1, 0x02000993) /* Setup */
     , (450511,   3, 0x20000014) /* SoundTable */
     , (450511,   6, 0x0400007E) /* PaletteBase */
     , (450511,   7, 0x10000325) /* ClothingBase */
     , (450511,   8, 0x06002285) /* Icon */
     , (450511,  22, 0x3400002B) /* PhysicsEffectTable */;


