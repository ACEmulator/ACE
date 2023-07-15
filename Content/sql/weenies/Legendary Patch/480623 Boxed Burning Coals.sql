DELETE FROM `weenie` WHERE `class_Id` = 480623;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480623, 'ace480623-boxedburningcoal', 38, '2021-11-01 00:00:00') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480623,   1,        128) /* ItemType - Misc */
     , (480623,   5,         50) /* EncumbranceVal */
     , (480623,  16,          8) /* ItemUseable - Contained */
     , (480623,  19,          15) /* Value */
     , (480623,  33,          1) /* Bonded - Bonded */
     , (480623,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480623,  94,         16) /* TargetType - Creature */
     , (480623, 114,          1) /* Attuned - Attuned */
     , (480623, 269,          3) /* UseCreateQuantity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480623,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480623,  39,     0.5) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480623,   1, 'Boxed Burning Coals') /* Name */
     , (480623,  14, 'Use this crate to retrieve its contents.') /* Use */
     , (480623,  16, 'A box containing three Burning Coals.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480623,   1, 0x0200011E) /* Setup */
     , (480623,   3, 0x20000014) /* SoundTable */
     , (480623,   8, 0x060072E9) /* Icon */
	 , (480623,  50,  0x06003328) /* IconOverlay */
     , (480623,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480623,  38,      480613) /* UseCreateItem - Blank Augmentation Gem */;
