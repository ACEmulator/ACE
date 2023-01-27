DELETE FROM `weenie` WHERE `class_Id` = 481000;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (481000, 'ace481000-chieftanspassage', 64, '2022-11-16 03:26:40') /* Hooker */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (481000,   1,        128) /* ItemType - Misc */
     , (481000,   3,         45) /* PaletteTemplate - PaleGreen */
     , (481000,   5,       5000) /* EncumbranceVal */
     , (481000,   8,         25) /* Mass */
     , (481000,   9,          0) /* ValidLocations - None */
     , (481000,  16,         32) /* ItemUseable - Remote */
     , (481000,  19,       1000) /* Value */
     , (481000,  33,          1) /* Bonded - Bonded */
     , (481000,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (481000, 150,        103) /* HookPlacement - Hook */
     , (481000, 151,          9) /* HookType - Floor, Yard */
     , (481000, 197,          4) /* HookGroup */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (481000,  13, True ) /* Ethereal */
     , (481000,  22, True ) /* Inscribable */
     , (481000,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (481000,  39,       1) /* DefaultScale */
     , (481000,  54,       3) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (481000,   1, 'Chieftans Passage') /* Name */
     , (481000,  14, 'This item can be hooked to the Floor or Yard hooks of mansions. Use this item to be transported into the Dark Design.') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (481000,   1, 0x0200003A) /* Setup */
     , (481000,   2, 0x090000A8) /* MotionTable */
     , (481000,   6, 0x040010AF) /* PaletteBase */
     , (481000,   7, 0x100002A4) /* ClothingBase */
     , (481000,   8, 0x06001039) /* Icon */
     , (481000,  22, 0x34000020) /* PhysicsEffectTable */
     , (481000,  36, 0x0E000016) /* MutateFilter */;


INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (481000,  7 /* Use */,      1, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  99 /* TeleportTarget */, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0x47F4000E /* @teleloc 0x002D0373 [200.141998 -4.327550 0.005000] -0.024927 0.000000 0.000000 -0.999689 */, 36, 132,  2.005000, 0.714421, 0, 0,  -0.699716);

/* Lifestoned Changelog:
{
  "LastModified": "2022-11-15T22:25:53.5666255-05:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "testing",
  "IsDone": false
}
*/
