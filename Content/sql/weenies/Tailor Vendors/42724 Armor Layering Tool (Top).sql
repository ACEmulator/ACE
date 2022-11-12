DELETE FROM `weenie` WHERE `class_Id` = 42724;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (42724, 'ace42724-armorlayeringtooltop', 38, '2021-11-01 00:00:00') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (42724,   1,       2048) /* ItemType - Gem */
     , (42724,   5,         10) /* EncumbranceVal */
     , (42724,  11,        100) /* MaxStackSize */
     , (42724,  12,          1) /* StackSize */
     , (42724,  13,         10) /* StackUnitEncumbrance */
     , (42724,  15,          1) /* StackUnitValue */
     , (42724,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (42724,  19,          2) /* Value */
     , (42724,  65,        101) /* Placement - Resting */
     , (42724,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (42724,  94,          6) /* TargetType - Vestements */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (42724,   1, False) /* Stuck */
     , (42724,  11, True ) /* IgnoreCollisions */
     , (42724,  13, True ) /* Ethereal */
     , (42724,  14, True ) /* GravityStatus */
     , (42724,  19, True ) /* Attackable */
     , (42724,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (42724,   1, 'Armor Layering Tool (Top)') /* Name */
     , (42724,  14, 'Use this tool on any loot generated piece of armor and make it show on top of other armor visually covering the same locations.') /* Use */
     , (42724,  16, 'A tool used to determine the layering of armor.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (42724,   1, 0x020004DD) /* Setup */
     , (42724,   3, 0x20000014) /* SoundTable */
     , (42724,   8, 0x06006C3E) /* Icon */
     , (42724,  22, 0x3400002B) /* PhysicsEffectTable */;
