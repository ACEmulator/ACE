DELETE FROM `weenie` WHERE `class_Id` = 20850;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (20850, 'slumlordcottage6051-6125', 55, '2005-02-09 10:00:00') /* SlumLord */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (20850,  16,         32) /* ItemUseable - Remote */
     , (20850,  86,         20) /* MinLevel */
     , (20850,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (20850,   1, True ) /* Stuck */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (20850,  39,     1.2) /* DefaultScale */
     , (20850,  54,       3) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (20850,   1, 'Cottage') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (20850,   1, 0x02000AAF) /* Setup */
     , (20850,   2, 0x090000B8) /* MotionTable */
     , (20850,   8, 0x0600218C) /* Icon */;

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (20850, 16,   1000002,  50, 0, 0, False) /* Create Cabbage (260) for HouseBuy */
     , (20850, 32,   1000002,  25, 0, 0, False) /* Create Pyreal (273) for HouseRent */;
