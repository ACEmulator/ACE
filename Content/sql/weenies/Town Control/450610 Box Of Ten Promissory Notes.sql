DELETE FROM `weenie` WHERE `class_Id` = 450610;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450610, 'ace450610-boxoftenpromissorynotespk', 38, '2021-11-01 00:00:00') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450610,   1,        128) /* ItemType - Misc */
     , (450610,   5,         50) /* EncumbranceVal */
     , (450610,  11,          1) /* MaxStackSize */
     , (450610,  12,          1) /* StackSize */
     , (450610,  13,         50) /* StackUnitEncumbrance */
     , (450610,  15,          4) /* StackUnitValue */
     , (450610,  16,          8) /* ItemUseable - Contained */
     , (450610,  19,          10) /* Value */
     , (450610,  33,          1) /* Bonded - Bonded */
     , (450610,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450610,  94,         16) /* TargetType - Creature */
     , (450610, 114,          1) /* Attuned - Attuned */
     , (450610, 269,         10) /* UseCreateQuantity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450610,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450610,  39,     0.5) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450610,   1, 'Box Of Ten Promissory Notes') /* Name */
     , (450610,  14, 'Use this crate to retrieve its contents.') /* Use */
     , (450610,  16, 'A box containing 10 Promissory Notes.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450610,   1, 0x0200011E) /* Setup */
     , (450610,   3, 0x20000014) /* SoundTable */
     , (450610,   8, 0x060072EB) /* Icon */
     , (450610,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450610,  38,      43901) /* UseCreateItem - Promissory Note */;
