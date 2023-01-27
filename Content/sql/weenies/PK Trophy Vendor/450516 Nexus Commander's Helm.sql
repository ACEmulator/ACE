DELETE FROM `weenie` WHERE `class_Id` = 450516;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450516, 'ace450516-nexuscommandershelmtailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450516,   1,          2) /* ItemType - Armor */
     , (450516,   3,          2) /* PaletteTemplate - Blue */
     , (450516,   4,      16384) /* ClothingPriority - Head */
     , (450516,   5,        0) /* EncumbranceVal */
     , (450516,   9,          1) /* ValidLocations - HeadWear */
     , (450516,  16,          1) /* ItemUseable - No */
     , (450516,  18,          1) /* UiEffects - Magical */
     , (450516,  19,       20) /* Value */
     , (450516,  28,        0) /* ArmorLevel */
     , (450516,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450516, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450516,  11, True ) /* IgnoreCollisions */
     , (450516,  13, True ) /* Ethereal */
     , (450516,  14, True ) /* GravityStatus */
     , (450516,  19, True ) /* Attackable */
     , (450516,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450516,   5,   -0.05) /* ManaRate */
     , (450516,  12,   0.667) /* Shade */
     , (450516,  13,       1) /* ArmorModVsSlash */
     , (450516,  14,       1) /* ArmorModVsPierce */
     , (450516,  15,       1) /* ArmorModVsBludgeon */
     , (450516,  16,     0.9) /* ArmorModVsCold */
     , (450516,  17,     0.5) /* ArmorModVsFire */
     , (450516,  18,     0.4) /* ArmorModVsAcid */
     , (450516,  19,     0.9) /* ArmorModVsElectric */
     , (450516, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450516,   1, 'Nexus Commander''s Helm') /* Name */
     , (450516,  16, 'A helm taken from the Commander of the Viamontian Knights in the Nexus.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450516,   1, 0x02000978) /* Setup */
     , (450516,   3, 0x20000014) /* SoundTable */
     , (450516,   6, 0x0400007E) /* PaletteBase */
     , (450516,   7, 0x10000640) /* ClothingBase */
     , (450516,   8, 0x0600619B) /* Icon */
     , (450516,  22, 0x3400002B) /* PhysicsEffectTable */;


