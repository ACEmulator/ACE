DELETE FROM `weenie` WHERE `class_Id` = 900021219;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (900021219, 'lightningcloudlowlifespan', 1, '2005-02-09 10:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (900021219,   1,        128) /* ItemType - Misc */
     , (900021219,   5,          0) /* EncumbranceVal */
     , (900021219,   8,          0) /* Mass */
     , (900021219,  16,          1) /* ItemUseable - No */
     , (900021219,  19,          0) /* Value */
     , (900021219,  93,       2068) /* PhysicsState - Ethereal, IgnoreCollisions, LightingOn */
     , (900021219, 267,         20) /* Lifespan */;


INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (900021219,   1, True ) /* Stuck */
     , (900021219,  13, True ) /* Ethereal */
     , (900021219,  14, False) /* GravityStatus */
     , (900021219,  15, True ) /* LightsStatus */
     , (900021219,  24, True ) /* UiHidden */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (900021219,   1, 'Lightning Cloud') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (900021219,   1, 0x02000D81) /* Setup */
     , (900021219,   8, 0x06001066) /* Icon */;
