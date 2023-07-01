DELETE FROM `weenie` WHERE `class_Id` = 480600;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480600, 'tcgemportalenlightenment', 38, '2005-02-09 10:00:00') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480600,   1,       2048) /* ItemType - Gem */
     , (480600,   3,         14) /* PaletteTemplate - Red */
     , (480600,   5,          5) /* EncumbranceVal */
     , (480600,   8,          5) /* Mass */
     , (480600,   9,          0) /* ValidLocations - None */
     , (480600,  16,          8) /* ItemUseable - Contained */
     , (480600,  18,          1) /* UiEffects - Magical */
     , (480600,  19,          20) /* Value */
     , (480600,  33,          0) /* Bonded - Normal */
     , (480600,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (480600,  94,         16) /* TargetType - Creature */
     , (480600, 106,        210) /* ItemSpellcraft */
     , (480600, 107,         70) /* ItemCurMana */
     , (480600, 108,         70) /* ItemMaxMana */
     , (480600, 109,         40) /* ItemDifficulty */
     , (480600, 110,          0) /* ItemAllegianceRankLimit */
     , (480600, 114,          0) /* Attuned - Normal */
     , (480600, 150,        103) /* HookPlacement - Hook */
     , (480600, 151,          2) /* HookType - Wall */
	 , (480600, 369,         275) /* UseRequiresLevel */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480600,  15, True ) /* LightsStatus */
     , (480600,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480600,  76,     0.5) /* Translucency */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480600,   1, 'Temple of the Font Sending Gem') /* Name */
     , (480600,  14, 'Double Click on this gem to be sent to the Temple of the Font.') /* Use */
     , (480600,  15, 'A glowing red gem.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480600,   1, 0x02000921) /* Setup */
     , (480600,   3, 0x20000014) /* SoundTable */
     , (480600,   6, 0x04000BEF) /* PaletteBase */
     , (480600,   7, 0x1000010B) /* ClothingBase */
     , (480600,   8, 0x06002370) /* Icon */
     , (480600,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480600,  36, 0x0E000016) /* MutateFilter */;

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (480600,  7 /* Use */,      1, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  99 /* TeleportTarget */, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL,  0x596B012C /* @teleloc 0x002B0371 [458.535004 -172.203003 0.005000] 0.933008 0.000000 0.000000 -0.359856 */, 9.959870,  -130.005005,   6.00500,  1.000000, 0, 0, 0);


