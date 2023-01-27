DELETE FROM `weenie` WHERE `class_Id` = 4200004;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200004, 'yaraqgeneratorswitch', 26, '2005-02-09 10:00:00') /* Switch */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200004,   1,        128) /* ItemType - Misc */
     , (4200004,   5,       6660) /* EncumbranceVal */
     , (4200004,   8,        200) /* Mass */
     , (4200004,   9,          0) /* ValidLocations - None */
     , (4200004,  16,         48) /* ItemUseable - ViewedRemote */
     , (4200004,  19,          0) /* Value */
     , (4200004,  81,          5) /* MaxGeneratedObjects */
     , (4200004,  82,          0) /* InitGeneratedObjects */
     , (4200004,  83,      65552) /* ActivationResponse - Talk, Generate */
     , (4200004,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200004, 119,          1) /* Active */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200004,   1, True ) /* Stuck */
     , (4200004,  11, True ) /* IgnoreCollisions */
     , (4200004,  18, True ) /* Visibility */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200004,  41,       0) /* RegenerationInterval */
     , (4200004,  43,     4.5) /* GeneratorRadius */
     , (4200004,  54,     250) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200004,   1, 'Yaraq Generator Switch') /* Name */
     , (4200004,  17, 'A TOWN CONTROL CRYSTAL HAS SPAWNED!') /* ActivationTalk */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200004,   1, 0x0200026B) /* Setup */
     , (4200004,   8, 0x06001066) /* Icon */;

INSERT INTO `weenie_properties_i_i_d` (`object_Id`, `type`, `value`)
VALUES (4200004,  16, 0x00000000) /* ActivationTarget */;

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (4200004, -1, 4200008, 0, 1, 1, 1, 2, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Yaraq Town Control Crystal (4200008) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: Scatter */;

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (4200004, -1, 4200015, 1, 1, 1, 1, 4, 0, 0, 0, 0x81640005, 20.041307, 107.961784, 30.004999, -0.708804, 0, 0, -0.705405) /* Generate Custom Chest (38457)  Location to (re)Generate: Static */;

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (4200004, -1, 4200015, 2, 1, 1, 1, 4, 0, 0, 0, 0x81640005, 20.041307, 105.961784, 30.004999, -0.708804, 0, 0, -0.705405) /* Generate Custom Chest (38457)  Location to (re)Generate: Static */;

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (4200004, -1, 4200015, 3, 1, 1, 1, 4, 0, 0, 0, 0x81640005, 20.041307, 109.961784, 30.004999, -0.708804, 0, 0, -0.705405) /* Generate Custom Chest (38457)  Location to (re)Generate: Static */;
