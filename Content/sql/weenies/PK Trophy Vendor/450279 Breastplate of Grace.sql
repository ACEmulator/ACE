DELETE FROM `weenie` WHERE `class_Id` = 450279;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450279, 'ace450279-breastplateofgracetailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450279,   1,          2) /* ItemType - Armor */
     , (450279,   3,         20) /* PaletteTemplate - Silver */
     , (450279,   4,       1024) /* ClothingPriority - OuterwearChest */
     , (450279,   5,        0) /* EncumbranceVal */
     , (450279,   9,        512) /* ValidLocations - ChestArmor */
     , (450279,  16,          1) /* ItemUseable - No */
     , (450279,  18,          1) /* UiEffects - Magical */
     , (450279,  19,       20) /* Value */
     , (450279,  28,        0) /* ArmorLevel */
     , (450279,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450279, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450279,  22, True ) /* Inscribable */
     , (450279, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450279,   5,  -0.033) /* ManaRate */
     , (450279,  12,       0) /* Shade */
     , (450279,  13,     1.3) /* ArmorModVsSlash */
     , (450279,  14,       1) /* ArmorModVsPierce */
     , (450279,  15,       1) /* ArmorModVsBludgeon */
     , (450279,  16,     0.8) /* ArmorModVsCold */
     , (450279,  17,     0.8) /* ArmorModVsFire */
     , (450279,  18,     0.8) /* ArmorModVsAcid */
     , (450279,  19,     0.8) /* ArmorModVsElectric */
     , (450279, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450279,   1, 'Breastplate of Grace') /* Name */
     , (450279,  16, 'A Breastplate bearing the mark of the Unicorn.') /* LongDesc */
     , (450279,  33, 'PickedUpBreastplate0806') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450279,   1, 0x0200161E) /* Setup */
     , (450279,   3, 0x20000014) /* SoundTable */
     , (450279,   6, 0x0400007E) /* PaletteBase */
     , (450279,   7, 0x10000696) /* ClothingBase */
     , (450279,   8, 0x060012F3) /* Icon */
     , (450279,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450279,  37,         14) /* ItemSkillLimit - ArcaneLore */;

