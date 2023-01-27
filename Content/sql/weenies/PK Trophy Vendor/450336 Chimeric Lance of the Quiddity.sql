DELETE FROM `weenie` WHERE `class_Id` = 450336;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450336, 'ace450336-chimericlanceofthequidditytailor', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450336,   1,          1) /* ItemType - MeleeWeapon */
     , (450336,   3,         82) /* PaletteTemplate - PinkPurple */
     , (450336,   5,        0) /* EncumbranceVal */
     , (450336,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450336,  16,          1) /* ItemUseable - No */
     , (450336,  18,          1) /* UiEffects - Magical */
     , (450336,  19,          20) /* Value */
     , (450336,  44,         0) /* Damage */
     , (450336,  45,          2) /* DamageType - Pierce */
     , (450336,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450336,  47,          2) /* AttackType - Thrust */
     , (450336,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (450336,  49,         35) /* WeaponTime */
     , (450336,  51,          1) /* CombatUse - Melee */
     , (450336,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450336, 353,          5) /* WeaponType - Spear */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450336,  22, True ) /* Inscribable */
     , (450336,  69, False) /* IsSellable */
     , (450336,  99, False) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450336,   5,   -0.05) /* ManaRate */
     , (450336,  21,     1.5) /* WeaponLength */
     , (450336,  22,     0.2) /* DamageVariance */
     , (450336,  29,    1.15) /* WeaponDefense */
     , (450336,  62,     1.2) /* WeaponOffense */
     , (450336, 147,     0.3) /* CriticalFrequency */
     , (450336, 157,       1) /* ResistanceModifier */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450336,   1, 'Chimeric Lance of the Quiddity') /* Name */
     , (450336,  16, 'A powerful but unstable weapon made from congealed Portal Energy, pulled from a rift into Portalspace itself.  The origin of these weapons is unknown, and they do not survive exposure to Dereth for more than a few hours.  (This weapon has a 3 hour duration from the time of its creation.)') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450336,   1, 0x02001180) /* Setup */
     , (450336,   3, 0x20000014) /* SoundTable */
     , (450336,   6, 0x04000BEF) /* PaletteBase */
     , (450336,   7, 0x100002E7) /* ClothingBase */
     , (450336,   8, 0x060035BE) /* Icon */
     , (450336,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450336,  52, 0x060065FB) /* IconUnderlay */;


