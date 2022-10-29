DELETE FROM `weenie` WHERE `class_Id` = 4200094;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200094, 'ace4200094-spearofwinterflame2h', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200094,   1,          1) /* ItemType - MeleeWeapon */
     , (4200094,   5,          0) /* EncumbranceVal */
     , (4200094,   8,         90) /* Mass */
     , (4200094,   9,    33554432) /* ValidLocations - MeleeWeapon */
     , (4200094,  16,          1) /* ItemUseable - No */
     , (4200094,  19,         20) /* Value */
     , (4200094,  44,          1) /* Damage */
     , (4200094,  45,         16) /* Damage Type - Fire */
     , (4200094,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (4200094,  47,          2) /* AttackType - Thrust */
     , (4200094,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (4200094,  49,         20) /* WeaponTime */
     , (4200094,  51,          1) /* CombatUse - Melee */
     , (4200094,  52,          1) /* ParentLocation - RightHand */
     , (4200094,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200094, 151,          2) /* HookType - Wall */
     , (4200094, 353,         11) /* WeaponType - TwoHanded */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200094,  11, True ) /* IgnoreCollisions */
     , (4200094,  13, True ) /* Ethereal */
     , (4200094,  14, True ) /* GravityStatus */
     , (4200094,  19, True ) /* Attackable */
     , (4200094,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200094,  21,       1) /* WeaponLength */
     , (4200094,  39,       1) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200094,   1, 'Spear of Winter Flame') /* Name */
     , (4200094,  16, 'A spear once wielded by the ancient slave Baranaith, and touched by the seething energies of his brother Farelaith.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200094,   1, 0x020014FC) /* Setup */
     , (4200094,   3, 0x20000014) /* SoundTable */
     , (4200094,   8, 0x0600628C) /* Icon */
     , (4200094,  22, 0x3400002B) /* PhysicsEffectTable */;


