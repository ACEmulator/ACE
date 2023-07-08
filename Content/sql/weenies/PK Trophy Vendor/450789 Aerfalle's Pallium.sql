DELETE FROM `weenie` WHERE `class_Id` = 450789;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450789, 'robeaerfallepk', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450789,   1,          4) /* ItemType - Clothing */
     , (450789,   3,         39) /* PaletteTemplate - Black */
     , (450789,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Feet */
     , (450789,   5,        0) /* EncumbranceVal */
     , (450789,   8,        150) /* Mass */
     , (450789,   9,      512) /* ValidLocations - Armor */
     , (450789,  16,          1) /* ItemUseable - No */
     , (450789,  18,          1) /* UiEffects - Magical */
     , (450789,  19,      20) /* Value */
     , (450789,  27,          1) /* ArmorType - Cloth */
     , (450789,  28,          0) /* ArmorLevel */
     , (450789,  36,       9999) /* ResistMagic */
     , (450789,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450789, 150,        103) /* HookPlacement - Hook */
     , (450789, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450789,  22, True ) /* Inscribable */
     , (450789,  23, True ) /* DestroyOnSell */
     , (450789,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450789,   5,   -0.05) /* ManaRate */
     , (450789,  12,     0.1) /* Shade */
     , (450789,  13,     0.8) /* ArmorModVsSlash */
     , (450789,  14,     0.8) /* ArmorModVsPierce */
     , (450789,  15,       1) /* ArmorModVsBludgeon */
     , (450789,  16,     0.8) /* ArmorModVsCold */
     , (450789,  17,     0.8) /* ArmorModVsFire */
     , (450789,  18,     0.8) /* ArmorModVsAcid */
     , (450789,  19,     0.8) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450789,   1, 'Aerfalle''s Pallium') /* Name */
     , (450789,  15, 'A black robe, woven from unusual material.') /* ShortDesc */
     , (450789,  16, 'A black robe which seems to have threads of chorizite woven into it. This item cannot be enchanted.') /* LongDesc */
     , (450789,  33, 'PalliumObtained') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450789,   1, 0x020001A6) /* Setup */
     , (450789,   3, 0x20000014) /* SoundTable */
     , (450789,   6, 0x0400007E) /* PaletteBase */
     , (450789,   7, 0x1000018D) /* ClothingBase */
     , (450789,   8, 0x06001B8E) /* Icon */
     , (450789,  22, 0x3400002B) /* PhysicsEffectTable */;

