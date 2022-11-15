DELETE FROM `weenie` WHERE `class_Id` = 450130;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450130, 'ace450130-undeadcaptainshattailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450130,   1,          4) /* ItemType - Armor */
     , (450130,   3,          4) /* PaletteTemplate - Brown */
     , (450130,   4,      16384) /* ClothingPriority - Head */
     , (450130,   5,        0) /* EncumbranceVal */
     , (450130,   9,          1) /* ValidLocations - HeadWear */
     , (450130,  19,        20) /* Value */
     , (450130,  28,         0) /* ArmorLevel */
     , (450130, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450130,  13,     0.5) /* ArmorModVsSlash */
     , (450130,  14,     0.4) /* ArmorModVsPierce */
     , (450130,  15,     0.4) /* ArmorModVsBludgeon */
     , (450130,  16,     0.6) /* ArmorModVsCold */
     , (450130,  17,     0.2) /* ArmorModVsFire */
     , (450130,  18,    0.75) /* ArmorModVsAcid */
     , (450130,  19,    0.35) /* ArmorModVsElectric */
     , (450130, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450130,   1, 'Undead Captain''s Hat') /* Name */
     , (450130,  16, 'A strange hat taken from an Undead Captain.  It still smells a bit like rot and saltwater.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450130,   1, 0x02001759) /* Setup */
     , (450130,   3, 0x20000014) /* SoundTable */
     , (450130,   6, 0x0400007E) /* PaletteBase */
     , (450130,   7, 0x100006F0) /* ClothingBase */
     , (450130,   8, 0x06006722) /* Icon */
     , (450130,  22, 0x3400002B) /* PhysicsEffectTable */;

INSERT INTO `weenie_properties_i_i_d` (`object_Id`, `type`, `value`)
VALUES (450130,   2, 0x90E78BEB) /* Container */
     , (450130,   3, 0x00000000) /* Wielder */;
