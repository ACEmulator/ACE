DELETE FROM `weenie` WHERE `class_Id` = 44880;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (44880, 'ace44880-armormiddlereductiontool', 38, '2021-11-01 00:00:00') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (44880,   1,       2048) /* ItemType - Gem */
     , (44880,   5,         10) /* EncumbranceVal */
     , (44880,  11,          1) /* MaxStackSize */
     , (44880,  12,          1) /* StackSize */
     , (44880,  13,         10) /* StackUnitEncumbrance */
     , (44880,  15,         2) /* StackUnitValue */
     , (44880,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (44880,  19,         2) /* Value */
     , (44880,  65,        101) /* Placement - Resting */
     , (44880,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (44880,  94,          6) /* TargetType - Vestements */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (44880,   1, False) /* Stuck */
     , (44880,  11, True ) /* IgnoreCollisions */
     , (44880,  13, True ) /* Ethereal */
     , (44880,  14, True ) /* GravityStatus */
     , (44880,  19, True ) /* Attackable */
     , (44880,  22, True ) /* Inscribable */
     , (44880,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (44880,   1, 'Armor Middle Reduction Tool') /* Name */
     , (44880,  14, 'Use this tool on any loot generated multi-slot armor in order to reduce it to a single slot. It will still cover the same slots in appearance but only a single slot in armor coverage.') /* Use */
     , (44880,  16, 'This tool will reduce Leggings to Tasset coverage.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (44880,   1, 0x020004DD) /* Setup */
     , (44880,   3, 0x20000014) /* SoundTable */
     , (44880,   8, 0x060070F2) /* Icon */
     , (44880,  22, 0x3400002B) /* PhysicsEffectTable */;
