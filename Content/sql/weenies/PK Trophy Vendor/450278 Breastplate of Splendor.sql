DELETE FROM `weenie` WHERE `class_Id` = 450278;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450278, 'ace450278-breastplateofsplendortailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450278,   1,          2) /* ItemType - Armor */
     , (450278,   3,         20) /* PaletteTemplate - Silver */
     , (450278,   4,       1024) /* ClothingPriority - OuterwearChest */
     , (450278,   5,        0) /* EncumbranceVal */
     , (450278,   9,        512) /* ValidLocations - ChestArmor */
     , (450278,  16,          1) /* ItemUseable - No */
     , (450278,  18,          1) /* UiEffects - Magical */
     , (450278,  19,       20) /* Value */
     , (450278,  28,        0) /* ArmorLevel */
     , (450278,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450278,  22, True ) /* Inscribable */
     , (450278, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450278,   5,  -0.033) /* ManaRate */
     , (450278,  12,       0) /* Shade */
     , (450278,  13,     1.3) /* ArmorModVsSlash */
     , (450278,  14,       1) /* ArmorModVsPierce */
     , (450278,  15,       1) /* ArmorModVsBludgeon */
     , (450278,  16,     0.8) /* ArmorModVsCold */
     , (450278,  17,     0.8) /* ArmorModVsFire */
     , (450278,  18,     0.8) /* ArmorModVsAcid */
     , (450278,  19,     0.8) /* ArmorModVsElectric */
     , (450278, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450278,   1, 'Breastplate of Splendor') /* Name */
     , (450278,  16, 'A Breastplate bearing the mark of the Firebird.') /* LongDesc */
     , (450278,  33, 'PickedUpBreastplate0806') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450278,   1, 0x0200161E) /* Setup */
     , (450278,   3, 0x20000014) /* SoundTable */
     , (450278,   6, 0x0400007E) /* PaletteBase */
     , (450278,   7, 0x10000695) /* ClothingBase */
     , (450278,   8, 0x060012F3) /* Icon */
     , (450278,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450278,  37,         14) /* ItemSkillLimit - ArcaneLore */;


