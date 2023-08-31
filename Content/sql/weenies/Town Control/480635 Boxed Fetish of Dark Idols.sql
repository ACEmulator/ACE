DELETE FROM `weenie` WHERE `class_Id` = 480635;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480635, 'ace480635-boxedfetishpk', 38, '2021-11-01 00:00:00') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480635,   1,        128) /* ItemType - Misc */
     , (480635,   5,         50) /* EncumbranceVal */
     , (480635,  16,          8) /* ItemUseable - Contained */
     , (480635,  19,          150) /* Value */
     , (480635,  33,          1) /* Bonded - Bonded */
     , (480635,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480635,  94,         16) /* TargetType - Creature */
     , (480635, 114,          1) /* Attuned - Attuned */
     , (480635, 269,          1) /* UseCreateQuantity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480635,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480635,  39,     0.5) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480635,   1, 'Boxed Fetish of Dark Idols') /* Name */
     , (480635,  14, 'Use this crate to retrieve its contents.') /* Use */
     , (480635,  16, 'A box containing one Fetish of Dark Idols.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480635,   1, 0x0200011E) /* Setup */
     , (480635,   3, 0x20000014) /* SoundTable */
     , (480635,   8, 0x060072E9) /* Icon */
     , (480635,  22, 0x3400002B) /* PhysicsEffectTable */
	 , (480635,  50, 0x060033DB) /* IconOverlay */
     , (480635,  38,      27795) /* UseCreateItem - Blank Augmentation Gem */;
