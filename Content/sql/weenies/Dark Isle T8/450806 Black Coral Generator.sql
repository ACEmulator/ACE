DELETE FROM `weenie` WHERE `class_Id` = 450806;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450806, 'ace450806-blackcoralgenerator', 1, '2021-11-01 00:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450806,  81,          10) /* MaxGeneratedObjects */
     , (450806,  82,          10) /* InitGeneratedObjects */
     , (450806,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450806,   1, True ) /* Stuck */
     , (450806,  11, True ) /* IgnoreCollisions */
     , (450806,  18, True ) /* Visibility */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450806,  41,     10) /* RegenerationInterval */
     , (450806,  43,      18) /* GeneratorRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450806,   1, 'Black Coral Generator') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450806,   1, 0x0200026B) /* Setup */
     , (450806,   8, 0x06001066) /* Icon */;

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (450806, 1, 38613, 10, 1, 10, 1, 2, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Blighted Coral Golem Generator (70080) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: Scatter */;
