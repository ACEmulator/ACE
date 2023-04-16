DELETE FROM `weenie` WHERE `class_Id` = 480559;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480559, 'ace480559-channelinggurukfistpk', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480559,   1,          1) /* ItemType - MeleeWeapon */
     , (480559,   5,        0) /* EncumbranceVal */
     , (480559,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480559,  16,          1) /* ItemUseable - No */
     , (480559,  18,          1) /* UiEffects - Magical */
     , (480559,  19,       20) /* Value */
     , (480559,  44,         0) /* Damage */
     , (480559,  45,          4) /* DamageType - Bludgeon */
     , (480559,  46,          1) /* DefaultCombatStyle - Unarmed */
     , (480559,  47,          1) /* AttackType - Punch */
     , (480559,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (480559,  49,         20) /* WeaponTime */
     , (480559,  51,          1) /* CombatUse - Melee */
     , (480559,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480559, 151,          2) /* HookType - Wall */
     , (480559, 353,          1) /* WeaponType - Unarmed */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480559,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480559,   5,   -0.05) /* ManaRate */
     , (480559,  21,       0) /* WeaponLength */
     , (480559,  39,     0.8) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480559,   1, 'Channeling Guruk Fist') /* Name */
     , (480559,  16, 'This normal Burun fist has had mucor-altered granite applied to it, resulting in a magically enhanced weapon with unique magical properties.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480559,   1, 0x02001679) /* Setup */
     , (480559,   3, 0x20000014) /* SoundTable */
     , (480559,   8, 0x060065A6) /* Icon */
     , (480559,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480559,  55,       4069) /* ProcSpell - Mucor Jolt */;

