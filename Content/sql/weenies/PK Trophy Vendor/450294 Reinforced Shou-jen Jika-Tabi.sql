DELETE FROM `weenie` WHERE `class_Id` = 450294;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450294, 'ace450294-reinforcedshoujenjikatabitailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450294,   1,          2) /* ItemType - Armor */
     , (450294,   3,          9) /* PaletteTemplate - Grey */
     , (450294,   4,      65536) /* ClothingPriority - Feet */
     , (450294,   5,        0) /* EncumbranceVal */
     , (450294,   9,        384) /* ValidLocations - LowerLegWear, FootWear */
     , (450294,  16,          1) /* ItemUseable - No */
     , (450294,  18,          1) /* UiEffects - Magical */
     , (450294,  19,      20) /* Value */
     , (450294,  28,        0) /* ArmorLevel */
     , (450294,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450294,  11, True ) /* IgnoreCollisions */
     , (450294,  13, True ) /* Ethereal */
     , (450294,  14, True ) /* GravityStatus */
     , (450294,  19, True ) /* Attackable */
     , (450294,  22, True ) /* Inscribable */
     , (450294, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450294,   5,  -0.017) /* ManaRate */
     , (450294,  12,       0) /* Shade */
     , (450294,  13,     0.6) /* ArmorModVsSlash */
     , (450294,  14,     0.6) /* ArmorModVsPierce */
     , (450294,  15,     0.6) /* ArmorModVsBludgeon */
     , (450294,  16,     1.4) /* ArmorModVsCold */
     , (450294,  17,     0.7) /* ArmorModVsFire */
     , (450294,  18,     1.2) /* ArmorModVsAcid */
     , (450294,  19,     1.4) /* ArmorModVsElectric */
     , (450294, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450294,   1, 'Reinforced Shou-jen Jika-Tabi') /* Name */
     , (450294,  33, 'HoshinoFortArmorPickup') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450294,   1, 0x020008CB) /* Setup */
     , (450294,   3, 0x20000014) /* SoundTable */
     , (450294,   6, 0x0400007E) /* PaletteBase */
     , (450294,   7, 0x10000834) /* ClothingBase */
     , (450294,   8, 0x060064E1) /* Icon */
     , (450294,  22, 0x3400002B) /* PhysicsEffectTable */;


