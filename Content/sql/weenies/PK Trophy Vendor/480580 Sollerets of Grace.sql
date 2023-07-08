DELETE FROM `weenie` WHERE `class_Id` = 480580;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480580, 'solleretsgracepk', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480580,   1,          2) /* ItemType - Armor */
     , (480580,   3,         20) /* PaletteTemplate - Silver */
     , (480580,   4,      65536) /* ClothingPriority - Feet */
     , (480580,   5,        0) /* EncumbranceVal */
     , (480580,   8,        360) /* Mass */
     , (480580,   9,        256) /* ValidLocations - FootWear */
     , (480580,  16,          1) /* ItemUseable - No */
     , (480580,  19,       20) /* Value */
     , (480580,  27,         32) /* ArmorType - Metal */
     , (480580,  28,        0) /* ArmorLevel */
     , (480580,  44,          3) /* Damage */
     , (480580,  45,          4) /* DamageType - Bludgeon */
     , (480580,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480580,  22, True ) /* Inscribable */
     , (480580,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480580,   5,  -0.033) /* ManaRate */
     , (480580,  12,    0.66) /* Shade */
     , (480580,  13,     1.2) /* ArmorModVsSlash */
     , (480580,  14,     0.8) /* ArmorModVsPierce */
     , (480580,  15,     0.8) /* ArmorModVsBludgeon */
     , (480580,  16,     1.2) /* ArmorModVsCold */
     , (480580,  17,     1.2) /* ArmorModVsFire */
     , (480580,  18,     0.6) /* ArmorModVsAcid */
     , (480580,  19,     0.6) /* ArmorModVsElectric */
     , (480580,  22,    0.75) /* DamageVariance */
     , (480580, 110,       1) /* BulkMod */
     , (480580, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480580,   1, 'Sollerets of Grace') /* Name */
     , (480580,  16, 'The sollerets represent the teachings of the adherents to the path of the Unicorn. They are lightweight and grant the gift of grace to the wearer.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480580,   1, 0x020000DE) /* Setup */
     , (480580,   3, 0x20000014) /* SoundTable */
     , (480580,   6, 0x0400007E) /* PaletteBase */
     , (480580,   7, 0x10000510) /* ClothingBase */
     , (480580,   8, 0x06003006) /* Icon */
     , (480580,  22, 0x3400002B) /* PhysicsEffectTable */;
