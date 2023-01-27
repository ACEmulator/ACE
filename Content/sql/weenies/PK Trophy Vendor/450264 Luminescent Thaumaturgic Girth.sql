DELETE FROM `weenie` WHERE `class_Id` = 450264;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450264, 'girthluminbluetailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450264,   1,          2) /* ItemType - Armor */
     , (450264,   3,          2) /* PaletteTemplate - Blue */
     , (450264,   4,       2048) /* ClothingPriority - OuterwearAbdomen */
     , (450264,   5,        0) /* EncumbranceVal */
     , (450264,   8,        325) /* Mass */
     , (450264,   9,       1024) /* ValidLocations - AbdomenArmor */
     , (450264,  16,          1) /* ItemUseable - No */
     , (450264,  18,          1) /* UiEffects - Magical */
     , (450264,  19,       20) /* Value */
     , (450264,  27,         32) /* ArmorType - Metal */
     , (450264,  28,        0) /* ArmorLevel */
     , (450264,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450264,  22, True ) /* Inscribable */
     , (450264,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450264,   5,    -0.5) /* ManaRate */
     , (450264,  12,     0.5) /* Shade */
     , (450264,  13,    0.75) /* ArmorModVsSlash */
     , (450264,  14,    0.75) /* ArmorModVsPierce */
     , (450264,  15,    0.75) /* ArmorModVsBludgeon */
     , (450264,  16,    0.75) /* ArmorModVsCold */
     , (450264,  17,       1) /* ArmorModVsFire */
     , (450264,  18,       1) /* ArmorModVsAcid */
     , (450264,  19,    0.75) /* ArmorModVsElectric */
     , (450264, 110,       1) /* BulkMod */
     , (450264, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450264,   1, 'Luminescent Thaumaturgic Girth') /* Name */
     , (450264,  16, 'A richly enchanted and ornate girth once worn by the Sentinels of Perfect Light, an order dedicated to aiding Lord Asheron against the darkness. ') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450264,   1, 0x020000D7) /* Setup */
     , (450264,   3, 0x20000014) /* SoundTable */
     , (450264,   6, 0x0400007E) /* PaletteBase */
     , (450264,   7, 0x1000044D) /* ClothingBase */
     , (450264,   8, 0x06002A54) /* Icon */
     , (450264,  22, 0x3400002B) /* PhysicsEffectTable */;


