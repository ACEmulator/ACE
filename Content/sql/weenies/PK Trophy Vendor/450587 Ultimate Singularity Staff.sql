DELETE FROM `weenie` WHERE `class_Id` = 450587;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450587, 'ace450587-ultimatesingularitystafftailor', 6, '2022-02-10 05:08:07') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450587,   1,          1) /* ItemType - MeleeWeapon */
     , (450587,   3,          8) /* PaletteTemplate - Green */
     , (450587,   5,        0) /* EncumbranceVal */
     , (450587,   8,         90) /* Mass */
     , (450587,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450587,  16,          1) /* ItemUseable - No */
     , (450587,  18,          1) /* UiEffects - Magical */
     , (450587,  19,          20) /* Value */
     , (450587,  33,          1) /* Bonded - Bonded */
     , (450587,  44,         0) /* Damage */
     , (450587,  45,          4) /* DamageType - Bludgeon */
     , (450587,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450587,  47,          6) /* AttackType - Thrust, Slash */
     , (450587,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (450587,  49,         30) /* WeaponTime */
     , (450587,  51,          1) /* CombatUse - Melee */
     , (450587,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450587, 150,        103) /* HookPlacement - Hook */
     , (450587, 151,          2) /* HookType - Wall */
     , (450587, 353,          7) /* WeaponType - Staff */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450587,  22, True ) /* Inscribable */
     , (450587,  23, True ) /* DestroyOnSell */
     , (450587,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450587,   5,  -0.033) /* ManaRate */
     , (450587,  21,    1.33) /* WeaponLength */
     , (450587,  22,     0.4) /* DamageVariance */
     , (450587,  29,    1.15) /* WeaponDefense */
     , (450587,  39,    0.67) /* DefaultScale */
     , (450587,  62,    1.15) /* WeaponOffense */
     , (450587, 136,     2.5) /* CriticalMultiplier */
     , (450587, 138,     1.8) /* SlayerDamageBonus */
     , (450587, 147,    0.25) /* CriticalFrequency */
     , (450587, 155,       1) /* IgnoreArmor */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450587,   1, 'Ultimate Singularity Staff') /* Name */
     , (450587,  15, 'A staff imbued with Singularity energy.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450587,   1, 0x02000B45) /* Setup */
     , (450587,   3, 0x20000014) /* SoundTable */
     , (450587,   6, 0x04000BEF) /* PaletteBase */
     , (450587,   7, 0x10000312) /* ClothingBase */
     , (450587,   8, 0x0600222E) /* Icon */
     , (450587,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450587,  36, 0x0E000014) /* MutateFilter */;


