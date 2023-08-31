DELETE FROM `weenie` WHERE `class_Id` = 46454;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (46454, 'ace46454-societygemoflesserluminance', 38, '2023-08-20 21:13:46') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (46454,   1,       2048) /* ItemType - Gem */
     , (46454,   5,         50) /* EncumbranceVal */
     , (46454,  11,         25) /* MaxStackSize */
     , (46454,  12,          1) /* StackSize */
     , (46454,  13,         50) /* StackUnitEncumbrance */
     , (46454,  15,          1) /* StackUnitValue */
     , (46454,  16,          1) /* ItemUseable - No */
     , (46454,  19,          1) /* Value */
     , (46454,  33,          1) /* Bonded - Bonded */
     , (46454,  53,        101) /* PlacementPosition - Resting */
     , (46454,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (46454, 114,          1) /* Attuned - Attuned */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (46454,  11, True ) /* IgnoreCollisions */
     , (46454,  13, True ) /* Ethereal */
     , (46454,  14, True ) /* GravityStatus */
     , (46454,  19, True ) /* Attackable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (46454,   1, 'Society Gem of Lesser Luminance') /* Name */
     , (46454,  14, 'Turn this gem into an Agent of the Arcanum to be granted 1,500 luminance.') /* Use */
     , (46454,  20, 'Society Gems of Lesser Luminance') /* PluralName */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (46454,   1, 0x02000179) /* Setup */
     , (46454,   3, 0x20000014) /* SoundTable */
     , (46454,   8, 0x06007096) /* Icon */
     , (46454,  22, 0x3400002B) /* PhysicsEffectTable */;
