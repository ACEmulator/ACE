DELETE FROM `weenie` WHERE `class_Id` = 450724;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450724, 'ace450724-fleetstrikegauntletstailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450724,   1,          2) /* ItemType - Armor */
     , (450724,   3,         61) /* PaletteTemplate - White */
     , (450724,   4,      32768) /* ClothingPriority - Hands */
     , (450724,   5,        0) /* EncumbranceVal */
     , (450724,   9,         32) /* ValidLocations - HandWear */
     , (450724,  16,          1) /* ItemUseable - No */
     , (450724,  19,       20) /* Value */
     , (450724,  28,        0) /* ArmorLevel */
     , (450724,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450724,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450724,   5,  -0.033) /* ManaRate */
     , (450724,  13,     1.4) /* ArmorModVsSlash */
     , (450724,  14,     1.4) /* ArmorModVsPierce */
     , (450724,  15,     1.2) /* ArmorModVsBludgeon */
     , (450724,  16,     0.7) /* ArmorModVsCold */
     , (450724,  17,     0.7) /* ArmorModVsFire */
     , (450724,  18,     0.5) /* ArmorModVsAcid */
     , (450724,  19,     0.7) /* ArmorModVsElectric */
     , (450724, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450724,   1, 'Fleet Strike Gauntlets') /* Name */
     , (450724,  16, 'A pair of bright leather gauntlets infused with magics to speed your actions.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450724,   1, 0x020000D8) /* Setup */
     , (450724,   3, 0x20000014) /* SoundTable */
     , (450724,   6, 0x0400007E) /* PaletteBase */
     , (450724,   7, 0x10000664) /* ClothingBase */
     , (450724,   8, 0x06002EF6) /* Icon */
     , (450724,  22, 0x3400002B) /* PhysicsEffectTable */;


