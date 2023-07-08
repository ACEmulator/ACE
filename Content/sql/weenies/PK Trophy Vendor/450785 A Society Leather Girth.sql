DELETE FROM `weenie` WHERE `class_Id` = 450785;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450785, 'girthleathernewbiequestPK', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450785,   1,          2) /* ItemType - Armor */
     , (450785,   3,          1) /* PaletteTemplate - AquaBlue */
     , (450785,   4,       2048) /* ClothingPriority - OuterwearAbdomen */
     , (450785,   5,        0) /* EncumbranceVal */
     , (450785,   8,         90) /* Mass */
     , (450785,   9,       1024) /* ValidLocations - AbdomenArmor */
     , (450785,  16,          1) /* ItemUseable - No */
     , (450785,  18,          1) /* UiEffects - Magical */
     , (450785,  19,          20) /* Value */
     , (450785,  27,          2) /* ArmorType - Leather */
     , (450785,  28,        0) /* ArmorLevel */
     , (450785,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450785,  22, True ) /* Inscribable */
     , (450785, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450785,   5,  -0.025) /* ManaRate */
     , (450785,  12,     0.3) /* Shade */
     , (450785,  13,       1) /* ArmorModVsSlash */
     , (450785,  14,       1) /* ArmorModVsPierce */
     , (450785,  15,       1) /* ArmorModVsBludgeon */
     , (450785,  16,     0.6) /* ArmorModVsCold */
     , (450785,  17,     0.6) /* ArmorModVsFire */
     , (450785,  18,     0.6) /* ArmorModVsAcid */
     , (450785,  19,     0.6) /* ArmorModVsElectric */
     , (450785, 110,       1) /* BulkMod */
     , (450785, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450785,   1, 'A Society Leather Girth') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450785,   1, 0x020000D7) /* Setup */
     , (450785,   3, 0x20000014) /* SoundTable */
     , (450785,   6, 0x0400007E) /* PaletteBase */
     , (450785,   7, 0x10000043) /* ClothingBase */
     , (450785,   8, 0x060012EF) /* Icon */
     , (450785,  22, 0x3400002B) /* PhysicsEffectTable */;

