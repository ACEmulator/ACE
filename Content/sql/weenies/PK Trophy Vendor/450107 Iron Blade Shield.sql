DELETE FROM `weenie` WHERE `class_Id` = 450107;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450107, 'ace450107-ironbladeshieldtailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450107,   1,          2) /* ItemType - Armor */
     , (450107,   5,        0) /* EncumbranceVal */
     , (450107,   9,    2097152) /* ValidLocations - Shield */
     , (450107,  16,          1) /* ItemUseable - No */
	 , (450107,  19,       20) /* Value */
     , (450107,  51,          4) /* CombatUse - Shield */
	 , (450107,  52,          3) /* ParentLocation - Shield */
     , (450107,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450107, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450107,  11, True ) /* IgnoreCollisions */
     , (450107,  13, True ) /* Ethereal */
     , (450107,  14, True ) /* GravityStatus */
     , (450107,  19, True ) /* Attackable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450107,  39,     1.3) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450107,   1, 'Iron Blade Shield') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450107,   1, 0x02001A07) /* Setup */
     , (450107,   3, 0x20000014) /* SoundTable */
     , (450107,   8, 0x06006E04) /* Icon */
     , (450107,  22, 0x3400002B) /* PhysicsEffectTable */;
