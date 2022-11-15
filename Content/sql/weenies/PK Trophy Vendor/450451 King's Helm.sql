DELETE FROM `weenie` WHERE `class_Id` = 450451;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450451, 'regaliaaluviantailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450451,   1,          2) /* ItemType - Armor */
     , (450451,   3,          4) /* PaletteTemplate - Brown */
     , (450451,   4,      16384) /* ClothingPriority - Head */
     , (450451,   5,        800) /* EncumbranceVal */
     , (450451,   8,         75) /* Mass */
     , (450451,   9,          1) /* ValidLocations - HeadWear */
     , (450451,  16,          1) /* ItemUseable - No */
     , (450451,  18,          1) /* UiEffects - Magical */
     , (450451,  19,       20) /* Value */
     , (450451,  27,          2) /* ArmorType - Leather */
     , (450451,  28,        0) /* ArmorLevel */
     , (450451,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450451, 150,        103) /* HookPlacement - Hook */
     , (450451, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450451,  22, True ) /* Inscribable */
     , (450451,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450451,   5,  -0.033) /* ManaRate */
     , (450451,  12,    0.66) /* Shade */
     , (450451,  13,     1.4) /* ArmorModVsSlash */
     , (450451,  14,     1.2) /* ArmorModVsPierce */
     , (450451,  15,     1.4) /* ArmorModVsBludgeon */
     , (450451,  16,     1.2) /* ArmorModVsCold */
     , (450451,  17,     1.2) /* ArmorModVsFire */
     , (450451,  18,     1.4) /* ArmorModVsAcid */
     , (450451,  19,       1) /* ArmorModVsElectric */
     , (450451, 110,       1) /* BulkMod */
     , (450451, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450451,   1, 'King''s Helm') /* Name */
     , (450451,  16, 'A finely crafted mask with the features of the legendary high king Pwyll upon it. It is a testament to the skill of its maker -- the features almost look life-like, and it is a comfortable fit on your head.') /* LongDesc */
     , (450451,  19, 'Aluvian') /* ItemHeritageGroupRestriction */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450451,   1, 0x0200097C) /* Setup */
     , (450451,   3, 0x20000014) /* SoundTable */
     , (450451,   6, 0x0400007E) /* PaletteBase */
     , (450451,   7, 0x1000026E) /* ClothingBase */
     , (450451,   8, 0x06001E9C) /* Icon */
     , (450451,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450451,  37,          4) /* ItemSkillLimit - Dagger */;

