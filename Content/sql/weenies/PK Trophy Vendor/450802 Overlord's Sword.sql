DELETE FROM `weenie` WHERE `class_Id` = 450802;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450802, 'swordoverlordnewpk', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450802,   1,          1) /* ItemType - MeleeWeapon */
     , (450802,   3,         61) /* PaletteTemplate - White */
     , (450802,   5,        0) /* EncumbranceVal */
     , (450802,   8,        420) /* Mass */
     , (450802,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450802,  16,          1) /* ItemUseable - No */
     , (450802,  18,          1) /* UiEffects - Magical */
     , (450802,  19,       20) /* Value */
     , (450802,  44,         0) /* Damage */
     , (450802,  45,         3) /* DamageType - Electric */
     , (450802,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450802,  47,          6) /* AttackType - Thrust, Slash */
     , (450802,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (450802,  51,          1) /* CombatUse - Melee */
     , (450802,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450802, 150,        103) /* HookPlacement - Hook */
     , (450802, 151,          2) /* HookType - Wall */
     , (450802, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450802,  22, True ) /* Inscribable */
     , (450802,  23, True ) /* DestroyOnSell */
     , (450802,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450802,   5,   -0.05) /* ManaRate */
     , (450802,  21,    0.94) /* WeaponLength */
     , (450802,  22,     0.5) /* DamageVariance */
     , (450802,  26,       0) /* MaximumVelocity */
     , (450802,  39,     1.3) /* DefaultScale */
     , (450802,  63,       1) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450802,   1, 'Overlord''s Sword') /* Name */
     , (450802,  33, 'PickedUpOverlordSword') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450802,   1, 0x02000EA9) /* Setup */
     , (450802,   3, 0x20000014) /* SoundTable */
     , (450802,   6, 0x040008B4) /* PaletteBase */
     , (450802,   8, 0x060029F0) /* Icon */
     , (450802,  22, 0x3400002B) /* PhysicsEffectTable */;

