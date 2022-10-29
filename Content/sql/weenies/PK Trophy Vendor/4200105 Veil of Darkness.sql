DELETE FROM `weenie` WHERE `class_Id` = 4200105;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200105, 'regaliagharundimhitailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200105,   1,          2) /* ItemType - Armor */
     , (4200105,   3,          4) /* PaletteTemplate - Brown */
     , (4200105,   4,      16384) /* ClothingPriority - Head */
     , (4200105,   5,          1) /* EncumbranceVal */
     , (4200105,   8,         75) /* Mass */
     , (4200105,   9,          1) /* ValidLocations - HeadWear */
     , (4200105,  16,          1) /* ItemUseable - No */
     , (4200105,  19,         20) /* Value */
     , (4200105,  27,          2) /* ArmorType - Leather */
     , (4200105,  28,          1) /* ArmorLevel */
     , (4200105,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200105, 150,        103) /* HookPlacement - Hook */
     , (4200105, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200105,  22, True ) /* Inscribable */
     , (4200105,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200105,   5,  -0.033) /* ManaRate */
     , (4200105,  12,    0.66) /* Shade */
     , (4200105,  13,       1) /* ArmorModVsSlash */
     , (4200105,  14,     1.2) /* ArmorModVsPierce */
     , (4200105,  15,     1.2) /* ArmorModVsBludgeon */
     , (4200105,  16,    1.35) /* ArmorModVsCold */
     , (4200105,  17,    1.35) /* ArmorModVsFire */
     , (4200105,  18,    1.35) /* ArmorModVsAcid */
     , (4200105,  19,    1.35) /* ArmorModVsElectric */
     , (4200105, 110,       1) /* BulkMod */
     , (4200105, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200105,   1, 'Veil of Darkness') /* Name */
     , (4200105,  16, 'A facial wrap that shields your face from sight. It is rumored that these were the same masks worn by the Elite Shagar Zharala that assassinated King Laszko.') /* LongDesc */
     , (4200105,  19, 'Gharu''ndim') /* ItemHeritageGroupRestriction */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200105,   1, 0x02000B89) /* Setup */
     , (4200105,   3, 0x20000014) /* SoundTable */
     , (4200105,   6, 0x0400007E) /* PaletteBase */
     , (4200105,   7, 0x10000340) /* ClothingBase */
     , (4200105,   8, 0x060022D9) /* Icon */
     , (4200105,  22, 0x3400002B) /* PhysicsEffectTable */
     , (4200105,  37,         10) /* ItemSkillLimit - Staff */;

