DELETE FROM `weenie` WHERE `class_Id` = 450274;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450274, 'breastplaterenegadegeneraltailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450274,   1,          2) /* ItemType - Armor */
     , (450274,   3,          9) /* PaletteTemplate - Grey */
     , (450274,   4,       1024) /* ClothingPriority - OuterwearChest */
     , (450274,   5,        0) /* EncumbranceVal */
     , (450274,   8,       1100) /* Mass */
     , (450274,   9,        512) /* ValidLocations - ChestArmor */
     , (450274,  16,          1) /* ItemUseable - No */
     , (450274,  18,          1) /* UiEffects - Magical */
     , (450274,  19,       20) /* Value */
     , (450274,  27,         32) /* ArmorType - Metal */
     , (450274,  28,        0) /* ArmorLevel */
     , (450274,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450274,  22, True ) /* Inscribable */
     , (450274,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450274,   5,   -0.05) /* ManaRate */
     , (450274,  12,       0) /* Shade */
     , (450274,  13,     0.8) /* ArmorModVsSlash */
     , (450274,  14,     0.8) /* ArmorModVsPierce */
     , (450274,  15,     0.8) /* ArmorModVsBludgeon */
     , (450274,  16,     0.8) /* ArmorModVsCold */
     , (450274,  17,     0.8) /* ArmorModVsFire */
     , (450274,  18,     0.8) /* ArmorModVsAcid */
     , (450274,  19,     0.8) /* ArmorModVsElectric */
     , (450274, 110,       1) /* BulkMod */
     , (450274, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450274,   1, 'Ornate Tumerok Breastplate') /* Name */
     , (450274,  15, 'This breastplate was taken from the Renegade Tumerok, General Amanua.') /* ShortDesc */
     , (450274,  33, 'RenegadeBreastplateGeneral') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450274,   1, 0x020000D2) /* Setup */
     , (450274,   3, 0x20000014) /* SoundTable */
     , (450274,   6, 0x0400007E) /* PaletteBase */
     , (450274,   7, 0x1000055A) /* ClothingBase */
     , (450274,   8, 0x06003394) /* Icon */
     , (450274,  22, 0x3400002B) /* PhysicsEffectTable */;


