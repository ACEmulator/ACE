DELETE FROM `weenie` WHERE `class_Id` = 4200120;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200120, 'spearsingularity2htailor', 6, '2005-02-09 10:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200120,   1,          1) /* ItemType - MeleeWeapon */
     , (4200120,   3,          2) /* PaletteTemplate - Blue */
     , (4200120,   5,          0) /* EncumbranceVal */
     , (4200120,   8,        140) /* Mass */
     , (4200120,   9,   33554432) /* ValidLocations - MeleeWeapon */
     , (4200120,  16,          1) /* ItemUseable - No */
     , (4200120,  18,          1) /* UiEffects - Magical */
     , (4200120,  19,         20) /* Value */
     , (4200120,  44,          1) /* Damage */
     , (4200120,  45,          8) /* DamageType - Cold */
     , (4200120,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (4200120,  47,          2) /* AttackType - Thrust */
     , (4200120,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (4200120,  49,         30) /* WeaponTime */
     , (4200120,  51,          1) /* CombatUse - Melee */
     , (4200120,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200120,  52,          1) /* ParentLocation - RightHand */
     , (4200120, 353,         11) /* WeaponType - TwoHanded */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200120,  22, True ) /* Inscribable */
     , (4200120,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200120,   5,  -0.033) /* ManaRate */
     , (4200120,  21,     1.5) /* WeaponLength */
     , (4200120,  22,     0.5) /* DamageVariance */
     , (4200120,  29,       1) /* WeaponDefense */
     , (4200120,  62,       1) /* WeaponOffense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200120,   1, 'Singularity Spear') /* Name */
     , (4200120,  15, 'A Spear imbued with the power of the Virindi.') /* ShortDesc */
     , (4200120,  16, 'A gift from Martine: Pierce away as well as you like, but you shall be lost when Death strikes.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200120,   1, 0x020009EC) /* Setup */
     , (4200120,   3, 0x20000014) /* SoundTable */
     , (4200120,   6, 0x04000BEF) /* PaletteBase */
     , (4200120,   7, 0x1000029E) /* ClothingBase */
     , (4200120,   8, 0x06001F8B) /* Icon */
     , (4200120,  22, 0x3400002B) /* PhysicsEffectTable */
     , (4200120,  36, 0x0E000014) /* MutateFilter */
     , (4200120,  37,          9) /* ItemSkillLimit - Spear */;
