DELETE FROM `weenie` WHERE `class_Id` = 450763;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450763, 'ace450763-surloshenshelmtailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450763,   1,          2) /* ItemType - Armor */
     , (450763,   3,          2) /* PaletteTemplate - Blue */
     , (450763,   4,      16384) /* ClothingPriority - Head */
     , (450763,   5,        0) /* EncumbranceVal */
     , (450763,   9,          1) /* ValidLocations - HeadWear */
     , (450763,  16,          1) /* ItemUseable - No */
     , (450763,  19,       20) /* Value */
     , (450763,  28,        0) /* ArmorLevel */
     , (450763,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450763, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450763,  11, True ) /* IgnoreCollisions */
     , (450763,  13, True ) /* Ethereal */
     , (450763,  14, True ) /* GravityStatus */
     , (450763,  19, True ) /* Attackable */
     , (450763,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450763,   5,  -0.025) /* ManaRate */
     , (450763,  13,     1.2) /* ArmorModVsSlash */
     , (450763,  14,     1.2) /* ArmorModVsPierce */
     , (450763,  15,     0.8) /* ArmorModVsBludgeon */
     , (450763,  16,     0.7) /* ArmorModVsCold */
     , (450763,  17,     1.1) /* ArmorModVsFire */
     , (450763,  18,     1.4) /* ArmorModVsAcid */
     , (450763,  19,     0.6) /* ArmorModVsElectric */
     , (450763, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450763,   1, 'Surloshen''s Helm') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450763,   1, 0x0200142D) /* Setup */
     , (450763,   3, 0x20000014) /* SoundTable */
     , (450763,   6, 0x0400007E) /* PaletteBase */
     , (450763,   7, 0x10000620) /* ClothingBase */
     , (450763,   8, 0x0600601C) /* Icon */
     , (450763,  22, 0x3400002B) /* PhysicsEffectTable */;

