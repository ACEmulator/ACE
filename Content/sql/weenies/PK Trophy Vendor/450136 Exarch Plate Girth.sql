DELETE FROM `weenie` WHERE `class_Id` = 450136;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450136, 'girthexarchseabluetailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450136,   1,          2) /* ItemType - Armor */
     , (450136,   3,          2) /* PaletteTemplate - Blue */
     , (450136,   4,       2048) /* ClothingPriority - OuterwearAbdomen */
     , (450136,   5,         0) /* EncumbranceVal */
     , (450136,   8,        325) /* Mass */
     , (450136,   9,       1024) /* ValidLocations - AbdomenArmor */
     , (450136,  16,          1) /* ItemUseable - No */
     , (450136,  18,          1) /* UiEffects - Magical */
     , (450136,  19,       20) /* Value */
     , (450136,  27,         32) /* ArmorType - Metal */
     , (450136,  28,          0) /* ArmorLevel */
     , (450136,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450136,  22, True ) /* Inscribable */
     , (450136,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450136,   5,  -0.125) /* ManaRate */
     , (450136,  12,     0.5) /* Shade */
     , (450136,  13,       0) /* ArmorModVsSlash */
     , (450136,  14,       0) /* ArmorModVsPierce */
     , (450136,  15,       0) /* ArmorModVsBludgeon */
     , (450136,  16,       0) /* ArmorModVsCold */
     , (450136,  17,       0) /* ArmorModVsFire */
     , (450136,  18,       0) /* ArmorModVsAcid */
     , (450136,  19,       0) /* ArmorModVsElectric */
     , (450136, 110,       1) /* BulkMod */
     , (450136, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450136,   1, 'Exarch Plate Girth') /* Name */
     , (450136,  16, 'A heavily enchanted crystalline girth, of the type once worn into battle by the Exarchs of the Yalaini Order of Hieromancers.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450136,   1, 0x020000D7) /* Setup */
     , (450136,   3, 0x20000014) /* SoundTable */
     , (450136,   6, 0x0400007E) /* PaletteBase */
     , (450136,   7, 0x10000295) /* ClothingBase */
     , (450136,   8, 0x06001BCB) /* Icon */
     , (450136,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450136,  41,         34) /* ItemSpecializedOnly - WarMagic */;

