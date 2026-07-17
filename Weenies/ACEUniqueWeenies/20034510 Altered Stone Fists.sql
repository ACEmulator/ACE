DELETE FROM `weenie` WHERE `class_Id` = 20034510;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (20034510, 'ace20034510-Alteredstonefists', 7, '2021-11-01 00:00:00') /* Portal */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (20034510,   1,      65536) /* ItemType - Portal */
     , (20034510,  16,         32) /* ItemUseable - Remote */
     , (20034510,  93,       1040) /* PhysicsState - IgnoreCollisions, Gravity */
     , (20034510,  95,          8) /* RadarBlipColor - Yellow */
     , (20034510, 111,         49) /* PortalBitmask - Unrestricted, NoSummon, NoRecall */
     , (20034510, 133,          4) /* ShowableOnRadar - ShowAlways */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (20034510,   1, True ) /* Stuck */
     , (20034510,  11, True ) /* IgnoreCollisions */
     , (20034510,  14, True ) /* GravityStatus */
     , (20034510,  19, True ) /* Attackable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (20034510,  39,     1.4) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (20034510,   1, 'Stone Fists') /* Name */;
     -- , (20034510,  14, 'If you have been honored with this champion''s password you may use this statue to enter the Champion Arena.') /* Use */
     -- , (20034510,  37, 'AccessBoss3') /* QuestRestriction */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (20034510,   1, 0x020007CA) /* Setup */
     , (20034510,   2, 0x090000DB) /* MotionTable */
     , (20034510,   3, 0x2000008C) /* SoundTable */
     , (20034510,   7, 0x100006C1) /* ClothingBase */
     , (20034510,   8, 0x06001224) /* Icon */;

INSERT INTO `weenie_properties_position` (`object_Id`, `position_Type`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (20034510, 2, 0x00B60114, 40, -43, -11.995, 1, 0, 0, 0) /* Destination */
/* @teleloc 0x00B60114 [40.000000 -43.000000 -11.995000] 1.000000 0.000000 0.000000 0.000000 */;
