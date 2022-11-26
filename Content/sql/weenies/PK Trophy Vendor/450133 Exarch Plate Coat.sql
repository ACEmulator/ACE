DELETE FROM `weenie` WHERE `class_Id` = 450133;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450133, 'coatexarchseabluetailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450133,   1,          2) /* ItemType - Armor */
     , (450133,   3,          2) /* PaletteTemplate - Blue */
     , (450133,   4,      1024) /* ClothingPriority - OuterwearChest, OuterwearUpperArms, OuterwearLowerArms */
     , (450133,   5,        0) /* EncumbranceVal */
     , (450133,   8,        700) /* Mass */
     , (450133,   9,       512) /* ValidLocations - ChestArmor, UpperArmArmor, LowerArmArmor */
     , (450133,  16,          1) /* ItemUseable - No */
     , (450133,  18,          1) /* UiEffects - Magical */
     , (450133,  19,       20) /* Value */
     , (450133,  27,         32) /* ArmorType - Metal */
     , (450133,  28,          0) /* ArmorLevel */
     , (450133,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450133,  11, True ) /* IgnoreCollisions */
     , (450133,  13, True ) /* Ethereal */
     , (450133,  14, True ) /* GravityStatus */
     , (450133,  19, True ) /* Attackable */
     , (450133,  22, True ) /* Inscribable */
     , (450133,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450133,   5,  -0.125) /* ManaRate */
     , (450133,  12,     0.5) /* Shade */
     , (450133,  13,       0) /* ArmorModVsSlash */
     , (450133,  14,       0) /* ArmorModVsPierce */
     , (450133,  15,       0) /* ArmorModVsBludgeon */
     , (450133,  16,       0) /* ArmorModVsCold */
     , (450133,  17,       0) /* ArmorModVsFire */
     , (450133,  18,       0) /* ArmorModVsAcid */
     , (450133,  19,       0) /* ArmorModVsElectric */
     , (450133, 110,       1) /* BulkMod */
     , (450133, 111,       1) /* SizeMod */
     , (450133, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450133,   1, 'Exarch Plate Coat') /* Name */
     , (450133,  16, 'A heavily enchanted crystalline coat, of the type once worn into battle by the Exarchs of the Yalaini Order of Hieromancers. The seal of the Yalaini Seaborne Empire is embossed on its chest.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450133,   1, 0x020000D4) /* Setup */
     , (450133,   3, 0x20000014) /* SoundTable */
     , (450133,   6, 0x0400007E) /* PaletteBase */
     , (450133,   7, 0x10000294) /* ClothingBase */
     , (450133,   8, 0x06001F6E) /* Icon */
     , (450133,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450133,  41,         34) /* ItemSpecializedOnly - WarMagic */;


