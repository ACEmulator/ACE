DELETE FROM `weenie` WHERE `class_Id` = 450597;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450597, 'regaliagharundimubertailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450597,   1,          2) /* ItemType - Armor */
     , (450597,   3,          4) /* PaletteTemplate - Brown */
     , (450597,   4,      16384) /* ClothingPriority - Head */
     , (450597,   5,        0) /* EncumbranceVal */
     , (450597,   8,         75) /* Mass */
     , (450597,   9,          1) /* ValidLocations - HeadWear */
     , (450597,  16,          1) /* ItemUseable - No */
     , (450597,  18,          1) /* UiEffects - Magical */
     , (450597,  19,       20) /* Value */
     , (450597,  27,          2) /* ArmorType - Leather */
     , (450597,  28,        0) /* ArmorLevel */
     , (450597,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450597, 150,        103) /* HookPlacement - Hook */
     , (450597, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450597,  22, True ) /* Inscribable */
     , (450597,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450597,   5,  -0.033) /* ManaRate */
     , (450597,  12,    0.66) /* Shade */
     , (450597,  13,       1) /* ArmorModVsSlash */
     , (450597,  14,     1.2) /* ArmorModVsPierce */
     , (450597,  15,     1.2) /* ArmorModVsBludgeon */
     , (450597,  16,    1.35) /* ArmorModVsCold */
     , (450597,  17,    1.35) /* ArmorModVsFire */
     , (450597,  18,    1.35) /* ArmorModVsAcid */
     , (450597,  19,    1.35) /* ArmorModVsElectric */
     , (450597, 110,       1) /* BulkMod */
     , (450597, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450597,   1, 'Shroud of Night') /* Name */
     , (450597,  16, 'An enhanced version of Janda''s ever popular mask. This version of the mask worn by assassins of the Elite Shagar Zharala who dispatched King Laszko is a triumph of the mask making trade.') /* LongDesc */
     , (450597,  19, 'Gharu''ndim') /* ItemHeritageGroupRestriction */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450597,   1, 0x02000E42) /* Setup */
     , (450597,   3, 0x20000014) /* SoundTable */
     , (450597,   6, 0x0400007E) /* PaletteBase */
     , (450597,   7, 0x1000040A) /* ClothingBase */
     , (450597,   8, 0x0600283C) /* Icon */
     , (450597,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450597,  37,         10) /* ItemSkillLimit - Staff */;

