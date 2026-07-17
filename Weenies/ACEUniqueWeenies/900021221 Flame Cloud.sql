DELETE FROM `weenie` WHERE `class_Id` = 900021221;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (900021221, 'firecloudlowlifepsan', 1, '2005-02-09 10:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (900021221,   1,        128) /* ItemType - Misc */
     , (900021221,   5,          0) /* EncumbranceVal */
     , (900021221,   8,          0) /* Mass */
     , (900021221,  16,          1) /* ItemUseable - No */
     , (900021221,  19,          0) /* Value */
     , (900021221,  93,       2068) /* PhysicsState - Ethereal, IgnoreCollisions, LightingOn */
     , (900021221, 267,         8) /* Lifespan */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (900021221,   1, True ) /* Stuck */
     , (900021221,  13, True ) /* Ethereal */
     , (900021221,  14, False) /* GravityStatus */
     , (900021221,  15, True ) /* LightsStatus */
     , (900021221,  24, True ) /* UiHidden */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (900021221,   1, 'Flame Cloud') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (900021221,   1, 0x02000B22) /* Setup */
     , (900021221,   8, 0x06001066) /* Icon */;
