DELETE FROM `weenie` WHERE `class_Id` = 450082;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450082, 'shielddiamondtailor', 1, '2021-11-01 00:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450082,   1,          2) /* ItemType - Armor */
     , (450082,   5,        0) /* EncumbranceVal */
     , (450082,   8,        460) /* Mass */
     , (450082,   9,    2097152) /* ValidLocations - Shield */
     , (450082,  16,          1) /* ItemUseable - No */
     , (450082,  18,          1) /* UiEffects - Magical */
     , (450082,  19,       20) /* Value */
     , (450082,  27,          2) /* ArmorType - Leather */
     , (450082,  28,        0) /* ArmorLevel */
     , (450082,  51,          4) /* CombatUse - Shield */
     , (450082,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450082, 150,        103) /* HookPlacement - Hook */
     , (450082, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450082,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450082,   5,   -0.05) /* ManaRate */
     , (450082,  13,       1) /* ArmorModVsSlash */
     , (450082,  14,       1) /* ArmorModVsPierce */
     , (450082,  15,     1.2) /* ArmorModVsBludgeon */
     , (450082,  16,     0.6) /* ArmorModVsCold */
     , (450082,  17,     0.6) /* ArmorModVsFire */
     , (450082,  18,     0.6) /* ArmorModVsAcid */
     , (450082,  19,     0.6) /* ArmorModVsElectric */
     , (450082,  39,     1.5) /* DefaultScale */
     , (450082, 110,       1) /* BulkMod */
     , (450082, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450082,   1, 'Diamond Shield') /* Name */
     , (450082,  15, 'A shield made of diamond.') /* ShortDesc */
     , (450082,  16, 'A shield made of diamond.  It is incredibly resilient, and seems to be nigh unbreakable.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450082,   1, 0x02000A33) /* Setup */
     , (450082,   3, 0x20000014) /* SoundTable */
     , (450082,   6, 0x04000BEF) /* PaletteBase */
     , (450082,   7, 0x10000320) /* ClothingBase */
     , (450082,   8, 0x06002267) /* Icon */
     , (450082,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450082,  37,         48) /* ItemSkillLimit - Shield */;


