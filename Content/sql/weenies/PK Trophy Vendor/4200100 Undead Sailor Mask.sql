DELETE FROM `weenie` WHERE `class_Id` = 4200100;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200100, 'ace4200100-undeadsailormasktailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200100,   1,          2) /* ItemType - Armor */
     , (4200100,   3,          4) /* PaletteTemplate - Brown */
     , (4200100,   4,      16384) /* ClothingPriority - Head */
     , (4200100,   5,          1) /* EncumbranceVal */
     , (4200100,   9,          1) /* ValidLocations - HeadWear */
     , (4200100,  19,         20) /* Value */
     , (4200100,  28,          1) /* ArmorLevel */
     , (4200100, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200100,  13,     0.5) /* ArmorModVsSlash */
     , (4200100,  14,     0.4) /* ArmorModVsPierce */
     , (4200100,  15,     0.4) /* ArmorModVsBludgeon */
     , (4200100,  16,     0.6) /* ArmorModVsCold */
     , (4200100,  17,     0.2) /* ArmorModVsFire */
     , (4200100,  18,    0.75) /* ArmorModVsAcid */
     , (4200100,  19,    0.35) /* ArmorModVsElectric */
     , (4200100, 166,       1) /* ResistNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200100,   1, 'Undead Sailor Mask') /* Name */
     , (4200100,  16, 'A Zombie mask, with an attached bandana and eyepatch.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200100,   1, 0x02001755) /* Setup */
     , (4200100,   3, 0x20000014) /* SoundTable */
     , (4200100,   6, 0x02000001) /* PaletteBase */
     , (4200100,   7, 0x100006ED) /* ClothingBase */
     , (4200100,   8, 0x0600671C) /* Icon */
     , (4200100,  22, 0x3400002B) /* PhysicsEffectTable */;

INSERT INTO `weenie_properties_i_i_d` (`object_Id`, `type`, `value`)
VALUES (4200100,   2, 0x501038C4) /* Container */
     , (4200100,   3, 0x00000000) /* Wielder */;
