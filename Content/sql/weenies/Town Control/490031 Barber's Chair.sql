DELETE FROM `weenie` WHERE `class_Id` = 490031;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490031, 'hookerbarberchair', 64, '2005-02-09 10:00:00') /* Hooker */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490031,   1,        128) /* ItemType - Misc */
     , (490031,   3,         82) /* PaletteTemplate - PinkPurple */
     , (490031,   5,        150) /* EncumbranceVal */
     , (490031,   8,         25) /* Mass */
     , (490031,   9,          0) /* ValidLocations - None */
     , (490031,  16,         32) /* ItemUseable - Remote */
     , (490031,  19,       100) /* Value */
     , (490031,  33,          1) /* Bonded - Bonded */
     , (490031,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (490031, 150,        103) /* HookPlacement - Hook */
     , (490031, 151,          9) /* HookType - Wall */
	 , (490031, 197,         16) /* HookGroup - SpellCastingItems */
	 , (490031, 280,       3) /* SharedCooldown */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490031,  13, True ) /* Ethereal */
     , (490031,  22, True ) /* Inscribable */
     , (490031,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (490031,  12,     0.5) /* Shade */
     , (490031,  39,     1.5) /* DefaultScale */
     , (490031,  54,       3) /* UseRadius */
	 , (490031, 167,      15) /* CooldownDuration */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490031,   1, 'Barber''s Chair') /* Name */
     , (490031,  15, 'Use this item to get open the Barber. This item can be used on an item hook.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490031,   1, 0x02000121) /* Setup */
     , (490031,   8, 0x0600211A) /* Icon */;

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (490031,  7 /* Use */,      1, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0, 101 /* StartBarber */, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);