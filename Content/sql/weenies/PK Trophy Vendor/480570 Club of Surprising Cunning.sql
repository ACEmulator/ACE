DELETE FROM `weenie` WHERE `class_Id` = 480570;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480570, 'ace480570-clubofsurprisingcunningpk', 6, '2021-11-07 08:12:46') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480570,   1,          1) /* ItemType - MeleeWeapon */
     , (480570,   5,        0) /* EncumbranceVal */
     , (480570,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480570,  16,          1) /* ItemUseable - No */
     , (480570,  18,          1) /* UiEffects - Magical */
     , (480570,  19,          20) /* Value */
     , (480570,  44,         0) /* Damage */
     , (480570,  45,          4) /* DamageType - Bludgeon */
     , (480570,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (480570,  47,          4) /* AttackType - Slash */
     , (480570,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (480570,  49,         40) /* WeaponTime */
     , (480570,  51,          1) /* CombatUse - Melee */
     , (480570,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480570, 114,          1) /* Attuned - Attuned */
     , (480570, 151,          2) /* HookType - Wall */
     , (480570, 353,          4) /* WeaponType - Mace */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480570,  22, True ) /* Inscribable */
     , (480570,  23, True ) /* DestroyOnSell */
     , (480570,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480570,   5,  -0.033) /* ManaRate */
     , (480570,  21,       0) /* WeaponLength */
     , (480570,  22,    0.33) /* DamageVariance */
     , (480570,  26,       0) /* MaximumVelocity */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480570,   1, 'Club of Surprising Cunning') /* Name */
     , (480570,  16, 'This heavy club was crafted and once wielded by the Merwart Mundagurg.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480570,   1, 0x020017E7) /* Setup */
     , (480570,   3, 0x20000014) /* SoundTable */
     , (480570,   8, 0x060067CC) /* Icon */
     , (480570,  22, 0x3400002B) /* PhysicsEffectTable */;

