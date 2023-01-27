DELETE FROM `weenie` WHERE `class_Id` = 450280;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450280, 'ace450280-breastplateofpowertailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450280,   1,          2) /* ItemType - Armor */
     , (450280,   3,         20) /* PaletteTemplate - Silver */
     , (450280,   4,       1024) /* ClothingPriority - OuterwearChest */
     , (450280,   5,        0) /* EncumbranceVal */
     , (450280,   9,        512) /* ValidLocations - ChestArmor */
     , (450280,  16,          1) /* ItemUseable - No */
     , (450280,  18,          1) /* UiEffects - Magical */
     , (450280,  19,       20) /* Value */
     , (450280,  28,        0) /* ArmorLevel */
     , (450280,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450280, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450280,  22, True ) /* Inscribable */
     , (450280, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450280,   5,  -0.033) /* ManaRate */
     , (450280,  12,       0) /* Shade */
     , (450280,  13,     1.3) /* ArmorModVsSlash */
     , (450280,  14,       1) /* ArmorModVsPierce */
     , (450280,  15,       1) /* ArmorModVsBludgeon */
     , (450280,  16,     0.8) /* ArmorModVsCold */
     , (450280,  17,     0.8) /* ArmorModVsFire */
     , (450280,  18,     0.8) /* ArmorModVsAcid */
     , (450280,  19,     0.8) /* ArmorModVsElectric */
     , (450280, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450280,   1, 'Breastplate of Power') /* Name */
     , (450280,  16, 'A Breastplate bearing the mark of the Dragon.') /* LongDesc */
     , (450280,  33, 'PickedUpBreastplate0806') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450280,   1, 0x0200161E) /* Setup */
     , (450280,   3, 0x20000014) /* SoundTable */
     , (450280,   6, 0x0400007E) /* PaletteBase */
     , (450280,   7, 0x10000697) /* ClothingBase */
     , (450280,   8, 0x060012F3) /* Icon */
     , (450280,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450280,  37,         14) /* ItemSkillLimit - ArcaneLore */;


