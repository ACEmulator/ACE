DELETE FROM `weenie` WHERE `class_Id` = 450291;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450291, 'ace450291-reinforcedshoujenshozokujacket', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450291,   1,          2) /* ItemType - Armor */
     , (450291,   3,          9) /* PaletteTemplate - Grey */
     , (450291,   4,      13312) /* ClothingPriority - OuterwearChest, OuterwearUpperArms, OuterwearLowerArms */
     , (450291,   5,        0) /* EncumbranceVal */
     , (450291,   9,       512) /* ValidLocations - ChestArmor, UpperArmArmor, LowerArmArmor */
     , (450291,  16,          1) /* ItemUseable - No */
     , (450291,  18,          1) /* UiEffects - Magical */
     , (450291,  19,      20) /* Value */
     , (450291,  28,        0) /* ArmorLevel */
     , (450291,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450291,  11, True ) /* IgnoreCollisions */
     , (450291,  13, True ) /* Ethereal */
     , (450291,  14, True ) /* GravityStatus */
     , (450291,  19, True ) /* Attackable */
     , (450291,  22, True ) /* Inscribable */
     , (450291,  69, False) /* IsSellable */
     , (450291, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450291,   5,  -0.017) /* ManaRate */
     , (450291,  12,       0) /* Shade */
     , (450291,  13,     0.6) /* ArmorModVsSlash */
     , (450291,  14,     0.6) /* ArmorModVsPierce */
     , (450291,  15,     0.6) /* ArmorModVsBludgeon */
     , (450291,  16,     1.4) /* ArmorModVsCold */
     , (450291,  17,     0.7) /* ArmorModVsFire */
     , (450291,  18,     1.2) /* ArmorModVsAcid */
     , (450291,  19,     1.4) /* ArmorModVsElectric */
     , (450291, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450291,   1, 'Reinforced Shou-jen Shozoku Jacket') /* Name */
     , (450291,  33, 'HoshinoFortArmorPickup') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450291,   1, 0x020001A6) /* Setup */
     , (450291,   3, 0x20000014) /* SoundTable */
     , (450291,   6, 0x0400007E) /* PaletteBase */
     , (450291,   7, 0x10000835) /* ClothingBase */
     , (450291,   8, 0x060064E2) /* Icon */
     , (450291,  22, 0x3400002B) /* PhysicsEffectTable */;

