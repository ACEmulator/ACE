DELETE FROM `weenie` WHERE `class_Id` = 44719;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (44719, 'ace44719-gemoflesserluminance', 38, '2023-08-20 21:13:00') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (44719,   1,       2048) /* ItemType - Gem */
     , (44719,   5,         50) /* EncumbranceVal */
     , (44719,  11,         10) /* MaxStackSize */
     , (44719,  12,          1) /* StackSize */
     , (44719,  13,         50) /* StackUnitEncumbrance */
     , (44719,  15,          1) /* StackUnitValue */
     , (44719,  16,          1) /* ItemUseable - No */
     , (44719,  19,          1) /* Value */
     , (44719,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (44719,  11, True ) /* IgnoreCollisions */
     , (44719,  13, True ) /* Ethereal */
     , (44719,  14, True ) /* GravityStatus */
     , (44719,  19, True ) /* Attackable */
     , (44719,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (44719,   1, 'Gem of Lesser Luminance') /* Name */
     , (44719,  14, 'Turn this gem into an Agent of the Arcanum to be granted 1,500 luminance.') /* Use */
     , (44719,  20, 'Gems of Lesser Luminance') /* PluralName */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (44719,   1, 0x02000179) /* Setup */
     , (44719,   3, 0x20000014) /* SoundTable */
     , (44719,   8, 0x06007096) /* Icon */
     , (44719,  22, 0x3400002B) /* PhysicsEffectTable */;
