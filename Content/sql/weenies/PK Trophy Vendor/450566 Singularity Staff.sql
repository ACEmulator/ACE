DELETE FROM `weenie` WHERE `class_Id` = 450566;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450566, 'staffsingularitynewtailor', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450566,   1,          1) /* ItemType - MeleeWeapon */
     , (450566,   3,          2) /* PaletteTemplate - Blue */
     , (450566,   5,        0) /* EncumbranceVal */
     , (450566,   8,         90) /* Mass */
     , (450566,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450566,  16,          1) /* ItemUseable - No */
     , (450566,  18,          1) /* UiEffects - Magical */
     , (450566,  19,          20) /* Value */
     , (450566,  44,         0) /* Damage */
     , (450566,  45,          4) /* DamageType - Bludgeon */
     , (450566,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450566,  47,          6) /* AttackType - Thrust, Slash */
     , (450566,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (450566,  49,         30) /* WeaponTime */
     , (450566,  51,          1) /* CombatUse - Melee */
     , (450566,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450566, 150,        103) /* HookPlacement - Hook */
     , (450566, 151,          2) /* HookType - Wall */
     , (450566, 353,          7) /* WeaponType - Staff */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450566,  22, True ) /* Inscribable */
     , (450566,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450566,   5,  -0.033) /* ManaRate */
     , (450566,  21,    1.33) /* WeaponLength */
     , (450566,  22,     0.4) /* DamageVariance */
     , (450566,  29,    1.07) /* WeaponDefense */
     , (450566,  39,    0.67) /* DefaultScale */
     , (450566,  62,    1.07) /* WeaponOffense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450566,   1, 'Singularity Staff') /* Name */
     , (450566,  15, 'A staff imbued with Singularity energy.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450566,   1, 0x02000B48) /* Setup */
     , (450566,   3, 0x20000014) /* SoundTable */
     , (450566,   6, 0x04000BEF) /* PaletteBase */
     , (450566,   7, 0x10000312) /* ClothingBase */
     , (450566,   8, 0x0600245B) /* Icon */
     , (450566,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450566,  36, 0x0E000014) /* MutateFilter */;


