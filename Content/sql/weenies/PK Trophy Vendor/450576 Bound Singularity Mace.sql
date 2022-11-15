DELETE FROM `weenie` WHERE `class_Id` = 450576;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450576, 'macesingularitynew2tailor', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450576,   1,          1) /* ItemType - MeleeWeapon */
     , (450576,   3,         82) /* PaletteTemplate - PinkPurple */
     , (450576,   5,        0) /* EncumbranceVal */
     , (450576,   8,        360) /* Mass */
     , (450576,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450576,  16,          1) /* ItemUseable - No */
     , (450576,  18,          1) /* UiEffects - Magical */
     , (450576,  19,          20) /* Value */
     , (450576,  33,          1) /* Bonded - Bonded */
     , (450576,  44,         0) /* Damage */
     , (450576,  45,          4) /* DamageType - Bludgeon */
     , (450576,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450576,  47,          4) /* AttackType - Slash */
     , (450576,  48,         45) /* WeaponSkill - LightWeapons */
     , (450576,  49,         40) /* WeaponTime */
     , (450576,  51,          1) /* CombatUse - Melee */
     , (450576,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450576, 353,          4) /* WeaponType - Mace */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450576,  22, True ) /* Inscribable */
     , (450576,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450576,   5,  -0.033) /* ManaRate */
     , (450576,  21,    0.62) /* WeaponLength */
     , (450576,  22,     0.5) /* DamageVariance */
     , (450576,  29,    1.07) /* WeaponDefense */
     , (450576,  62,    1.07) /* WeaponOffense */
     , (450576, 136,     2.5) /* CriticalMultiplier */
     , (450576, 138,     1.8) /* SlayerDamageBonus */
     , (450576, 147,    0.25) /* CriticalFrequency */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450576,   1, 'Bound Singularity Mace') /* Name */
     , (450576,  15, 'A mace imbued with Singularity energy.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450576,   1, 0x02001109) /* Setup */
     , (450576,   3, 0x20000014) /* SoundTable */
     , (450576,   6, 0x04000BEF) /* PaletteBase */
     , (450576,   7, 0x10000273) /* ClothingBase */
     , (450576,   8, 0x060033E9) /* Icon */
     , (450576,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450576,  36, 0x0E000014) /* MutateFilter */;

