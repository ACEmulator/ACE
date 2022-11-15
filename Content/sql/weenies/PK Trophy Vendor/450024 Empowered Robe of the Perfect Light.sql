DELETE FROM `weenie` WHERE `class_Id` = 450024;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450024, 'ace450024-empoweredrobeoftheperfectlighttailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450024,   1,          4) /* ItemType - Clothing */
     , (450024,   4,       1024) /* ClothingPriority - OuterwearChest */
     , (450024,   5,        0) /* EncumbranceVal */
     , (450024,   9,        512) /* ValidLocations - ChestArmor */
     , (450024,  16,          1) /* ItemUseable - No */
     , (450024,  19,     20) /* Value */
     , (450024,  28,        0) /* ArmorLevel */
     , (450024,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450024,  22, True ) /* Inscribable */
     , (450024,  69, False) /* IsSellable */
     , (450024,  99, False) /* Ivoryable */
     , (450024, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450024,   5,    -0.5) /* ManaRate */
     , (450024,  13,     0.6) /* ArmorModVsSlash */
     , (450024,  14,     0.6) /* ArmorModVsPierce */
     , (450024,  15,     0.6) /* ArmorModVsBludgeon */
     , (450024,  16,     0.6) /* ArmorModVsCold */
     , (450024,  17,     0.6) /* ArmorModVsFire */
     , (450024,  18,     0.6) /* ArmorModVsAcid */
     , (450024,  19,     0.6) /* ArmorModVsElectric */
     , (450024, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450024,   1, 'Empowered Robe of the Perfect Light') /* Name */
     , (450024,  14, 'This robe may be tailored onto breastplates.') /* Use */
     , (450024,  16, 'A loose-fitting, armored Empyrean robe, designed to be worn over other armor pieces.  The armor plates are forged from Thaumaturgic Plate Crystal, and embedded within the fabric are small threads of Thaumaturgic Crystal.  The entire robe hums with power.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450024,   1, 0x020001A6) /* Setup */
     , (450024,   3, 0x20000014) /* SoundTable */
     , (450024,   7, 0x100007CE) /* ClothingBase */
     , (450024,   8, 0x0600673F) /* Icon */
     , (450024,  22, 0x3400002B) /* PhysicsEffectTable */;


