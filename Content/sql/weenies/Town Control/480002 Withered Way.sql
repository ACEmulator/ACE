DELETE FROM `weenie` WHERE `class_Id` = 480002;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480002, 'ace480002-withered way', 64, '2022-11-16 03:10:06') /* Hooker */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480002,   1,        128) /* ItemType - Misc */
     , (480002,   3,          5) /* PaletteTemplate - DarkBlue */
     , (480002,   5,       5000) /* EncumbranceVal */
     , (480002,   8,         25) /* Mass */
     , (480002,   9,          0) /* ValidLocations - None */
     , (480002,  16,         32) /* ItemUseable - Remote */
     , (480002,  19,       250) /* Value */
     , (480002,  33,          1) /* Bonded - Bonded */
     , (480002,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480002, 150,        103) /* HookPlacement - Hook */
     , (480002, 151,          9) /* HookType - Floor, Yard */
     , (480002, 197,          4) /* HookGroup - PortalItems */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480002,  13, True ) /* Ethereal */
     , (480002,  22, True ) /* Inscribable */
     , (480002,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480002,  39,     1.5) /* DefaultScale */
     , (480002,  54,       3) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480002,   1, 'Withered Way') /* Name */
     , (480002,  14, 'This item can be hooked to the Floor or Yard hooks of mansions. Use this item to be transported to Withered.') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480002,   1, 0x02000E08) /* Setup */
     , (480002,   2, 0x09000007) /* MotionTable */
     , (480002,   3, 0x20000005) /* SoundTable */
     , (480002,   4, 0x30000002) /* CombatTable */
     , (480002,   6, 0x04001425) /* PaletteBase */
     , (480002,   7, 0x10000483) /* ClothingBase */
     , (480002,   8, 0x0600103D) /* Icon */
     , (480002,  22, 0x34000017) /* PhysicsEffectTable */;

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (480002,  7 /* Use */,      1, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  99 /* TeleportTarget */, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0x1B120015 /* @teleloc 0x1B120015 [59.413898 115.663002 -0.895000] -0.959528 0.000000 0.000000 0.281613 */, 59.4139, 115.663, -0.895, -0.959528, 0, 0, 0.281613);
