DELETE FROM `weenie` WHERE `class_Id` = 12462;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (12462, 'slumlordcottage1076-1150', 55, '2005-02-09 10:00:00') /* SlumLord */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (12462,  16,         32) /* ItemUseable - Remote */
     , (12462,  86,         20) /* MinLevel */
     , (12462,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (12462,   1, True ) /* Stuck */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (12462,  39,     1.2) /* DefaultScale */
     , (12462,  54,       3) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (12462,   1, 'Cottage') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (12462,   1, 0x02000AAF) /* Setup */
     , (12462,   2, 0x090000B8) /* MotionTable */
     , (12462,   8, 0x0600218C) /* Icon */;

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (12462, 16,   1000002,  50, 0, 0, False) /* Create Cabbage (260) for HouseBuy */
     , (12462, 32,   1000002,  25, 0, 0, False) /* Create Pyreal (273) for HouseRent */;
