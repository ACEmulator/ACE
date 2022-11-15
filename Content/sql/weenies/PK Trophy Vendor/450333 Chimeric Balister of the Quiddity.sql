DELETE FROM `weenie` WHERE `class_Id` = 450333;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450333, 'ace450333-chimericbalisterofthequidditytailor', 3, '2021-11-01 00:00:00') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450333,   1,        256) /* ItemType - MissileWeapon */
     , (450333,   3,         82) /* PaletteTemplate - PinkPurple */
     , (450333,   5,        0) /* EncumbranceVal */
     , (450333,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (450333,  16,          1) /* ItemUseable - No */
     , (450333,  18,          1) /* UiEffects - Magical */
     , (450333,  19,          20) /* Value */
     , (450333,  44,         0) /* Damage */
     , (450333,  45,          1) /* DamageType - Slash */
     , (450333,  46,         32) /* DefaultCombatStyle - Crossbow */
     , (450333,  48,         47) /* WeaponSkill - MissileWeapons */
     , (450333,  49,         35) /* WeaponTime */
     , (450333,  50,          2) /* AmmoType - Bolt */
     , (450333,  51,          2) /* CombatUse - Missile */
     , (450333,  52,          2) /* ParentLocation - LeftHand */
     , (450333,  53,          3) /* PlacementPosition - LeftHand */
     , (450333,  60,        200) /* WeaponRange */
     , (450333,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450333, 353,          9) /* WeaponType - Crossbow */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450333,  22, True ) /* Inscribable */
     , (450333,  69, False) /* IsSellable */
     , (450333,  99, False) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450333,   5,   -0.05) /* ManaRate */
     , (450333,  26,    27.3) /* MaximumVelocity */
     , (450333,  29,     1.2) /* WeaponDefense */
     , (450333,  39,    1.25) /* DefaultScale */

     , (450333, 147,     0.3) /* CriticalFrequency */
     , (450333, 157,       1) /* ResistanceModifier */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450333,   1, 'Chimeric Balister of the Quiddity') /* Name */
     , (450333,  16, 'A powerful but unstable weapon made from congealed Portal Energy, pulled from a rift into Portalspace itself.  The origin of these weapons is unknown, and they do not survive exposure to Dereth for more than a few hours.  (This weapon has a 3 hour duration from the time of its creation.)') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450333,   1, 0x02001186) /* Setup */
     , (450333,   3, 0x20000014) /* SoundTable */
     , (450333,   6, 0x04000BEF) /* PaletteBase */
     , (450333,   7, 0x100002E7) /* ClothingBase */
     , (450333,   8, 0x060035C3) /* Icon */
     , (450333,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450333,  52, 0x060065FB) /* IconUnderlay */;

