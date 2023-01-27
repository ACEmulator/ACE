DELETE FROM `weenie` WHERE `class_Id` = 450125;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450125, 'ace450125-gurogguisetailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450125,   1,          4) /* ItemType - Armor */
     , (450125,   3,          4) /* PaletteTemplate - Brown */
     , (450125,   4,     1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Head, Hands, Feet */
     , (450125,   5,       0) /* EncumbranceVal */
     , (450125,   9,      512) /* ValidLocations - HeadWear, HandWear, Armor */
     , (450125,  19,         20) /* Value */
     , (450125,  28,         0) /* ArmorLevel */
     , (450125, 151,          9) /* HookType - Floor, Yard */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450125,  13,     0.5) /* ArmorModVsSlash */
     , (450125,  14,     0.5) /* ArmorModVsPierce */
     , (450125,  15,    0.75) /* ArmorModVsBludgeon */
     , (450125,  16,    0.65) /* ArmorModVsCold */
     , (450125,  17,    0.55) /* ArmorModVsFire */
     , (450125,  18,    0.55) /* ArmorModVsAcid */
     , (450125,  19,    0.65) /* ArmorModVsElectric */
     , (450125, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450125,   1, 'Gurog Guise') /* Name */
     , (450125,  14, 'This costume can be placed on Floor and Yard house hooks.') /* Use */
     , (450125,  16, 'A finely crafted Gurog costume, lined and padded to make for all of the extra room.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450125,   1, 0x02001A23) /* Setup */
     , (450125,   3, 0x20000014) /* SoundTable */
     , (450125,   7, 0x100007BB) /* ClothingBase */
     , (450125,   8, 0x060070C2) /* Icon */
     , (450125,  22, 0x3400002B) /* PhysicsEffectTable */;

INSERT INTO `weenie_properties_i_i_d` (`object_Id`, `type`, `value`)
VALUES (450125,   2, 0x9E495102) /* Container */
     , (450125,   3, 0x00000000) /* Wielder */;
