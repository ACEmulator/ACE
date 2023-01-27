DELETE FROM `weenie` WHERE `class_Id` = 4200080;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200080, 'portaltcbobo', 7, '2022-01-25 10:00:00') /* Podtide's Town Master's Portal to Ayan Baqur */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200080,   1,      65536) /* ItemType - Portal */
     , (4200080,  16,         32) /* ItemUseable - Remote */
     , (4200080,  93,       3084) /* PhysicsState - Ethereal, ReportCollisions, Gravity, LightingOn */
     , (4200080, 111,          48) /* idk blocked or sth ask tindale -- plus ev thinks its untieable unrecallable unsummonable but hes an idiot */
     , (4200080, 133,          4) /* ShowableOnRadar - ShowAlways */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200080,   1, True ) /* Stuck */
     , (4200080,  11, False) /* IgnoreCollisions */
     , (4200080,  12, True ) /* ReportCollisions */
     , (4200080,  13, True ) /* Ethereal */
     , (4200080,  15, True ) /* LightsStatus */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200080,  54,    -0.1) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200080,   1, 'Bobo Waterfall') /* Name -- TODO - Quest Rescriction*/;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200080,   1, 0x020001B3) /* Setup */
     , (4200080,   2, 0x09000003) /* MotionTable */
     , (4200080,   8, 0x0600106B) /* Icon */;

INSERT INTO `weenie_properties_position` (`object_Id`, `position_Type`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (4200080, 2, 0xEA740024, 11.850273, 82.063774, 79.904999, -0.999121, 0, 0, -0.041927) /* Destination */
/* @teleloc 0x01C9022D [72.900002 -30.200001 0.000000] 0.139173 0.000000 0.000000 -0.990268 */;
