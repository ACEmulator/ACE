DELETE FROM `weenie` WHERE `class_Id` = 450484;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450484, 'masktuskertailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450484,   1,          4) /* ItemType - Armor */
     , (450484,   3,          4) /* PaletteTemplate - Brown */
     , (450484,   4,      16384) /* ClothingPriority - Head */
     , (450484,   5,        0) /* EncumbranceVal */
     , (450484,   8,         75) /* Mass */
     , (450484,   9,          1) /* ValidLocations - HeadWear */
     , (450484,  16,          1) /* ItemUseable - No */
     , (450484,  19,       20) /* Value */
     , (450484,  27,          2) /* ArmorType - Leather */
     , (450484,  28,         0) /* ArmorLevel */
     , (450484,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450484, 150,        103) /* HookPlacement - Hook */
     , (450484, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450484,  22, True ) /* Inscribable */
     , (450484,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450484,  12,    0.66) /* Shade */
     , (450484,  13,    0.75) /* ArmorModVsSlash */
     , (450484,  14,     0.4) /* ArmorModVsPierce */
     , (450484,  15,     0.5) /* ArmorModVsBludgeon */
     , (450484,  16,     0.5) /* ArmorModVsCold */
     , (450484,  17,    0.35) /* ArmorModVsFire */
     , (450484,  18,     0.5) /* ArmorModVsAcid */
     , (450484,  19,       1) /* ArmorModVsElectric */
     , (450484, 110,       1) /* BulkMod */
     , (450484, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450484,   1, 'Tusker Mask') /* Name */
     , (450484,  16, 'A mask that is finely stitched, managing to keep the fur looking natural, while maneuvering the natural features to fit a human head.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450484,   1, 0x0200095A) /* Setup */
     , (450484,   3, 0x20000014) /* SoundTable */
     , (450484,   6, 0x0400007E) /* PaletteBase */
     , (450484,   7, 0x10000258) /* ClothingBase */
     , (450484,   8, 0x06001E32) /* Icon */
     , (450484,  22, 0x3400002B) /* PhysicsEffectTable */;

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (450484, 25 /* Wield */,      1, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  22 /* StampQuest */, 0, 1, NULL, 'TuskerMask', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (450484, 26 /* UnWield */,      1, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  31 /* EraseQuest */, 0, 1, NULL, 'TuskerMask', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);
