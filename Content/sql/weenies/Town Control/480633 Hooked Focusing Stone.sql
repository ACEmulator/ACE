DELETE FROM `weenie` WHERE `class_Id` = 480633;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480633, 'hookerfocusingstone', 64, '2005-02-09 10:00:00') /* Hooker */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480633,   1,        128) /* ItemType - Misc */
     , (480633,   3,         14) /* PaletteTemplate - Red */
     , (480633,   5,        150) /* EncumbranceVal */
     , (480633,   8,         25) /* Mass */
     , (480633,   9,          0) /* ValidLocations - None */
     , (480633,  16,         32) /* ItemUseable - Remote */
     , (480633,  19,       250) /* Value */
     , (480633,  33,          1) /* Bonded - Bonded */
     , (480633,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480633, 150,        103) /* HookPlacement - Hook */
     , (480633, 151,          2) /* HookType - Wall */
	 , (480633, 197,         16) /* HookGroup - SpellCastingItems */
	 , (480633, 280,       3) /* SharedCooldown */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480633,  15, True ) /* LightsStatus */
     , (480633,  13, True ) /* Ethereal */
     , (480633,  22, True ) /* Inscribable */
     , (480633,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480633,  39,     0.7) /* DefaultScale */
     , (480633,  54,       3) /* UseRadius */
	 , (480633, 167,      5) /* CooldownDuration */
	 , (480633,  76,     0.2) /* Translucency */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480633,   1, 'Focusing Stone') /* Name */
     , (480633,  14, 'This item can be placed on wall hooks, where it can be used to cast Brilliance') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480633,   1, 0x020009F0) /* Setup */
     , (480633,   3, 0x20000014) /* SoundTable */
     , (480633,   6, 0x04000BF8) /* PaletteBase */
     , (480633,   7, 0x10000249) /* ClothingBase */
     , (480633,   8, 0x06001F8E) /* Icon */
     , (480633,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480633,  27, 0x400000E1) /* UseUserAnimation - UseMagicWand */
     , (480633,  28,       2348) /* Spell - Brilliance */
     , (480633,  36, 0x0E000016) /* MutateFilter */;

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (480633,  7 /* Use */,      1, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  19 /* CastSpellInstant */, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 2348 /* Inky Armor */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);