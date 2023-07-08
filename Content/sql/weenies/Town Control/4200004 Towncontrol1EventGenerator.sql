DELETE FROM `weenie` WHERE `class_Id` = 4200004;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200004, 'towncontrolevent1gen', 10, '2005-02-09 10:00:00') /* creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200004,  81,       1) /* MaxGeneratedObjects */
     , (4200004,  82,       1) /* InitGeneratedObjects */
     , (4200004,  93,    1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200004, 103,       1) /* GeneratorDestructionType - Destroy */
     , (4200004, 142,       3) /* GeneratorTimeType - Event */
     , (4200004, 145,       1) /* GeneratorEndDestructionType - Destroy */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200004,   1, True ) /* Stuck */
     , (4200004,  11, True ) /* IgnoreCollisions */
     , (4200004,  18, True ) /* Visibility */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200004,   1,       5) /* HeartbeatInterval */
     , (4200004,   2,       0) /* HeartbeatTimestamp */
	 , (4200004,  41,       0) /* RegenerationInterval */
     , (4200004,  43,       0) /* GeneratorRadius */
     , (4200004, 121,       0) /* GeneratorInitialDelay */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200004,   1, 'towncontrolevent1') /* Name */
     , (4200004,  34, 'towncontrol1') /* GeneratorEvent */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200004,   1, 0x0200026B) /* Setup */
     , (4200004,   8, 0x06001066) /* Icon */;

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (4200004, -1, 4200008, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Yaraq Town Control Crystal (4200008) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: Ontop */;
