DELETE FROM `weenie` WHERE `class_Id` = 4200006;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200006, 'yaraqgeneratorswitch', 26, '2005-02-09 10:00:00') /* Switch */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200006,   1,        128) /* ItemType - Misc */
     , (4200006,   5,       6660) /* EncumbranceVal */
     , (4200006,   8,        200) /* Mass */
     , (4200006,   9,          0) /* ValidLocations - None */
     , (4200006,  16,         48) /* ItemUseable - ViewedRemote */
     , (4200006,  19,          0) /* Value */
     , (4200006,  81,          5) /* MaxGeneratedObjects */
     , (4200006,  82,          0) /* InitGeneratedObjects */
     , (4200006,  83,      65552) /* ActivationResponse - Talk, Generate */
     , (4200006,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200006, 119,          1) /* Active */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200006,   1, True ) /* Stuck */
     , (4200006,  11, True ) /* IgnoreCollisions */
     , (4200006,  18, True ) /* Visibility */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200006,  41,       0) /* RegenerationInterval */
     , (4200006,  43,     4.5) /* GeneratorRadius */
     , (4200006,  54,     250) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200006,   1, 'Yaraq Generator Switch') /* Name */
     , (4200006,  17, 'A TOWN CONTROL CRYSTAL HAS SPAWNED!') /* ActivationTalk */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200006,   1, 0x020018C0) /* Setup */
     , (4200006,   2, 0x09000172) /* MotionTable */
     , (4200006,   3, 0x20000014) /* SoundTable */
     , (4200006,   8, 0x0600106B) /* Icon */;

INSERT INTO `weenie_properties_i_i_d` (`object_Id`, `type`, `value`)
VALUES (4200006,  16, 0x00000000) /* ActivationTarget */;

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (4200006, -1, 450700, 1, 1, 1, 1, 4, 0, 0, 0, 0x81640010, 35.630840,  168.191620,  23.035761, 0.018690, 0, 0,  -0.999825) /* Generate Custom Chest (38457)  Location to (re)Generate: Static */;
