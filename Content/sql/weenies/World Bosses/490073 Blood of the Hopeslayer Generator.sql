DELETE FROM `weenie` WHERE `class_Id` = 490073;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490073, 'ace490073-bloodofthehopeslayergen', 1, '2021-11-01 00:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490073,  81,          33) /* MaxGeneratedObjects */
     , (490073,  82,          33) /* InitGeneratedObjects */
     , (490073,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (490073, 103,          2) /* GeneratorDestructionType - Destroy */
     , (490073, 145,          2) /* GeneratorEndDestructionType - Destroy */
     , (490073, 267,        180) /* Lifespan */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490073,   1, True ) /* Stuck */
     , (490073,  11, True ) /* IgnoreCollisions */
     , (490073,  18, True ) /* Visibility */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (490073,  41,      1000) /* RegenerationInterval */
     , (490073,  43,      30) /* GeneratorRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490073,   1, 'Martine''s Cloak Gen') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490073,   1, 0x0200026B) /* Setup */
     , (490073,   8, 0x06001066) /* Icon */;

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (490073, -1, 490029, 0, 9, 9, 1, 2, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Splinter of Anger (51578) (x9 up to max of 9) - Regenerate upon Destruction - Location to (re)Generate: Scatter */
, (490073, -1, 480608, 0, 9, 9, 1, 2, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Splinter of Anger (51578) (x9 up to max of 9) - Regenerate upon Destruction - Location to (re)Generate: Scatter */
, (490073, -1, 2888, 0, 10, 10, 1, 64, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0)
, (490073, -1, 8359, 0, 1, 1, 1, 4, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0);