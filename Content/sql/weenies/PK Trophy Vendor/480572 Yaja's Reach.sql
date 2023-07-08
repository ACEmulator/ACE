DELETE FROM `weenie` WHERE `class_Id` = 480572;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480572, 'spearyajapk', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480572,   1,          1) /* ItemType - MeleeWeapon */
     , (480572,   5,        600) /* EncumbranceVal */
     , (480572,   8,        140) /* Mass */
     , (480572,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480572,  16,          1) /* ItemUseable - No */
     , (480572,  18,          1) /* UiEffects - Magical */
     , (480572,  19,       20) /* Value */
     , (480572,  44,         0) /* Damage */
     , (480572,  45,          3) /* DamageType - Slash, Pierce */
     , (480572,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (480572,  47,          6) /* AttackType - Thrust, Slash */
     , (480572,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (480572,  49,         35) /* WeaponTime */
     , (480572,  51,          1) /* CombatUse - Melee */
     , (480572,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480572, 150,        103) /* HookPlacement - Hook */
     , (480572, 151,          2) /* HookType - Wall */
     , (480572, 353,          5) /* WeaponType - Spear */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480572,  22, True ) /* Inscribable */
     , (480572,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480572,   5,  -0.033) /* ManaRate */
     , (480572,  21,     1.5) /* WeaponLength */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480572,   1, 'Yaja''s Reach') /* Name */
     , (480572,  16, 'The arm of the Marionette, Yaja. Its gauntlet has been removed to expose bony talons.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480572,   1, 0x02001094) /* Setup */
     , (480572,   3, 0x20000014) /* SoundTable */
     , (480572,   8, 0x06003330) /* Icon */
     , (480572,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480572,  36, 0x0E000014) /* MutateFilter */;

