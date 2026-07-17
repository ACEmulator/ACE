DELETE FROM `weenie` WHERE `class_Id` = 900021218;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (900021218, 'largeflamecloudlowlifespan', 1, '2005-02-09 10:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (900021218,   1,        128) /* ItemType - Misc */
     , (900021218,   5,          0) /* EncumbranceVal */
     , (900021218,   8,          0) /* Mass */
     , (900021218,  16,          1) /* ItemUseable - No */
     , (900021218,  19,          0) /* Value */
     , (900021218,  93,       2068) /* PhysicsState - Ethereal, IgnoreCollisions, LightingOn */
         , (900021218, 267,         20) /* Lifespan */;
INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (900021218,   1, True ) /* Stuck */
     , (900021218,  13, True ) /* Ethereal */
     , (900021218,  14, False) /* GravityStatus */
     , (900021218,  15, True ) /* LightsStatus */
     , (900021218,  24, True ) /* UiHidden */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (900021218,   1, 'Flame Cloud') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (900021218,   1, 0x02000D7F) /* Setup */
     , (900021218,   8, 0x06001066) /* Icon */;
