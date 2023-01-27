DELETE FROM `weenie` WHERE `class_Id` = 450021;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450021, 'ace450021-radiantbloodrobetailor', 44, '2021-11-01 00:00:00') /* CraftTool */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450021,   1,       4) /* ItemType - Gem */
     , (450021,   4,       1024) /* ClothingPriority - OuterwearChest */
     , (450021,   5,        0) /* EncumbranceVal */
     , (450021,  11,          1) /* MaxStackSize */
     , (450021,  12,          1) /* StackSize */
     , (450021,  13,        0) /* StackUnitEncumbrance */
     , (450021,  15,         50) /* StackUnitValue */
     , (450021,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (450021,  19,         20) /* Value */
     , (450021,  28,          0) /* ArmorLevel */
     , (450021,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450021,  94,          6) /* TargetType - Vestements */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450021,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450021,  13,     1.3) /* ArmorModVsSlash */
     , (450021,  14,       1) /* ArmorModVsPierce */
     , (450021,  15,       1) /* ArmorModVsBludgeon */
     , (450021,  16,     0.4) /* ArmorModVsCold */
     , (450021,  17,     0.4) /* ArmorModVsFire */
     , (450021,  18,     0.6) /* ArmorModVsAcid */
     , (450021,  19,     0.4) /* ArmorModVsElectric */
     , (450021, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450021,   1, 'Radiant Blood Robe') /* Name */
     , (450021,  14, 'Use this applier to tailor an armored robe onto a Radiant Blood Breastplate.') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450021,   1, 0x020001A6) /* Setup */
     , (450021,   3, 0x20000014) /* SoundTable */
     , (450021,   7, 0x100007D6) /* ClothingBase */
     , (450021,   8, 0x06007025) /* Icon */
     , (450021,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450021,  50, 0x060011F7) /* IconOverlay */;
