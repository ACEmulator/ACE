DELETE FROM `weenie` WHERE `class_Id` = 450457;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450457, 'regaliaaluvianextremetailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450457,   1,          2) /* ItemType - Armor */
     , (450457,   3,          4) /* PaletteTemplate - Brown */
     , (450457,   4,      16384) /* ClothingPriority - Head */
     , (450457,   5,        0) /* EncumbranceVal */
     , (450457,   8,         75) /* Mass */
     , (450457,   9,          1) /* ValidLocations - HeadWear */
     , (450457,  16,          1) /* ItemUseable - No */
     , (450457,  18,          1) /* UiEffects - Magical */
     , (450457,  19,       20) /* Value */
     , (450457,  27,          2) /* ArmorType - Leather */
     , (450457,  28,        0) /* ArmorLevel */
     , (450457,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450457, 150,        103) /* HookPlacement - Hook */
     , (450457, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450457,  22, True ) /* Inscribable */
     , (450457,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450457,   5,  -0.033) /* ManaRate */
     , (450457,  12,    0.66) /* Shade */
     , (450457,  13,     1.4) /* ArmorModVsSlash */
     , (450457,  14,     1.2) /* ArmorModVsPierce */
     , (450457,  15,     1.4) /* ArmorModVsBludgeon */
     , (450457,  16,     1.2) /* ArmorModVsCold */
     , (450457,  17,     1.2) /* ArmorModVsFire */
     , (450457,  18,     1.4) /* ArmorModVsAcid */
     , (450457,  19,       1) /* ArmorModVsElectric */
     , (450457, 110,       1) /* BulkMod */
     , (450457, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450457,   1, 'Alfric''s Bull') /* Name */
     , (450457,  16, 'An ornate representation of the heraldic bull of Viamont Royal Governor Alfric, who rounded up and executed the bloodline of High King Pwyll II during the reign of Alfrega the Mad.') /* LongDesc */
     , (450457,  19, 'Aluvian') /* ItemHeritageGroupRestriction */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450457,   1, 0x02000FAF) /* Setup */
     , (450457,   3, 0x20000014) /* SoundTable */
     , (450457,   6, 0x0400007E) /* PaletteBase */
     , (450457,   7, 0x100004C6) /* ClothingBase */
     , (450457,   8, 0x06002D36) /* Icon */
     , (450457,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450457,  37,          4) /* ItemSkillLimit - Dagger */;

