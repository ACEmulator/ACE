DELETE FROM `weenie` WHERE `class_Id` = 450564;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450564, 'spearsingularitynewtailor', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450564,   1,          1) /* ItemType - MeleeWeapon */
     , (450564,   3,          2) /* PaletteTemplate - Blue */
     , (450564,   5,        0) /* EncumbranceVal */
     , (450564,   8,        320) /* Mass */
     , (450564,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450564,  16,          1) /* ItemUseable - No */
     , (450564,  18,          1) /* UiEffects - Magical */
     , (450564,  19,          20) /* Value */
     , (450564,  44,         0) /* Damage */
     , (450564,  45,          2) /* DamageType - Pierce */
     , (450564,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450564,  47,          2) /* AttackType - Thrust */
     , (450564,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (450564,  49,         30) /* WeaponTime */
     , (450564,  51,          1) /* CombatUse - Melee */
     , (450564,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450564, 353,          5) /* WeaponType - Spear */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450564,  22, True ) /* Inscribable */
     , (450564,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450564,   5,  -0.033) /* ManaRate */
     , (450564,  21,    0.75) /* WeaponLength */
     , (450564,  22,    0.66) /* DamageVariance */
     , (450564,  29,    1.07) /* WeaponDefense */
     , (450564,  62,    1.07) /* WeaponOffense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450564,   1, 'Singularity Spear') /* Name */
     , (450564,  15, 'A spear imbued with Singularity energy.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450564,   1, 0x020009EC) /* Setup */
     , (450564,   3, 0x20000014) /* SoundTable */
     , (450564,   6, 0x04000BEF) /* PaletteBase */
     , (450564,   7, 0x1000029E) /* ClothingBase */
     , (450564,   8, 0x06001F8B) /* Icon */
     , (450564,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450564,  36, 0x0E000014) /* MutateFilter */;

