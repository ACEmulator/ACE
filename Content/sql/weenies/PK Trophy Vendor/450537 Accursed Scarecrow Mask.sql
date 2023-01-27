DELETE FROM `weenie` WHERE `class_Id` = 450537;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450537, 'ace450537-accursedscarecrowmasktailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450537,   1,          4) /* ItemType - Armor */
     , (450537,   4,      16384) /* ClothingPriority - Head */
     , (450537,   5,         0) /* EncumbranceVal */
     , (450537,   9,          1) /* ValidLocations - HeadWear */
     , (450537,  16,          1) /* ItemUseable - No */
     , (450537,  19,         20) /* Value */
     , (450537,  28,         0) /* ArmorLevel */
     , (450537,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450537, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450537,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450537,  13,    0.45) /* ArmorModVsSlash */
     , (450537,  14,    0.26) /* ArmorModVsPierce */
     , (450537,  15,    0.26) /* ArmorModVsBludgeon */
     , (450537,  16,    0.27) /* ArmorModVsCold */
     , (450537,  17,     0.5) /* ArmorModVsFire */
     , (450537,  18,    0.26) /* ArmorModVsAcid */
     , (450537,  19,    0.45) /* ArmorModVsElectric */
     , (450537, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450537,   1, 'Accursed Scarecrow Mask') /* Name */
     , (450537,  16, 'A hollowed out pumpkin that, oddly enough, fits right over your head!') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450537,   1, 0x02000B71) /* Setup */
     , (450537,   3, 0x20000014) /* SoundTable */
     , (450537,   7, 0x10000868) /* ClothingBase */
     , (450537,   8, 0x060022A2) /* Icon */
     , (450537,  22, 0x3400002B) /* PhysicsEffectTable */;
