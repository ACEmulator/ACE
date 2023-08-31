DELETE FROM `weenie` WHERE `class_Id` = 480637;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480637, 'hookedvirindiconsulessence', 64, '2005-02-09 10:00:00') /* Hooker */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480637,   1,        128) /* ItemType - Misc */
     , (480637,   3,         14) /* PaletteTemplate - Red */
     , (480637,   5,        150) /* EncumbranceVal */
     , (480637,   8,         25) /* Mass */
     , (480637,   9,          0) /* ValidLocations - None */
     , (480637,  16,         32) /* ItemUseable - Remote */
     , (480637,  19,       250) /* Value */
     , (480637,  33,          1) /* Bonded - Bonded */
     , (480637,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480637, 150,        103) /* HookPlacement - Hook */
     , (480637, 151,          2) /* HookType - Wall */
	 , (480637, 197,         16) /* HookGroup - SpellCastingItems */
	 , (480637, 280,       3) /* SharedCooldown */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480637,  15, True ) /* LightsStatus */
     , (480637,  13, True ) /* Ethereal */
     , (480637,  22, True ) /* Inscribable */
     , (480637,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480637,  39,     2) /* DefaultScale */
     , (480637,  54,       3) /* UseRadius */
	 , (480637, 167,      10) /* CooldownDuration */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480637,   1, 'Virindi Consul Essence') /* Name */
     , (480637,  14, 'This item can be placed on wall hooks, where it can be used to cast Virindi Whisper V') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480637,   1, 0x02000EBA) /* Setup */
     , (480637,   3, 0x20000014) /* SoundTable */
     , (480637,   6, 0x040014B4) /* PaletteBase */
     , (480637,   7, 0x1000010B) /* ClothingBase */
     , (480637,   8, 0x06006B39) /* Icon */
     , (480637,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480637,  28,       5156) /* Spell - Virindi Whisper V */;

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (480637,  7 /* Use */,      1, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  19 /* CastSpellInstant */, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 5156 /* Inky Armor */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);