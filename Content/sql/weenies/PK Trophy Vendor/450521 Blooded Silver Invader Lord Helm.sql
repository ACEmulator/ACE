DELETE FROM `weenie` WHERE `class_Id` = 450521;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450521, 'helminvaderlordsilverbloodedtailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450521,   1,          2) /* ItemType - Armor */
     , (450521,   3,          1) /* PaletteTemplate - AquaBlue */
     , (450521,   4,      16384) /* ClothingPriority - Head */
     , (450521,   5,        0) /* EncumbranceVal */
     , (450521,   9,          1) /* ValidLocations - HeadWear */
     , (450521,  16,          1) /* ItemUseable - No */
     , (450521,  18,          1) /* UiEffects - Magical */
     , (450521,  19,       20) /* Value */
     , (450521,  28,        0) /* ArmorLevel */
     , (450521,  33,          0) /* Bonded - Normal */
     , (450521,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450521, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450521,  11, True ) /* IgnoreCollisions */
     , (450521,  13, True ) /* Ethereal */
     , (450521,  14, True ) /* GravityStatus */
     , (450521,  19, True ) /* Attackable */
     , (450521,  22, True ) /* Inscribable */
     , (450521,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450521,   5,   -0.05) /* ManaRate */
     , (450521,  12,       0) /* Shade */
     , (450521,  13,       1) /* ArmorModVsSlash */
     , (450521,  14,       1) /* ArmorModVsPierce */
     , (450521,  15,     1.2) /* ArmorModVsBludgeon */
     , (450521,  16,     0.8) /* ArmorModVsCold */
     , (450521,  17,     0.8) /* ArmorModVsFire */
     , (450521,  18,     0.7) /* ArmorModVsAcid */
     , (450521,  19,     0.7) /* ArmorModVsElectric */
     , (450521, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450521,   1, 'Blooded Silver Invader Lord Helm') /* Name */
     , (450521,  16, 'A helm taken from one of the Knights of the Viamontian Silver Legion and fortified with royal blood.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450521,   1, 0x02001339) /* Setup */
     , (450521,   3, 0x20000014) /* SoundTable */
     , (450521,   7, 0x100005D9) /* ClothingBase */
     , (450521,   8, 0x06005A8C) /* Icon */
     , (450521,  22, 0x3400002B) /* PhysicsEffectTable */;


