DELETE FROM `weenie` WHERE `class_Id` = 4200085;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200085, 'portaltclighthousehermit', 7, '2022-01-25 10:00:00') /* Podtide's Town Master's Portal to Ayan Baqur */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200085,   1,      65536) /* ItemType - Portal */
     , (4200085,  16,         32) /* ItemUseable - Remote */
     , (4200085,  93,       3084) /* PhysicsState - Ethereal, ReportCollisions, Gravity, LightingOn */
     , (4200085, 111,          48) /* idk blocked or sth ask tindale -- plus ev thinks its untieable unrecallable unsummonable but hes an idiot */
     , (4200085, 133,          4) /* ShowableOnRadar - ShowAlways */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200085,   1, True ) /* Stuck */
     , (4200085,  11, False) /* IgnoreCollisions */
     , (4200085,  12, True ) /* ReportCollisions */
     , (4200085,  13, True ) /* Ethereal */
     , (4200085,  15, True ) /* LightsStatus */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200085,  54,    -0.1) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200085,   1, 'Lighthouse Hermit') /* Name --*/;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200085,   1, 0x020001B3) /* Setup */
     , (4200085,   2, 0x09000003) /* MotionTable */
     , (4200085,   8, 0x0600106B) /* Icon */;

INSERT INTO `weenie_properties_position` (`object_Id`, `position_Type`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (4200085, 2, 0x435D0035, 144.88997, 111.9331, 35.914112, -0.42048296, 0, 0, -0.9073004) /* Destination */
/* @teleloc 0x01C9022D [72.900002 -30.200001 0.000000] 0.139173 0.000000 0.000000 -0.990268 */;
