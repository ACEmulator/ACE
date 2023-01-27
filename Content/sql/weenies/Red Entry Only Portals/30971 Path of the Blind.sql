DELETE FROM `weenie` WHERE `class_Id` = 30971;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (30971, 'portalpathblind', 7, '2021-11-08 06:01:47') /* Portal */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (30971,   1,      65536) /* ItemType - Portal */
     , (30971,  16,         32) /* ItemUseable - Remote */
     , (30971,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (30971,  86,        140) /* MinLevel */
     , (30971,  93,       3084) /* PhysicsState - Ethereal, ReportCollisions, Gravity, LightingOn */
     , (30971, 111,          8) /* PortalBitmask - Unrestricted */
     , (30971, 133,          4) /* ShowableOnRadar - ShowAlways */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (30971,   1, True ) /* Stuck */
     , (30971,  11, False) /* IgnoreCollisions */
     , (30971,  12, True ) /* ReportCollisions */
     , (30971,  13, True ) /* Ethereal */
     , (30971,  15, True ) /* LightsStatus */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (30971,  54,    -0.1) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (30971,   1, 'Path of the Blind') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (30971,   1, 0x020005D5) /* Setup */
     , (30971,   2, 0x09000003) /* MotionTable */
     , (30971,   8, 0x0600106B) /* Icon */;

INSERT INTO `weenie_properties_position` (`object_Id`, `position_Type`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (30971, 2, 0x002A034A, 52.8552, -248.075, 0.005, 1, 0, 0, 0) /* Destination */
/* @teleloc 0x002A034A [52.855202 -248.074997 0.005000] 1.000000 0.000000 0.000000 0.000000 */;
