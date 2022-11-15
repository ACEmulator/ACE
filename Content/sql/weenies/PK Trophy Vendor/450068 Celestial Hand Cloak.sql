DELETE FROM `weenie` WHERE `class_Id` = 450068;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450068, 'ace450068-celestialhandcloaktailor', 44, '2021-11-01 00:00:00') /* CraftTool */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450068,   1,       4) /* ItemType - Gem */
     , (450068,   4,     131072) /* ClothingPriority - 131072 */
     , (450068,   5,        0) /* EncumbranceVal */
     , (450068,  11,          1) /* MaxStackSize */
     , (450068,  12,          1) /* StackSize */
     , (450068,  13,        919) /* StackUnitEncumbrance */
     , (450068,  15,         50) /* StackUnitValue */
     , (450068,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (450068,  19,         20) /* Value */
     , (450068,  28,          0) /* ArmorLevel */
     , (450068,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450068,  94,          4) /* TargetType - Clothing */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450068,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450068,  13,     0.8) /* ArmorModVsSlash */
     , (450068,  14,     0.8) /* ArmorModVsPierce */
     , (450068,  15,       1) /* ArmorModVsBludgeon */
     , (450068,  16,     0.2) /* ArmorModVsCold */
     , (450068,  17,     0.2) /* ArmorModVsFire */
     , (450068,  18,     0.1) /* ArmorModVsAcid */
     , (450068,  19,     0.2) /* ArmorModVsElectric */
     , (450068, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450068,   1, 'Celestial Hand Cloak') /* Name */
     , (450068,  14, 'Use this applier to tailor the Celestial Hand heraldry onto a cloak.') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450068,   1, 0x02001B2A) /* Setup */
     , (450068,   3, 0x20000014) /* SoundTable */
     , (450068,   7, 0x100007F5) /* ClothingBase */
     , (450068,   8, 0x060070A5) /* Icon */
     , (450068,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450068,  50, 0x060011F7) /* IconOverlay */;
