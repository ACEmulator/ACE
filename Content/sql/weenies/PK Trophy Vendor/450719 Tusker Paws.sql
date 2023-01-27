DELETE FROM `weenie` WHERE `class_Id` = 450719;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450719, 'glovestuskertailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450719,   1,          2) /* ItemType - Armor */
     , (450719,   3,          4) /* PaletteTemplate - Brown */
     , (450719,   4,      32768) /* ClothingPriority - Hands */
     , (450719,   5,        0) /* EncumbranceVal */
     , (450719,   8,       2000) /* Mass */
     , (450719,   9,         32) /* ValidLocations - HandWear */
     , (450719,  16,          1) /* ItemUseable - No */
     , (450719,  19,       20) /* Value */
     , (450719,  27,         32) /* ArmorType - Metal */
     , (450719,  28,        0) /* ArmorLevel */
     , (450719,  44,         0) /* Damage */
     , (450719,  45,          4) /* DamageType - Bludgeon */
     , (450719,  49,        100) /* WeaponTime */
     , (450719,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450719, 150,        103) /* HookPlacement - Hook */
     , (450719, 151,          6) /* HookType - Wall, Ceiling */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450719,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450719,  12,       1) /* Shade */
     , (450719,  13,     0.8) /* ArmorModVsSlash */
     , (450719,  14,     0.8) /* ArmorModVsPierce */
     , (450719,  15,    0.66) /* ArmorModVsBludgeon */
     , (450719,  16,    0.66) /* ArmorModVsCold */
     , (450719,  17,     0.7) /* ArmorModVsFire */
     , (450719,  18,    0.44) /* ArmorModVsAcid */
     , (450719,  19,    0.24) /* ArmorModVsElectric */
     , (450719,  22,    0.75) /* DamageVariance */
     , (450719,  29,     0.8) /* WeaponDefense */
     , (450719,  62,     1.2) /* WeaponOffense */
     , (450719, 110,       1) /* BulkMod */
     , (450719, 111,       1) /* SizeMod */
     , (450719, 136,       3) /* CriticalMultiplier */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450719,   1, 'Tusker Paws') /* Name */
     , (450719,  15, 'A pair of tusker paws.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450719,   1, 0x02000E85) /* Setup */
     , (450719,   3, 0x20000014) /* SoundTable */
     , (450719,   6, 0x0400007E) /* PaletteBase */
     , (450719,   7, 0x10000434) /* ClothingBase */
     , (450719,   8, 0x0600298C) /* Icon */
     , (450719,  22, 0x3400002B) /* PhysicsEffectTable */;
