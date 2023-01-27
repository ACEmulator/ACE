DELETE FROM `weenie` WHERE `class_Id` = 450194;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450194, 'staffrareallaspectstailor', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450194,   1,          1) /* ItemType - MeleeWeapon */
     , (450194,   5,        0) /* EncumbranceVal */
     , (450194,   8,         90) /* Mass */
     , (450194,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450194,  16,          1) /* ItemUseable - No */
     , (450194,  19,      20) /* Value */
     , (450194,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450194,  44,         0) /* Damage */
     , (450194,  45,          4) /* DamageType - Bludgeon */
     , (450194,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450194,  47,          6) /* AttackType - Thrust, Slash */
     , (450194,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (450194,  49,         35) /* WeaponTime */
     , (450194,  51,          1) /* CombatUse - Melee */
     , (450194,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450194, 151,          2) /* HookType - Wall */
     , (450194, 353,          7) /* WeaponType - Staff */;


INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450194,  11, True ) /* IgnoreCollisions */
     , (450194,  13, True ) /* Ethereal */
     , (450194,  14, True ) /* GravityStatus */
     , (450194,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450194,   5,  -0.033) /* ManaRate */
     , (450194,  21,       0) /* WeaponLength */
     , (450194,  22,     0.2) /* DamageVariance */
     , (450194,  26,       0) /* MaximumVelocity */
     , (450194,  29,    1.18) /* WeaponDefense */
     , (450194,  39,     0.7) /* DefaultScale */
     , (450194,  62,    1.18) /* WeaponOffense */
     , (450194,  63,       1) /* DamageMod */
     , (450194, 155,       1) /* IgnoreArmor */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450194,   1, 'Staff of All Aspects') /* Name */
     , (450194,  16, 'Made of the finest silver, this staff has been polished so that it''s surface sparkles like a mirror. Added enchantments have increased its mirror-like qualities, allowing its wielder a better chance to deflect or resist elemental damage.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450194,   1, 0x0200135C) /* Setup */
     , (450194,   3, 0x20000014) /* SoundTable */
     , (450194,   6, 0x04000BEF) /* PaletteBase */
     , (450194,   7, 0x10000860) /* ClothingBase */
     , (450194,   8, 0x06005BA7) /* Icon */
     , (450194,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450194,  36, 0x0E000012) /* MutateFilter */
     , (450194,  46, 0x38000032) /* TsysMutationFilter */
     , (450194,  52, 0x06005B0C) /* IconUnderlay */;


