DELETE FROM `weenie` WHERE `class_Id` = 480630;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480630, 'ace480630-Shoushioutpost', 64, '2022-11-16 03:26:40') /* Hooker */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480630,   1,        128) /* ItemType - Misc */
     , (480630,   3,         20) /* PaletteTemplate - Silver */
     , (480630,   5,       5000) /* EncumbranceVal */
     , (480630,   8,         25) /* Mass */
     , (480630,   9,          0) /* ValidLocations - None */
     , (480630,  16,         32) /* ItemUseable - Remote */
     , (480630,  19,       200) /* Value */
     , (480630,  33,          1) /* Bonded - Bonded */
     , (480630,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480630, 150,        103) /* HookPlacement - Hook */
     , (480630, 151,          9) /* HookType - Floor, Yard */
     , (480630, 197,          4) /* HookGroup */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480630,  13, True ) /* Ethereal */
     , (480630,  22, True ) /* Inscribable */
     , (480630,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480630,  39,       1) /* DefaultScale */
     , (480630,  54,       3) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480630,   1, 'Shoushi Outpost') /* Name */
     , (480630,  14, 'This item can be hooked to the Floor or Yard hooks of mansions. Use this item to be transported to the Shoushi Governor.') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480630,   1, 0x02001A18) /* Setup */
     , (480630,   2, 0x09000001) /* MotionTable */
     , (480630,   3, 0x20000015) /* SoundTable */
     , (480630,   4, 0x30000000) /* CombatTable */
     , (480630,   6, 0x0400007E) /* PaletteBase */
     , (480630,   7, 0x1000086B) /* ClothingBase */
     , (480630,   8, 0x06001036) /* Icon */
     , (480630,   9, 0x0500326F) /* EyesTexture */
     , (480630,  10, 0x0500326A) /* NoseTexture */
     , (480630,  11, 0x0500326B) /* MouthTexture */
     , (480630,  17, 0x04001978) /* SkinPalette */
     , (480630,  18, 0x0100491E) /* HeadObject */
     , (480630,  22, 0x34000004) /* PhysicsEffectTable */;

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (480630,  7 /* Use */,      1, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  99 /* TeleportTarget */, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0xDE510016 /* @teleloc 0x002A034A [52.855202 -248.074997 0.005000] 1.000000 0.000000 0.000000 0.000000 */, 48.076149, 121.470108,  16.005001, 0.003087, 0, 0, -0.999995);

/* Lifestoned Changelog:
{
  "LastModified": "2022-11-15T22:24:08.0849064-05:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "testing",
  "IsDone": false
}
*/
