DELETE FROM `weenie` WHERE `class_Id` = 900021217;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (900021217, 'largeacidcloudlowlifespan', 1, '2005-02-09 10:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (900021217,   1,        128) /* ItemType - Misc */
     , (900021217,   5,          0) /* EncumbranceVal */
     , (900021217,   8,          0) /* Mass */
     , (900021217,  16,          1) /* ItemUseable - No */
     , (900021217,  19,          0) /* Value */
     , (900021217,  93,       2068) /* PhysicsState - Ethereal, IgnoreCollisions, LightingOn */
         , (900021217, 267,         20) /* Lifespan */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (900021217,   1, True ) /* Stuck */
     , (900021217,  13, True ) /* Ethereal */
     , (900021217,  14, False) /* GravityStatus */
     , (900021217,  15, True ) /* LightsStatus */
     , (900021217,  24, True ) /* UiHidden */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (900021217,   1, 'Acidic Cloud') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (900021217,   1, 0x02000D7E) /* Setup */
     , (900021217,   8, 0x06001066) /* Icon */;
