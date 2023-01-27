DELETE FROM `weenie` WHERE `class_Id` = 450023;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450023, 'ace450023-celestialhandrobetailor', 44, '2021-11-01 00:00:00') /* CraftTool */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450023,   1,       4) /* ItemType - Gem */
     , (450023,   4,       1024) /* ClothingPriority - OuterwearChest */
     , (450023,   5,        0) /* EncumbranceVal */
     , (450023,  11,          1) /* MaxStackSize */
     , (450023,  12,          1) /* StackSize */
     , (450023,  13,        0) /* StackUnitEncumbrance */
     , (450023,  15,         0) /* StackUnitValue */
     , (450023,  19,         20) /* Value */
     , (450023,  28,          0) /* ArmorLevel */
     , (450023,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450023,  94,          6) /* TargetType - Vestements */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450023,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450023,  13,     1.3) /* ArmorModVsSlash */
     , (450023,  14,       1) /* ArmorModVsPierce */
     , (450023,  15,       1) /* ArmorModVsBludgeon */
     , (450023,  16,     0.4) /* ArmorModVsCold */
     , (450023,  17,     0.4) /* ArmorModVsFire */
     , (450023,  18,     0.6) /* ArmorModVsAcid */
     , (450023,  19,     0.4) /* ArmorModVsElectric */
     , (450023, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450023,   1, 'Celestial Hand Robe') /* Name */
     , (450023,  14, 'Use this applier to tailor an armored robe onto a Celestial Hand Breastplate.') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450023,   1, 0x020001A6) /* Setup */
     , (450023,   3, 0x20000014) /* SoundTable */
     , (450023,   7, 0x100007D4) /* ClothingBase */
     , (450023,   8, 0x06007023) /* Icon */
     , (450023,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450023,  50, 0x060011F7) /* IconOverlay */;
