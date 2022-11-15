DELETE FROM `weenie` WHERE `class_Id` = 450138;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450138, 'girthexarchsilvertailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450138,   1,          2) /* ItemType - Armor */
     , (450138,   3,         20) /* PaletteTemplate - Silver */
     , (450138,   4,       2048) /* ClothingPriority - OuterwearAbdomen */
     , (450138,   5,         0) /* EncumbranceVal */
     , (450138,   8,        325) /* Mass */
     , (450138,   9,       1024) /* ValidLocations - AbdomenArmor */
     , (450138,  16,          1) /* ItemUseable - No */
     , (450138,  18,          1) /* UiEffects - Magical */
     , (450138,  19,       20) /* Value */
     , (450138,  27,         32) /* ArmorType - Metal */
     , (450138,  28,          0) /* ArmorLevel */
     , (450138,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450138,  22, True ) /* Inscribable */
     , (450138,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450138,   5,  -0.125) /* ManaRate */
     , (450138,  12,     0.5) /* Shade */
     , (450138,  13,       0) /* ArmorModVsSlash */
     , (450138,  14,       0) /* ArmorModVsPierce */
     , (450138,  15,       0) /* ArmorModVsBludgeon */
     , (450138,  16,       0) /* ArmorModVsCold */
     , (450138,  17,       0) /* ArmorModVsFire */
     , (450138,  18,       0) /* ArmorModVsAcid */
     , (450138,  19,       0) /* ArmorModVsElectric */
     , (450138, 110,       1) /* BulkMod */
     , (450138, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450138,   1, 'Exarch Plate Girth') /* Name */
     , (450138,  16, 'A heavily enchanted crystalline girth, of the type once worn into battle by the Exarchs of the Yalaini Order of Hieromancers.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450138,   1, 0x020000D7) /* Setup */
     , (450138,   3, 0x20000014) /* SoundTable */
     , (450138,   6, 0x0400007E) /* PaletteBase */
     , (450138,   7, 0x10000295) /* ClothingBase */
     , (450138,   8, 0x06001BCB) /* Icon */
     , (450138,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450138,  41,         34) /* ItemSpecializedOnly - WarMagic */;


