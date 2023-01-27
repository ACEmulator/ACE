DELETE FROM `weenie` WHERE `class_Id` = 450586;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450586, 'ace450586-ultimatesingularityspeartailor', 6, '2022-02-10 05:08:07') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450586,   1,          1) /* ItemType - MeleeWeapon */
     , (450586,   3,          8) /* PaletteTemplate - Green */
     , (450586,   5,        0) /* EncumbranceVal */
     , (450586,   8,        320) /* Mass */
     , (450586,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450586,  16,          1) /* ItemUseable - No */
     , (450586,  18,          1) /* UiEffects - Magical */
     , (450586,  19,          20) /* Value */
     , (450586,  33,          1) /* Bonded - Bonded */
     , (450586,  44,         0) /* Damage */
     , (450586,  45,          2) /* DamageType - Pierce */
     , (450586,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450586,  47,          2) /* AttackType - Thrust */
     , (450586,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (450586,  49,         30) /* WeaponTime */
     , (450586,  51,          1) /* CombatUse - Melee */
     , (450586,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450586, 353,          5) /* WeaponType - Spear */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450586,  22, True ) /* Inscribable */
     , (450586,  23, True ) /* DestroyOnSell */
     , (450586,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450586,   5,  -0.033) /* ManaRate */
     , (450586,  21,    0.75) /* WeaponLength */
     , (450586,  22,    0.66) /* DamageVariance */
     , (450586,  29,    1.15) /* WeaponDefense */
     , (450586,  62,    1.15) /* WeaponOffense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450586,   1, 'Ultimate Singularity Spear') /* Name */
     , (450586,  15, 'A spear imbued with Singularity energy.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450586,   1, 0x02000B46) /* Setup */
     , (450586,   3, 0x20000014) /* SoundTable */
     , (450586,   6, 0x04000BEF) /* PaletteBase */
     , (450586,   7, 0x1000029E) /* ClothingBase */
     , (450586,   8, 0x0600222F) /* Icon */
     , (450586,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450586,  36, 0x0E000014) /* MutateFilter */;

