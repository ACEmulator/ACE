DELETE FROM `weenie` WHERE `class_Id` = 450458;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450458, 'regaliagharundimextremetailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450458,   1,          2) /* ItemType - Armor */
     , (450458,   3,          4) /* PaletteTemplate - Brown */
     , (450458,   4,      16384) /* ClothingPriority - Head */
     , (450458,   5,        0) /* EncumbranceVal */
     , (450458,   8,         75) /* Mass */
     , (450458,   9,          1) /* ValidLocations - HeadWear */
     , (450458,  16,          1) /* ItemUseable - No */
     , (450458,  18,          1) /* UiEffects - Magical */
     , (450458,  19,       20) /* Value */
     , (450458,  27,          2) /* ArmorType - Leather */
     , (450458,  28,        0) /* ArmorLevel */
     , (450458,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450458, 150,        103) /* HookPlacement - Hook */
     , (450458, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450458,  22, True ) /* Inscribable */
     , (450458,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450458,   5,  -0.033) /* ManaRate */
     , (450458,  12,    0.66) /* Shade */
     , (450458,  13,       1) /* ArmorModVsSlash */
     , (450458,  14,     1.2) /* ArmorModVsPierce */
     , (450458,  15,     1.2) /* ArmorModVsBludgeon */
     , (450458,  16,    1.35) /* ArmorModVsCold */
     , (450458,  17,    1.35) /* ArmorModVsFire */
     , (450458,  18,    1.35) /* ArmorModVsAcid */
     , (450458,  19,    1.35) /* ArmorModVsElectric */
     , (450458, 110,       1) /* BulkMod */
     , (450458, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450458,   1, 'The Poet''s Mask') /* Name */
     , (450458,  16, 'A finely detailed mask representing the visage of Yasif ibn Salayyar, the Poet and Royal Emissary of Gharu''n.') /* LongDesc */
     , (450458,  19, 'Gharu''ndim') /* ItemHeritageGroupRestriction */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450458,   1, 0x02000FB0) /* Setup */
     , (450458,   3, 0x20000014) /* SoundTable */
     , (450458,   6, 0x0400007E) /* PaletteBase */
     , (450458,   7, 0x100004C5) /* ClothingBase */
     , (450458,   8, 0x06002D37) /* Icon */
     , (450458,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450458,  37,         10) /* ItemSkillLimit - Staff */;

