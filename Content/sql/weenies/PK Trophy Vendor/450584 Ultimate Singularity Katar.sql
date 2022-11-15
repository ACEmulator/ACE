DELETE FROM `weenie` WHERE `class_Id` = 450584;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450584, 'ace450584-ultimatesingularitykatartailor', 6, '2022-02-10 05:08:07') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450584,   1,          1) /* ItemType - MeleeWeapon */
     , (450584,   3,          8) /* PaletteTemplate - Green */
     , (450584,   5,        0) /* EncumbranceVal */
     , (450584,   8,         90) /* Mass */
     , (450584,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450584,  16,          1) /* ItemUseable - No */
     , (450584,  18,          1) /* UiEffects - Magical */
     , (450584,  19,          20) /* Value */
     , (450584,  33,          1) /* Bonded - Bonded */
     , (450584,  44,         0) /* Damage */
     , (450584,  45,          3) /* DamageType - Slash, Pierce */
     , (450584,  46,          1) /* DefaultCombatStyle - Unarmed */
     , (450584,  47,          1) /* AttackType - Punch */
     , (450584,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (450584,  49,         20) /* WeaponTime */
     , (450584,  51,          1) /* CombatUse - Melee */
     , (450584,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450584, 353,          1) /* WeaponType - Unarmed */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450584,  22, True ) /* Inscribable */
     , (450584,  23, True ) /* DestroyOnSell */
     , (450584,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450584,   5,  -0.033) /* ManaRate */
     , (450584,  21,    0.51) /* WeaponLength */
     , (450584,  22,    0.71) /* DamageVariance */
     , (450584,  29,    1.15) /* WeaponDefense */
     , (450584,  62,    1.15) /* WeaponOffense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450584,   1, 'Ultimate Singularity Katar') /* Name */
     , (450584,  15, 'A katar imbued with Singularity energy.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450584,   1, 0x02000B43) /* Setup */
     , (450584,   3, 0x20000014) /* SoundTable */
     , (450584,   6, 0x04000BEF) /* PaletteBase */
     , (450584,   7, 0x10000311) /* ClothingBase */
     , (450584,   8, 0x0600222C) /* Icon */
     , (450584,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450584,  36, 0x0E000014) /* MutateFilter */;

