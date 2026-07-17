DELETE FROM `weenie` WHERE `class_Id` = 7001021;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (7001021, 'portaltoFrozenValley', 7, '2005-02-09 10:00:00') /* Portal */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (7001021,   1,      65536) /* ItemType - Portal */
     , (7001021,  16,         32) /* ItemUseable - Remote */
     , (7001021,  93,       3084) /* PhysicsState - Ethereal, ReportCollisions, Gravity, LightingOn */
     , (7001021, 111,          1) /* PortalBitmask - Unrestricted */
     , (7001021, 133,          4) /* ShowableOnRadar - ShowAlways */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (7001021,   1, True ) /* Stuck */
     , (7001021,  11, False) /* IgnoreCollisions */
     , (7001021,  12, True ) /* ReportCollisions */
     , (7001021,  13, True ) /* Ethereal */
     , (7001021,  15, True ) /* LightsStatus */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (7001021,  54,    -0.1) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (7001021,   1, 'Frozen Valley Portal') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (7001021,   1, 0x020001B3) /* Setup */
     , (7001021,   2, 0x09000003) /* MotionTable */
     , (7001021,   8, 0x0600106B) /* Icon */;

INSERT INTO `weenie_properties_position` (`object_Id`, `position_Type`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES

-- (7001021, 2, 0xE64E002F, 138.304, 161.905, 20.04, 0.92388, 0, 0, -0.382684) /* Destination */
/* @teleloc 0xE64E002F [138.304001 161.904999 20.040001] 0.923880 0.000000 0.000000 -0.382684 */
(7001021, 2,0x77E7002E, 121.633705, 124.958954, 106.006004, 0.993174, 0.000000, 0.000000, -0.116646) /* Destination */;
/* 0xB54B0030 [132.000000 179.999023 116.006004] 0.540302 0.000000 0.000000 0.8*/
