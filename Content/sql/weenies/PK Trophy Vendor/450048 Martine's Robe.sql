DELETE FROM `weenie` WHERE `class_Id` = 450048;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450048, 'robemartinetailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450048,   1,          4) /* ItemType - Clothing */
     , (450048,   3,         13) /* PaletteTemplate - Purple */
     , (450048,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Feet */
     , (450048,   5,        0) /* EncumbranceVal */
     , (450048,   8,        450) /* Mass */
     , (450048,   9,      512) /* ValidLocations - Armor */
     , (450048,  16,          1) /* ItemUseable - No */
     , (450048,  18,          1) /* UiEffects - Magical */
     , (450048,  19,       20) /* Value */
     , (450048,  27,          1) /* ArmorType - Cloth */
     , (450048,  28,         0) /* ArmorLevel */
     , (450048,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450048, 150,        103) /* HookPlacement - Hook */
     , (450048, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450048,  22, True ) /* Inscribable */
     , (450048,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450048,   5,  -0.033) /* ManaRate */
     , (450048,  12,    0.81) /* Shade */
     , (450048,  13,     0.4) /* ArmorModVsSlash */
     , (450048,  14,     0.4) /* ArmorModVsPierce */
     , (450048,  15,     0.4) /* ArmorModVsBludgeon */
     , (450048,  16,     0.4) /* ArmorModVsCold */
     , (450048,  17,     0.4) /* ArmorModVsFire */
     , (450048,  18,     0.4) /* ArmorModVsAcid */
     , (450048,  19,     0.4) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450048,   1, 'Martine''s Robe') /* Name */
     , (450048,  15, 'A purple robe once worn by the half-man, half-virindi, Candeth Martine.') /* ShortDesc */
     , (450048,  33, 'MartineRobe') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450048,   1, 0x020001A6) /* Setup */
     , (450048,   3, 0x20000014) /* SoundTable */
     , (450048,   6, 0x0400007E) /* PaletteBase */
     , (450048,   7, 0x100003F2) /* ClothingBase */
     , (450048,   8, 0x060027CA) /* Icon */
     , (450048,  22, 0x3400002B) /* PhysicsEffectTable */;

