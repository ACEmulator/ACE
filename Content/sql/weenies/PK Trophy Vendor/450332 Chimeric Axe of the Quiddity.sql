DELETE FROM `weenie` WHERE `class_Id` = 450332;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450332, 'ace450332-chimericaxeofthequidditytailor', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450332,   1,          1) /* ItemType - MeleeWeapon */
     , (450332,   3,         82) /* PaletteTemplate - PinkPurple */
     , (450332,   5,        0) /* EncumbranceVal */
     , (450332,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450332,  16,          1) /* ItemUseable - No */
     , (450332,  18,          1) /* UiEffects - Magical */
     , (450332,  19,          20) /* Value */
     , (450332,  44,         0) /* Damage */
     , (450332,  45,          1) /* DamageType - Slash */
     , (450332,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450332,  47,          4) /* AttackType - Slash */
     , (450332,  48,         45) /* WeaponSkill - LightWeapons */
     , (450332,  49,         35) /* WeaponTime */
     , (450332,  51,          1) /* CombatUse - Melee */
     , (450332,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450332, 353,          3) /* WeaponType - Axe */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450332,  22, True ) /* Inscribable */
     , (450332,  69, False) /* IsSellable */
     , (450332,  99, False) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450332,   5,   -0.05) /* ManaRate */
     , (450332,  21,    0.75) /* WeaponLength */
     , (450332,  22,    0.15) /* DamageVariance */
     , (450332,  29,    1.15) /* WeaponDefense */
     , (450332,  62,     1.2) /* WeaponOffense */
     , (450332, 147,     0.3) /* CriticalFrequency */
     , (450332, 157,       1) /* ResistanceModifier */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450332,   1, 'Chimeric Axe of the Quiddity') /* Name */
     , (450332,  16, 'A powerful but unstable weapon made from congealed Portal Energy, pulled from a rift into Portalspace itself.  The origin of these weapons is unknown, and they do not survive exposure to Dereth for more than a few hours.  (This weapon has a 3 hour duration from the time of its creation.)') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450332,   1, 0x02001183) /* Setup */
     , (450332,   3, 0x20000014) /* SoundTable */
     , (450332,   6, 0x04000BEF) /* PaletteBase */
     , (450332,   7, 0x100002E7) /* ClothingBase */
     , (450332,   8, 0x060035C1) /* Icon */
     , (450332,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450332,  52, 0x060065FB) /* IconUnderlay */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (450332,  2096,      2)  /* Aura of Infected Caress */
     , (450332,  2101,      2)  /* Aura of Cragstone's Will */
     , (450332,  2106,      2)  /* Aura of Elysa's Sight */
     , (450332,  2116,      2)  /* Aura of Atlan's Alacrity */
     , (450332,  2504,      2)  /* Major Light Weapon Aptitude */
     , (450332,  2579,      2)  /* Minor Coordination */
     , (450332,  2583,      2)  /* Minor Strength */;
