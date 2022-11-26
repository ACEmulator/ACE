DELETE FROM `weenie` WHERE `class_Id` = 450726;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450726, 'ace450726-machinistsglovestailor', 2, '2021-12-14 05:15:31') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450726,   1,          2) /* ItemType - Armor */
     , (450726,   3,          4) /* PaletteTemplate - Brown */
     , (450726,   4,      32768) /* ClothingPriority - Hands */
     , (450726,   5,        0) /* EncumbranceVal */
     , (450726,   8,         90) /* Mass */
     , (450726,   9,         32) /* ValidLocations - HandWear */
     , (450726,  16,          1) /* ItemUseable - No */
     , (450726,  19,       20) /* Value */
     , (450726,  27,          2) /* ArmorType - Leather */
     , (450726,  28,        0) /* ArmorLevel */
     , (450726,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450726, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450726,  22, True ) /* Inscribable */
     , (450726,  69, False) /* IsSellable */
     , (450726, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450726,   5,  -0.017) /* ManaRate */
     , (450726,  12,    0.66) /* Shade */
     , (450726,  13,     1.2) /* ArmorModVsSlash */
     , (450726,  14,     1.2) /* ArmorModVsPierce */
     , (450726,  15,     1.4) /* ArmorModVsBludgeon */
     , (450726,  16,     1.4) /* ArmorModVsCold */
     , (450726,  17,       1) /* ArmorModVsFire */
     , (450726,  18,     0.8) /* ArmorModVsAcid */
     , (450726,  19,     0.8) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450726,   1, 'Machinist''s Gloves') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450726,   1, 0x020000D8) /* Setup */
     , (450726,   3, 0x20000014) /* SoundTable */
     , (450726,   6, 0x0400007E) /* PaletteBase */
     , (450726,   7, 0x100004E4) /* ClothingBase */
     , (450726,   8, 0x06002E91) /* Icon */
     , (450726,  22, 0x3400002B) /* PhysicsEffectTable */;


