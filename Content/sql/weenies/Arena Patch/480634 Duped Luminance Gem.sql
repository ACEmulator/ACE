DELETE FROM `weenie` WHERE `class_Id` = 480634;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480634, 'ace480634-dupedlumgem', 38, '2021-11-01 00:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480634,   1,       2048) /* ItemType - Gem */
     , (480634,   5,         10) /* EncumbranceVal */
     , (480634,  11,        100) /* MaxStackSize */
     , (480634,  12,          1) /* StackSize */
     , (480634,  13,         10) /* StackUnitEncumbrance */
	 , (480634,  33,         -1) /* Bonded - Slippery */
     , (480634,  15,      10000) /* StackUnitValue */
     , (480634,  16,          1) /* ItemUseable - No */
     , (480634,  19,      10000) /* Value */
     , (480634,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480634,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480634,   1, 'Duped Gem of Greater Luminance') /* Name */
     , (480634,  15, 'This gem looks almost identical to a Gem of Greater Luminance, it would fool those without an untrained eye. Hand it to an Anti Parazi for a reward of 15,000 luminance.') /* ShortDesc */
	 , (480634,  20, 'Duped Gems of Greater Luminance') /* PluralName */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480634,   1, 0x02000179) /* Setup */
     , (480634,   3, 0x20000014) /* SoundTable */
     , (480634,   8, 0x06007095) /* Icon */
     , (480634,  22, 0x3400002B) /* PhysicsEffectTable */;

