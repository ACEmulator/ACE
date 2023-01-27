DELETE FROM `weenie` WHERE `class_Id` = 450289;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450289, 'ace450289-shoujenjikatabitailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450289,   1,          2) /* ItemType - Armor */
     , (450289,   3,          9) /* PaletteTemplate - Grey */
     , (450289,   4,      65536) /* ClothingPriority - Feet */
     , (450289,   5,        0) /* EncumbranceVal */
     , (450289,   9,        384) /* ValidLocations - LowerLegWear, FootWear */
     , (450289,  16,          1) /* ItemUseable - No */
     , (450289,  18,          1) /* UiEffects - Magical */
     , (450289,  19,      20) /* Value */
     , (450289,  28,        0) /* ArmorLevel */
     , (450289,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450289,  11, True ) /* IgnoreCollisions */
     , (450289,  13, True ) /* Ethereal */
     , (450289,  14, True ) /* GravityStatus */
     , (450289,  19, True ) /* Attackable */
     , (450289,  22, True ) /* Inscribable */
     , (450289,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450289,   5,  -0.017) /* ManaRate */
     , (450289,  13,     0.6) /* ArmorModVsSlash */
     , (450289,  14,     0.6) /* ArmorModVsPierce */
     , (450289,  15,     0.6) /* ArmorModVsBludgeon */
     , (450289,  16,     1.4) /* ArmorModVsCold */
     , (450289,  17,     0.7) /* ArmorModVsFire */
     , (450289,  18,     1.2) /* ArmorModVsAcid */
     , (450289,  19,     1.4) /* ArmorModVsElectric */
     , (450289, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450289,   1, 'Shou-jen Jika-Tabi') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450289,   1, 0x020008CB) /* Setup */
     , (450289,   3, 0x20000014) /* SoundTable */
     , (450289,   7, 0x10000698) /* ClothingBase */
     , (450289,   8, 0x060064E1) /* Icon */
     , (450289,  22, 0x3400002B) /* PhysicsEffectTable */;

