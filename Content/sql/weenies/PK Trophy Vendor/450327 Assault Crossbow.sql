DELETE FROM `weenie` WHERE `class_Id` = 450327;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450327, 'crossbowtumerokwartailor', 3, '2021-11-01 00:00:00') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450327,   1,        256) /* ItemType - MissileWeapon */
     , (450327,   3,         14) /* PaletteTemplate - Red */
     , (450327,   5,        0) /* EncumbranceVal */
     , (450327,   8,        220) /* Mass */
     , (450327,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (450327,  16,          1) /* ItemUseable - No */
     , (450327,  18,          1) /* UiEffects - Magical */
     , (450327,  19,       20) /* Value */
     , (450327,  44,          0) /* Damage */
     , (450327,  46,         32) /* DefaultCombatStyle - Crossbow */
     , (450327,  48,         47) /* WeaponSkill - MissileWeapons */
     , (450327,  49,         60) /* WeaponTime */
     , (450327,  50,          2) /* AmmoType - Bolt */
     , (450327,  51,          2) /* CombatUse - Missile */
     , (450327,  52,          2) /* ParentLocation - LeftHand */
     , (450327,  53,          3) /* PlacementPosition - LeftHand */
     , (450327,  60,        192) /* WeaponRange */
     , (450327,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450327, 150,        103) /* HookPlacement - Hook */
     , (450327, 151,          2) /* HookType - Wall */
     , (450327, 353,          9) /* WeaponType - Crossbow */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450327,  11, True ) /* IgnoreCollisions */
     , (450327,  13, True ) /* Ethereal */
     , (450327,  14, True ) /* GravityStatus */
     , (450327,  15, True ) /* LightsStatus */
     , (450327,  19, True ) /* Attackable */
     , (450327,  22, True ) /* Inscribable */
     , (450327,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450327,   5,  -0.025) /* ManaRate */
     , (450327,  21,    0.75) /* WeaponLength */
     , (450327,  22,       0) /* DamageVariance */
     , (450327,  26,    27.3) /* MaximumVelocity */
     , (450327,  29,    1.06) /* WeaponDefense */
     , (450327,  39,     1.2) /* DefaultScale */
     , (450327,  62,       1) /* WeaponOffense */
     , (450327,  63,     2.4) /* DamageMod */
     , (450327,  77,       1) /* PhysicsScriptIntensity */
     , (450327, 138,     2.5) /* SlayerDamageBonus */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450327,   1, 'Assault Crossbow') /* Name */
     , (450327,  16, 'A reward for defeating the leaders of the Reedshark Clan.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450327,   1, 0x02000EC2) /* Setup */
     , (450327,   3, 0x20000014) /* SoundTable */
     , (450327,   6, 0x04000BEF) /* PaletteBase */
     , (450327,   7, 0x100002E7) /* ClothingBase */
     , (450327,   8, 0x06002106) /* Icon */
     , (450327,  19, 0x00000058) /* ActivationAnimation */
     , (450327,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450327,  30,         87) /* PhysicsScript - BreatheLightning */;

