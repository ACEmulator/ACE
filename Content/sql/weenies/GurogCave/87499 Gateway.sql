DELETE FROM `weenie` WHERE `class_Id` = 87499;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (87499, 'ace87499-gateway', 7, '2021-11-01 00:00:00') /* Portal */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (87499,   1,      65536) /* ItemType - Portal */
     , (87499,  16,         32) /* ItemUseable - Remote */
     , (87499,  93,       2052) /* PhysicsState - Ethereal, LightingOn */
     , (87499, 111,         17) /* PortalBitmask - Unrestricted, NoSummon */
     , (87499, 133,          4) /* ShowableOnRadar - ShowAlways */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (87499,   1, True ) /* Stuck */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (87499,  54,    0.75) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (87499,   1, 'Gateway') /* Name */
     , (87499,  14, 'You must use this portal to activate it. Walking through the portal will not activate it.') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (87499,   1, 0x020006F4) /* Setup */
     , (87499,   2, 0x09000003) /* MotionTable */
     , (87499,   8, 0x0600106B) /* Icon */;

INSERT INTO `weenie_properties_position` (`object_Id`, `position_Type`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (87499, 2, 0xF93B0413, 179.049, 91.2016, -45.995, -0.382683, 0, 0, -0.92388) /* Destination */
/* @teleloc 0xF93B0413 [179.048996 91.201599 -45.994999] -0.382683 0.000000 0.000000 -0.923880 */;
