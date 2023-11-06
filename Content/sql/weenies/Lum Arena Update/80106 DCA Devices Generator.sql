DELETE FROM `weenie` WHERE `class_Id` = 80106;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (80106, 'ace80106-dcadevicesgenerator', 1, '2021-11-01 00:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (80106,  81,          1) /* MaxGeneratedObjects */
     , (80106,  82,          1) /* InitGeneratedObjects */
     , (80106,  93,       1040) /* PhysicsState - IgnoreCollisions, Gravity */
     , (80106, 100,          1) /* GeneratorType - Relative */
     , (80106, 103,          2) /* GeneratorDestructionType - Destroy */
     , (80106, 145,          2) /* GeneratorEndDestructionType - Destroy */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (80106,   1, True ) /* Stuck */
     , (80106,  11, True ) /* IgnoreCollisions */
     , (80106,  13, False) /* Ethereal */
     , (80106,  14, True ) /* GravityStatus */
     , (80106,  24, True ) /* UiHidden */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (80106,  41,      60) /* RegenerationInterval */
     , (80106,  43,       5) /* GeneratorRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (80106,   1, 'DCA Devices Generator') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (80106,   1, 0x020018AB) /* Setup */
     , (80106,   8, 0x06001066) /* Icon */;

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (80106, 0.0588, 80107, 15, 1, 1, 1, 1, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Arena Heal (80107) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: OnTop */
     , (80106, 0.1176, 80108, 15, 1, 1, 1, 1, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Arena Stamina (80108) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: OnTop */
     , (80106, 0.1764, 80109, 15, 1, 1, 1, 1, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Arena Mana (80109) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: OnTop */
     , (80106, 0.2352, 80110, 15, 1, 1, 1, 1, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Arena Max Health (80110) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: OnTop */
     , (80106, 0.294, 80111, 15, 1, 1, 1, 1, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Arena Max Stamina (80111) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: OnTop */
     , (80106, 0.3528, 80112, 15, 1, 1, 1, 1, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Arena Max Mana (80112) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: OnTop */
     , (80106, 0.4116, 80113, 15, 1, 1, 1, 1, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Arena Atlatl (80113) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: OnTop */
     , (80106, 0.4704, 80114, 15, 1, 1, 1, 1, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Arena Axe (80114) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: OnTop */
     , (80106, 0.5292, 80115, 15, 1, 1, 1, 1, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Arena Bow (80115) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: OnTop */
     , (80106, 0.588, 80116, 15, 1, 1, 1, 1, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Arena Claw (80116) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: OnTop */
     , (80106, 0.6468, 80117, 15, 1, 1, 1, 1, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Arena Crossbow (80117) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: OnTop */
     , (80106, 0.7056, 80118, 15, 1, 1, 1, 1, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Arena Dagger (80118) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: OnTop */
     , (80106, 0.7644, 80119, 15, 1, 1, 1, 1, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Arena Mace (80119) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: OnTop */
     , (80106, 0.8232, 80120, 15, 1, 1, 1, 1, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Arena Spear (80120) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: OnTop */
     , (80106, 0.882, 80121, 15, 1, 1, 1, 1, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Arena Staff (80121) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: OnTop */
     , (80106, 0.9408, 80122, 15, 1, 1, 1, 1, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Arena Sword (80122) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: OnTop */
     , (80106, 0.9996, 80123, 15, 1, 1, 1, 1, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Arena Wand (80123) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: OnTop */;
