DELETE FROM `weenie` WHERE `class_Id` = 490072;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490072, 'ace490072-martinecloakgen', 1, '2021-11-01 00:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490072,  81,          33) /* MaxGeneratedObjects */
     , (490072,  82,          33) /* InitGeneratedObjects */
     , (490072,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (490072, 103,          2) /* GeneratorDestructionType - Destroy */
     , (490072, 145,          2) /* GeneratorEndDestructionType - Destroy */
     , (490072, 267,        180) /* Lifespan */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490072,   1, True ) /* Stuck */
     , (490072,  11, True ) /* IgnoreCollisions */
     , (490072,  18, True ) /* Visibility */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (490072,  41,      1000) /* RegenerationInterval */
     , (490072,  43,      30) /* GeneratorRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490072,   1, 'Martine''s Cloak Gen') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490072,   1, 0x0200026B) /* Setup */
     , (490072,   8, 0x06001066) /* Icon */;

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (490072, -1, 490028, 0, 9, 9, 1, 2, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Splinter of Anger (51578) (x9 up to max of 9) - Regenerate upon Destruction - Location to (re)Generate: Scatter */
, (490072, -1, 480608, 0, 9, 9, 1, 2, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Splinter of Anger (51578) (x9 up to max of 9) - Regenerate upon Destruction - Location to (re)Generate: Scatter */
, (490072, -1, 2888, 0, 10, 10, 1, 64, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0)
, (490072, -1, 8359, 0, 1, 1, 1, 4, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0);