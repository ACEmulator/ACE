DELETE FROM `weenie` WHERE `class_Id` = 450339;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450339, 'ace450339-chimerictwohandedbladeofthequidditytailor', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450339,   1,          1) /* ItemType - MeleeWeapon */
     , (450339,   3,         82) /* PaletteTemplate - PinkPurple */
     , (450339,   5,        0) /* EncumbranceVal */
     , (450339,   9,   33554432) /* ValidLocations - TwoHanded */
     , (450339,  16,          1) /* ItemUseable - No */
     , (450339,  18,          1) /* UiEffects - Magical */
     , (450339,  19,          0) /* Value */
     , (450339,  44,         0) /* Damage */
     , (450339,  45,          3) /* DamageType - Slash, Pierce */
     , (450339,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (450339,  47,          6) /* AttackType - Thrust, Slash */
     , (450339,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (450339,  49,         35) /* WeaponTime */
     , (450339,  51,          5) /* CombatUse - TwoHanded */
     , (450339,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450339, 353,         11) /* WeaponType - TwoHanded */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450339,  22, True ) /* Inscribable */
     , (450339,  69, False) /* IsSellable */
     , (450339,  99, False) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450339,   5,   -0.05) /* ManaRate */
     , (450339,  21,       0) /* WeaponLength */
     , (450339,  22,    0.25) /* DamageVariance */
     , (450339,  29,    1.15) /* WeaponDefense */
     , (450339,  39,     1.1) /* DefaultScale */
     , (450339,  62,     1.2) /* WeaponOffense */
     , (450339, 147,     0.3) /* CriticalFrequency */
     , (450339, 157,       1) /* ResistanceModifier */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450339,   1, 'Chimeric Two Handed Blade of the Quiddity') /* Name */
     , (450339,  16, 'A powerful but unstable weapon made from congealed Portal Energy, pulled from a rift into Portalspace itself.  The origin of these weapons is unknown, and they do not survive exposure to Dereth for more than a few hours.  (This weapon has a 3 hour duration from the time of its creation.)') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450339,   1, 0x02001182) /* Setup */
     , (450339,   3, 0x20000014) /* SoundTable */
     , (450339,   6, 0x04000BEF) /* PaletteBase */
     , (450339,   7, 0x100002E7) /* ClothingBase */
     , (450339,   8, 0x060035C0) /* Icon */
     , (450339,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450339,  52, 0x060065FB) /* IconUnderlay */;


