DELETE FROM `weenie` WHERE `class_Id` = 4200013;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200013, 'ace4200013-Castle', 7, '2021-11-17 16:56:08') /* Portal */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200013,   1,      65536) /* ItemType - Portal */
     , (4200013,  16,         32) /* ItemUseable - Remote */
     , (4200013,  93,       3084) /* PhysicsState - Ethereal, ReportCollisions, Gravity, LightingOn */
     , (4200013, 111,         49) /* PortalBitmask - Unrestricted, NoSummon, NoRecall */
     , (4200013, 133,          4) /* ShowableOnRadar - ShowAlways */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200013,   1, True ) /* Stuck */
     , (4200013,  12, True ) /* ReportCollisions */
     , (4200013,  13, True ) /* Ethereal */
     , (4200013,  14, True ) /* GravityStatus */
     , (4200013,  15, True ) /* LightsStatus */
     , (4200013,  19, True ) /* Attackable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200013,  54,    -0.1) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200013,   1, 'Castle Top') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200013,   1, 0x020001B3) /* Setup */
     , (4200013,   2, 0x09000003) /* MotionTable */
     , (4200013,   8, 0x0600106B) /* Icon */;

INSERT INTO `weenie_properties_position` (`object_Id`, `position_Type`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (4200013, 2, 0x4FF1001A, 84.4807, 43.2229, 84.006, -1, 0, 0, 0) /* Destination */
/* @teleloc 0x4FF1001A [84.480698 43.222900 84.005997] -1.000000 0.000000 0.000000 0.000000 */;
