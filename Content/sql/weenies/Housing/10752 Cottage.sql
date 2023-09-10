DELETE FROM `weenie` WHERE `class_Id` = 10752;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10752, 'slumlordtestcheap', 55, '2005-02-09 10:00:00') /* SlumLord */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10752,  16,         32) /* ItemUseable - Remote */
     , (10752,  86,         15) /* MinLevel */
     , (10752,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (10752, 149,          0) /* HouseStatus - InActive */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10752,   1, True ) /* Stuck */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (10752,  39,     1.2) /* DefaultScale */
     , (10752,  54,       3) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10752,   1, 'Cottage') /* Name */
     , (10752,  34, 'CottageEventTest') /* GeneratorEvent */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10752,   1, 0x02000AAF) /* Setup */
     , (10752,   2, 0x090000B8) /* MotionTable */
     , (10752,   8, 0x0600218C) /* Icon */;

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (10752, 16,   1000002,  50, 0, 0, False) /* Create Cabbage (260) for HouseBuy */
     , (10752, 32,   1000002,  25, 0, 0, False) /* Create Pyreal (273) for HouseRent */;
