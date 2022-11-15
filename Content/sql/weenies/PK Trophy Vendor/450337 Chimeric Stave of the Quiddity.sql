DELETE FROM `weenie` WHERE `class_Id` = 450337;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450337, 'ace450337-chimericstaveofthequiddity', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450337,   1,          1) /* ItemType - MeleeWeapon */
     , (450337,   3,         82) /* PaletteTemplate - PinkPurple */
     , (450337,   5,        0) /* EncumbranceVal */
     , (450337,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450337,  16,          1) /* ItemUseable - No */
     , (450337,  18,          1) /* UiEffects - Magical */
     , (450337,  19,          20) /* Value */
     , (450337,  44,         0) /* Damage */
     , (450337,  45,          4) /* DamageType - Bludgeon */
     , (450337,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450337,  47,          4) /* AttackType - Slash */
     , (450337,  48,         45) /* WeaponSkill - LightWeapons */
     , (450337,  49,         35) /* WeaponTime */
     , (450337,  51,          1) /* CombatUse - Melee */
     , (450337,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450337, 353,          7) /* WeaponType - Staff */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450337,  22, True ) /* Inscribable */
     , (450337,  69, False) /* IsSellable */
     , (450337,  99, False) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450337,   5,   -0.05) /* ManaRate */
     , (450337,  21,    1.33) /* WeaponLength */
     , (450337,  22,     0.1) /* DamageVariance */
     , (450337,  29,    1.15) /* WeaponDefense */
     , (450337,  39,    0.67) /* DefaultScale */
     , (450337,  62,     1.2) /* WeaponOffense */
     , (450337, 147,     0.3) /* CriticalFrequency */
     , (450337, 157,       1) /* ResistanceModifier */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450337,   1, 'Chimeric Stave of the Quiddity') /* Name */
     , (450337,  16, 'A powerful but unstable weapon made from congealed Portal Energy, pulled from a rift into Portalspace itself.  The origin of these weapons is unknown, and they do not survive exposure to Dereth for more than a few hours.  (This weapon has a 3 hour duration from the time of its creation.)') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450337,   1, 0x02001188) /* Setup */
     , (450337,   3, 0x20000014) /* SoundTable */
     , (450337,   6, 0x04000BEF) /* PaletteBase */
     , (450337,   7, 0x100002E7) /* ClothingBase */
     , (450337,   8, 0x060035BF) /* Icon */
     , (450337,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450337,  52, 0x060065FB) /* IconUnderlay */;
