DELETE FROM `weenie` WHERE `class_Id` = 11719;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (11719, 'slumlordvillamoderate', 55, '2005-02-09 10:00:00') /* SlumLord */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (11719,  16,         32) /* ItemUseable - Remote */
     , (11719,  86,         35) /* MinLevel */
     , (11719,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (11719,   1, True ) /* Stuck */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (11719,  39,     1.2) /* DefaultScale */
     , (11719,  54,       3) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (11719,   1, 'Villa') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (11719,   1, 0x02000AAF) /* Setup */
     , (11719,   2, 0x090000B8) /* MotionTable */
     , (11719,   8, 0x0600218C) /* Icon */;

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (11719, 16,   1000002,  100, 0, 0, False) /* Create Cabbage (260) for HouseBuy */
     , (11719, 32,   1000002,  40, 0, 0, False) /* Create Pyreal (273) for HouseRent */;
