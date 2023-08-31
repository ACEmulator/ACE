DELETE FROM `weenie` WHERE `class_Id` = 480632;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480632, 'ace480632-yaraqoutpost', 64, '2022-11-16 03:26:40') /* Hooker */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480632,   1,        128) /* ItemType - Misc */
     , (480632,   3,         20) /* PaletteTemplate - Silver */
     , (480632,   5,       5000) /* EncumbranceVal */
     , (480632,   8,         25) /* Mass */
	 , (480632,   9,          0) /* ValidLocations - None */
     , (480632,  16,         32) /* ItemUseable - Remote */
     , (480632,  19,       200) /* Value */
     , (480632,  33,          1) /* Bonded - Bonded */
     , (480632,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480632, 150,        103) /* HookPlacement - Hook */
     , (480632, 151,          9) /* HookType - Floor, Yard */
     , (480632, 197,          1) /* HookGroup */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480632,  13, True ) /* Ethereal */
     , (480632,  22, True ) /* Inscribable */
     , (480632,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480632,  39,       1) /* DefaultScale */
     , (480632,  54,       3) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480632,   1, 'Yaraq Outpost') /* Name */
     , (480632,  14, 'This item can be hooked to the Floor or Yard hooks of mansions. Use this item to be transported to the Holtburg Governor.') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480632,   1, 0x020016EC) /* Setup */
     , (480632,   2, 0x09000009) /* MotionTable */
     , (480632,   3, 0x20000015) /* SoundTable */
     , (480632,   4, 0x30000000) /* CombatTable */
     , (480632,   6, 0x0400007E) /* PaletteBase */
     , (480632,   7, 0x1000086B) /* ClothingBase */
     , (480632,   8, 0x06001036) /* Icon */
     , (480632,   9, 0x0500326F) /* EyesTexture */
     , (480632,  10, 0x0500326A) /* NoseTexture */
     , (480632,  11, 0x0500326B) /* MouthTexture */
     , (480632,  17, 0x04001978) /* SkinPalette */
     , (480632,  18, 0x0100491E) /* HeadObject */
     , (480632,  22, 0x34000004) /* PhysicsEffectTable */;

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (480632,  7 /* Use */,      1, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  99 /* TeleportTarget */, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0x81640006 /* @teleloc 0x002A034A [52.855202 -248.074997 0.005000] 1.000000 0.000000 0.000000 0.000000 */, 0.576489, 143.601151,  0.699269, 0.697868, 0, 0, -0.714859);

/* Lifestoned Changelog:
{
  "LastModified": "2022-11-15T22:24:08.0849064-05:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "testing",
  "IsDone": false
}
*/
