DELETE FROM `weenie` WHERE `class_Id` = 450067;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450067, 'ace450067-eldrytchwebcloaktailor', 44, '2021-11-01 00:00:00') /* CraftTool */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450067,   1,       4) /* ItemType - Gem */
     , (450067,   4,     131072) /* ClothingPriority - 131072 */
     , (450067,   5,        0) /* EncumbranceVal */
     , (450067,  11,          1) /* MaxStackSize */
     , (450067,  12,          1) /* StackSize */
     , (450067,  13,        919) /* StackUnitEncumbrance */
     , (450067,  15,         50) /* StackUnitValue */
     , (450067,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (450067,  19,         20) /* Value */
     , (450067,  28,          0) /* ArmorLevel */
     , (450067,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450067,  94,          4) /* TargetType - Clothing */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450067,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450067,  13,     0.8) /* ArmorModVsSlash */
     , (450067,  14,     0.8) /* ArmorModVsPierce */
     , (450067,  15,       1) /* ArmorModVsBludgeon */
     , (450067,  16,     0.2) /* ArmorModVsCold */
     , (450067,  17,     0.2) /* ArmorModVsFire */
     , (450067,  18,     0.1) /* ArmorModVsAcid */
     , (450067,  19,     0.2) /* ArmorModVsElectric */
     , (450067, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450067,   1, 'Eldrytch Web Cloak') /* Name */
     , (450067,  14, 'Use this applier to tailor the Eldrytch Web heraldry onto a cloak.') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450067,   1, 0x02001B2A) /* Setup */
     , (450067,   3, 0x20000014) /* SoundTable */
     , (450067,   7, 0x100007F7) /* ClothingBase */
     , (450067,   8, 0x060070A7) /* Icon */
     , (450067,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450067,  50, 0x060011F7) /* IconOverlay */;
