DELETE FROM `weenie` WHERE `class_Id` = 450200;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450200, 'swordraredesertwyrmtailor', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450200,   1,          1) /* ItemType - MeleeWeapon */
     , (450200,   5,        0) /* EncumbranceVal */
     , (450200,   8,         90) /* Mass */
     , (450200,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450200,  16,          1) /* ItemUseable - No */
     , (450200,  19,      20) /* Value */
     , (450200,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450200,  44,         0) /* Damage */
     , (450200,  45,          3) /* DamageType - Slash, Pierce */
     , (450200,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450200,  47,        166) /* AttackType - Thrust, Slash, DoubleSlash, DoubleThrust */
     , (450200,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (450200,  49,         40) /* WeaponTime */
     , (450200,  51,          1) /* CombatUse - Melee */
     , (450200,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450200, 151,          2) /* HookType - Wall */
     , (450200, 353,          2) /* WeaponType - Sword */;


INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450200,  11, True ) /* IgnoreCollisions */
     , (450200,  13, True ) /* Ethereal */
     , (450200,  14, True ) /* GravityStatus */
     , (450200,  19, True ) /* Attackable */
     , (450200,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450200,   5,  -0.033) /* ManaRate */
     , (450200,  21,       0) /* WeaponLength */
     , (450200,  22,   0.205) /* DamageVariance */
     , (450200,  26,       0) /* MaximumVelocity */
     , (450200,  29,    1.18) /* WeaponDefense */
     , (450200,  39,     1.1) /* DefaultScale */
     , (450200,  62,    1.18) /* WeaponOffense */
     , (450200,  63,       1) /* DamageMod */
     , (450200, 155,       1) /* IgnoreArmor */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450200,   1, 'Desert Wyrm') /* Name */
     , (450200,  16, 'This sword hails from the Jaladhaqa Qalathina of Gharu''n, and is one of that warrior guild''s most honored weapons. It is not owned by any one person, but by the guild in general. The sword has become a symbol of one''s skill with the sword. To earn the right to hold the blade one must earn it by force of arms. The rules are simple: you must defeat the current owner of the blade in fair and honorable combat without killing him. Those who currently wield this blade command great respect within the guild. The Desert Wyrm was thought lost in the last great battle with Zharalim.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450200,   1, 0x02001363) /* Setup */
     , (450200,   3, 0x20000014) /* SoundTable */
     , (450200,   6, 0x04000BEF) /* PaletteBase */
     , (450200,   7, 0x10000860) /* ClothingBase */
     , (450200,   8, 0x06005BB5) /* Icon */
     , (450200,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450200,  36, 0x0E000012) /* MutateFilter */
     , (450200,  46, 0x38000032) /* TsysMutationFilter */
     , (450200,  52, 0x06005B0C) /* IconUnderlay */;

