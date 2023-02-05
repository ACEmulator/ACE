DELETE FROM `weenie` WHERE `class_Id` = 482000;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (482000, 'ace482000-commanderspassage', 64, '2022-11-16 03:26:40') /* Hooker */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (482000,   1,        128) /* ItemType - Misc */
     , (482000,   3,         20) /* PaletteTemplate - Silver */
     , (482000,   5,       5000) /* EncumbranceVal */
     , (482000,   8,         25) /* Mass */
     , (482000,   9,          0) /* ValidLocations - None */
     , (482000,  16,         32) /* ItemUseable - Remote */
     , (482000,  19,       1000) /* Value */
     , (482000,  33,          1) /* Bonded - Bonded */
     , (482000,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (482000, 150,        103) /* HookPlacement - Hook */
     , (482000, 151,          9) /* HookType - Floor, Yard */
     , (482000, 197,          4) /* HookGroup */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (482000,  13, True ) /* Ethereal */
     , (482000,  22, True ) /* Inscribable */
     , (482000,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (482000,  39,       1) /* DefaultScale */
     , (482000,  54,       3) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (482000,   1, 'Commanders Passage') /* Name */
     , (482000,  14, 'This item can be hooked to the Floor or Yard hooks of mansions. Use this item to be transported into the Path of the Blind.') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (482000,   1, 0x02000038) /* Setup */
     , (482000,   2, 0x090000AB) /* MotionTable */
     , (482000,   6, 0x040010AF) /* PaletteBase */
     , (482000,   7, 0x100002A7) /* ClothingBase */
     , (482000,   8, 0x06001037) /* Icon */
     , (482000,  22, 0x3400001E) /* PhysicsEffectTable */
     , (482000,  36, 0x0E000016) /* MutateFilter */;


INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (482000,  7 /* Use */,      1, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  99 /* TeleportTarget */, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0x43F1001B /* @teleloc 0x002A034A [52.855202 -248.074997 0.005000] 1.000000 0.000000 0.000000 0.000000 */, 91.322601, 52.338699,  40.005001, -0.986258, 0, 0, -0.165213);

/* Lifestoned Changelog:
{
  "LastModified": "2022-11-15T22:24:08.0849064-05:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "testing",
  "IsDone": false
}
*/
