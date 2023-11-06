DELETE FROM `weenie` WHERE `class_Id` = 490019;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490019, 'ace490019-SVdevice', 64, '2022-11-16 03:26:40') /* Hooker */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490019,   1,        128) /* ItemType - Misc */
     , (490019,   3,         20) /* PaletteTemplate - Silver */
     , (490019,   5,       5000) /* EncumbranceVal */
     , (490019,   8,         25) /* Mass */
     , (490019,   9,          0) /* ValidLocations - None */
     , (490019,  16,         32) /* ItemUseable - Remote */
     , (490019,  19,       200) /* Value */
     , (490019,  33,          1) /* Bonded - Bonded */
     , (490019,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (490019, 150,        103) /* HookPlacement - Hook */
     , (490019, 151,          9) /* HookType - Floor, Yard */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490019,  13, True ) /* Ethereal */
     , (490019,  22, True ) /* Inscribable */
     , (490019,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (490019,  39,       1) /* DefaultScale */
     , (490019,  54,       3) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490019,   0.5, 'Snowy Valley') /* Name */
     , (490019,  14, 'This item can be hooked to the Floor or Yard hooks of mansions. Use this item to be transported to Snowy Valley.') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490019,   1, 0x02001502) /* Setup */
     , (490019,   2, 0x0900019A) /* MotionTable */
     , (490019,   8, 0x06006591) /* Icon */
     , (490019,  22, 0x34000072) /* PhysicsEffectTable */;

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (490019,  7 /* Use */,      1, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  99 /* TeleportTarget */, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL,   0x7AE80001 /* @teleloc 0x002A034A [52.855202 -248.074997 0.005000] 1.000000 0.000000 0.000000 0.000000 */, 4.609425, 19.947752,  120.005005, -0.676059, 0, 0,   0.736848);

/* Lifestoned Changelog:
{
  "LastModified": "2022-11-15T22:24:08.0849064-05:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "testing",
  "IsDone": false
}
*/
