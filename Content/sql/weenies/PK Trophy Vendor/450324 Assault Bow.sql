DELETE FROM `weenie` WHERE `class_Id` = 450324;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450324, 'bowtumerokwartailor', 3, '2021-11-01 00:00:00') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450324,   1,        256) /* ItemType - MissileWeapon */
     , (450324,   3,         14) /* PaletteTemplate - Red */
     , (450324,   5,        0) /* EncumbranceVal */
     , (450324,   8,        220) /* Mass */
     , (450324,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (450324,  16,          1) /* ItemUseable - No */
     , (450324,  18,          1) /* UiEffects - Magical */
     , (450324,  19,       20) /* Value */
     , (450324,  44,          0) /* Damage */
     , (450324,  46,         16) /* DefaultCombatStyle - Bow */
     , (450324,  48,         47) /* WeaponSkill - MissileWeapons */
     , (450324,  49,         40) /* WeaponTime */
     , (450324,  50,          1) /* AmmoType - Arrow */
     , (450324,  51,          2) /* CombatUse - Missile */
     , (450324,  52,          2) /* ParentLocation - LeftHand */
     , (450324,  53,          3) /* PlacementPosition - LeftHand */
     , (450324,  60,        200) /* WeaponRange */
     , (450324,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450324, 150,        103) /* HookPlacement - Hook */
     , (450324, 151,          2) /* HookType - Wall */
     , (450324, 353,          8) /* WeaponType - Bow */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450324,  11, True ) /* IgnoreCollisions */
     , (450324,  13, True ) /* Ethereal */
     , (450324,  14, True ) /* GravityStatus */
     , (450324,  15, True ) /* LightsStatus */
     , (450324,  19, True ) /* Attackable */
     , (450324,  22, True ) /* Inscribable */
     , (450324,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450324,   5,  -0.025) /* ManaRate */
     , (450324,  21,    0.75) /* WeaponLength */
     , (450324,  22,       0) /* DamageVariance */
     , (450324,  26,    26.3) /* MaximumVelocity */
     , (450324,  29,    1.06) /* WeaponDefense */
     , (450324,  39,     1.2) /* DefaultScale */
     , (450324,  62,       1) /* WeaponOffense */
     , (450324,  63,     2.1) /* DamageMod */
     , (450324,  77,       1) /* PhysicsScriptIntensity */
     , (450324, 138,     2.5) /* SlayerDamageBonus */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450324,   1, 'Assault Bow') /* Name */
     , (450324,  16, 'A reward for defeating the leaders of the Gromnie Clan.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450324,   1, 0x02000EBE) /* Setup */
     , (450324,   3, 0x20000014) /* SoundTable */
     , (450324,   6, 0x04000BEF) /* PaletteBase */
     , (450324,   7, 0x100002E7) /* ClothingBase */
     , (450324,   8, 0x060020FF) /* Icon */
     , (450324,  19, 0x00000058) /* ActivationAnimation */
     , (450324,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450324,  30,         87) /* PhysicsScript - BreatheLightning */;

