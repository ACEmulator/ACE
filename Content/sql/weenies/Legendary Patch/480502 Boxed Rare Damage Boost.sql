DELETE FROM `weenie` WHERE `class_Id` = 480502;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480502, 'ace480502-boxedburningcoal', 38, '2021-11-01 00:00:00') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480502,   1,        128) /* ItemType - Misc */
     , (480502,   5,         50) /* EncumbranceVal */
     , (480502,  16,          8) /* ItemUseable - Contained */
     , (480502,  19,          10) /* Value */
     , (480502,  33,          1) /* Bonded - Bonded */
     , (480502,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480502,  94,         16) /* TargetType - Creature */
     , (480502, 114,          1) /* Attuned - Attuned */
     , (480502, 269,          1) /* UseCreateQuantity */
	 , (480502, 369,         200) /* UseRequiresLevel */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480502,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480502,  39,     0.5) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480502,   1, 'Boxed Gem of Rare Damage Boost V') /* Name */
     , (480502,  14, 'Use this crate to retrieve its contents.') /* Use */
     , (480502,  16, 'A box containing one Luminous Crystal of Rare Damage Boost V.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480502,   1, 0x0200011E) /* Setup */
     , (480502,   3, 0x20000014) /* SoundTable */
     , (480502,   8, 0x060072E9) /* Icon */
     , (480502,  50, 0x06005B2B) /* IconOverlay */
     , (480502,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480502,  38,      52023) /* UseCreateItem - Blank Augmentation Gem */;
