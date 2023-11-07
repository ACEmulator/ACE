DELETE FROM `weenie` WHERE `class_Id` = 490018;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490018, 'ace490018-darkisledevice', 64, '2022-11-16 03:26:40') /* Hooker */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490018,   1,        128) /* ItemType - Misc */
     , (490018,   3,         20) /* PaletteTemplate - Silver */
     , (490018,   5,       5000) /* EncumbranceVal */
     , (490018,   8,         25) /* Mass */
     , (490018,   9,          0) /* ValidLocations - None */
     , (490018,  16,         32) /* ItemUseable - Remote */
     , (490018,  19,       200) /* Value */
     , (490018,  33,          1) /* Bonded - Bonded */
     , (490018,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (490018, 150,        103) /* HookPlacement - Hook */
     , (490018, 151,          9) /* HookType - Floor, Yard */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490018,  13, True ) /* Ethereal */
     , (490018,  22, True ) /* Inscribable */
     , (490018,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (490018,  39,       0.75) /* DefaultScale */
     , (490018,  54,       3) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490018,   0.5, 'Dark Isle') /* Name */
     , (490018,  14, 'This item can be hooked to the Floor or Yard hooks of mansions. Use this item to be transported to Dark Isle - No level requirement.') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490018,   1, 0x02001494) /* Setup */
     , (490018,   2, 0x09000197) /* MotionTable */
     , (490018,   6, 0x04001EB6) /* PaletteBase */
     , (490018,   7, 0x10000636) /* ClothingBase */
     , (490018,   8, 0x0600650E) /* Icon */
     , (490018,  22, 0x340000B6) /* PhysicsEffectTable */;

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (490018,  7 /* Use */,      1, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  99 /* TeleportTarget */, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0xC8E9002B /* @teleloc 0x002A034A [52.855202 -248.074997 0.005000] 1.000000 0.000000 0.000000 0.000000 */, 142.959198,  59.523922,  0.005000, -0.722117, 0, 0, 0.691770);

/* Lifestoned Changelog:
{
  "LastModified": "2022-11-15T22:24:08.0849064-05:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "testing",
  "IsDone": false
}
*/
