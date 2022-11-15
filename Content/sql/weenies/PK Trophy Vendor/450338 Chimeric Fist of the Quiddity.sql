DELETE FROM `weenie` WHERE `class_Id` = 450338;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450338, 'ace450338-chimericfistofthequidditytailor', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450338,   1,          1) /* ItemType - MeleeWeapon */
     , (450338,   3,         82) /* PaletteTemplate - PinkPurple */
     , (450338,   5,        0) /* EncumbranceVal */
     , (450338,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450338,  16,          1) /* ItemUseable - No */
     , (450338,  18,          1) /* UiEffects - Magical */
     , (450338,  19,          20) /* Value */
     , (450338,  44,         0) /* Damage */
     , (450338,  45,          4) /* DamageType - Bludgeon */
     , (450338,  46,          1) /* DefaultCombatStyle - Unarmed */
     , (450338,  47,          1) /* AttackType - Punch */
     , (450338,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (450338,  49,         35) /* WeaponTime */
     , (450338,  51,          1) /* CombatUse - Melee */
     , (450338,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450338, 353,          1) /* WeaponType - Unarmed */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450338,  22, True ) /* Inscribable */
     , (450338,  69, False) /* IsSellable */
     , (450338,  99, False) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450338,   5,   -0.05) /* ManaRate */
     , (450338,  21,    0.52) /* WeaponLength */
     , (450338,  22,    0.25) /* DamageVariance */
     , (450338,  29,    1.15) /* WeaponDefense */
     , (450338,  39,     0.8) /* DefaultScale */
     , (450338,  62,     1.2) /* WeaponOffense */
     , (450338, 147,     0.3) /* CriticalFrequency */
     , (450338, 157,       1) /* ResistanceModifier */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450338,   1, 'Chimeric Fist of the Quiddity') /* Name */
     , (450338,  16, 'A powerful but unstable weapon made from congealed Portal Energy, pulled from a rift into Portalspace itself.  The origin of these weapons is unknown, and they do not survive exposure to Dereth for more than a few hours.  (This weapon has a 3 hour duration from the time of its creation.)') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450338,   1, 0x0200117F) /* Setup */
     , (450338,   3, 0x20000014) /* SoundTable */
     , (450338,   6, 0x04000BEF) /* PaletteBase */
     , (450338,   7, 0x100002E7) /* ClothingBase */
     , (450338,   8, 0x060035C5) /* Icon */
     , (450338,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450338,  52, 0x060065FB) /* IconUnderlay */;
