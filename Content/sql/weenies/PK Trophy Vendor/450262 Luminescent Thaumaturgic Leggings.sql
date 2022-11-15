DELETE FROM `weenie` WHERE `class_Id` = 450262;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450262, 'leggingslumingreentailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450262,   1,          2) /* ItemType - Armor */
     , (450262,   3,          8) /* PaletteTemplate - Green */
     , (450262,   4,        768) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs */
     , (450262,   5,        0) /* EncumbranceVal */
     , (450262,   8,        500) /* Mass */
     , (450262,   9,      24576) /* ValidLocations - UpperLegArmor, LowerLegArmor */
     , (450262,  16,          1) /* ItemUseable - No */
     , (450262,  18,          1) /* UiEffects - Magical */
     , (450262,  19,       20) /* Value */
     , (450262,  27,         32) /* ArmorType - Metal */
     , (450262,  28,        0) /* ArmorLevel */
     , (450262,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450262,  22, True ) /* Inscribable */
     , (450262,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450262,   5,    -0.5) /* ManaRate */
     , (450262,  12,     0.5) /* Shade */
     , (450262,  13,    0.75) /* ArmorModVsSlash */
     , (450262,  14,    0.75) /* ArmorModVsPierce */
     , (450262,  15,    0.75) /* ArmorModVsBludgeon */
     , (450262,  16,    0.75) /* ArmorModVsCold */
     , (450262,  17,       1) /* ArmorModVsFire */
     , (450262,  18,       1) /* ArmorModVsAcid */
     , (450262,  19,    0.75) /* ArmorModVsElectric */
     , (450262, 110,       1) /* BulkMod */
     , (450262, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450262,   1, 'Luminescent Thaumaturgic Leggings') /* Name */
     , (450262,  16, 'A richly enchanted and ornate pair of leggings once worn by the Sentinels of Perfect Light, an order dedicated to aiding Lord Asheron against the darkness.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450262,   1, 0x020001A8) /* Setup */
     , (450262,   3, 0x20000014) /* SoundTable */
     , (450262,   6, 0x0400007E) /* PaletteBase */
     , (450262,   7, 0x1000044F) /* ClothingBase */
     , (450262,   8, 0x06002A5E) /* Icon */
     , (450262,  22, 0x3400002B) /* PhysicsEffectTable */;


