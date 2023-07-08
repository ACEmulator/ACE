DELETE FROM `weenie` WHERE `class_Id` = 450784;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450784, 'basinetleathernewbiequestPK', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450784,   1,          2) /* ItemType - Armor */
     , (450784,   3,          1) /* PaletteTemplate - AquaBlue */
     , (450784,   4,      16384) /* ClothingPriority - Head */
     , (450784,   5,        0) /* EncumbranceVal */
     , (450784,   8,        110) /* Mass */
     , (450784,   9,          1) /* ValidLocations - HeadWear */
     , (450784,  16,          1) /* ItemUseable - No */
     , (450784,  18,          1) /* UiEffects - Magical */
     , (450784,  19,          20) /* Value */
     , (450784,  27,          2) /* ArmorType - Leather */
     , (450784,  28,        0) /* ArmorLevel */
     , (450784,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450784, 150,        103) /* HookPlacement - Hook */
     , (450784, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450784,  22, True ) /* Inscribable */
     , (450784, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450784,   5,  -0.025) /* ManaRate */
     , (450784,  12,     0.3) /* Shade */
     , (450784,  13,       1) /* ArmorModVsSlash */
     , (450784,  14,       1) /* ArmorModVsPierce */
     , (450784,  15,       1) /* ArmorModVsBludgeon */
     , (450784,  16,     0.6) /* ArmorModVsCold */
     , (450784,  17,     0.6) /* ArmorModVsFire */
     , (450784,  18,     0.6) /* ArmorModVsAcid */
     , (450784,  19,     0.6) /* ArmorModVsElectric */
     , (450784, 110,       1) /* BulkMod */
     , (450784, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450784,   1, 'A Society Leather Basinet') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450784,   1, 0x02000268) /* Setup */
     , (450784,   3, 0x20000014) /* SoundTable */
     , (450784,   6, 0x0400007E) /* PaletteBase */
     , (450784,   7, 0x10000038) /* ClothingBase */
     , (450784,   8, 0x06001351) /* Icon */
     , (450784,  22, 0x3400002B) /* PhysicsEffectTable */;


