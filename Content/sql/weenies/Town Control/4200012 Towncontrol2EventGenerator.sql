DELETE FROM `weenie` WHERE `class_Id` = 4200012;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200012, 'towncontrolevent2gen', 10, '2005-02-09 10:00:00') /* creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200012,  81,       1) /* MaxGeneratedObjects */
     , (4200012,  82,       1) /* InitGeneratedObjects */
     , (4200012,  93,    1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200012, 103,       1) /* GeneratorDestructionType - Nothing */
     , (4200012, 142,       3) /* GeneratorTimeType - Event */
     , (4200012, 145,       1) /* GeneratorEndDestructionType - Nothing */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200012,   1, True ) /* Stuck */
     , (4200012,  11, True ) /* IgnoreCollisions */
     , (4200012,  18, True ) /* Visibility */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200012,   1,       5) /* HeartbeatInterval */
     , (4200012,   2,       0) /* HeartbeatTimestamp */
	 , (4200012,  41,       0) /* RegenerationInterval */
     , (4200012,  43,       0) /* GeneratorRadius */
     , (4200012, 121,       0) /* GeneratorInitialDelay */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200012,   1, 'towncontrol2') /* Name */
     , (4200012,  34, 'towncontrol2') /* GeneratorEvent */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200012,   1, 0x0200026B) /* Setup */
     , (4200012,   8, 0x06001066) /* Icon */;

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (4200012, -1, 4200007, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Holtburg Town Control Crystal (4200007) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: Ontop */;
