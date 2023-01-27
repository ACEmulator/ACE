DELETE FROM `weenie` WHERE `class_Id` = 1910225;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1910225, '1910225', 7, '2019-08-20 01:45:15') /* Portal */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1910225,   1,      65536) /* ItemType - Portal */
     , (1910225,  16,         32) /* ItemUseable - Remote */
     , (1910225,  86,        150) /* MinLevel */
     , (1910225,  93,       3084) /* PhysicsState - Ethereal, ReportCollisions, Gravity, LightingOn */
     , (1910225, 111,         49) /* PortalBitmask - NotPassable, Unrestricted, NoSummon, NoRecall */
     , (1910225, 133,          4) /* ShowableOnRadar - ShowAlways */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1910225,   1, True ) /* Stuck */
     , (1910225,  12, True ) /* ReportCollisions */
     , (1910225,  13, True ) /* Ethereal */
     , (1910225,  14, True ) /* GravityStatus */
     , (1910225,  15, True ) /* LightsStatus */
     , (1910225,  19, True ) /* Attackable */
     , (1910225,  88, True ) /* PortalShowDestination */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1910225,  54, -0.100000001490116) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1910225,   1, 'Death Isle') /* Name */
     , (1910225,  16, 'A portal to an island filled with Death.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1910225,   1,   33555925) /* Setup */
     , (1910225,   2,  150994947) /* MotionTable */
     , (1910225,   8,  100667499) /* Icon */;

INSERT INTO `weenie_properties_position` (`object_Id`, `position_Type`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (1910225, 2, 3370713099, 38.069, 60.3064, 0.337795, -0.066654, 0, 0, -0.997776) /* Destination */
/* @teleloc 0xC8E9000B [38.069000 60.306400 0.337795] -0.066654 0.000000 0.000000 -0.997776 */;
