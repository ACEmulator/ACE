DELETE FROM `weenie` WHERE `class_Id` = 11716;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (11716, 'slumlordmansionmoderate', 55, '2005-02-09 10:00:00') /* SlumLord */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (11716,  16,         32) /* ItemUseable - Remote */
     , (11716,  86,         50) /* MinLevel */
     , (11716,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (11716, 149,          0) /* HouseStatus - InActive */
     , (11716, 163,          6) /* AllegianceMinLevel */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (11716,   1, True ) /* Stuck */
     , (11716,  76, True ) /* HouseRequiresMonarch */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (11716,  39,     1.2) /* DefaultScale */
     , (11716,  54,       3) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (11716,   1, 'Mansion') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (11716,   1, 0x02000AAF) /* Setup */
     , (11716,   2, 0x090000B8) /* MotionTable */
     , (11716,   8, 0x0600218C) /* Icon */;

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (11716, 16,   1000002,  500, 0, 0, False) /* Create Cabbage (260) for HouseBuy */
     , (11716, 32,   1000002,  150, 0, 0, False) /* Create Pyreal (273) for HouseRent */;
