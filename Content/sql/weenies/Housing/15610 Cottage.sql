DELETE FROM `weenie` WHERE `class_Id` = 15610;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (15610, 'slumlordcottage2726-2800', 55, '2005-02-09 10:00:00') /* SlumLord */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (15610,  16,         32) /* ItemUseable - Remote */
     , (15610,  86,         20) /* MinLevel */
     , (15610,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (15610,   1, True ) /* Stuck */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (15610,  39,     1.2) /* DefaultScale */
     , (15610,  54,       3) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (15610,   1, 'Cottage') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (15610,   1, 0x02000AAF) /* Setup */
     , (15610,   2, 0x090000B8) /* MotionTable */
     , (15610,   8, 0x0600218C) /* Icon */;

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (15610, 16,   1000002,  50, 0, 0, False) /* Create Cabbage (260) for HouseBuy */
     , (15610, 32,   1000002,  25, 0, 0, False) /* Create Pyreal (273) for HouseRent */;