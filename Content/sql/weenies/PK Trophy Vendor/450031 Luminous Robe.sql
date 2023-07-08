DELETE FROM `weenie` WHERE `class_Id` = 450031;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450031, 'ace450031-luminousrobetailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450031,   1,          4) /* ItemType - Clothing */
     , (450031,   3,         15) /* PaletteTemplate - RedPurple */
     , (450031,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Feet */
     , (450031,   5,        0) /* EncumbranceVal */
     , (450031,   9,      512) /* ValidLocations - Armor */
     , (450031,  16,          1) /* ItemUseable - No */
     , (450031,  19,      20) /* Value */
     , (450031,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450031,  28,        0) /* ArmorLevel */
     , (450031,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450031, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450031,  22, True ) /* Inscribable */
     , (450031, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450031,   5,  -0.033) /* ManaRate */
     , (450031,  13,       1) /* ArmorModVsSlash */
     , (450031,  14,       1) /* ArmorModVsPierce */
     , (450031,  15,       1) /* ArmorModVsBludgeon */
     , (450031,  16,     1.5) /* ArmorModVsCold */
     , (450031,  17,     0.9) /* ArmorModVsFire */
     , (450031,  18,     0.9) /* ArmorModVsAcid */
     , (450031,  19,     0.9) /* ArmorModVsElectric */
     , (450031, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450031,   1, 'Luminous Robe') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450031,   1, 0x020001A6) /* Setup */
     , (450031,   3, 0x20000014) /* SoundTable */
     , (450031,   6, 0x0400007E) /* PaletteBase */
     , (450031,   7, 0x10000613) /* ClothingBase */
     , (450031,   8, 0x06005F69) /* Icon */
     , (450031,  22, 0x3400002B) /* PhysicsEffectTable */;


