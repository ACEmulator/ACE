DELETE FROM `weenie` WHERE `class_Id` = 490056;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490056, 'daggertikolapk', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490056,   1,          1) /* ItemType - MeleeWeapon */
     , (490056,   3,         20) /* PaletteTemplate - Silver */
     , (490056,   5,        120) /* EncumbranceVal */
     , (490056,   8,         80) /* Mass */
     , (490056,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (490056,  16,          1) /* ItemUseable - No */
     , (490056,  18,          1) /* UiEffects - Magical */
     , (490056,  19,       20) /* Value */
     , (490056,  44,         0) /* Damage */
     , (490056,  45,         16) /* DamageType - Fire */
     , (490056,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (490056,  47,          6) /* AttackType - Thrust, Slash */
     , (490056,  48,         45) /* WeaponSkill - LightWeapons */
     , (490056,  49,         20) /* WeaponTime */
     , (490056,  51,          1) /* CombatUse - Melee */
     , (490056,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (490056, 115,        125) /* ItemSkillLevelLimit */
     , (490056, 150,        103) /* HookPlacement - Hook */
     , (490056, 151,          2) /* HookType - Wall */
     , (490056, 353,          6) /* WeaponType - Dagger */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490056,  11, True ) /* IgnoreCollisions */
     , (490056,  13, True ) /* Ethereal */
     , (490056,  14, True ) /* GravityStatus */
     , (490056,  19, True ) /* Attackable */
     , (490056,  22, True ) /* Inscribable */
     , (490056,  23, True ) /* DestroyOnSell */
     , (490056,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (490056,   5,  -0.025) /* ManaRate */
     , (490056,  21,    0.35) /* WeaponLength */
     , (490056,  22,    0.75) /* DamageVariance */
     , (490056,  26,       0) /* MaximumVelocity */
     , (490056,  39,    1.25) /* DefaultScale */
     , (490056,  63,       1) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490056,   1, 'Dagger of Tikola') /* Name */
     , (490056,  16, 'A terribly sharp, dangerous dagger that seems to be of unusual Empyrean make.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490056,   1, 0x0200085C) /* Setup */
     , (490056,   3, 0x20000014) /* SoundTable */
     , (490056,   6, 0x04000BEF) /* PaletteBase */
     , (490056,   7, 0x10000219) /* ClothingBase */
     , (490056,   8, 0x06001607) /* Icon */
     , (490056,  22, 0x3400002B) /* PhysicsEffectTable */
     , (490056,  37,         45) /* ItemSkillLimit - LightWeapons */;
