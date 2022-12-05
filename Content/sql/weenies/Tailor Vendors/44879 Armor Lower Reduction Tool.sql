DELETE FROM `weenie` WHERE `class_Id` = 44879;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (44879, 'ace44879-armorlowerreductiontool', 38, '2021-11-01 00:00:00') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (44879,   1,       2048) /* ItemType - Gem */
     , (44879,   5,         10) /* EncumbranceVal */
     , (44879,  11,          1) /* MaxStackSize */
     , (44879,  12,          1) /* StackSize */
     , (44879,  13,         10) /* StackUnitEncumbrance */
     , (44879,  15,         2) /* StackUnitValue */
     , (44879,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (44879,  19,         2) /* Value */
     , (44879,  65,        101) /* Placement - Resting */
     , (44879,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (44879,  94,          6) /* TargetType - Vestements */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (44879,   1, False) /* Stuck */
     , (44879,  11, True ) /* IgnoreCollisions */
     , (44879,  13, True ) /* Ethereal */
     , (44879,  14, True ) /* GravityStatus */
     , (44879,  19, True ) /* Attackable */
     , (44879,  22, True ) /* Inscribable */
     , (44879,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (44879,   1, 'Armor Lower Reduction Tool') /* Name */
     , (44879,  14, 'Use this tool on any loot generated multi-slot armor in order to reduce it to a single slot. It will still cover the same slots in appearance but only a single slot in armor coverage.') /* Use */
     , (44879,  16, 'This tool will reduce Sleeves to Bracers and Leggings to Greaves coverage.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (44879,   1, 0x020004DD) /* Setup */
     , (44879,   3, 0x20000014) /* SoundTable */
     , (44879,   8, 0x060070F1) /* Icon */
     , (44879,  22, 0x3400002B) /* PhysicsEffectTable */;
