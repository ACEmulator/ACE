DELETE FROM `weenie` WHERE `class_Id` = 450609;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450609, 'ace450609-boxedaugmentationgempk', 38, '2021-11-01 00:00:00') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450609,   1,        128) /* ItemType - Misc */
     , (450609,   5,         50) /* EncumbranceVal */
     , (450609,  11,          1) /* MaxStackSize */
     , (450609,  12,          1) /* StackSize */
     , (450609,  13,         50) /* StackUnitEncumbrance */
     , (450609,  15,          3) /* StackUnitValue */
     , (450609,  16,          8) /* ItemUseable - Contained */
     , (450609,  19,          100) /* Value */
     , (450609,  33,          1) /* Bonded - Bonded */
     , (450609,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450609,  94,         16) /* TargetType - Creature */
     , (450609, 114,          1) /* Attuned - Attuned */
     , (450609, 269,          1) /* UseCreateQuantity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450609,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450609,  39,     0.5) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450609,   1, 'Boxed Augmentation Gem') /* Name */
     , (450609,  14, 'Use this crate to retrieve its contents.') /* Use */
     , (450609,  16, 'A box containing a Blank Augmentation Gem.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450609,   1, 0x0200011E) /* Setup */
     , (450609,   3, 0x20000014) /* SoundTable */
     , (450609,   8, 0x060072E9) /* Icon */
     , (450609,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450609,  38,      29295) /* UseCreateItem - Blank Augmentation Gem */;
