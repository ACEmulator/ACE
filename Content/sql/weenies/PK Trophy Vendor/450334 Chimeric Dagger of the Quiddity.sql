DELETE FROM `weenie` WHERE `class_Id` = 450334;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450334, 'ace450334-chimericdaggerofthequidditytailor', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450334,   1,          1) /* ItemType - MeleeWeapon */
     , (450334,   3,         82) /* PaletteTemplate - PinkPurple */
     , (450334,   5,        0) /* EncumbranceVal */
     , (450334,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450334,  16,          1) /* ItemUseable - No */
     , (450334,  18,          1) /* UiEffects - Magical */
     , (450334,  19,          20) /* Value */
     , (450334,  44,         0) /* Damage */
     , (450334,  45,          2) /* DamageType - Pierce */
     , (450334,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450334,  47,          2) /* AttackType - Thrust */
     , (450334,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (450334,  49,         20) /* WeaponTime */
     , (450334,  51,          1) /* CombatUse - Melee */
     , (450334,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450334, 353,          6) /* WeaponType - Dagger */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450334,  22, True ) /* Inscribable */
     , (450334,  69, False) /* IsSellable */
     , (450334,  99, False) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450334,   5,   -0.05) /* ManaRate */
     , (450334,  21,     0.4) /* WeaponLength */
     , (450334,  22,    0.15) /* DamageVariance */
     , (450334,  29,    1.15) /* WeaponDefense */
     , (450334,  62,     1.2) /* WeaponOffense */
     , (450334, 147,     0.3) /* CriticalFrequency */
     , (450334, 157,       1) /* ResistanceModifier */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450334,   1, 'Chimeric Dagger of the Quiddity') /* Name */
     , (450334,  16, 'A powerful but unstable weapon made from congealed Portal Energy, pulled from a rift into Portalspace itself.  The origin of these weapons is unknown, and they do not survive exposure to Dereth for more than a few hours.  (This weapon has a 3 hour duration from the time of its creation.)') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450334,   1, 0x02001189) /* Setup */
     , (450334,   3, 0x20000014) /* SoundTable */
     , (450334,   6, 0x04000BEF) /* PaletteBase */
     , (450334,   7, 0x100002E7) /* ClothingBase */
     , (450334,   8, 0x060035C6) /* Icon */
     , (450334,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450334,  52, 0x060065FB) /* IconUnderlay */;
