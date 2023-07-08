DELETE FROM `weenie` WHERE `class_Id` = 480518;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480518, 'ace480518-crestofkingspk', 1, '2021-11-01 00:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480518,   1,          2) /* ItemType - Armor */
     , (480518,   5,        0) /* EncumbranceVal */
     , (480518,   8,        460) /* Mass */
     , (480518,   9,    2097152) /* ValidLocations - Shield */
     , (480518,  16,          1) /* ItemUseable - No */
     , (480518,  19,       20) /* Value */
     , (480518,  27,          2) /* ArmorType - Leather */
     , (480518,  28,        0) /* ArmorLevel */
     , (480518,  51,          4) /* CombatUse - Shield */
     , (480518,  56,        0) /* ShieldValue */
     , (480518,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480518, 150,        103) /* HookPlacement - Hook */
     , (480518, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480518,  22, True ) /* Inscribable */
     , (480518,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480518,   5,   -0.05) /* ManaRate */
     , (480518,  13,       1) /* ArmorModVsSlash */
     , (480518,  14,     1.2) /* ArmorModVsPierce */
     , (480518,  15,     0.9) /* ArmorModVsBludgeon */
     , (480518,  16,     0.8) /* ArmorModVsCold */
     , (480518,  17,     0.5) /* ArmorModVsFire */
     , (480518,  18,       1) /* ArmorModVsAcid */
     , (480518,  19,     0.4) /* ArmorModVsElectric */
     , (480518,  39,    1) /* DefaultScale */
     , (480518, 110,       1) /* BulkMod */
     , (480518, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480518,   1, 'Crest of Kings') /* Name */
     , (480518,  15, 'A large carved shield.') /* ShortDesc */
     , (480518,  16, 'A large carved shield, with a detailed picture of a mattekar upon it.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480518,   1, 0x02000A16) /* Setup */
     , (480518,   3, 0x20000014) /* SoundTable */
     , (480518,   8, 0x06002019) /* Icon */
     , (480518,  22, 0x3400002B) /* PhysicsEffectTable */;
