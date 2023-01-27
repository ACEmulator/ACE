DELETE FROM `weenie` WHERE `class_Id` = 480000;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480000, 'ace480000-queenspassage', 64, '2022-11-16 03:10:06') /* Hooker */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480000,   1,        128) /* ItemType - Misc */
     , (480000,   3,          5) /* PaletteTemplate - DarkBlue */
     , (480000,   5,       5000) /* EncumbranceVal */
     , (480000,   8,         25) /* Mass */
     , (480000,   9,          0) /* ValidLocations - None */
     , (480000,  16,         32) /* ItemUseable - Remote */
     , (480000,  19,       1000) /* Value */
     , (480000,  33,          1) /* Bonded - Bonded */
     , (480000,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480000, 150,        103) /* HookPlacement - Hook */
     , (480000, 151,          9) /* HookType - Floor, Yard */
     , (480000, 197,          4) /* HookGroup */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480000,  13, True ) /* Ethereal */
     , (480000,  22, True ) /* Inscribable */
     , (480000,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480000,  39,       1) /* DefaultScale */
     , (480000,  54,       3) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480000,   1, 'Queens Passage') /* Name */
     , (480000,  14, 'This item can be hooked to the Floor or Yard hooks of mansions. Use this item to be transported into the Egg Orchard.') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480000,   1,   33559086) /* Setup */
     , (480000,   3,  536870932) /* SoundTable */
     , (480000,   6,   67113288) /* PaletteBase */
     , (480000,   8,  100667623) /* Icon */;

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (480000,  7 /* Use */,      1, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  99 /* TeleportTarget */, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0x43F70015 /* @teleloc 0x002B0371 [458.535004 -172.203003 0.005000] 0.933008 0.000000 0.000000 -0.359856 */, 60.000000, 108.000000,  102.004997, 0.999688, 0, 0, -0.024997);

/* Lifestoned Changelog:
{
  "LastModified": "2022-11-15T21:22:21.547236-05:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "testing",
  "IsDone": false
}
*/
