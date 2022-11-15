DELETE FROM `weenie` WHERE `class_Id` = 450585;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450585, 'ace450585-ultimatesingularitymacetailor', 6, '2022-02-10 05:08:07') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450585,   1,          1) /* ItemType - MeleeWeapon */
     , (450585,   3,          8) /* PaletteTemplate - Green */
     , (450585,   5,        0) /* EncumbranceVal */
     , (450585,   8,        360) /* Mass */
     , (450585,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450585,  16,          1) /* ItemUseable - No */
     , (450585,  18,          1) /* UiEffects - Magical */
     , (450585,  19,          20) /* Value */
     , (450585,  33,          1) /* Bonded - Bonded */
     , (450585,  44,         0) /* Damage */
     , (450585,  45,          4) /* DamageType - Bludgeon */
     , (450585,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450585,  47,          4) /* AttackType - Slash */
     , (450585,  48,         45) /* WeaponSkill - LightWeapons */
     , (450585,  49,         40) /* WeaponTime */
     , (450585,  51,          1) /* CombatUse - Melee */
     , (450585,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450585, 353,          4) /* WeaponType - Mace */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450585,  22, True ) /* Inscribable */
     , (450585,  23, True ) /* DestroyOnSell */
     , (450585,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450585,   5,  -0.033) /* ManaRate */
     , (450585,  21,    0.62) /* WeaponLength */
     , (450585,  22,     0.5) /* DamageVariance */
     , (450585,  29,    1.15) /* WeaponDefense */
     , (450585,  62,    1.15) /* WeaponOffense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450585,   1, 'Ultimate Singularity Mace') /* Name */
     , (450585,  15, 'A mace imbued with Singularity energy.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450585,   1, 0x02000B44) /* Setup */
     , (450585,   3, 0x20000014) /* SoundTable */
     , (450585,   6, 0x04000BEF) /* PaletteBase */
     , (450585,   7, 0x10000273) /* ClothingBase */
     , (450585,   8, 0x0600222D) /* Icon */
     , (450585,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450585,  36, 0x0E000014) /* MutateFilter */;

