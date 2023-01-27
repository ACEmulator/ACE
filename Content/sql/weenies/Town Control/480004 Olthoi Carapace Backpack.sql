DELETE FROM `weenie` WHERE `class_Id` = 480004;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480004, 'backpackolthoi48', 21, '2005-02-09 10:00:00') /* Container */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480004,   1,        512) /* ItemType - Container */
     , (480004,   5,          1) /* EncumbranceVal */
     , (480004,   6,         48) /* ItemsCapacity */
     , (480004,   7,          0) /* ContainersCapacity */
     , (480004,   8,          0) /* Mass */
     , (480004,   9,          0) /* ValidLocations - None */
     , (480004,  16,         56) /* ItemUseable - ContainedViewedRemote */
     , (480004,  19,      250) /* Value */
     , (480004,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480004,  96,       2000) /* EncumbranceCapacity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480004,  22, True ) /* Inscribable */
     , (480004,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480004,  39,     0.8) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480004,   1, 'Olthoi Carapace Backpack') /* Name */
     , (480004,  16, 'A large chitinous olthoi carapace crafted into a strong, but very light backpack.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480004,   1, 0x02000CD8) /* Setup */
     , (480004,   3, 0x20000014) /* SoundTable */
     , (480004,   8, 0x06001A67) /* Icon */
     , (480004,  22, 0x3400002B) /* PhysicsEffectTable */;
