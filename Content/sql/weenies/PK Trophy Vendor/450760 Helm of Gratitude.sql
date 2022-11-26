DELETE FROM `weenie` WHERE `class_Id` = 450760;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450760, 'helmfiungratitudetailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450760,   1,          2) /* ItemType - Armor */
     , (450760,   3,          1) /* PaletteTemplate - AquaBlue */
     , (450760,   4,      16384) /* ClothingPriority - Head */
     , (450760,   5,        0) /* EncumbranceVal */
     , (450760,   9,          1) /* ValidLocations - HeadWear */
     , (450760,  16,          1) /* ItemUseable - No */
     , (450760,  19,       20) /* Value */
     , (450760,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450760,  27,         32) /* ArmorType - Metal */
     , (450760,  28,        0) /* ArmorLevel */
     , (450760,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450760, 150,        103) /* HookPlacement - Hook */
     , (450760, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450760,  11, True ) /* IgnoreCollisions */
     , (450760,  13, True ) /* Ethereal */
     , (450760,  14, True ) /* GravityStatus */
     , (450760,  19, True ) /* Attackable */
     , (450760,  22, True ) /* Inscribable */
     , (450760,  69, False) /* IsSellable */
     , (450760, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450760,   5,  -0.025) /* ManaRate */
     , (450760,  12,       0) /* Shade */
     , (450760,  13,   0.857) /* ArmorModVsSlash */
     , (450760,  14,   0.857) /* ArmorModVsPierce */
     , (450760,  15,    1.03) /* ArmorModVsBludgeon */
     , (450760,  16,     1.2) /* ArmorModVsCold */
     , (450760,  17,    1.03) /* ArmorModVsFire */
     , (450760,  18,     1.2) /* ArmorModVsAcid */
     , (450760,  19,     1.2) /* ArmorModVsElectric */
     , (450760, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450760,   1, 'Helm of Gratitude') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450760,   1, 0x02001337) /* Setup */
     , (450760,   3, 0x20000014) /* SoundTable */
     , (450760,   7, 0x100005D7) /* ClothingBase */
     , (450760,   8, 0x06005A54) /* Icon */
     , (450760,  22, 0x3400002B) /* PhysicsEffectTable */;


