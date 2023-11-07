DELETE FROM `weenie` WHERE `class_Id` = 490020;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490020, 'ace490020-recallgemPKmacro', 38, '2021-11-01 00:00:00') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490020,   1,       2048) /* ItemType - Gem */
     , (490020,   3,         93) /* PaletteTemplate - Amber */
     , (490020,   5,         10) /* EncumbranceVal */
     , (490020,   8,         10) /* Mass */
     , (490020,  11,          1) /* MaxStackSize */
     , (490020,  12,          1) /* StackSize */
     , (490020,  16,          8) /* ItemUseable - Contained */
     , (490020,  18,          1) /* UiEffects - Magical */
     , (490020,  19,          0) /* Value */
     , (490020,  33,          0) /* Bonded - Normal */
     , (490020,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (490020,  94,         16) /* TargetType - Creature */
     , (490020, 106,        210) /* ItemSpellcraft */
     , (490020, 107,         70) /* ItemCurMana */
     , (490020, 108,         70) /* ItemMaxMana */
     , (490020, 109,         10) /* ItemDifficulty */
     , (490020, 114,          0) /* Attuned - Normal */
	 , (490020, 280,        750) /* SharedCooldown */
     , (490020, 369,         200) /* UseRequiresLevel */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490020,  22, True ) /* Inscribable */
     , (490020,  63, True ) /* UnlimitedUse */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (490020,  76,     0.5) /* Translucency */
     , (490020, 167,      1800) /* CooldownDuration */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490020,   1, 'Golems Portal Gem') /* Name */
     , (490020,  14, 'Double Click on this portal gem to transport yourself to the Facility Hub.') /* Use */
     , (490020,  15, 'A gem teeming with portal energy. ') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490020,   1, 0x02000921) /* Setup */
     , (490020,   3, 0x20000014) /* SoundTable */
     , (490020,   6, 0x04000BEF) /* PaletteBase */
     , (490020,   8, 0x06003346) /* Icon */
     , (490020,  22, 0x3400002B) /* PhysicsEffectTable */
     , (490020,  27, 0x10000057) /* UseUserAnimation - Sanctuary */;

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (490020,  7 /* Use */,      1, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  99 /* TeleportTarget */, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);