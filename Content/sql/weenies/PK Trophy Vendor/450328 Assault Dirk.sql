DELETE FROM `weenie` WHERE `class_Id` = 450328;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450328, 'daggertumerokwartailor', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450328,   1,          1) /* ItemType - MeleeWeapon */
     , (450328,   3,         14) /* PaletteTemplate - Red */
     , (450328,   5,        0) /* EncumbranceVal */
     , (450328,   8,         90) /* Mass */
     , (450328,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450328,  16,          1) /* ItemUseable - No */
     , (450328,  18,          1) /* UiEffects - Magical */
     , (450328,  19,       20) /* Value */
     , (450328,  44,         0) /* Damage */
     , (450328,  45,          3) /* DamageType - Slash, Pierce */
     , (450328,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450328,  47,          4) /* AttackType - Slash */
     , (450328,  48,         45) /* WeaponSkill - LightWeapons */
     , (450328,  49,         25) /* WeaponTime */
     , (450328,  51,          1) /* CombatUse - Melee */
     , (450328,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450328, 150,        103) /* HookPlacement - Hook */
     , (450328, 151,          2) /* HookType - Wall */
     , (450328, 353,          6) /* WeaponType - Dagger */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450328,  11, True ) /* IgnoreCollisions */
     , (450328,  13, True ) /* Ethereal */
     , (450328,  14, True ) /* GravityStatus */
     , (450328,  15, True ) /* LightsStatus */
     , (450328,  19, True ) /* Attackable */
     , (450328,  22, True ) /* Inscribable */
     , (450328,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450328,   5,  -0.025) /* ManaRate */
     , (450328,  21,    0.75) /* WeaponLength */
     , (450328,  22,     0.7) /* DamageVariance */
     , (450328,  26,       0) /* MaximumVelocity */
     , (450328,  29,    1.06) /* WeaponDefense */
     , (450328,  39,     1.2) /* DefaultScale */
     , (450328,  62,    1.06) /* WeaponOffense */
     , (450328,  63,       1) /* DamageMod */
     , (450328,  77,       1) /* PhysicsScriptIntensity */
     , (450328, 138,     2.5) /* SlayerDamageBonus */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450328,   1, 'Assault Dirk') /* Name */
     , (450328,  16, 'A reward for defeating the leaders of the Shreth Clan.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450328,   1, 0x02000EC0) /* Setup */
     , (450328,   3, 0x20000014) /* SoundTable */
     , (450328,   6, 0x04000BEF) /* PaletteBase */
     , (450328,   7, 0x100002E7) /* ClothingBase */
     , (450328,   8, 0x06002101) /* Icon */
     , (450328,  19, 0x00000058) /* ActivationAnimation */
     , (450328,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450328,  30,         87) /* PhysicsScript - BreatheLightning */;

