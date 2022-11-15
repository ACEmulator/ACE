DELETE FROM `weenie` WHERE `class_Id` = 450456;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450456, 'regaliaaluvianubertailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450456,   1,          2) /* ItemType - Armor */
     , (450456,   3,          4) /* PaletteTemplate - Brown */
     , (450456,   4,      16384) /* ClothingPriority - Head */
     , (450456,   5,        0) /* EncumbranceVal */
     , (450456,   8,         75) /* Mass */
     , (450456,   9,          1) /* ValidLocations - HeadWear */
     , (450456,  16,          1) /* ItemUseable - No */
     , (450456,  18,          1) /* UiEffects - Magical */
     , (450456,  19,       20) /* Value */
     , (450456,  27,          2) /* ArmorType - Leather */
     , (450456,  28,        0) /* ArmorLevel */
     , (450456,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450456, 150,        103) /* HookPlacement - Hook */
     , (450456, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450456,  22, True ) /* Inscribable */
     , (450456,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450456,   5,  -0.033) /* ManaRate */
     , (450456,  12,    0.66) /* Shade */
     , (450456,  13,     1.4) /* ArmorModVsSlash */
     , (450456,  14,     1.2) /* ArmorModVsPierce */
     , (450456,  15,     1.4) /* ArmorModVsBludgeon */
     , (450456,  16,     1.2) /* ArmorModVsCold */
     , (450456,  17,     1.2) /* ArmorModVsFire */
     , (450456,  18,     1.4) /* ArmorModVsAcid */
     , (450456,  19,       1) /* ArmorModVsElectric */
     , (450456, 110,       1) /* BulkMod */
     , (450456, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450456,   1, 'Pwyll''s Guard') /* Name */
     , (450456,  16, 'An ornate and flawless rendition of High King Pwyll. Alexander the Deft has captured the look and feel of the High King in exquisite fashion.') /* LongDesc */
     , (450456,  19, 'Aluvian') /* ItemHeritageGroupRestriction */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450456,   1, 0x02000E41) /* Setup */
     , (450456,   3, 0x20000014) /* SoundTable */
     , (450456,   6, 0x0400007E) /* PaletteBase */
     , (450456,   7, 0x10000409) /* ClothingBase */
     , (450456,   8, 0x0600283B) /* Icon */
     , (450456,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450456,  37,          4) /* ItemSkillLimit - Dagger */;
