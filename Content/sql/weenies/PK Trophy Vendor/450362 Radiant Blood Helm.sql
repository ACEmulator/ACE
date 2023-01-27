DELETE FROM `weenie` WHERE `class_Id` = 450362;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450362, 'ace450362-radiantbloodhelmtailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450362,   1,          2) /* ItemType - Armor */
     , (450362,   4,      16384) /* ClothingPriority - Head */
     , (450362,   5,        0) /* EncumbranceVal */
     , (450362,   9,          1) /* ValidLocations - HeadWear */
     , (450362,  16,          1) /* ItemUseable - No */
     , (450362,  18,          1) /* UiEffects - Magical */
     , (450362,  19,       20) /* Value */
     , (450362,  27,         32) /* ArmorType - Metal */
     , (450362,  28,        0) /* ArmorLevel */
     , (450362,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450362, 150,        103) /* HookPlacement - Hook */
     , (450362, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450362,  22, True ) /* Inscribable */
     , (450362, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450362,   5,  -0.067) /* ManaRate */
     , (450362,  13,     1.3) /* ArmorModVsSlash */
     , (450362,  14,       1) /* ArmorModVsPierce */
     , (450362,  15,       1) /* ArmorModVsBludgeon */
     , (450362,  16,     0.4) /* ArmorModVsCold */
     , (450362,  17,     0.4) /* ArmorModVsFire */
     , (450362,  18,     0.6) /* ArmorModVsAcid */
     , (450362,  19,     0.4) /* ArmorModVsElectric */
     , (450362, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450362,   1, 'Radiant Blood Helm') /* Name */
     , (450362,  16, 'Radiant Blood Helm') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450362,   1, 0x02001630) /* Setup */
     , (450362,   3, 0x20000014) /* SoundTable */
     , (450362,   7, 0x10000749) /* ClothingBase */
     , (450362,   8, 0x06006932) /* Icon */
     , (450362,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450362,  36, 0x0E000012) /* MutateFilter */;
