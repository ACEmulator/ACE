DELETE FROM `weenie` WHERE `class_Id` = 450091;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450091, 'shieldinfiltrationtailor', 1, '2005-02-09 10:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450091,   1,          2) /* ItemType - Armor */
     , (450091,   5,       0) /* EncumbranceVal */
     , (450091,   8,        230) /* Mass */
     , (450091,   9,    2097152) /* ValidLocations - Shield */
     , (450091,  16,          1) /* ItemUseable - No */
     , (450091,  19,       20) /* Value */
     , (450091,  27,          2) /* ArmorType - Leather */
     , (450091,  28,        0) /* ArmorLevel */
     , (450091,  36,       9999) /* ResistMagic */
     , (450091,  51,          4) /* CombatUse - Shield */
     , (450091,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450091, 150,        103) /* HookPlacement - Hook */
     , (450091, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450091,  22, True ) /* Inscribable */
     , (450091,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450091,   5,  -0.025) /* ManaRate */
     , (450091,  13,     1.3) /* ArmorModVsSlash */
     , (450091,  14,     1.2) /* ArmorModVsPierce */
     , (450091,  15,     1.3) /* ArmorModVsBludgeon */
     , (450091,  16,     1.2) /* ArmorModVsCold */
     , (450091,  17,     1.3) /* ArmorModVsFire */
     , (450091,  18,     1.2) /* ArmorModVsAcid */
     , (450091,  19,       1) /* ArmorModVsElectric */
     , (450091, 110,       1) /* BulkMod */
     , (450091, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450091,   1, 'Doppelganger Shield') /* Name */
     , (450091,  16, 'A shield looted from the corpse of the Shadow Lugian Shoktok.') /* LongDesc */
     , (450091,  33, 'InfiltrationShieldAcquired0205') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450091,   1, 0x020012D4) /* Setup */
     , (450091,   3, 0x20000014) /* SoundTable */
     , (450091,   8, 0x06003759) /* Icon */
     , (450091,  22, 0x3400002B) /* PhysicsEffectTable */;


