DELETE FROM `weenie` WHERE `class_Id` = 20851;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (20851, 'slumlordcottage6126-6200', 55, '2005-02-09 10:00:00') /* SlumLord */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (20851,  16,         32) /* ItemUseable - Remote */
     , (20851,  86,         20) /* MinLevel */
     , (20851,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (20851,   1, True ) /* Stuck */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (20851,  39,     1.2) /* DefaultScale */
     , (20851,  54,       3) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (20851,   1, 'Cottage') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (20851,   1, 0x02000AAF) /* Setup */
     , (20851,   2, 0x090000B8) /* MotionTable */
     , (20851,   8, 0x0600218C) /* Icon */;

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (20851, 16,   1000002,  50, 0, 0, False) /* Create Cabbage (260) for HouseBuy */
     , (20851, 32,   1000002,  25, 0, 0, False) /* Create Pyreal (273) for HouseRent */;
