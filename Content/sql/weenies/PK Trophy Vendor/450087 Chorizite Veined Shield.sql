DELETE FROM `weenie` WHERE `class_Id` = 450087;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450087, 'shieldrenegadetailor', 1, '2021-11-01 00:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450087,   1,          2) /* ItemType - Armor */
     , (450087,   5,       0) /* EncumbranceVal */
     , (450087,   8,        140) /* Mass */
     , (450087,   9,    2097152) /* ValidLocations - Shield */
     , (450087,  16,          1) /* ItemUseable - No */
     , (450087,  19,       20) /* Value */
     , (450087,  27,          2) /* ArmorType - Leather */
     , (450087,  28,        0) /* ArmorLevel */
     , (450087,  36,       9999) /* ResistMagic */
     , (450087,  51,          4) /* CombatUse - Shield */
     , (450087,  56,        310) /* ShieldValue */
     , (450087,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450087, 150,        103) /* HookPlacement - Hook */
     , (450087, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450087,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450087,   5,       0) /* ManaRate */
     , (450087,  13,       1) /* ArmorModVsSlash */
     , (450087,  14,       1) /* ArmorModVsPierce */
     , (450087,  15,       1) /* ArmorModVsBludgeon */
     , (450087,  16,    0.85) /* ArmorModVsCold */
     , (450087,  17,       1) /* ArmorModVsFire */
     , (450087,  18,       1) /* ArmorModVsAcid */
     , (450087,  19,       1) /* ArmorModVsElectric */
     , (450087,  39,       1) /* DefaultScale */
     , (450087, 110,       1) /* BulkMod */
     , (450087, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450087,   1, 'Chorizite Veined Shield') /* Name */
     , (450087,  15, 'Chorizite has been pounded into this shield. The shield cannot received further enchantment from spells and seems to hinder the access to mana while offering better defense to magical attacks.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450087,   1, 0x020010D5) /* Setup */
     , (450087,   3, 0x20000014) /* SoundTable */
     , (450087,   8, 0x06003389) /* Icon */
     , (450087,  22, 0x3400002B) /* PhysicsEffectTable */;

