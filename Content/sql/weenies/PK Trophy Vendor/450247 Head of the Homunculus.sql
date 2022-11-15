DELETE FROM `weenie` WHERE `class_Id` = 450247;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450247, 'orbhomunculustailor', 35, '2005-02-09 10:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450247,   1,      32768) /* ItemType - Caster */
     , (450247,   5,        0) /* EncumbranceVal */
     , (450247,   8,        800) /* Mass */
     , (450247,   9,   16777216) /* ValidLocations - Held */
     , (450247,  16,    6291464) /* ItemUseable - SourceContainedTargetRemoteNeverWalk */
     , (450247,  18,          1) /* UiEffects - Magical */
     , (450247,  19,       20) /* Value */
     , (450247,  46,        512) /* DefaultCombatStyle - Magic */
     , (450247,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450247,  94,         16) /* TargetType - Creature */
     , (450247, 150,        103) /* HookPlacement - Hook */
     , (450247, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450247,  22, True ) /* Inscribable */
     , (450247,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450247,   5,  -0.033) /* ManaRate */
     , (450247,  29,       1) /* WeaponDefense */
     , (450247,  39,       1) /* DefaultScale */
     , (450247, 144,    0.06) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450247,   1, 'Head of the Homunculus') /* Name */
     , (450247,  14, 'This item can be hooked on wall hooks. Your War Magic will be tested if you attempt to use an Idol Gem on the head.') /* Use */
     , (450247,  16, 'A small stone head. There are two small indentations where its eyes were removed.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450247,   1, 0x020010D8) /* Setup */
     , (450247,   3, 0x20000014) /* SoundTable */
     , (450247,   8, 0x060033B5) /* Icon */
     , (450247,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450247,  27, 0x400000E1) /* UseUserAnimation - UseMagicWand */;


INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (450247, 25 /* Wield */,   0.05, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  18 /* DirectBroadcast */, 0, 1, NULL, 'A voice seems to whisper in your ear, "Find my eyes that I may look upon you in favor."', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);
