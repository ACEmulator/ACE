DELETE FROM `weenie` WHERE `class_Id` = 15608;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (15608, 'slumlordapartment', 55, '2005-02-09 10:00:00') /* SlumLord */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (15608,  16,         32) /* ItemUseable - Remote */
     , (15608,  86,         20) /* MinLevel */
     , (15608,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (15608,   1, True ) /* Stuck */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (15608,  54,       3) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (15608,   1, 'Apartment') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (15608,   1, 0x02000C7A) /* Setup */
     , (15608,   2, 0x090000EA) /* MotionTable */
     , (15608,   8, 0x0600218C) /* Icon */;

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (15608, 16,   1000002, 50, 0, 0, False) /* Create Pyreal (273) for HouseBuy */
     , (15608, 32,   1000002, 25, 0, 0, False) /* Create Pyreal (273) for HouseRent */;
