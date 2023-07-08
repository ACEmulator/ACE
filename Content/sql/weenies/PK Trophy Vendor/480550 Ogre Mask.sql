DELETE FROM `weenie` WHERE `class_Id` = 480550;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480550, 'regaliashopk', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480550,   1,          2) /* ItemType - Armor */
     , (480550,   3,          4) /* PaletteTemplate - Brown */
     , (480550,   4,      16384) /* ClothingPriority - Head */
     , (480550,   5,        0) /* EncumbranceVal */
     , (480550,   8,         75) /* Mass */
     , (480550,   9,          1) /* ValidLocations - HeadWear */
     , (480550,  16,          1) /* ItemUseable - No */
     , (480550,  18,          1) /* UiEffects - Magical */
     , (480550,  19,       20) /* Value */
     , (480550,  27,          2) /* ArmorType - Leather */
     , (480550,  28,        0) /* ArmorLevel */
     , (480550,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480550, 150,        103) /* HookPlacement - Hook */
     , (480550, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480550,  22, True ) /* Inscribable */
     , (480550,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480550,   5,  -0.033) /* ManaRate */
     , (480550,  12,    0.66) /* Shade */
     , (480550,  13,     1.3) /* ArmorModVsSlash */
     , (480550,  14,     1.3) /* ArmorModVsPierce */
     , (480550,  15,       1) /* ArmorModVsBludgeon */
     , (480550,  16,     1.5) /* ArmorModVsCold */
     , (480550,  17,       1) /* ArmorModVsFire */
     , (480550,  18,     1.5) /* ArmorModVsAcid */
     , (480550,  19,     1.2) /* ArmorModVsElectric */
     , (480550, 110,       1) /* BulkMod */
     , (480550, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480550,   1, 'Ogre Mask') /* Name */
     , (480550,  16, 'A traditional ogre mask of the Sho Culture, made with beautiful craftsmanship. It has been sculpted into the face of a fearsome creature that Koji once encountered on her travels.') /* LongDesc */
     , (480550,  19, 'Sho') /* ItemHeritageGroupRestriction */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480550,   1, 0x0200097E) /* Setup */
     , (480550,   3, 0x20000014) /* SoundTable */
     , (480550,   6, 0x0400007E) /* PaletteBase */
     , (480550,   7, 0x10000270) /* ClothingBase */
     , (480550,   8, 0x06001E9E) /* Icon */
     , (480550,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480550,  37,         13) /* ItemSkillLimit - UnarmedCombat */;


