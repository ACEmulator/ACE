DELETE FROM `weenie` WHERE `class_Id` = 11714;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (11714, 'slumlordmansioncheap', 55, '2021-11-01 00:00:00') /* SlumLord */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (11714,  16,         32) /* ItemUseable - Remote */
     , (11714,  86,         50) /* MinLevel */
     , (11714,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (11714, 163,          6) /* AllegianceMinLevel */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (11714,   1, True ) /* Stuck */
     , (11714,  76, True ) /* HouseRequiresMonarch */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (11714,  39,     1.2) /* DefaultScale */
     , (11714,  54,       3) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (11714,   1, 'Mansion') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (11714,   1, 0x02000AAF) /* Setup */
     , (11714,   2, 0x090000B8) /* MotionTable */
     , (11714,   8, 0x0600218C) /* Icon */;

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (11714, 16,   1000002,  500, 0, 0, False) /* Create Cabbage (260) for HouseBuy */
     , (11714, 32,   1000002,  150, 0, 0, False) /* Create Pyreal (273) for HouseRent */;
