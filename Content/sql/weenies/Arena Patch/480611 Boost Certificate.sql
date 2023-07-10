DELETE FROM `weenie` WHERE `class_Id` = 480611;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480611, 'ace480611-Boostcertificate', 1, '2021-11-01 00:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480611,   1,        128) /* ItemType - Misc */
     , (480611,   5,          5) /* EncumbranceVal */
     , (480611,  16,          1) /* ItemUseable - No */
     , (480611,  19,          2) /* Value */
     , (480611,  33,          1) /* Bonded - Bonded */
     , (480611,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480611, 114,          1) /* Attuned - Attuned */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480611,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480611,   1, 'Experience Certificate') /* Name */
     , (480611,  15, 'A certificate that grants the bearer 2,000,000,000 experience. Bring this certificate to Darkbeat. ') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480611,   1, 0x020000E3) /* Setup */
     , (480611,   3, 0x20000014) /* SoundTable */
     , (480611,   8, 0x060072E7) /* Icon */
     , (480611,  22, 0x3400002B) /* PhysicsEffectTable */;
