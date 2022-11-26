DELETE FROM `weenie` WHERE `class_Id` = 450722;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450722, 'glovesspellcastingtailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450722,   1,          2) /* ItemType - Armor */
     , (450722,   4,      32768) /* ClothingPriority - Hands */
     , (450722,   5,        0) /* EncumbranceVal */
     , (450722,   9,         32) /* ValidLocations - HandWear */
     , (450722,  16,          1) /* ItemUseable - No */
     , (450722,  19,      20) /* Value */
     , (450722,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450722,  28,        0) /* ArmorLevel */
     , (450722,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450722,  11, True ) /* IgnoreCollisions */
     , (450722,  13, True ) /* Ethereal */
     , (450722,  14, True ) /* GravityStatus */
     , (450722,  19, True ) /* Attackable */
     , (450722,  22, True ) /* Inscribable */
     , (450722,  69, False) /* IsSellable */
     , (450722, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450722,   5,  -0.017) /* ManaRate */
     , (450722,  13,     0.6) /* ArmorModVsSlash */
     , (450722,  14,     0.6) /* ArmorModVsPierce */
     , (450722,  15,     0.6) /* ArmorModVsBludgeon */
     , (450722,  16,     1.4) /* ArmorModVsCold */
     , (450722,  17,     0.3) /* ArmorModVsFire */
     , (450722,  18,     0.6) /* ArmorModVsAcid */
     , (450722,  19,     1.2) /* ArmorModVsElectric */
     , (450722,  22,       0) /* DamageVariance */
     , (450722, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450722,   1, 'Fiun Spellcasting Gloves') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450722,   1, 0x02001334) /* Setup */
     , (450722,   3, 0x20000014) /* SoundTable */
     , (450722,   6, 0x0400007E) /* PaletteBase */
     , (450722,   7, 0x100005D5) /* ClothingBase */
     , (450722,   8, 0x06005A3E) /* Icon */
     , (450722,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450722,  36, 0x0E000012) /* MutateFilter */
     , (450722,  46, 0x38000032) /* TsysMutationFilter */;

