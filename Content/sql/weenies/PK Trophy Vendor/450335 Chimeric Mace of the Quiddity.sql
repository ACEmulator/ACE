DELETE FROM `weenie` WHERE `class_Id` = 450335;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450335, 'ace450335-chimericmaceofthequiddity', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450335,   1,          1) /* ItemType - MeleeWeapon */
     , (450335,   3,         82) /* PaletteTemplate - PinkPurple */
     , (450335,   5,        0) /* EncumbranceVal */
     , (450335,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450335,  16,          1) /* ItemUseable - No */
     , (450335,  18,          1) /* UiEffects - Magical */
     , (450335,  19,          20) /* Value */
     , (450335,  44,         0) /* Damage */
     , (450335,  45,          4) /* DamageType - Bludgeon */
     , (450335,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450335,  47,          4) /* AttackType - Slash */
     , (450335,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (450335,  49,         40) /* WeaponTime */
     , (450335,  51,          1) /* CombatUse - Melee */
     , (450335,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450335, 353,          4) /* WeaponType - Mace */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450335,  22, True ) /* Inscribable */
     , (450335,  69, False) /* IsSellable */
     , (450335,  99, False) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450335,   5,   -0.05) /* ManaRate */
     , (450335,  21,    0.62) /* WeaponLength */
     , (450335,  22,     0.1) /* DamageVariance */
     , (450335,  29,    1.15) /* WeaponDefense */
     , (450335,  62,     1.2) /* WeaponOffense */
     , (450335, 147,     0.3) /* CriticalFrequency */
     , (450335, 157,       1) /* ResistanceModifier */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450335,   1, 'Chimeric Mace of the Quiddity') /* Name */
     , (450335,  16, 'A powerful but unstable weapon made from congealed Portal Energy, pulled from a rift into Portalspace itself.  The origin of these weapons is unknown, and they do not survive exposure to Dereth for more than a few hours.  (This weapon has a 3 hour duration from the time of its creation.)') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450335,   1, 0x02001181) /* Setup */
     , (450335,   3, 0x20000014) /* SoundTable */
     , (450335,   6, 0x04000BEF) /* PaletteBase */
     , (450335,   7, 0x100002E7) /* ClothingBase */
     , (450335,   8, 0x060035BD) /* Icon */
     , (450335,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450335,  52, 0x060065FB) /* IconUnderlay */;

