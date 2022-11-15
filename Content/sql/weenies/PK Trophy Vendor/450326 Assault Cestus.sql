DELETE FROM `weenie` WHERE `class_Id` = 450326;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450326, 'cestustumerokwartailor', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450326,   1,          1) /* ItemType - MeleeWeapon */
     , (450326,   3,         14) /* PaletteTemplate - Red */
     , (450326,   5,        0) /* EncumbranceVal */
     , (450326,   8,         90) /* Mass */
     , (450326,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450326,  16,          1) /* ItemUseable - No */
     , (450326,  18,          1) /* UiEffects - Magical */
     , (450326,  19,       20) /* Value */
     , (450326,  44,         0) /* Damage */
     , (450326,  45,          4) /* DamageType - Bludgeon */
     , (450326,  46,          1) /* DefaultCombatStyle - Unarmed */
     , (450326,  47,          1) /* AttackType - Punch */
     , (450326,  48,         45) /* WeaponSkill - LightWeapons */
     , (450326,  49,         25) /* WeaponTime */
     , (450326,  51,          1) /* CombatUse - Melee */
     , (450326,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450326, 150,        103) /* HookPlacement - Hook */
     , (450326, 151,          2) /* HookType - Wall */
     , (450326, 353,          1) /* WeaponType - Unarmed */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450326,  11, True ) /* IgnoreCollisions */
     , (450326,  13, True ) /* Ethereal */
     , (450326,  14, True ) /* GravityStatus */
     , (450326,  15, True ) /* LightsStatus */
     , (450326,  19, True ) /* Attackable */
     , (450326,  22, True ) /* Inscribable */
     , (450326,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450326,   5,  -0.025) /* ManaRate */
     , (450326,  21,    0.75) /* WeaponLength */
     , (450326,  22,     0.7) /* DamageVariance */
     , (450326,  26,       0) /* MaximumVelocity */
     , (450326,  29,    1.06) /* WeaponDefense */
     , (450326,  39,     0.8) /* DefaultScale */
     , (450326,  62,    1.06) /* WeaponOffense */
     , (450326,  63,       1) /* DamageMod */
     , (450326,  77,       1) /* PhysicsScriptIntensity */
     , (450326, 138,     2.5) /* SlayerDamageBonus */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450326,   1, 'Assault Cestus') /* Name */
     , (450326,  16, 'A reward for defeating the leaders of the Serpent Clan.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450326,   1, 0x02000EBF) /* Setup */
     , (450326,   3, 0x20000014) /* SoundTable */
     , (450326,   6, 0x04000BEF) /* PaletteBase */
     , (450326,   7, 0x100002E7) /* ClothingBase */
     , (450326,   8, 0x06002100) /* Icon */
     , (450326,  19, 0x00000058) /* ActivationAnimation */
     , (450326,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450326,  30,         87) /* PhysicsScript - BreatheLightning */;

