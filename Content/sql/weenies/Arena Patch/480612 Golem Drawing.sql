DELETE FROM `weenie` WHERE `class_Id` = 480612;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480612, 'ace480612-golemdrawingcertificate', 1, '2021-11-01 00:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480612,   1,        128) /* ItemType - Misc */
     , (480612,   5,          5) /* EncumbranceVal */
     , (480612,  16,          1) /* ItemUseable - No */
     , (480612,  19,          2) /* Value */
     , (480612,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480612,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480612,   1, 'Darkbeat''s Golem Drawing') /* Name */
     , (480612,  15, 'A Darkbeat masterpiece! Recieve a box of augmentation gems as a reward. ') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480612,   1, 0x020019B9) /* Setup */
     , (480612,   8, 0x060012D3) /* Icon */;
