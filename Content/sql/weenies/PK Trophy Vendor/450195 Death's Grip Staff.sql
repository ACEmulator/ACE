DELETE FROM `weenie` WHERE `class_Id` = 450195;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450195, 'staffraredeathsgripstaff`tailor', 6, '2021-12-21 17:24:33') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450195,   1,          1) /* ItemType - MeleeWeapon */
     , (450195,   5,        0) /* EncumbranceVal */
     , (450195,   8,         90) /* Mass */
     , (450195,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450195,  16,          1) /* ItemUseable - No */
     , (450195,  18,        128) /* UiEffects - Frost */
     , (450195,  19,      20) /* Value */
     , (450195,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450195,  44,         0) /* Damage */
     , (450195,  45,          8) /* DamageType - Cold */
     , (450195,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450195,  47,          6) /* AttackType - Thrust, Slash */
     , (450195,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (450195,  49,         35) /* WeaponTime */
     , (450195,  51,          1) /* CombatUse - Melee */
     , (450195,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450195, 151,          2) /* HookType - Wall */
     , (450195, 353,          7) /* WeaponType - Staff */;


INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450195,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450195,   5,   -0.05) /* ManaRate */
     , (450195,  21,       0) /* WeaponLength */
     , (450195,  22,    0.25) /* DamageVariance */
     , (450195,  26,       0) /* MaximumVelocity */
     , (450195,  29,    1.18) /* WeaponDefense */
     , (450195,  39,     0.9) /* DefaultScale */
     , (450195,  62,    1.18) /* WeaponOffense */
     , (450195,  63,       1) /* DamageMod */
     , (450195, 136,       3) /* CriticalMultiplier */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450195,   1, 'Death''s Grip Staff') /* Name */
     , (450195,  16, 'This staff was crafted by a dabbler in death magic, and the creator''s obsession shows. Whether the creator was a true necromancer or a talented pretender, the staff does seem to exude the chill of the grave.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450195,   1, 0x0200135D) /* Setup */
     , (450195,   3, 0x20000014) /* SoundTable */
     , (450195,   6, 0x04000BEF) /* PaletteBase */
     , (450195,   7, 0x10000860) /* ClothingBase */
     , (450195,   8, 0x06005BA9) /* Icon */
     , (450195,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450195,  36, 0x0E000012) /* MutateFilter */
     , (450195,  46, 0x38000032) /* TsysMutationFilter */
     , (450195,  52, 0x06005B0C) /* IconUnderlay */;


