DELETE FROM `weenie` WHERE `class_Id` = 19076;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (19076, 'slumlordvilla4001-4050', 55, '2005-02-09 10:00:00') /* SlumLord */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (19076,  16,         32) /* ItemUseable - Remote */
     , (19076,  86,         35) /* MinLevel */
     , (19076,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (19076,   1, True ) /* Stuck */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (19076,  39,     1.2) /* DefaultScale */
     , (19076,  54,       3) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (19076,   1, 'Villa') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (19076,   1, 0x02000AAF) /* Setup */
     , (19076,   2, 0x090000B8) /* MotionTable */
     , (19076,   8, 0x0600218C) /* Icon */;

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (19076, 16,   1000002,  100, 0, 0, False) /* Create Cabbage (260) for HouseBuy */
     , (19076, 32,   1000002,  40, 0, 0, False) /* Create Pyreal (273) for HouseRent */;
