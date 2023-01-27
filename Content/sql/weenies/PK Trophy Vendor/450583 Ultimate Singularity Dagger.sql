DELETE FROM `weenie` WHERE `class_Id` = 450583;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450583, 'ace450583-ultimatesingularitydaggertailor', 6, '2022-02-10 05:08:07') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450583,   1,          1) /* ItemType - MeleeWeapon */
     , (450583,   3,          8) /* PaletteTemplate - Green */
     , (450583,   5,        0) /* EncumbranceVal */
     , (450583,   8,         90) /* Mass */
     , (450583,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450583,  16,          1) /* ItemUseable - No */
     , (450583,  18,          1) /* UiEffects - Magical */
     , (450583,  19,          20) /* Value */
     , (450583,  33,          1) /* Bonded - Bonded */
     , (450583,  44,         0) /* Damage */
     , (450583,  45,          3) /* DamageType - Slash, Pierce */
     , (450583,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450583,  47,        166) /* AttackType - Thrust, Slash, DoubleSlash, DoubleThrust */
     , (450583,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (450583,  49,         20) /* WeaponTime */
     , (450583,  51,          1) /* CombatUse - Melee */
     , (450583,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450583, 353,          6) /* WeaponType - Dagger */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450583,  22, True ) /* Inscribable */
     , (450583,  23, True ) /* DestroyOnSell */
     , (450583,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450583,   5,  -0.033) /* ManaRate */
     , (450583,  21,     0.4) /* WeaponLength */
     , (450583,  22,    0.15) /* DamageVariance */
     , (450583,  29,    1.15) /* WeaponDefense */
     , (450583,  62,    1.15) /* WeaponOffense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450583,   1, 'Ultimate Singularity Dagger') /* Name */
     , (450583,  15, 'A dagger imbued with Singularity energy.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450583,   1, 0x02000B42) /* Setup */
     , (450583,   3, 0x20000014) /* SoundTable */
     , (450583,   6, 0x04000BEF) /* PaletteBase */
     , (450583,   7, 0x10000310) /* ClothingBase */
     , (450583,   8, 0x0600222B) /* Icon */
     , (450583,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450583,  36, 0x0E000014) /* MutateFilter */;

