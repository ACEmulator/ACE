DELETE FROM `weenie` WHERE `class_Id` = 900021222;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (900021222, 'smallcoldcloudlowlifespan', 1, '2005-02-09 10:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (900021222,   1,        128) /* ItemType - Misc */
     , (900021222,   5,          0) /* EncumbranceVal */
     , (900021222,   8,          0) /* Mass */
     , (900021222,  16,          1) /* ItemUseable - No */
     , (900021222,  19,          0) /* Value */
     , (900021222,  93,       2068) /* PhysicsState - Ethereal, IgnoreCollisions, LightingOn */
         , (900021222, 267,         8) /* Lifespan */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (900021222,   1, True ) /* Stuck */
     , (900021222,  13, True ) /* Ethereal */
     , (900021222,  14, False) /* GravityStatus */
     , (900021222,  15, True ) /* LightsStatus */
     , (900021222,  24, True ) /* UiHidden */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (900021222,   1, 'Frost Cloud') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (900021222,   1, 0x02000997) /* Setup */
     , (900021222,   8, 0x06001066) /* Icon */;
