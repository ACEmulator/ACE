DELETE FROM `weenie` WHERE `class_Id` = 450135;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450135, 'coatexarchsilvertailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450135,   1,          2) /* ItemType - Armor */
     , (450135,   3,         20) /* PaletteTemplate - Silver */
     , (450135,   4,     1024) /* ClothingPriority - OuterwearChest, OuterwearUpperArms, OuterwearLowerArms */
     , (450135,   5,        0) /* EncumbranceVal */
     , (450135,   8,        700) /* Mass */
     , (450135,   9,       512) /* ValidLocations - ChestArmor, UpperArmArmor, LowerArmArmor */
     , (450135,  16,          1) /* ItemUseable - No */
     , (450135,  18,          1) /* UiEffects - Magical */
     , (450135,  19,       20) /* Value */
     , (450135,  27,         32) /* ArmorType - Metal */
     , (450135,  28,          0) /* ArmorLevel */
     , (450135,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450135,  11, True ) /* IgnoreCollisions */
     , (450135,  13, True ) /* Ethereal */
     , (450135,  14, True ) /* GravityStatus */
     , (450135,  19, True ) /* Attackable */
     , (450135,  22, True ) /* Inscribable */
     , (450135,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450135,   5,  -0.125) /* ManaRate */
     , (450135,  12,     0.5) /* Shade */
     , (450135,  13,       0) /* ArmorModVsSlash */
     , (450135,  14,       0) /* ArmorModVsPierce */
     , (450135,  15,       0) /* ArmorModVsBludgeon */
     , (450135,  16,       0) /* ArmorModVsCold */
     , (450135,  17,       0) /* ArmorModVsFire */
     , (450135,  18,       0) /* ArmorModVsAcid */
     , (450135,  19,       0) /* ArmorModVsElectric */
     , (450135, 110,       1) /* BulkMod */
     , (450135, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450135,   1, 'Exarch Plate Coat') /* Name */
     , (450135,  16, 'A heavily enchanted crystalline coat, of the type once worn into battle by the Exarchs of the Yalaini Order of Hieromancers. The seal of the Yalaini Seaborne Empire is embossed on its chest.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450135,   1, 0x020000D4) /* Setup */
     , (450135,   3, 0x20000014) /* SoundTable */
     , (450135,   6, 0x0400007E) /* PaletteBase */
     , (450135,   7, 0x10000294) /* ClothingBase */
     , (450135,   8, 0x06001F70) /* Icon */
     , (450135,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450135,  41,         34) /* ItemSpecializedOnly - WarMagic */;

