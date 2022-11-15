DELETE FROM `weenie` WHERE `class_Id` = 450454;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450454, 'regaliashotailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450454,   1,          2) /* ItemType - Armor */
     , (450454,   3,          4) /* PaletteTemplate - Brown */
     , (450454,   4,      16384) /* ClothingPriority - Head */
     , (450454,   5,        0) /* EncumbranceVal */
     , (450454,   8,         75) /* Mass */
     , (450454,   9,          1) /* ValidLocations - HeadWear */
     , (450454,  16,          1) /* ItemUseable - No */
     , (450454,  18,          1) /* UiEffects - Magical */
     , (450454,  19,       20) /* Value */
     , (450454,  27,          2) /* ArmorType - Leather */
     , (450454,  28,        0) /* ArmorLevel */
     , (450454,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450454, 150,        103) /* HookPlacement - Hook */
     , (450454, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450454,  22, True ) /* Inscribable */
     , (450454,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450454,   5,  -0.033) /* ManaRate */
     , (450454,  12,    0.66) /* Shade */
     , (450454,  13,     1.3) /* ArmorModVsSlash */
     , (450454,  14,     1.3) /* ArmorModVsPierce */
     , (450454,  15,       1) /* ArmorModVsBludgeon */
     , (450454,  16,     1.5) /* ArmorModVsCold */
     , (450454,  17,       1) /* ArmorModVsFire */
     , (450454,  18,     1.5) /* ArmorModVsAcid */
     , (450454,  19,     1.2) /* ArmorModVsElectric */
     , (450454, 110,       1) /* BulkMod */
     , (450454, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450454,   1, 'Ogre Mask') /* Name */
     , (450454,  16, 'A traditional ogre mask of the Sho Culture, made with beautiful craftsmanship. It has been sculpted into the face of a fearsome creature that Koji once encountered on her travels.') /* LongDesc */
     , (450454,  19, 'Sho') /* ItemHeritageGroupRestriction */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450454,   1, 0x0200097E) /* Setup */
     , (450454,   3, 0x20000014) /* SoundTable */
     , (450454,   6, 0x0400007E) /* PaletteBase */
     , (450454,   7, 0x10000270) /* ClothingBase */
     , (450454,   8, 0x06001E9E) /* Icon */
     , (450454,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450454,  37,         13) /* ItemSkillLimit - UnarmedCombat */;


