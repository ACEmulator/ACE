DELETE FROM `weenie` WHERE `class_Id` = 450284;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450284, 'ace450284-squalidleggingstailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450284,   1,          2) /* ItemType - Armor */
     , (450284,   3,         21) /* PaletteTemplate - Gold */
     , (450284,   4,        768) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs */
     , (450284,   5,       0) /* EncumbranceVal */
     , (450284,   9,      24576) /* ValidLocations - UpperLegArmor, LowerLegArmor */
     , (450284,  16,          1) /* ItemUseable - No */
     , (450284,  19,       20) /* Value */
     , (450284,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450284,  28,        0) /* ArmorLevel */
     , (450284,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450284,  22, True ) /* Inscribable */
     , (450284,  69, True ) /* IsSellable */
     , (450284, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450284,   5,   -0.05) /* ManaRate */
     , (450284,  13,     1.3) /* ArmorModVsSlash */
     , (450284,  14,     1.1) /* ArmorModVsPierce */
     , (450284,  15,     1.1) /* ArmorModVsBludgeon */
     , (450284,  16,     0.6) /* ArmorModVsCold */
     , (450284,  17,     0.6) /* ArmorModVsFire */
     , (450284,  18,     0.6) /* ArmorModVsAcid */
     , (450284,  19,     0.6) /* ArmorModVsElectric */
     , (450284, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450284,   1, 'Squalid Leggings') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450284,   1, 0x020001A8) /* Setup */
     , (450284,   3, 0x20000014) /* SoundTable */
     , (450284,   6, 0x0400007E) /* PaletteBase */
     , (450284,   7, 0x10000615) /* ClothingBase */
     , (450284,   8, 0x06005F8F) /* Icon */
     , (450284,  22, 0x3400002B) /* PhysicsEffectTable */;

