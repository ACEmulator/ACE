DELETE FROM `weenie` WHERE `class_Id` = 450729;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450729, 'bootsursuintailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450729,   1,          2) /* ItemType - Armor */
     , (450729,   3,          6) /* PaletteTemplate - DeepBrown */
     , (450729,   4,      65536) /* ClothingPriority - Feet */
     , (450729,   5,        0) /* EncumbranceVal */
     , (450729,   8,        140) /* Mass */
     , (450729,   9,        384) /* ValidLocations - LowerLegWear, FootWear */
     , (450729,  16,          1) /* ItemUseable - No */
     , (450729,  19,        20) /* Value */
     , (450729,  27,          2) /* ArmorType - Leather */
     , (450729,  28,         0) /* ArmorLevel */
     , (450729,  44,          2) /* Damage */
     , (450729,  45,          4) /* DamageType - Bludgeon */
     , (450729,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450729, 150,        103) /* HookPlacement - Hook */
     , (450729, 151,          1) /* HookType - Floor */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450729,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450729,  12,     0.6) /* Shade */
     , (450729,  13,       1) /* ArmorModVsSlash */
     , (450729,  14,     0.8) /* ArmorModVsPierce */
     , (450729,  15,       1) /* ArmorModVsBludgeon */
     , (450729,  16,     0.5) /* ArmorModVsCold */
     , (450729,  17,     0.5) /* ArmorModVsFire */
     , (450729,  18,     0.3) /* ArmorModVsAcid */
     , (450729,  19,     0.6) /* ArmorModVsElectric */
     , (450729,  22,    0.75) /* DamageVariance */
     , (450729, 110,       1) /* BulkMod */
     , (450729, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450729,   1, 'Ursuin Boots') /* Name */
     , (450729,  16, 'A pair of shaggy boots, sewn and laced with the cured hide of an Ursuin.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450729,   1, 0x020008CB) /* Setup */
     , (450729,   3, 0x20000014) /* SoundTable */
     , (450729,   6, 0x0400007E) /* PaletteBase */
     , (450729,   7, 0x100002B0) /* ClothingBase */
     , (450729,   8, 0x06000FAE) /* Icon */
     , (450729,  22, 0x3400002B) /* PhysicsEffectTable */;
