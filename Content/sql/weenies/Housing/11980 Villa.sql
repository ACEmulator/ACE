DELETE FROM `weenie` WHERE `class_Id` = 11980;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (11980, 'slumlordvilla926-970', 55, '2005-02-09 10:00:00') /* SlumLord */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (11980,  16,         32) /* ItemUseable - Remote */
     , (11980,  86,         35) /* MinLevel */
     , (11980,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (11980,   1, True ) /* Stuck */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (11980,  39,     1.2) /* DefaultScale */
     , (11980,  54,       3) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (11980,   1, 'Villa') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (11980,   1, 0x02000AAF) /* Setup */
     , (11980,   2, 0x090000B8) /* MotionTable */
     , (11980,   8, 0x0600218C) /* Icon */;

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (11980, 16,   1000002,  100, 0, 0, False) /* Create Cabbage (260) for HouseBuy */
     , (11980, 32,   1000002,  40, 0, 0, False) /* Create Pyreal (273) for HouseRent */;
