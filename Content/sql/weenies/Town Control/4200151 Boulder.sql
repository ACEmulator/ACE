DELETE FROM `weenie` WHERE `class_Id` = 4200151;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200151, 'ace4200151-boulder', 1, '2020-05-31 06:03:27') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200151,   1,        128) /* ItemType - Misc */
     , (4200151,   5,      70000) /* EncumbranceVal */
     , (4200151,   8,      14000) /* Mass */
     , (4200151,   9,          0) /* ValidLocations - None */
     , (4200151,  16,          1) /* ItemUseable - No */
     , (4200151,  19,        900) /* Value */
     , (4200151,  93,       1032) /* PhysicsState - ReportCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200151,   1, True ) /* Stuck */
     , (4200151,  13, False) /* Ethereal */
     , (4200151,  24, True ) /* UiHidden */
     , (4200151,  41, True ) /* ReportCollisionsAsEnvironment */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200151,  44,      -1) /* TimeToRot */
     , (4200151,  39,       2.2) /* DefaultScale */
;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200151,   1, 'Boulder') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200151,   1, 0x02001761) /* Setup */
     , (4200151,   8, 0x06001F64) /* Icon */;
