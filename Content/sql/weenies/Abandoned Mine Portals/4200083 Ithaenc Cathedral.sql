DELETE FROM `weenie` WHERE `class_Id` = 4200083;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200083, 'portaltcsanc', 7, '2022-01-25 10:00:00') /* Podtide's Town Master's Portal to Ayan Baqur */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200083,   1,      65536) /* ItemType - Portal */
     , (4200083,  16,         32) /* ItemUseable - Remote */
     , (4200083,  93,       3084) /* PhysicsState - Ethereal, ReportCollisions, Gravity, LightingOn */
     , (4200083, 111,          48) /* idk blocked or sth ask tindale -- plus ev thinks its untieable unrecallable unsummonable but hes an idiot */
     , (4200083, 133,          4) /* ShowableOnRadar - ShowAlways */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200083,   1, True ) /* Stuck */
     , (4200083,  11, False) /* IgnoreCollisions */
     , (4200083,  12, True ) /* ReportCollisions */
     , (4200083,  13, True ) /* Ethereal */
     , (4200083,  15, True ) /* LightsStatus */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200083,  54,    -0.1) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200083,   1, 'Ithaenc Cathedral') /* Name -- TODO - Quest Rescriction*/;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200083,   1, 0x020001B3) /* Setup */
     , (4200083,   2, 0x09000003) /* MotionTable */
     , (4200083,   8, 0x0600106B) /* Icon */;

INSERT INTO `weenie_properties_position` (`object_Id`, `position_Type`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (4200083, 2, 0xF418000B, 36, 60, 191, 1, 0, 0, 0) /* Destination */
/* @teleloc 0x01C9022D [72.900002 -30.200001 0.000000] 0.139173 0.000000 0.000000 -0.990268 */;
