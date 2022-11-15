DELETE FROM `weenie` WHERE `class_Id` = 450331;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450331, 'ace450331-chimericatlatlofthequidditytailor', 3, '2021-11-01 00:00:00') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450331,   1,        256) /* ItemType - MissileWeapon */
     , (450331,   3,         82) /* PaletteTemplate - PinkPurple */
     , (450331,   5,        0) /* EncumbranceVal */
     , (450331,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (450331,  16,          1) /* ItemUseable - No */
     , (450331,  18,          1) /* UiEffects - Magical */
     , (450331,  19,          20) /* Value */
     , (450331,  44,         0) /* Damage */
     , (450331,  45,          1) /* DamageType - Slash */
     , (450331,  46,       1024) /* DefaultCombatStyle - Atlatl */
     , (450331,  48,         47) /* WeaponSkill - MissileWeapons */
     , (450331,  49,         35) /* WeaponTime */
     , (450331,  50,          4) /* AmmoType - Atlatl */
     , (450331,  51,          2) /* CombatUse - Missile */
     , (450331,  53,        101) /* PlacementPosition - Resting */
     , (450331,  60,        140) /* WeaponRange */
     , (450331,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450331, 353,         10) /* WeaponType - Thrown */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450331,  22, True ) /* Inscribable */
     , (450331,  69, False) /* IsSellable */
     , (450331,  99, False) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450331,   5,   -0.05) /* ManaRate */
     , (450331,  26,    24.9) /* MaximumVelocity */
     , (450331,  29,     1.2) /* WeaponDefense */
     , (450331,  63,     2.9) /* DamageMod */
     , (450331, 147,     0.3) /* CriticalFrequency */
     , (450331, 157,       1) /* ResistanceModifier */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450331,   1, 'Chimeric Atlatl of the Quiddity') /* Name */
     , (450331,  16, 'A powerful but unstable weapon made from congealed Portal Energy, pulled from a rift into Portalspace itself.  The origin of these weapons is unknown, and they do not survive exposure to Dereth for more than a few hours.  (This weapon has a 3 hour duration from the time of its creation.)') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450331,   1, 0x02001185) /* Setup */
     , (450331,   3, 0x20000014) /* SoundTable */
     , (450331,   6, 0x04000BEF) /* PaletteBase */
     , (450331,   7, 0x100002E7) /* ClothingBase */
     , (450331,   8, 0x060035C2) /* Icon */
     , (450331,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450331,  52, 0x060065FB) /* IconUnderlay */;
