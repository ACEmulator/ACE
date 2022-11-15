DELETE FROM `weenie` WHERE `class_Id` = 450593;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450593, 'ace450593-purifiedmouryouwakizashitailor', 6, '2022-07-13 15:31:07') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450593,   1,          1) /* ItemType - MeleeWeapon */
     , (450593,   5,        0) /* EncumbranceVal */
     , (450593,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450593,  16,          1) /* ItemUseable - No */
     , (450593,  18,          1) /* UiEffects - Magical */
     , (450593,  19,          20) /* Value */
     , (450593,  33,          1) /* Bonded - Bonded */
     , (450593,  44,         0) /* Damage */
     , (450593,  45,         16) /* DamageType - Fire */
     , (450593,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450593,  47,        160) /* AttackType - DoubleSlash, DoubleThrust */
     , (450593,  48,         45) /* WeaponSkill - LightWeapons */
     , (450593,  49,         35) /* WeaponTime */
     , (450593,  51,          1) /* CombatUse - Melee */
     , (450593,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450593, 151,          2) /* HookType - Wall */
     , (450593, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450593,  22, True ) /* Inscribable */
     , (450593,  69, False) /* IsSellable */
     , (450593,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450593,   5,   -0.05) /* ManaRate */
     , (450593,  21,       0) /* WeaponLength */
     , (450593,  22,    0.35) /* DamageVariance */
     , (450593,  26,       0) /* MaximumVelocity */
     , (450593,  29,    1.15) /* WeaponDefense */
     , (450593,  62,    1.25) /* WeaponOffense */
     , (450593,  63,       1) /* DamageMod */
     , (450593, 136,       2) /* CriticalMultiplier */
     , (450593, 138,       3) /* SlayerDamageBonus */
     , (450593, 155,       1) /* IgnoreArmor */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450593,   1, 'Purified Mouryou Wakizashi') /* Name */
     , (450593,  16, 'A spectral wakizashi that burns with an inner light.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450593,   1, 0x02001BA1) /* Setup */
     , (450593,   3, 0x20000014) /* SoundTable */
     , (450593,   8, 0x060073DF) /* Icon */
     , (450593,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450593,  52, 0x060067E8) /* IconUnderlay */;

