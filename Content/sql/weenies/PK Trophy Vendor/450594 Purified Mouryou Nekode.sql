DELETE FROM `weenie` WHERE `class_Id` = 450594;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450594, 'ace450594-purifiedmouryounekodetailor', 6, '2022-07-13 15:31:07') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450594,   1,          1) /* ItemType - MeleeWeapon */
     , (450594,   5,        0) /* EncumbranceVal */
     , (450594,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450594,  16,          1) /* ItemUseable - No */
     , (450594,  18,          1) /* UiEffects - Magical */
     , (450594,  19,          20) /* Value */
     , (450594,  33,          1) /* Bonded - Bonded */
     , (450594,  44,         0) /* Damage */
     , (450594,  45,         16) /* DamageType - Fire */
     , (450594,  46,          1) /* DefaultCombatStyle - Unarmed */
     , (450594,  47,          1) /* AttackType - Punch */
     , (450594,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (450594,  49,         35) /* WeaponTime */
     , (450594,  51,          1) /* CombatUse - Melee */
     , (450594,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450594, 114,          0) /* Attuned - Normal */
     , (450594, 151,          2) /* HookType - Wall */
     , (450594, 353,          1) /* WeaponType - Unarmed */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450594,  22, True ) /* Inscribable */
     , (450594,  69, False) /* IsSellable */
     , (450594,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450594,   5,   -0.05) /* ManaRate */
     , (450594,  21,       0) /* WeaponLength */
     , (450594,  22,    0.35) /* DamageVariance */
     , (450594,  26,       0) /* MaximumVelocity */
     , (450594,  29,    1.15) /* WeaponDefense */
     , (450594,  62,    1.25) /* WeaponOffense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450594,   1, 'Purified Mouryou Nekode') /* Name */
     , (450594,  16, 'A spectral nekode that burns with an inner light.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450594,   1, 0x02001BA2) /* Setup */
     , (450594,   3, 0x20000014) /* SoundTable */
     , (450594,   8, 0x060073E0) /* Icon */
     , (450594,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450594,  52, 0x060067E8) /* IconUnderlay */;


