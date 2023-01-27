DELETE FROM `weenie` WHERE `class_Id` = 450591;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450591, 'ace450591-purifiedmouryounanjoutachitailor', 6, '2022-07-13 15:31:07') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450591,   1,          1) /* ItemType - MeleeWeapon */
     , (450591,   5,        0) /* EncumbranceVal */
     , (450591,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450591,  16,          1) /* ItemUseable - No */
     , (450591,  18,          1) /* UiEffects - Magical */
     , (450591,  19,          20) /* Value */
     , (450591,  33,          1) /* Bonded - Bonded */
     , (450591,  44,         0) /* Damage */
     , (450591,  45,         16) /* DamageType - Fire */
     , (450591,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450591,  47,        160) /* AttackType - DoubleSlash, DoubleThrust */
     , (450591,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (450591,  49,         35) /* WeaponTime */
     , (450591,  51,          1) /* CombatUse - Melee */
     , (450591,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450591, 114,          0) /* Attuned - Normal */
     , (450591, 151,          2) /* HookType - Wall */
     , (450591, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450591,  22, True ) /* Inscribable */
     , (450591,  69, False) /* IsSellable */
     , (450591,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450591,   5,   -0.05) /* ManaRate */
     , (450591,  21,       0) /* WeaponLength */
     , (450591,  22,    0.35) /* DamageVariance */
     , (450591,  26,       0) /* MaximumVelocity */
     , (450591,  29,    1.15) /* WeaponDefense */
     , (450591,  62,    1.25) /* WeaponOffense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450591,   1, 'Purified Mouryou Nanjou-tachi') /* Name */
     , (450591,  16, 'A spectral nanjou-tachi that burns with an inner light.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450591,   1, 0x02001BA0) /* Setup */
     , (450591,   3, 0x20000014) /* SoundTable */
     , (450591,   8, 0x060073E2) /* Icon */
     , (450591,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450591,  52, 0x060067E8) /* IconUnderlay */;


