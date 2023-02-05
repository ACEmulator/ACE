DELETE FROM `weenie` WHERE `class_Id` = 480001;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480001, 'ace480001-kingsspassage', 64, '2022-11-16 03:10:06') /* Hooker */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480001,   1,        128) /* ItemType - Misc */
     , (480001,   3,          5) /* PaletteTemplate - DarkBlue */
     , (480001,   5,       5000) /* EncumbranceVal */
     , (480001,   8,         25) /* Mass */
     , (480001,   9,          0) /* ValidLocations - None */
     , (480001,  16,         32) /* ItemUseable - Remote */
     , (480001,  19,       1000) /* Value */
     , (480001,  33,          1) /* Bonded - Bonded */
     , (480001,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480001, 150,        103) /* HookPlacement - Hook */
     , (480001, 151,          9) /* HookType - Floor, Yard */
     , (480001, 197,          4) /* HookGroup */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480001,  13, True ) /* Ethereal */
     , (480001,  22, True ) /* Inscribable */
     , (480001,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480001,  39,       1.5) /* DefaultScale */
     , (480001,  54,       3) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480001,   1, 'King''s Passage') /* Name */
     , (480001,  14, 'This item can be hooked to the Floor or Yard hooks of mansions. Use this item to be transported into the Creepy Canyon.') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480001,   1, 0x02000964) /* Setup */
     , (480001,   2, 0x090000A7) /* MotionTable */
     , (480001,   6, 0x040010AF) /* PaletteBase */
     , (480001,   7, 0x100002A3) /* ClothingBase */
     , (480001,   8, 0x06001033) /* Icon */
     , (480001,  22, 0x34000027) /* PhysicsEffectTable */
     , (480001,  36, 0x0E000016) /* MutateFilter */;
	 
INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (480001,  7 /* Use */,      1, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  99 /* TeleportTarget */, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0x0F850022 /* @teleloc 0x002B0371 [458.535004 -172.203003 0.005000] 0.933008 0.000000 0.000000 -0.359856 */, 113.716316, 35.805218,  15.022646, -0.279298, 0, 0, 0.960204);


