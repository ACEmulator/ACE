DELETE FROM `weenie` WHERE `class_Id` = 42149813;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (42149813, 'ace42149813-iceheaumeoffroregold', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (42149813,   1,          2) /* ItemType - Armor */
     , (42149813,   3,         21) /* PaletteTemplate - Gold */
     , (42149813,   4,      16384) /* ClothingPriority - Head */
     , (42149813,   5,       1100) /* EncumbranceVal */
     , (42149813,   8,        340) /* Mass */
     , (42149813,   9,          1) /* ValidLocations - HeadWear */
     , (42149813,  16,          1) /* ItemUseable - No */
     , (42149813,  18,        512) /* UiEffects - Bludgeoning */
     , (42149813,  19,         20) /* Value */
     , (42149813,  27,          0) /* ArmorType - None */
     , (42149813,  28,          1) /* ArmorLevel */
     , (42149813,  53,        101) /* PlacementPosition - Resting */
     , (42149813,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (42149813, 150,        103) /* HookPlacement - Hook */
     , (42149813, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (42149813,  11, True ) /* IgnoreCollisions */
     , (42149813,  13, True ) /* Ethereal */
     , (42149813,  14, True ) /* GravityStatus */
     , (42149813,  19, True ) /* Attackable */
     , (42149813,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (42149813,  12,     0.6) /* Shade */
     , (42149813,  13,     1.3) /* ArmorModVsSlash */
     , (42149813,  14,       1) /* ArmorModVsPierce */
     , (42149813,  15,     1.1) /* ArmorModVsBludgeon */
     , (42149813,  16,       2) /* ArmorModVsCold */
     , (42149813,  17,       2) /* ArmorModVsFire */
     , (42149813,  18,     0.7) /* ArmorModVsAcid */
     , (42149813,  19,       0) /* ArmorModVsElectric */
     , (42149813, 110,       1) /* BulkMod */
     , (42149813, 111,       1) /* SizeMod */
     , (42149813, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (42149813,   1, 'Ice Heaume of Frore') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (42149813,   1, 0x02000330) /* Setup */
     , (42149813,   3, 0x20000014) /* SoundTable */
     , (42149813,   6, 0x0400007E) /* PaletteBase */
     , (42149813,   7, 0x100000AD) /* ClothingBase */
     , (42149813,   8, 100669414) /* Icon */
     , (42149813,  22, 0x3400002B) /* PhysicsEffectTable */;
