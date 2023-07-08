DELETE FROM `weenie` WHERE `class_Id` = 53447;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (53447, 'ace53447-eldrytchwebgauntlet', 7, '2021-11-01 00:00:00') /* Portal */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (53447,   1,      65536) /* ItemType - Portal */
     , (53447,  16,         32) /* ItemUseable - Remote */
     , (53447,  86,        180) /* MinLevel */
     , (53447,  93,       3084) /* PhysicsState - Ethereal, ReportCollisions, Gravity, LightingOn */
     , (53447, 111,         49) /* PortalBitmask - Unrestricted, NoSummon, NoRecall */
     , (53447, 133,          4) /* ShowableOnRadar - ShowAlways */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (53447,   1, True ) /* Stuck */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (53447,  54,    -0.1) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (53447,   1, 'Eldrytch Web Gauntlet') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (53447,   1, 0x020001B3) /* Setup */
     , (53447,   2, 0x09000003) /* MotionTable */
     , (53447,   8, 0x0600106B) /* Icon */;

INSERT INTO `weenie_properties_position` (`object_Id`, `position_Type`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (53447, 2, 0x596B0110, 120, -80, 0.005, -0.707107, 0, 0, -0.707107) /* Destination */
/* @teleloc 0x596B0110 [120.000000 -80.000000 0.005000] -0.707107 0.000000 0.000000 -0.707107 */;
