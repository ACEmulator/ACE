DELETE FROM `weenie` WHERE `class_Id` = 450145;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450145, 'girthaurorbluetailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450145,   1,          2) /* ItemType - Armor */
     , (450145,   3,          2) /* PaletteTemplate - Blue */
     , (450145,   4,       2048) /* ClothingPriority - OuterwearAbdomen */
     , (450145,   5,        0) /* EncumbranceVal */
     , (450145,   8,        325) /* Mass */
     , (450145,   9,       1024) /* ValidLocations - AbdomenArmor */
     , (450145,  16,          1) /* ItemUseable - No */
     , (450145,  18,          1) /* UiEffects - Magical */
     , (450145,  19,       20) /* Value */
     , (450145,  27,         32) /* ArmorType - Metal */
     , (450145,  28,        0) /* ArmorLevel */
     , (450145,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450145,  22, True ) /* Inscribable */
     , (450145,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450145,   5,    -0.5) /* ManaRate */
     , (450145,  12,     0.5) /* Shade */
     , (450145,  13,    0.75) /* ArmorModVsSlash */
     , (450145,  14,    0.75) /* ArmorModVsPierce */
     , (450145,  15,    0.75) /* ArmorModVsBludgeon */
     , (450145,  16,    0.75) /* ArmorModVsCold */
     , (450145,  17,       1) /* ArmorModVsFire */
     , (450145,  18,       1) /* ArmorModVsAcid */
     , (450145,  19,    0.75) /* ArmorModVsElectric */
     , (450145, 110,       1) /* BulkMod */
     , (450145, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450145,   1, 'Auroric Exarch Girth') /* Name */
     , (450145,  16, 'A richly enchanted and ornate girth once worn by the Sentinels of Perfect Light, an order dedicated to aiding Lord Asheron against the darkness.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450145,   1, 0x020000D7) /* Setup */
     , (450145,   3, 0x20000014) /* SoundTable */
     , (450145,   6, 0x0400007E) /* PaletteBase */
     , (450145,   7, 0x1000044C) /* ClothingBase */
     , (450145,   8, 0x06002A53) /* Icon */
     , (450145,  22, 0x3400002B) /* PhysicsEffectTable */;

