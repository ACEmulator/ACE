DELETE FROM `weenie` WHERE `class_Id` = 450459;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450459, 'regaliashoextremetailors', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450459,   1,          2) /* ItemType - Armor */
     , (450459,   3,          4) /* PaletteTemplate - Brown */
     , (450459,   4,      16384) /* ClothingPriority - Head */
     , (450459,   5,        0) /* EncumbranceVal */
     , (450459,   8,         75) /* Mass */
     , (450459,   9,          1) /* ValidLocations - HeadWear */
     , (450459,  16,          1) /* ItemUseable - No */
     , (450459,  18,          1) /* UiEffects - Magical */
     , (450459,  19,       20) /* Value */
     , (450459,  27,          2) /* ArmorType - Leather */
     , (450459,  28,        0) /* ArmorLevel */
     , (450459,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450459, 150,        103) /* HookPlacement - Hook */
     , (450459, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450459,  22, True ) /* Inscribable */
     , (450459,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450459,   5,  -0.033) /* ManaRate */
     , (450459,  12,    0.66) /* Shade */
     , (450459,  13,     1.3) /* ArmorModVsSlash */
     , (450459,  14,     1.3) /* ArmorModVsPierce */
     , (450459,  15,       1) /* ArmorModVsBludgeon */
     , (450459,  16,     1.5) /* ArmorModVsCold */
     , (450459,  17,       1) /* ArmorModVsFire */
     , (450459,  18,     1.5) /* ArmorModVsAcid */
     , (450459,  19,     1.2) /* ArmorModVsElectric */
     , (450459, 110,       1) /* BulkMod */
     , (450459, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450459,   1, 'Koji''s Visage') /* Name */
     , (450459,  16, 'A lovely and delicately detailed mask representing Koji herself. ') /* LongDesc */
     , (450459,  19, 'Sho') /* ItemHeritageGroupRestriction */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450459,   1, 0x02000FAE) /* Setup */
     , (450459,   3, 0x20000014) /* SoundTable */
     , (450459,   6, 0x0400007E) /* PaletteBase */
     , (450459,   7, 0x100004C4) /* ClothingBase */
     , (450459,   8, 0x06002D27) /* Icon */
     , (450459,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450459,  37,         13) /* ItemSkillLimit - UnarmedCombat */;

