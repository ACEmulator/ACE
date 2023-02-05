DELETE FROM `weenie` WHERE `class_Id` = 480005;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480005, 'ace480005-pumpkinbackpackpk', 21, '2021-11-01 00:00:00') /* Container */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480005,   1,        512) /* ItemType - Container */
     , (480005,   5,          1) /* EncumbranceVal */
     , (480005,   6,         48) /* ItemsCapacity */
     , (480005,   7,          0) /* ContainersCapacity */
     , (480005,   9,          0) /* ValidLocations - None */
     , (480005,  16,         56) /* ItemUseable - ContainedViewedRemote */
     , (480005,  19,        250) /* Value */
     , (480005,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480005,  96,       2000) /* EncumbranceCapacity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480005,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480005,  39,     1.3) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480005,   1, 'Pumpkin Backpack') /* Name */
     , (480005,  14, 'Use this item to close it.') /* Use */
     , (480005,  16, 'A hollowed out pumpkin with some straps so it can be used to carry things.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480005,   1, 0x02000949) /* Setup */
     , (480005,   3, 0x20000014) /* SoundTable */
     , (480005,   6, 0x04001008) /* PaletteBase */
     , (480005,   8, 0x06001E2B) /* Icon */
     , (480005,  22, 0x3400002B) /* PhysicsEffectTable */;
