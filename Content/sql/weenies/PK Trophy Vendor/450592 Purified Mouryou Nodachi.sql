DELETE FROM `weenie` WHERE `class_Id` = 450592;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450592, 'ace450592-purifiedmouryounodachitailor', 6, '2022-07-13 15:31:07') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450592,   1,          1) /* ItemType - MeleeWeapon */
     , (450592,   5,        0) /* EncumbranceVal */
     , (450592,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450592,  16,          1) /* ItemUseable - No */
     , (450592,  18,          1) /* UiEffects - Magical */
     , (450592,  19,          20) /* Value */
     , (450592,  33,          1) /* Bonded - Bonded */
     , (450592,  44,         0) /* Damage */
     , (450592,  45,         16) /* DamageType - Fire */
     , (450592,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (450592,  47,          6) /* AttackType - Thrust, Slash */
     , (450592,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (450592,  49,         35) /* WeaponTime */
     , (450592,  51,          5) /* CombatUse - TwoHanded */
     , (450592,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450592, 114,          0) /* Attuned - Normal */
     , (450592, 151,          2) /* HookType - Wall */
     , (450592, 353,         11) /* WeaponType - TwoHanded */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450592,  22, True ) /* Inscribable */
     , (450592,  69, False) /* IsSellable */
     , (450592,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450592,   5,   -0.05) /* ManaRate */
     , (450592,  21,       0) /* WeaponLength */
     , (450592,  22,     0.4) /* DamageVariance */
     , (450592,  26,       0) /* MaximumVelocity */
     , (450592,  29,    1.15) /* WeaponDefense */
     , (450592,  62,    1.25) /* WeaponOffense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450592,   1, 'Purified Mouryou Nodachi') /* Name */
     , (450592,  16, 'A spectral nodachi that burns with an inner light.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450592,   1, 0x02001B9E) /* Setup */
     , (450592,   3, 0x20000014) /* SoundTable */
     , (450592,   8, 0x060073DE) /* Icon */
     , (450592,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450592,  52, 0x060067E8) /* IconUnderlay */;

