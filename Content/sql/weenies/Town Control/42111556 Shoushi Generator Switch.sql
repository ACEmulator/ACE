DELETE FROM `weenie` WHERE `class_Id` = 42111556;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (42111556, 'shoushigeneratorswitch', 26, '2005-02-09 10:00:00') /* Switch */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (42111556,   1,        128) /* ItemType - Misc */
     , (42111556,   5,       6660) /* EncumbranceVal */
     , (42111556,   8,        200) /* Mass */
     , (42111556,   9,          0) /* ValidLocations - None */
     , (42111556,  16,         48) /* ItemUseable - ViewedRemote */
     , (42111556,  19,          0) /* Value */
     , (42111556,  81,          5) /* MaxGeneratedObjects */
     , (42111556,  82,          0) /* InitGeneratedObjects */
     , (42111556,  83,      65552) /* ActivationResponse - Talk, Generate */
     , (42111556,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (42111556, 119,          1) /* Active */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (42111556,   1, True ) /* Stuck */
     , (42111556,  11, True ) /* IgnoreCollisions */
     , (42111556,  18, True ) /* Visibility */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (42111556,  41,       0) /* RegenerationInterval */
     , (42111556,  43,     4.5) /* GeneratorRadius */
     , (42111556,  54,     250) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (42111556,   1, 'Shoushi Generator Switch') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (42111556,   1, 0x020018C0) /* Setup */
     , (42111556,   2, 0x09000172) /* MotionTable */
     , (42111556,   3, 0x20000014) /* SoundTable */
     , (42111556,   8, 0x0600106B) /* Icon */;

INSERT INTO `weenie_properties_i_i_d` (`object_Id`, `type`, `value`)
VALUES (42111556,  16, 0x00000000) /* ActivationTarget */;

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (42111556, -1, 452000, 1, 1, 1, 1, 4, 0, 0, 0, 0xDE51000C, 29.453243, 90.639473,  17.550562, -0.684240, 0, 0,  0.729257) /* Generate Shoushi Combat Arena (452000)  Location to (re)Generate: Static */;

