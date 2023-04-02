DELETE FROM `weenie` WHERE `class_Id` = 480493;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480493, 'PKEvent1gen', 10, '2005-02-09 10:00:00') /* creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480493,  81,       1) /* MaxGeneratedObjects */
     , (480493,  82,       1) /* InitGeneratedObjects */
     , (480493,  93,    1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480493, 103,       1) /* GeneratorDestructionType - Destroy */
     , (480493, 142,       3) /* GeneratorTimeType - Event */
     , (480493, 145,       1) /* GeneratorEndDestructionType - Destroy */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480493,   1, True ) /* Stuck */
     , (480493,  11, True ) /* IgnoreCollisions */
     , (480493,  18, True ) /* Visibility */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480493,   1,       5) /* HeartbeatInterval */
     , (480493,   2,       0) /* HeartbeatTimestamp */
	 , (480493,  41,       0) /* RegenerationInterval */
     , (480493,  43,       0) /* GeneratorRadius */
     , (480493, 121,       0) /* GeneratorInitialDelay */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480493,   1, 'PKEvent1gen') /* Name */
     , (480493,  34, 'PKEvent1') /* GeneratorEvent */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480493,   1, 0x0200026B) /* Setup */
     , (480493,   8, 0x06001066) /* Icon */;

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (480493,  5 /* HeartBeat */,   1, NULL, 0x8000003D /* NonCombat */, 0x41000003 /* Ready */, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES  (@parent_id,  0,  24 /* StopEvent */, 30, 1, NULL, 'PKEvent1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (480493, -1, 480487, 1, 1, 1, 1, 4, 0, 0, 0, 0x00070144, 74.159538, -74.230431, 0.005000, 0.365849, 0.000000, 0.000000, -0.930674) /* Generate Custom Chest (38457)  Location to (re)Generate: Static */;