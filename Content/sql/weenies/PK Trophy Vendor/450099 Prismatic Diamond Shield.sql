DELETE FROM `weenie` WHERE `class_Id` = 450099;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450099, 'ace450099-prismaticdiamondshieldtailor', 1, '2021-11-01 00:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450099,   1,          2) /* ItemType - Armor */
     , (450099,   5,        0) /* EncumbranceVal */
     , (450099,   8,        460) /* Mass */
     , (450099,   9,    2097152) /* ValidLocations - Shield */
     , (450099,  16,          1) /* ItemUseable - No */
     , (450099,  18,          1) /* UiEffects - Magical */
     , (450099,  19,       20) /* Value */
     , (450099,  27,          2) /* ArmorType - Leather */
     , (450099,  28,        0) /* ArmorLevel */
     , (450099,  51,          4) /* CombatUse - Shield */
     , (450099,  56,        180) /* ShieldValue */
     , (450099,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450099, 150,        103) /* HookPlacement - Hook */
     , (450099, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450099,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450099,   5,   -0.05) /* ManaRate */
     , (450099,  13,       1) /* ArmorModVsSlash */
     , (450099,  14,       1) /* ArmorModVsPierce */
     , (450099,  15,     1.2) /* ArmorModVsBludgeon */
     , (450099,  16,       2) /* ArmorModVsCold */
     , (450099,  17,       2) /* ArmorModVsFire */
     , (450099,  18,       2) /* ArmorModVsAcid */
     , (450099,  19,       2) /* ArmorModVsElectric */
     , (450099,  39,     1.5) /* DefaultScale */
     , (450099, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450099,   1, 'Prismatic Diamond Shield') /* Name */
     , (450099,  16, 'A shield made of diamond infused with the power of the Elements.  It is incredibly resilient, and seems to be nigh unbreakable. A soft glow surrounds the shield and storms can be seen waxing and waning within the crystal surface.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450099,   1, 0x02000A33) /* Setup */
     , (450099,   3, 0x20000014) /* SoundTable */
     , (450099,   7, 0x10000320) /* ClothingBase */
     , (450099,   8, 0x06002A2C) /* Icon */
     , (450099,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450099,  37,         48) /* ItemSkillLimit - Shield */;


