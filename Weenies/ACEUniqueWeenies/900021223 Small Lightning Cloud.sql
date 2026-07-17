DELETE FROM `weenie` WHERE `class_Id` = 900021223;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (900021223, 'smalllightningcore', 1, '2005-02-09 10:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (900021223,   1,        128) /* ItemType - Misc */
     , (900021223,   5,          0) /* EncumbranceVal */
     , (900021223,   8,          0) /* Mass */
     , (900021223,  16,          1) /* ItemUseable - No */
     , (900021223,  19,          0) /* Value */
     , (900021223,  93,       2068) /* PhysicsState - Ethereal, IgnoreCollisions, LightingOn */
         , (900021223, 267,         8) /* Lifespan */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (900021223,   1, True ) /* Stuck */
     , (900021223,  13, True ) /* Ethereal */
     , (900021223,  14, False) /* GravityStatus */
     , (900021223,  15, True ) /* LightsStatus */
     , (900021223,  24, True ) /* UiHidden */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (900021223,   1, 'Lightning Cloud') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (900021223,   1, 0x02000B6C) /* Setup */
     , (900021223,   8, 0x06001066) /* Icon */;
