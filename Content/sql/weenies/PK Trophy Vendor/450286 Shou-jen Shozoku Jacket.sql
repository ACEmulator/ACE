DELETE FROM `weenie` WHERE `class_Id` = 450286;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450286, 'ace450286-shoujenshozokujackettailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450286,   1,          2) /* ItemType - Armor */
     , (450286,   3,          9) /* PaletteTemplate - Grey */
     , (450286,   4,      1024) /* ClothingPriority - OuterwearChest, OuterwearUpperArms, OuterwearLowerArms */
     , (450286,   5,        0) /* EncumbranceVal */
     , (450286,   9,       512) /* ValidLocations - ChestArmor, UpperArmArmor, LowerArmArmor */
     , (450286,  16,          1) /* ItemUseable - No */
     , (450286,  18,          1) /* UiEffects - Magical */
     , (450286,  19,      20) /* Value */
     , (450286,  28,        0) /* ArmorLevel */
     , (450286,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450286,  11, True ) /* IgnoreCollisions */
     , (450286,  13, True ) /* Ethereal */
     , (450286,  14, True ) /* GravityStatus */
     , (450286,  19, True ) /* Attackable */
     , (450286,  22, True ) /* Inscribable */
     , (450286,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450286,   5,  -0.017) /* ManaRate */
     , (450286,  13,     0.6) /* ArmorModVsSlash */
     , (450286,  14,     0.6) /* ArmorModVsPierce */
     , (450286,  15,     0.6) /* ArmorModVsBludgeon */
     , (450286,  16,     1.4) /* ArmorModVsCold */
     , (450286,  17,     0.7) /* ArmorModVsFire */
     , (450286,  18,     1.2) /* ArmorModVsAcid */
     , (450286,  19,     1.4) /* ArmorModVsElectric */
     , (450286, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450286,   1, 'Shou-jen Shozoku Jacket') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450286,   1, 0x020001A6) /* Setup */
     , (450286,   3, 0x20000014) /* SoundTable */
     , (450286,   7, 0x10000699) /* ClothingBase */
     , (450286,   8, 0x060064E2) /* Icon */
     , (450286,  22, 0x3400002B) /* PhysicsEffectTable */;


