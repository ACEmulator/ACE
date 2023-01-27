DELETE FROM `weenie` WHERE `class_Id` = 450257;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450257, 'coatluminredtailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450257,   1,          2) /* ItemType - Armor */
     , (450257,   3,         14) /* PaletteTemplate - Red */
     , (450257,   4,      1024) /* ClothingPriority - OuterwearChest, OuterwearUpperArms, OuterwearLowerArms */
     , (450257,   5,        0) /* EncumbranceVal */
     , (450257,   8,        750) /* Mass */
     , (450257,   9,       512) /* ValidLocations - ChestArmor, UpperArmArmor, LowerArmArmor */
     , (450257,  16,          1) /* ItemUseable - No */
     , (450257,  18,          1) /* UiEffects - Magical */
     , (450257,  19,       20) /* Value */
     , (450257,  27,         32) /* ArmorType - Metal */
     , (450257,  28,        0) /* ArmorLevel */
     , (450257,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450257,  11, True ) /* IgnoreCollisions */
     , (450257,  13, True ) /* Ethereal */
     , (450257,  14, True ) /* GravityStatus */
     , (450257,  19, True ) /* Attackable */
     , (450257,  22, True ) /* Inscribable */
     , (450257,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450257,   5,    -0.5) /* ManaRate */
     , (450257,  12,     0.5) /* Shade */
     , (450257,  13,    0.75) /* ArmorModVsSlash */
     , (450257,  14,    0.75) /* ArmorModVsPierce */
     , (450257,  15,    0.75) /* ArmorModVsBludgeon */
     , (450257,  16,    0.75) /* ArmorModVsCold */
     , (450257,  17,       1) /* ArmorModVsFire */
     , (450257,  18,       1) /* ArmorModVsAcid */
     , (450257,  19,    0.75) /* ArmorModVsElectric */
     , (450257, 110,       1) /* BulkMod */
     , (450257, 111,       1) /* SizeMod */
     , (450257, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450257,   1, 'Luminescent Thaumaturgic Coat') /* Name */
     , (450257,  16, 'A richly enchanted and ornate coat once worn by the Sentinels of Perfect Light, an order dedicated to aiding Lord Asheron against the darkness. The seal of the Lightbringer adorns the chestplate.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450257,   1, 0x020000D4) /* Setup */
     , (450257,   3, 0x20000014) /* SoundTable */
     , (450257,   6, 0x0400007E) /* PaletteBase */
     , (450257,   7, 0x1000044B) /* ClothingBase */
     , (450257,   8, 0x06002A4E) /* Icon */
     , (450257,  22, 0x3400002B) /* PhysicsEffectTable */;


