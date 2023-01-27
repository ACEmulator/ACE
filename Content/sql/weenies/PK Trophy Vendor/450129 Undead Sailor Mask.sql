DELETE FROM `weenie` WHERE `class_Id` = 450129;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450129, 'ace450129-undeadsailormasktailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450129,   1,          4) /* ItemType - Armor */
     , (450129,   3,          4) /* PaletteTemplate - Brown */
     , (450129,   4,      16384) /* ClothingPriority - Head */
     , (450129,   5,        0) /* EncumbranceVal */
     , (450129,   9,          1) /* ValidLocations - HeadWear */
     , (450129,  19,        20) /* Value */
     , (450129,  28,         0) /* ArmorLevel */
     , (450129, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450129,  13,     0.5) /* ArmorModVsSlash */
     , (450129,  14,     0.4) /* ArmorModVsPierce */
     , (450129,  15,     0.4) /* ArmorModVsBludgeon */
     , (450129,  16,     0.6) /* ArmorModVsCold */
     , (450129,  17,     0.2) /* ArmorModVsFire */
     , (450129,  18,    0.75) /* ArmorModVsAcid */
     , (450129,  19,    0.35) /* ArmorModVsElectric */
     , (450129, 166,       1) /* ResistNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450129,   1, 'Undead Sailor Mask') /* Name */
     , (450129,  16, 'A Zombie mask, with an attached bandana and eyepatch.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450129,   1, 0x02001755) /* Setup */
     , (450129,   3, 0x20000014) /* SoundTable */
     , (450129,   6, 0x02000001) /* PaletteBase */
     , (450129,   7, 0x100006ED) /* ClothingBase */
     , (450129,   8, 0x0600671C) /* Icon */
     , (450129,  22, 0x3400002B) /* PhysicsEffectTable */;

INSERT INTO `weenie_properties_i_i_d` (`object_Id`, `type`, `value`)
VALUES (450129,   2, 0x501038C4) /* Container */
     , (450129,   3, 0x00000000) /* Wielder */;
