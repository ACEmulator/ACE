DELETE FROM `weenie` WHERE `class_Id` = 30960;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (30960, 'portaldarkdesign', 7, '2021-11-08 06:01:47') /* Portal */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (30960,   1,      65536) /* ItemType - Portal */
     , (30960,  16,         32) /* ItemUseable - Remote */
     , (30960,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (30960,  86,        140) /* MinLevel */
     , (30960,  93,       3084) /* PhysicsState - Ethereal, ReportCollisions, Gravity, LightingOn */
     , (30960, 111,          8) /* PortalBitmask - Unrestricted */
     , (30960, 133,          4) /* ShowableOnRadar - ShowAlways */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (30960,   1, True ) /* Stuck */
     , (30960,  11, False) /* IgnoreCollisions */
     , (30960,  12, True ) /* ReportCollisions */
     , (30960,  13, True ) /* Ethereal */
     , (30960,  15, True ) /* LightsStatus */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (30960,  54,    -0.1) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (30960,   1, 'Dark Design') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (30960,   1, 0x020005D5) /* Setup */
     , (30960,   2, 0x09000003) /* MotionTable */
     , (30960,   8, 0x0600106B) /* Icon */;

INSERT INTO `weenie_properties_position` (`object_Id`, `position_Type`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (30960, 2, 0x002D0373, 200.142, -4.32755, 0.005, -0.024927, 0, 0, -0.999689) /* Destination */
/* @teleloc 0x002D0373 [200.141998 -4.327550 0.005000] -0.024927 0.000000 0.000000 -0.999689 */;
