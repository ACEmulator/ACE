DELETE FROM `weenie` WHERE `class_Id` = 4200005;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200005, 'towncontrolevent3gen', 10, '2005-02-09 10:00:00') /* creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200005,  81,       2) /* MaxGeneratedObjects */
     , (4200005,  82,       2) /* InitGeneratedObjects */
     , (4200005,  93,    1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200005, 103,       2) /* GeneratorDestructionType - Destroy */
     , (4200005, 142,       3) /* GeneratorTimeType - Event */
     , (4200005, 145,       2) /* GeneratorEndDestructionType - Destroy */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200005,   1, True ) /* Stuck */
     , (4200005,  11, True ) /* IgnoreCollisions */
     , (4200005,  18, True ) /* Visibility */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200005,   1,       5) /* HeartbeatInterval */
     , (4200005,   2,       0) /* HeartbeatTimestamp */
	 , (4200005,  41,       0) /* RegenerationInterval */
     , (4200005,  43,       0) /* GeneratorRadius */
     , (4200005, 121,       0) /* GeneratorInitialDelay */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200005,   1, 'towncontrolevent3') /* Name */
     , (4200005,  34, 'Towncontrol3') /* GeneratorEvent */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200005,   1, 0x0200026B) /* Setup */
     , (4200005,   8, 0x06001066) /* Icon */;

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (4200005,  5 /* HeartBeat */,   1, NULL, 0x8000003D /* NonCombat */, 0x41000003 /* Ready */, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES  (@parent_id,  0,  24 /* StopEvent */, 915, 1, NULL, 'Towncontrol3', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (4200005, -1, 42132032, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Yaraq Town Control Crystal (4200008) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: Ontop */;
