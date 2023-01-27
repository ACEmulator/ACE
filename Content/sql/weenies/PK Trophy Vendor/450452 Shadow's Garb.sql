DELETE FROM `weenie` WHERE `class_Id` = 450452;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450452, 'regaliagharundimtailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450452,   1,          2) /* ItemType - Armor */
     , (450452,   3,          4) /* PaletteTemplate - Brown */
     , (450452,   4,      16384) /* ClothingPriority - Head */
     , (450452,   5,        0) /* EncumbranceVal */
     , (450452,   8,         75) /* Mass */
     , (450452,   9,          1) /* ValidLocations - HeadWear */
     , (450452,  16,          1) /* ItemUseable - No */
     , (450452,  18,          1) /* UiEffects - Magical */
     , (450452,  19,       20) /* Value */
     , (450452,  27,          2) /* ArmorType - Leather */
     , (450452,  28,        0) /* ArmorLevel */
     , (450452,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450452, 150,        103) /* HookPlacement - Hook */
     , (450452, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450452,  22, True ) /* Inscribable */
     , (450452,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450452,   5,  -0.033) /* ManaRate */
     , (450452,  12,    0.66) /* Shade */
     , (450452,  13,       1) /* ArmorModVsSlash */
     , (450452,  14,     1.2) /* ArmorModVsPierce */
     , (450452,  15,     1.2) /* ArmorModVsBludgeon */
     , (450452,  16,    1.35) /* ArmorModVsCold */
     , (450452,  17,    1.35) /* ArmorModVsFire */
     , (450452,  18,    1.35) /* ArmorModVsAcid */
     , (450452,  19,    1.35) /* ArmorModVsElectric */
     , (450452, 110,       1) /* BulkMod */
     , (450452, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450452,   1, 'Shadow''s Garb') /* Name */
     , (450452,  16, 'A facial wrap that protects your face from sandstorms, and occludes your face from the eyes of others. It is rumored that these were the same masks worn by the Shagar Zharala during their assassination of King Laszko.') /* LongDesc */
     , (450452,  19, 'Gharu''ndim') /* ItemHeritageGroupRestriction */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450452,   1, 0x0200097D) /* Setup */
     , (450452,   3, 0x20000014) /* SoundTable */
     , (450452,   6, 0x0400007E) /* PaletteBase */
     , (450452,   7, 0x1000026F) /* ClothingBase */
     , (450452,   8, 0x06001E9D) /* Icon */
     , (450452,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450452,  37,         10) /* ItemSkillLimit - Staff */;


