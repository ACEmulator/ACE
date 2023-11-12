DELETE FROM `weenie` WHERE `class_Id` = 490053;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490053, 'ace490053-shadowfireswordoflostlightpk', 6, '2023-05-15 03:25:02') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490053,   1,          1) /* ItemType - MeleeWeapon */
     , (490053,   5,        450) /* EncumbranceVal */
     , (490053,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (490053,  16,          1) /* ItemUseable - No */
     , (490053,  18,          1) /* UiEffects - Magical */
     , (490053,  19,      20) /* Value */
     , (490053,  44,         0) /* Damage */
     , (490053,  45,         16) /* DamageType - Fire */
     , (490053,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (490053,  47,          6) /* AttackType - Thrust, Slash */
     , (490053,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (490053,  49,         30) /* WeaponTime */
     , (490053,  51,          1) /* CombatUse - Melee */
     , (490053,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (490053, 151,          2) /* HookType - Wall */
     , (490053, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490053,  22, True ) /* Inscribable */
     , (490053,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (490053,   5,  -0.033) /* ManaRate */
     , (490053,  21,       0) /* WeaponLength */
     , (490053,  22,     0.5) /* DamageVariance */
     , (490053,  26,       0) /* MaximumVelocity */
     , (490053,  39,     1.3) /* DefaultScale */
     , (490053,  62,    0) /* WeaponOffense */
     , (490053,  63,       1) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490053,   1, 'Shadowfire Sword of Lost Light') /* Name */
     , (490053,  16, 'The Empowered Sword of Lost Light, infused with the power of Shadowfire, which is deadly to Shadows.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490053,   1, 0x02001886) /* Setup */
     , (490053,   3, 0x20000014) /* SoundTable */
     , (490053,   8, 0x06002BD1) /* Icon */
     , (490053,  22, 0x3400002B) /* PhysicsEffectTable */;


