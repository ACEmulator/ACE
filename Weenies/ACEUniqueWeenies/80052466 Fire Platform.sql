DELETE FROM `weenie` WHERE `class_Id` = 80052466;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (80052466, 'Fireplatform', 13, '2021-11-17 16:56:08') /* HotSpot */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (80052466,   1,        128) /* ItemType - Misc */
     , (80052466,   5,          1) /* EncumbranceVal */
     , (80052466,  16,          1) /* ItemUseable - No */
     , (80052466,  19,          1) /* Value */
     , (80052466,  44,        150) /* Damage */
     , (80052466,  81,          5) /* MaxGeneratedObjects */
     , (80052466,  82,          1) /* InitGeneratedObjects */
     , (80052466,  45,         16) /* DamageType - Fire */
     , (80052466,  93,         12) /* PhysicsState - Ethereal, ReportCollisions */
     , (80052466, 119,          0) /* Active */
     , (80052466, 267,         8) /* Lifespan */
     , (80052466, 100,          1) /* GeneratorType - Relative */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (80052466,   1, True ) /* Stuck */
     , (80052466,  11, False) /* IgnoreCollisions */
     , (80052466,  12, True ) /* ReportCollisions */
     , (80052466,  13, True ) /* Ethereal */
     , (80052466,  14, False) /* GravityStatus */
     , (80052466,  24, True ) /* UiHidden */
     , (80052466,  55, True ) /* IsHot */
     , (80052466,  57, False) /* AffectsAis */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (80052466,  22,    0.15) /* DamageVariance */
     , (80052466,  39,       .7) /* DefaultScale */
     , (80052466,  76,     1) /* Translucency */
     , (80052466, 105,       2) /* HotspotCycleTime */
     , (80052466, 106,     0.2) /* HotspotCycleTimeVariance */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (80052466,   1, 'Fire Platform') /* Name */
     , (80052466,  17, 'You suffer %i damage from the searing flames!') /* ActivationTalk */;

SET @parent_id = LAST_INSERT_ID();


-- INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
-- VALUES (@parent_id,  2,  72 /* Generate */, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

-- INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
-- VALUES (44034,  2 /* Death */,    1, NULL, NULL, NULL, NULL, NULL, NULL, NULL);


-- INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
-- VALUES (80052466, 1, 21221, 0, 1, 1, 1, 1, 3, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Fire Cloud*/;


INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (80052466,   1, 0x020018C5) /* Setup */
     , (80052466,   3, 0x20000052) /* SoundTable */
     , (80052466,   8, 0x06001049) /* Icon */;
