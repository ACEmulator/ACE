DELETE FROM `weenie` WHERE `class_Id` = 30964;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (30964, 'portaleggorchard', 7, '2021-11-08 06:01:47') /* Portal */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (30964,   1,      65536) /* ItemType - Portal */
     , (30964,  16,         32) /* ItemUseable - Remote */
     , (30964,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (30964,  86,        140) /* MinLevel */
     , (30964,  93,       3084) /* PhysicsState - Ethereal, ReportCollisions, Gravity, LightingOn */
     , (30964, 111,          8) /* PortalBitmask - Unrestricted */
     , (30964, 133,          4) /* ShowableOnRadar - ShowAlways */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (30964,   1, True ) /* Stuck */
     , (30964,  11, False) /* IgnoreCollisions */
     , (30964,  12, True ) /* ReportCollisions */
     , (30964,  13, True ) /* Ethereal */
     , (30964,  15, True ) /* LightsStatus */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (30964,  54,    -0.1) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (30964,   1, 'Egg Orchard') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (30964,   1, 0x020005D5) /* Setup */
     , (30964,   2, 0x09000003) /* MotionTable */
     , (30964,   8, 0x0600106B) /* Icon */;

INSERT INTO `weenie_properties_position` (`object_Id`, `position_Type`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (30964, 2, 0x002B0371, 458.535, -172.203, 0.005, 0.933008, 0, 0, -0.359856) /* Destination */
/* @teleloc 0x002B0371 [458.535004 -172.203003 0.005000] 0.933008 0.000000 0.000000 -0.359856 */;
