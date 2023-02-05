DELETE FROM `weenie` WHERE `class_Id` = 480006;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480006, 'ace480006-tmonbackpackpk', 21, '2021-11-01 00:00:00') /* Container */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480006,   1,        512) /* ItemType - Container */
     , (480006,   5,          1) /* EncumbranceVal */
     , (480006,   6,         48) /* ItemsCapacity */
     , (480006,  16,         56) /* ItemUseable - ContainedViewedRemote */
     , (480006,  19,        250) /* Value */
     , (480006,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480006,   2, False) /* Open */
     , (480006,  22, True ) /* Inscribable */
     , (480006,  34, False) /* DefaultOpen */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480006,  39,       3) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480006,   1, 'T-Mon BackPack') /* Name */
     , (480006,  14, 'Use this item to close it.') /* Use */
     , (480006,  16, 'A Tremendous Monouga skull connected to a series of straps so that it can be used to hold things as a backpack.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480006,   1, 0x0200163A) /* Setup */
     , (480006,   3, 0x20000014) /* SoundTable */
     , (480006,   8, 0x06006547) /* Icon */
     , (480006,  22, 0x3400002B) /* PhysicsEffectTable */;
