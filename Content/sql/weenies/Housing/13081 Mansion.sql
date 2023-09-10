DELETE FROM `weenie` WHERE `class_Id` = 13081;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (13081, 'slumlordmansion1441-1450', 55, '2021-11-01 00:00:00') /* SlumLord */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (13081,  16,         32) /* ItemUseable - Remote */
     , (13081,  86,         50) /* MinLevel */
     , (13081,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (13081, 163,          6) /* AllegianceMinLevel */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (13081,   1, True ) /* Stuck */
     , (13081,  76, True ) /* HouseRequiresMonarch */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (13081,  39,     1.2) /* DefaultScale */
     , (13081,  54,       3) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (13081,   1, 'Mansion') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (13081,   1, 0x02000AAF) /* Setup */
     , (13081,   2, 0x090000B8) /* MotionTable */
     , (13081,   8, 0x0600218C) /* Icon */;

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (13081, 16,   1000002,  500, 0, 0, False) /* Create Cabbage (260) for HouseBuy */
     , (13081, 32,   1000002,  150, 0, 0, False) /* Create Pyreal (273) for HouseRent */;
