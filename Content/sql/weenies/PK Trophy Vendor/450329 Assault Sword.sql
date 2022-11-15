DELETE FROM `weenie` WHERE `class_Id` = 450329;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450329, 'swordtumerokwartailor', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450329,   1,          1) /* ItemType - MeleeWeapon */
     , (450329,   3,         14) /* PaletteTemplate - Red */
     , (450329,   5,        0) /* EncumbranceVal */
     , (450329,   8,        220) /* Mass */
     , (450329,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450329,  16,          1) /* ItemUseable - No */
     , (450329,  18,          1) /* UiEffects - Magical */
     , (450329,  19,       20) /* Value */
     , (450329,  44,         0) /* Damage */
     , (450329,  45,          3) /* DamageType - Slash, Pierce */
     , (450329,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450329,  47,          6) /* AttackType - Thrust, Slash */
     , (450329,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (450329,  49,         40) /* WeaponTime */
     , (450329,  51,          1) /* CombatUse - Melee */
     , (450329,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450329, 150,        103) /* HookPlacement - Hook */
     , (450329, 151,          2) /* HookType - Wall */
     , (450329, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450329,  11, True ) /* IgnoreCollisions */
     , (450329,  13, True ) /* Ethereal */
     , (450329,  14, True ) /* GravityStatus */
     , (450329,  15, True ) /* LightsStatus */
     , (450329,  19, True ) /* Attackable */
     , (450329,  22, True ) /* Inscribable */
     , (450329,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450329,   5,  -0.025) /* ManaRate */
     , (450329,  21,    0.75) /* WeaponLength */
     , (450329,  22,     0.5) /* DamageVariance */
     , (450329,  26,       0) /* MaximumVelocity */
     , (450329,  29,    1.06) /* WeaponDefense */
     , (450329,  39,     1.2) /* DefaultScale */
     , (450329,  62,    1.06) /* WeaponOffense */
     , (450329,  63,       1) /* DamageMod */
     , (450329,  77,       1) /* PhysicsScriptIntensity */
     , (450329, 138,     2.5) /* SlayerDamageBonus */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450329,   1, 'Assault Sword') /* Name */
     , (450329,  16, 'A reward for defeating the leaders of the Serpent Clan.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450329,   1, 0x02000EC1) /* Setup */
     , (450329,   3, 0x20000014) /* SoundTable */
     , (450329,   6, 0x04000BEF) /* PaletteBase */
     , (450329,   7, 0x100002E7) /* ClothingBase */
     , (450329,   8, 0x06002105) /* Icon */
     , (450329,  19, 0x00000058) /* ActivationAnimation */
     , (450329,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450329,  30,         87) /* PhysicsScript - BreatheLightning */;
