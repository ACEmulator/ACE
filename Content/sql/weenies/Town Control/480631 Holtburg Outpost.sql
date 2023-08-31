DELETE FROM `weenie` WHERE `class_Id` = 480631;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480631, 'ace480631-holtburgoutpost', 64, '2022-11-16 03:26:40') /* Hooker */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480631,   1,        128) /* ItemType - Misc */
     , (480631,   3,         20) /* PaletteTemplate - Silver */
     , (480631,   5,       5000) /* EncumbranceVal */
     , (480631,   8,         25) /* Mass */
     , (480631,   9,          0) /* ValidLocations - None */
     , (480631,  16,         32) /* ItemUseable - Remote */
     , (480631,  19,       200) /* Value */
     , (480631,  33,          1) /* Bonded - Bonded */
     , (480631,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480631, 150,        103) /* HookPlacement - Hook */
     , (480631, 151,          9) /* HookType - Floor, Yard */
     , (480631, 197,          4) /* HookGroup */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480631,  13, True ) /* Ethereal */
     , (480631,  22, True ) /* Inscribable */
     , (480631,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480631,  39,       1) /* DefaultScale */
     , (480631,  54,       3) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480631,   1, 'Holtburg Outpost') /* Name */
     , (480631,  14, 'This item can be hooked to the Floor or Yard hooks of mansions. Use this item to be transported to the Holtburg Governor.') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480631,   1, 0x020016E1) /* Setup */
     , (480631,   2, 0x09000007) /* MotionTable */
     , (480631,   3, 0x20000005) /* SoundTable */
     , (480631,   4, 0x30000002) /* CombatTable */
     , (480631,   6, 0x04001425) /* PaletteBase */
     , (480631,   7, 0x10000410) /* ClothingBase */
     , (480631,   8, 0x06001036) /* Icon */
     , (480631,  22, 0x34000017) /* PhysicsEffectTable */;


INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (480631,  7 /* Use */,      1, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  99 /* TeleportTarget */, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0xA5B4003B /* @teleloc 0x002A034A [52.855202 -248.074997 0.005000] 1.000000 0.000000 0.000000 0.000000 */, 176.617722, 71.680115,  46.005001, 0.697868, 0, 0, 0.716226);

/* Lifestoned Changelog:
{
  "LastModified": "2022-11-15T22:24:08.0849064-05:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "testing",
  "IsDone": false
}
*/
