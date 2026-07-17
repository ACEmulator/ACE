DELETE FROM `weenie` WHERE `class_Id` = 90004172;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (90004172, 'fireandacidgen2', 1, '2005-02-09 10:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (90004172,  81,          2) /* MaxGeneratedObjects */
     , (90004172,  82,          2) /* InitGeneratedObjects */
     , (90004172,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (90004172,   1, True ) /* Stuck */
     , (90004172,  11, True ) /* IgnoreCollisions */
     , (90004172,  18, True ) /* Visibility */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (90004172,  41,      60) /* RegenerationInterval */
     , (90004172,  43,       3) /* GeneratorRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (90004172,   1, 'Drudge Camp Generator') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (90004172,   1, 0x0200026B) /* Setup */
     , (90004172,   8, 0x06001066) /* Icon */;

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES


     -- (90004172, 0.1, 21221, 1800, 1, -1, 1, 4, -1, 0, 0, 0, 2.4, 2.4, 0, 1, 0, 0, 0) /* Generate Drudge Skulker (7) (x1 up to max of -1) - Regenerate upon Destruction - Location to (re)Generate: Specific */
     -- , (90004172, 0.1, 80052466, 1800, 1, -1, 1, 4, -1, 0, 0, 0, 2.4, 2.4, 0, 1, 0, 0, 0) /* Generate Drudge Slinker (193) (x1 up to max of -1) - Regenerate upon Destruction - Location to (re)Generate: Specific */
     -- , (90004172, 0.1, 80052466, 1800, 1, -1, 1, 4, -1, 0, 0, 0, 2.4, 2.4, 0, 1, 0, 0, 0) /* Generate Drudge Sneaker (940) (x1 up to max of -1) - Regenerate upon Destruction - Location to (re)Generate: Specific */
     (90004172, -1, 80052466, 1800, 1, -1, 1, 4, -1, 0, 0, 0, 2.4, 2.4, 0, 1, 0, 0, 0) /* Generate Drudge Prowler (192) (x1 up to max of -1) - Regenerate upon Destruction - Location to (re)Generate: Specific */
     , (90004172, -1, 900021221, 1800, 1, -1, 1, 4, -1, 0, 0, 0, 2.4, 2.4, 0, 1, 0, 0, 0) /* Generate Bonfire (4179) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: Specific */;
