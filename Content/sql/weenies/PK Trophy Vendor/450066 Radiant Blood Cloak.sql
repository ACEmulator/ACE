DELETE FROM `weenie` WHERE `class_Id` = 450066;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450066, 'ace450066-radiantbloodcloaktailor', 44, '2021-11-01 00:00:00') /* CraftTool */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450066,   1,       4) /* ItemType - Gem */
     , (450066,   4,     131072) /* ClothingPriority - 131072 */
     , (450066,   5,        0) /* EncumbranceVal */
     , (450066,  11,          1) /* MaxStackSize */
     , (450066,  12,          1) /* StackSize */
     , (450066,  13,        919) /* StackUnitEncumbrance */
     , (450066,  15,         50) /* StackUnitValue */
     , (450066,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (450066,  19,         20) /* Value */
     , (450066,  28,          0) /* ArmorLevel */
     , (450066,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450066,  94,          4) /* TargetType - Clothing */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450066,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450066,  13,     0.8) /* ArmorModVsSlash */
     , (450066,  14,     0.8) /* ArmorModVsPierce */
     , (450066,  15,       1) /* ArmorModVsBludgeon */
     , (450066,  16,     0.2) /* ArmorModVsCold */
     , (450066,  17,     0.2) /* ArmorModVsFire */
     , (450066,  18,     0.1) /* ArmorModVsAcid */
     , (450066,  19,     0.2) /* ArmorModVsElectric */
     , (450066, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450066,   1, 'Radiant Blood Cloak') /* Name */
     , (450066,  14, 'Use this applier to tailor the Radiant Blood heraldry onto a cloak.') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450066,   1, 0x02001B2A) /* Setup */
     , (450066,   3, 0x20000014) /* SoundTable */
     , (450066,   7, 0x100007F8) /* ClothingBase */
     , (450066,   8, 0x060070A8) /* Icon */
     , (450066,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450066,  50, 0x060011F7) /* IconOverlay */;
