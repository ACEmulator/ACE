DELETE FROM `weenie` WHERE `class_Id` = 450022;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450022, 'ace450022-eldrytchwebrobetailor', 44, '2021-11-01 00:00:00') /* CraftTool */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450022,   1,       4) /* ItemType - Gem */
     , (450022,   4,       1024) /* ClothingPriority - OuterwearChest */
     , (450022,   5,        0) /* EncumbranceVal */
     , (450022,  11,          1) /* MaxStackSize */
     , (450022,  12,          1) /* StackSize */
     , (450022,  13,        919) /* StackUnitEncumbrance */
     , (450022,  15,         50) /* StackUnitValue */
     , (450022,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (450022,  19,         20) /* Value */
     , (450022,  28,          0) /* ArmorLevel */
     , (450022,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450022,  94,          6) /* TargetType - Vestements */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450022,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450022,  13,     1.3) /* ArmorModVsSlash */
     , (450022,  14,       1) /* ArmorModVsPierce */
     , (450022,  15,       1) /* ArmorModVsBludgeon */
     , (450022,  16,     0.4) /* ArmorModVsCold */
     , (450022,  17,     0.4) /* ArmorModVsFire */
     , (450022,  18,     0.6) /* ArmorModVsAcid */
     , (450022,  19,     0.4) /* ArmorModVsElectric */
     , (450022, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450022,   1, 'Eldrytch Web Robe') /* Name */
     , (450022,  14, 'Use this applier to tailor an armored robe onto a Eldrytch Web Breastplate.') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450022,   1, 0x020001A6) /* Setup */
     , (450022,   3, 0x20000014) /* SoundTable */
     , (450022,   7, 0x100007D5) /* ClothingBase */
     , (450022,   8, 0x06007024) /* Icon */
     , (450022,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450022,  50, 0x060011F7) /* IconOverlay */;
