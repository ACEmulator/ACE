DELETE FROM `weenie` WHERE `class_Id` = 450137;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450137, 'girthexarchseagreytailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450137,   1,          2) /* ItemType - Armor */
     , (450137,   3,          9) /* PaletteTemplate - Grey */
     , (450137,   4,       2048) /* ClothingPriority - OuterwearAbdomen */
     , (450137,   5,         0) /* EncumbranceVal */
     , (450137,   8,        325) /* Mass */
     , (450137,   9,       1024) /* ValidLocations - AbdomenArmor */
     , (450137,  16,          1) /* ItemUseable - No */
     , (450137,  18,          1) /* UiEffects - Magical */
     , (450137,  19,       20) /* Value */
     , (450137,  27,         32) /* ArmorType - Metal */
     , (450137,  28,          0) /* ArmorLevel */
     , (450137,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450137,  22, True ) /* Inscribable */
     , (450137,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450137,   5,  -0.125) /* ManaRate */
     , (450137,  12,     0.5) /* Shade */
     , (450137,  13,       0) /* ArmorModVsSlash */
     , (450137,  14,       0) /* ArmorModVsPierce */
     , (450137,  15,       0) /* ArmorModVsBludgeon */
     , (450137,  16,       0) /* ArmorModVsCold */
     , (450137,  17,       0) /* ArmorModVsFire */
     , (450137,  18,       0) /* ArmorModVsAcid */
     , (450137,  19,       0) /* ArmorModVsElectric */
     , (450137, 110,       1) /* BulkMod */
     , (450137, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450137,   1, 'Exarch Plate Girth') /* Name */
     , (450137,  16, 'A heavily enchanted crystalline girth, of the type once worn into battle by the Exarchs of the Yalaini Order of Hieromancers.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450137,   1, 0x020000D7) /* Setup */
     , (450137,   3, 0x20000014) /* SoundTable */
     , (450137,   6, 0x0400007E) /* PaletteBase */
     , (450137,   7, 0x10000295) /* ClothingBase */
     , (450137,   8, 0x06001BCB) /* Icon */
     , (450137,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450137,  41,         34) /* ItemSpecializedOnly - WarMagic */;


