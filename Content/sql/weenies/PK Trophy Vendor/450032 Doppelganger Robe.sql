DELETE FROM `weenie` WHERE `class_Id` = 450032;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450032, 'robeinfiltrationtailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450032,   1,          4) /* ItemType - Clothing */
     , (450032,   3,          4) /* PaletteTemplate - Brown */
     , (450032,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Head, Feet */
     , (450032,   5,        0) /* EncumbranceVal */
     , (450032,   8,        150) /* Mass */
     , (450032,   9,      512) /* ValidLocations - HeadWear, Armor */
     , (450032,  16,          1) /* ItemUseable - No */
     , (450032,  19,       20) /* Value */
     , (450032,  27,          1) /* ArmorType - Cloth */
     , (450032,  28,        0) /* ArmorLevel */
     , (450032,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450032, 150,        103) /* HookPlacement - Hook */
     , (450032, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450032,  22, True ) /* Inscribable */
     , (450032,  23, True ) /* DestroyOnSell */
     , (450032, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450032,   5,  -0.025) /* ManaRate */
     , (450032,  12,     0.5) /* Shade */
     , (450032,  13,     1.2) /* ArmorModVsSlash */
     , (450032,  14,     0.7) /* ArmorModVsPierce */
     , (450032,  15,     0.6) /* ArmorModVsBludgeon */
     , (450032,  16,     0.8) /* ArmorModVsCold */
     , (450032,  17,     0.6) /* ArmorModVsFire */
     , (450032,  18,     0.7) /* ArmorModVsAcid */
     , (450032,  19,     0.4) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450032,   1, 'Doppelganger Robe') /* Name */
     , (450032,  16, 'A robe looted from the corpse of the Shadow Lugian Urleg.') /* LongDesc */
     , (450032,  33, 'InfiltrationRobeAcquired0205') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450032,   1, 0x020001A6) /* Setup */
     , (450032,   3, 0x20000014) /* SoundTable */
     , (450032,   6, 0x0400007E) /* PaletteBase */
     , (450032,   7, 0x100005AA) /* ClothingBase */
     , (450032,   8, 0x06001B92) /* Icon */
     , (450032,  22, 0x3400002B) /* PhysicsEffectTable */;


