DELETE FROM `weenie` WHERE `class_Id` = 450119;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450119, 'regaliashoextremetailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450119,   1,          2) /* ItemType - Armor */
     , (450119,   3,          4) /* PaletteTemplate - Brown */
     , (450119,   4,      16384) /* ClothingPriority - Head */
     , (450119,   5,        0) /* EncumbranceVal */
     , (450119,   8,         75) /* Mass */
     , (450119,   9,          1) /* ValidLocations - HeadWear */
     , (450119,  16,          1) /* ItemUseable - No */
     , (450119,  18,          1) /* UiEffects - Magical */
     , (450119,  19,       20) /* Value */
     , (450119,  27,          2) /* ArmorType - Leather */
     , (450119,  28,        0) /* ArmorLevel */
     , (450119,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450119, 150,        103) /* HookPlacement - Hook */
     , (450119, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450119,  22, True ) /* Inscribable */
     , (450119,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450119,   5,  -0.033) /* ManaRate */
     , (450119,  12,    0.66) /* Shade */
     , (450119,  13,     1.3) /* ArmorModVsSlash */
     , (450119,  14,     1.3) /* ArmorModVsPierce */
     , (450119,  15,       1) /* ArmorModVsBludgeon */
     , (450119,  16,     1.5) /* ArmorModVsCold */
     , (450119,  17,       1) /* ArmorModVsFire */
     , (450119,  18,     1.5) /* ArmorModVsAcid */
     , (450119,  19,     1.2) /* ArmorModVsElectric */
     , (450119, 110,       1) /* BulkMod */
     , (450119, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450119,   1, 'Koji''s Visage') /* Name */
     , (450119,  16, 'A lovely and delicately detailed mask representing Koji herself. ') /* LongDesc */
     , (450119,  19, 'Sho') /* ItemHeritageGroupRestriction */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450119,   1, 0x02000FAE) /* Setup */
     , (450119,   3, 0x20000014) /* SoundTable */
     , (450119,   6, 0x0400007E) /* PaletteBase */
     , (450119,   7, 0x100004C4) /* ClothingBase */
     , (450119,   8, 0x06002D27) /* Icon */
     , (450119,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450119,  37,         13) /* ItemSkillLimit - UnarmedCombat */;


