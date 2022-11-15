DELETE FROM `weenie` WHERE `class_Id` = 450563;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450563, 'macesingularitynewtailor', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450563,   1,          1) /* ItemType - MeleeWeapon */
     , (450563,   3,          2) /* PaletteTemplate - Blue */
     , (450563,   5,        0) /* EncumbranceVal */
     , (450563,   8,        360) /* Mass */
     , (450563,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450563,  16,          1) /* ItemUseable - No */
     , (450563,  18,          1) /* UiEffects - Magical */
     , (450563,  19,          20) /* Value */
     , (450563,  44,         0) /* Damage */
     , (450563,  45,          4) /* DamageType - Bludgeon */
     , (450563,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450563,  47,          4) /* AttackType - Slash */
     , (450563,  48,         45) /* WeaponSkill - LightWeapons */
     , (450563,  49,         40) /* WeaponTime */
     , (450563,  51,          1) /* CombatUse - Melee */
     , (450563,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450563, 114,          1) /* Attuned - Attuned */
	 , (450563, 353,          4) /* WeaponType - Mace */ ;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450563,  22, True ) /* Inscribable */
     , (450563,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450563,   5,  -0.033) /* ManaRate */
     , (450563,  21,    0.62) /* WeaponLength */
     , (450563,  22,     0.5) /* DamageVariance */
     , (450563,  29,    1.07) /* WeaponDefense */
     , (450563,  62,    1.07) /* WeaponOffense */
     , (450563, 136,     2.5) /* CriticalMultiplier */
     , (450563, 138,     1.8) /* SlayerDamageBonus */
     , (450563, 147,    0.25) /* CriticalFrequency */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450563,   1, 'Singularity Mace') /* Name */
     , (450563,  15, 'A mace imbued with Singularity energy.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450563,   1, 0x020009EB) /* Setup */
     , (450563,   3, 0x20000014) /* SoundTable */
     , (450563,   6, 0x04000BEF) /* PaletteBase */
     , (450563,   7, 0x10000273) /* ClothingBase */
     , (450563,   8, 0x06001F8A) /* Icon */
     , (450563,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450563,  36, 0x0E000014) /* MutateFilter */
	 , (450563,  37,          5) /* ItemSkillLimit - Mace */;

