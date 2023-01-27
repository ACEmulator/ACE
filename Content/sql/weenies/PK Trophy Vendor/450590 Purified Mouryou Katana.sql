DELETE FROM `weenie` WHERE `class_Id` = 450590;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450590, 'ace450590-purifiedmouryoukatanatailor', 6, '2022-07-13 15:31:07') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450590,   1,          1) /* ItemType - MeleeWeapon */
     , (450590,   5,        0) /* EncumbranceVal */
     , (450590,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450590,  16,          1) /* ItemUseable - No */
     , (450590,  18,          1) /* UiEffects - Magical */
     , (450590,  19,          20) /* Value */
     , (450590,  33,          1) /* Bonded - Bonded */
     , (450590,  44,         0) /* Damage */
     , (450590,  45,         16) /* DamageType - Fire */
     , (450590,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450590,  47,        160) /* AttackType - DoubleSlash, DoubleThrust */
     , (450590,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (450590,  49,         35) /* WeaponTime */
     , (450590,  51,          1) /* CombatUse - Melee */
     , (450590,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450590, 114,          0) /* Attuned - Normal */
     , (450590, 151,          2) /* HookType - Wall */
     , (450590, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450590,  22, True ) /* Inscribable */
     , (450590,  69, False) /* IsSellable */
     , (450590,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450590,   5,   -0.05) /* ManaRate */
     , (450590,  21,       0) /* WeaponLength */
     , (450590,  22,    0.35) /* DamageVariance */
     , (450590,  26,       0) /* MaximumVelocity */
     , (450590,  29,    1.15) /* WeaponDefense */
     , (450590,  62,    1.25) /* WeaponOffense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450590,   1, 'Purified Mouryou Katana') /* Name */
     , (450590,  16, 'A spectral katana that burns with an inner light.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450590,   1, 0x02001B9F) /* Setup */
     , (450590,   3, 0x20000014) /* SoundTable */
     , (450590,   8, 0x060073DD) /* Icon */
     , (450590,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450590,  52, 0x060067E8) /* IconUnderlay */;


