DELETE FROM `weenie` WHERE `class_Id` = 450134;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450134, 'coatexarchseagreytailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450134,   1,          2) /* ItemType - Armor */
     , (450134,   3,          9) /* PaletteTemplate - Grey */
     , (450134,   4,      1024) /* ClothingPriority - OuterwearChest, OuterwearUpperArms, OuterwearLowerArms */
     , (450134,   5,        0) /* EncumbranceVal */
     , (450134,   8,        700) /* Mass */
     , (450134,   9,       512) /* ValidLocations - ChestArmor, UpperArmArmor, LowerArmArmor */
     , (450134,  16,          1) /* ItemUseable - No */
     , (450134,  18,          1) /* UiEffects - Magical */
     , (450134,  19,       20) /* Value */
     , (450134,  27,         32) /* ArmorType - Metal */
     , (450134,  28,          0) /* ArmorLevel */
     , (450134,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450134,  11, True ) /* IgnoreCollisions */
     , (450134,  13, True ) /* Ethereal */
     , (450134,  14, True ) /* GravityStatus */
     , (450134,  19, True ) /* Attackable */
     , (450134,  22, True ) /* Inscribable */
     , (450134,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450134,   5,  -0.125) /* ManaRate */
     , (450134,  12,     0.5) /* Shade */
     , (450134,  13,       0) /* ArmorModVsSlash */
     , (450134,  14,       0) /* ArmorModVsPierce */
     , (450134,  15,       0) /* ArmorModVsBludgeon */
     , (450134,  16,       0) /* ArmorModVsCold */
     , (450134,  17,       0) /* ArmorModVsFire */
     , (450134,  18,       0) /* ArmorModVsAcid */
     , (450134,  19,       0) /* ArmorModVsElectric */
     , (450134, 110,       1) /* BulkMod */
     , (450134, 111,       1) /* SizeMod */
     , (450134, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450134,   1, 'Exarch Plate Coat') /* Name */
     , (450134,  16, 'A heavily enchanted crystalline coat, of the type once worn into battle by the Exarchs of the Yalaini Order of Hieromancers. The seal of the Yalaini Seaborne Empire is embossed on its chest.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450134,   1, 0x020000D4) /* Setup */
     , (450134,   3, 0x20000014) /* SoundTable */
     , (450134,   6, 0x0400007E) /* PaletteBase */
     , (450134,   7, 0x10000294) /* ClothingBase */
     , (450134,   8, 0x06001F6F) /* Icon */
     , (450134,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450134,  41,         34) /* ItemSpecializedOnly - WarMagic */;
