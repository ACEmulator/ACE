DELETE FROM `weenie` WHERE `class_Id` = 8800387;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (8800387, 'magic-lugian-generator', 1, '2005-02-09 10:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (8800387,  81,          3) /* MaxGeneratedObjects */
     , (8800387,  82,          3) /* InitGeneratedObjects */
     , (8800387,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (8800387,   1, True ) /* Stuck */
     , (8800387,  11, True ) /* IgnoreCollisions */
     , (8800387,  18, True ) /* Visibility */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (8800387,  41,      60) /* RegenerationInterval */
     , (8800387,  43,       2) /* GeneratorRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (8800387,   1, 'Lugian Generator') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (8800387,   1, 0x0200026B) /* Setup */
     , (8800387,   8, 0x06001066) /* Icon */;



INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES

     (8800387, 0.4, 910000004, 1800,  1, 1, 1, 4, -1, 0, 0, 0, -2, -1, 0, 0.819152, 0, 0, 0.573577) /* Generate Gotrok Tiatus (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: Specific */
     , (8800387, 0.75, 910000005, 1800, 1, 1, 1, 4, -1, 0, 0, 0, 2.4, 3.4, 0, 0.965926, 0, 0, 0.258819) /* Generate Gotrok Montok (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: Specific */
     , (8800387, 0.85, 910000003, 1800,1, 1, 1, 4, -1, 0, 0, 0, 2.4, -1.4, 0, 0.996195, 0, 0, 0.087156) /* Generate Gotrok Extas (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: Specific */
     , (8800387, 0.95, 910000002, 1800, 1, 1, 1, 4, -1, 0, 0, 0, -2, -2, 0, 0.996195, 0, 0, 0.087156) /* Generate Extas Raider (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: Specific */
     -- , (8800387, 1, 8999, 1800, 1, 1, 1, 4, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Steel Chest (8999) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: Specific */
     ;
;
