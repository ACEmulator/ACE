DELETE FROM `weenie` WHERE `class_Id` = 4200002;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200002, 'holtburggeneratorswitch', 26, '2005-02-09 10:00:00') /* Switch */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200002,   1,        128) /* ItemType - Misc */
     , (4200002,   5,       6660) /* EncumbranceVal */
     , (4200002,   8,        200) /* Mass */
     , (4200002,   9,          0) /* ValidLocations - None */
     , (4200002,  16,         48) /* ItemUseable - ViewedRemote */
     , (4200002,  19,          0) /* Value */
     , (4200002,  81,          5) /* MaxGeneratedObjects */
     , (4200002,  82,          0) /* InitGeneratedObjects */
     , (4200002,  83,      65552) /* ActivationResponse - Talk, Generate */
     , (4200002,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200002, 119,          1) /* Active */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200002,   1, True ) /* Stuck */
     , (4200002,  11, True ) /* IgnoreCollisions */
     , (4200002,  18, True ) /* Visibility */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200002,  41,       0) /* RegenerationInterval */
     , (4200002,  43,     4.5) /* GeneratorRadius */
     , (4200002,  54,     250) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200002,   1, 'Holtburg Generator Switch') /* Name */
     , (4200002,  17, 'A TOWN CONTROL CRYSTAL HAS SPAWNED!') /* ActivationTalk */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200002,   1, 0x0200026B) /* Setup */
     , (4200002,   8, 0x06001066) /* Icon */;

INSERT INTO `weenie_properties_i_i_d` (`object_Id`, `type`, `value`)
VALUES (4200002,  16, 0x00000000) /* ActivationTarget */;

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (4200002, -1, 4200007, 0, 1, 1, 1, 2, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Holtburg Town Control Crystal (4200007) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: Scatter */;

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (4200002, -1, 4200015, 1, 1, 1, 1, 4, 0, 0, 0, 0xA5B4002B, 133.286011, 53.899799, 52.000000, 1, 0, 0, 0) /* Generate Custom Chest (38457)  Location to (re)Generate: Static */;

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (4200002, -1, 4200015, 2, 1, 1, 1, 4, 0, 0, 0, 0xA5B4002B, 131.286011, 53.899799, 52.000000, 1, 0, 0, 0) /* Generate Custom Chest (38457)  Location to (re)Generate: Static */;

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (4200002, -1, 4200015, 3, 1, 1, 1, 4, 0, 0, 0, 0xA5B4002B, 135.286011, 53.899799, 52.000000, 1, 0, 0, 0) /* Generate Custom Chest (38457)  Location to (re)Generate: Static */;

