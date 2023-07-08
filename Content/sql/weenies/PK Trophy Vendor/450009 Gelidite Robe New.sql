DELETE FROM `weenie` WHERE `class_Id` = 450009;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450009, 'robegeliditenewtailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450009,   1,          4) /* ItemType - Clothing */
     , (450009,   3,          1) /* PaletteTemplate - AquaBlue */
     , (450009,   4,      1024) 
     , (450009,   5,        10) /* EncumbranceVal */
     , (450009,   8,        150) /* Mass */
     , (450009,   9,      512) /* ValidLocations - HeadWear, Armor */
     , (450009,  16,          1) /* ItemUseable - No */
     , (450009,  18,          1) /* UiEffects - Magical */
     , (450009,  19,       20) /* Value */
     , (450009,  27,          1) /* ArmorType - Cloth */
     , (450009,  28,        0) /* ArmorLevel */
     , (450009,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450009, 150,        103) /* HookPlacement - Hook */
     , (450009, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450009,  22, True ) /* Inscribable */
     , (450009,  23, True ) /* DestroyOnSell */
     , (450009, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450009,   5,  -0.333) /* ManaRate */
     , (450009,  12,     0.1) /* Shade */
     , (450009,  13,     0.5) /* ArmorModVsSlash */
     , (450009,  14,     0.5) /* ArmorModVsPierce */
     , (450009,  15,       1) /* ArmorModVsBludgeon */
     , (450009,  16,    0.75) /* ArmorModVsCold */
     , (450009,  17,    0.75) /* ArmorModVsFire */
     , (450009,  18,    0.25) /* ArmorModVsAcid */
     , (450009,  19,    0.35) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450009,   1, 'Gelidite Robe') /* Name */
     , (450009,  15, 'An icy blue robe.') /* ShortDesc */
     , (450009,  16, 'An icy blue robe, worn by the Gelidites of Frore when they walked the living world. This artifact is several millennia old.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450009,   1, 0x020001A6) /* Setup */
     , (450009,   3, 0x20000014) /* SoundTable */
     , (450009,   6, 0x0400007E) /* PaletteBase */
     , (450009,   7, 0x1000052B) /* ClothingBase */
     , (450009,   8, 0x06001B90) /* Icon */
     , (450009,  22, 0x3400002B) /* PhysicsEffectTable */;
