DELETE FROM `weenie` WHERE `class_Id` = 450131;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450131, 'ace450131-visageofmenileshtailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450131,   1,          2) /* ItemType - Armor */
     , (450131,   4,      16384) /* ClothingPriority - Head */
     , (450131,   5,        0) /* EncumbranceVal */
     , (450131,   9,          1) /* ValidLocations - HeadWear */
     , (450131,  16,          1) /* ItemUseable - No */
     , (450131,  18,          1) /* UiEffects - Magical */
     , (450131,  19,          20) /* Value */
     , (450131,  28,        0) /* ArmorLevel */
     , (450131,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450131, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450131,  22, True ) /* Inscribable */
     , (450131,  23, True ) /* DestroyOnSell */
     , (450131,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450131,   5,   -0.05) /* ManaRate */
     , (450131,  13,     1.1) /* ArmorModVsSlash */
     , (450131,  14,       1) /* ArmorModVsPierce */
     , (450131,  15,     0.8) /* ArmorModVsBludgeon */
     , (450131,  16,     1.1) /* ArmorModVsCold */
     , (450131,  17,     0.7) /* ArmorModVsFire */
     , (450131,  18,       1) /* ArmorModVsAcid */
     , (450131,  19,     0.8) /* ArmorModVsElectric */
     , (450131, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450131,   1, 'Visage of Menilesh') /* Name */
     , (450131,  16, 'This armored mask was once a symbol of the office of Rytheran, the Dericostian lord of Menilesh.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450131,   1, 0x0200183B) /* Setup */
     , (450131,   3, 0x20000014) /* SoundTable */
     , (450131,   7, 0x1000072F) /* ClothingBase */
     , (450131,   8, 0x0600681A) /* Icon */
     , (450131,  22, 0x3400002B) /* PhysicsEffectTable */;

