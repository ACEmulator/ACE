DELETE FROM `weenie` WHERE `class_Id` = 450567;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450567, 'swordsingularitynewtailor', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450567,   1,          1) /* ItemType - MeleeWeapon */
     , (450567,   3,          2) /* PaletteTemplate - Blue */
     , (450567,   5,        0) /* EncumbranceVal */
     , (450567,   8,        180) /* Mass */
     , (450567,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450567,  16,          1) /* ItemUseable - No */
     , (450567,  18,          1) /* UiEffects - Magical */
     , (450567,  19,          20) /* Value */
     , (450567,  44,         0) /* Damage */
     , (450567,  45,          3) /* DamageType - Slash, Pierce */
     , (450567,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450567,  47,          6) /* AttackType - Thrust, Slash */
     , (450567,  48,         45) /* WeaponSkill - LightWeapons */
     , (450567,  49,         40) /* WeaponTime */
     , (450567,  51,          1) /* CombatUse - Melee */
     , (450567,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450567, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450567,  22, True ) /* Inscribable */
     , (450567,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450567,   5,  -0.033) /* ManaRate */
     , (450567,  21,    0.95) /* WeaponLength */
     , (450567,  22,    0.16) /* DamageVariance */
     , (450567,  26,       0) /* MaximumVelocity */
     , (450567,  29,    1.07) /* WeaponDefense */
     , (450567,  39,     1.1) /* DefaultScale */
     , (450567,  62,    1.07) /* WeaponOffense */
     , (450567,  63,       1) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450567,   1, 'Singularity Sword') /* Name */
     , (450567,  15, 'A sword imbued with Singularity energy.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450567,   1, 0x020009E9) /* Setup */
     , (450567,   3, 0x20000014) /* SoundTable */
     , (450567,   6, 0x04000BEF) /* PaletteBase */
     , (450567,   7, 0x1000029F) /* ClothingBase */
     , (450567,   8, 0x06001F8C) /* Icon */
     , (450567,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450567,  36, 0x0E000014) /* MutateFilter */;


