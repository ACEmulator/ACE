DELETE FROM `weenie` WHERE `class_Id` = 4200045;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200045, 'portalholtburgtoabandonedmine', 7, '2005-02-09 10:00:00') /* Portal */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200045,   1,      65536) /* ItemType - Portal */
     , (4200045,  16,         32) /* ItemUseable - Remote */
     , (4200045,  93,       3084) /* PhysicsState - Ethereal, ReportCollisions, Gravity, LightingOn */
     , (4200045, 111,         56) /* PortalBitmask - NoSummon, NoRecall, No NPK */
     , (4200045, 133,          4) /* ShowableOnRadar - ShowAlways */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200045,   1, True ) /* Stuck */
     , (4200045,  11, False) /* IgnoreCollisions */
     , (4200045,  12, True ) /* ReportCollisions */
     , (4200045,  13, True ) /* Ethereal */
     , (4200045,  15, True ) /* LightsStatus */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200045,  54,    -0.1) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200045,   1, 'Abandoned Mine') /* Name */
     , (4200045,  37, 'HoltburgTownControlOwner') /* QuestRestriction */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200045,   1, 0x020001B3) /* Setup */
     , (4200045,   2, 0x09000003) /* MotionTable */
     , (4200045,   8, 0x0600106B) /* Icon */;

INSERT INTO `weenie_properties_position` (`object_Id`, `position_Type`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (4200045, 2, 0x01C9022D, 72.9, -30.2, 0, 0.139173, 0, 0, -0.990268) /* Destination */
/* @teleloc 0x01C9022D [72.900002 -30.200001 0.000000] 0.139173 0.000000 0.000000 -0.990268 */;
