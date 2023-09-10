DELETE FROM `weenie` WHERE `class_Id` = 13078;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (13078, 'slumlordcottage1151-1275', 55, '2005-02-09 10:00:00') /* SlumLord */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (13078,  16,         32) /* ItemUseable - Remote */
     , (13078,  86,         20) /* MinLevel */
     , (13078,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (13078,   1, True ) /* Stuck */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (13078,  39,     1.2) /* DefaultScale */
     , (13078,  54,       3) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (13078,   1, 'Cottage') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (13078,   1, 0x02000AAF) /* Setup */
     , (13078,   2, 0x090000B8) /* MotionTable */
     , (13078,   8, 0x0600218C) /* Icon */;

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (13078, 16,   1000002,  50, 0, 0, False) /* Create Cabbage (260) for HouseBuy */
     , (13078, 32,   1000002,  25, 0, 0, False) /* Create Pyreal (273) for HouseRent */;
