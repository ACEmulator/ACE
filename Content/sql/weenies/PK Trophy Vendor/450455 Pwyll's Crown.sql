DELETE FROM `weenie` WHERE `class_Id` = 450455;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450455, 'regaliaaluvianhitailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450455,   1,          2) /* ItemType - Armor */
     , (450455,   3,          4) /* PaletteTemplate - Brown */
     , (450455,   4,      16384) /* ClothingPriority - Head */
     , (450455,   5,        0) /* EncumbranceVal */
     , (450455,   8,         75) /* Mass */
     , (450455,   9,          1) /* ValidLocations - HeadWear */
     , (450455,  16,          1) /* ItemUseable - No */
     , (450455,  18,          1) /* UiEffects - Magical */
     , (450455,  19,       20) /* Value */
     , (450455,  27,          2) /* ArmorType - Leather */
     , (450455,  28,        0) /* ArmorLevel */
     , (450455,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450455, 150,        103) /* HookPlacement - Hook */
     , (450455, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450455,  22, True ) /* Inscribable */
     , (450455,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450455,   5,  -0.033) /* ManaRate */
     , (450455,  12,    0.66) /* Shade */
     , (450455,  13,     1.4) /* ArmorModVsSlash */
     , (450455,  14,     1.2) /* ArmorModVsPierce */
     , (450455,  15,     1.4) /* ArmorModVsBludgeon */
     , (450455,  16,     1.2) /* ArmorModVsCold */
     , (450455,  17,     1.2) /* ArmorModVsFire */
     , (450455,  18,     1.4) /* ArmorModVsAcid */
     , (450455,  19,       1) /* ArmorModVsElectric */
     , (450455, 110,       1) /* BulkMod */
     , (450455, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450455,   1, 'Pwyll''s Crown') /* Name */
     , (450455,  16, 'This masterfully crafted mask makes other masks look like child''s play. The features almost look alive, and it is a comfortable fit on your head.') /* LongDesc */
     , (450455,  19, 'Aluvian') /* ItemHeritageGroupRestriction */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450455,   1, 0x02000B88) /* Setup */
     , (450455,   3, 0x20000014) /* SoundTable */
     , (450455,   6, 0x0400007E) /* PaletteBase */
     , (450455,   7, 0x1000033F) /* ClothingBase */
     , (450455,   8, 0x060022D8) /* Icon */
     , (450455,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450455,  37,          4) /* ItemSkillLimit - Dagger */;


