DELETE FROM `weenie` WHERE `class_Id` = 450202;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450202, 'uararefistthreeprinciplestailor', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450202,   1,          1) /* ItemType - MeleeWeapon */
     , (450202,   3,          4) /* PaletteTemplate - Brown */
     , (450202,   5,        0) /* EncumbranceVal */
     , (450202,   8,         90) /* Mass */
     , (450202,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450202,  16,          1) /* ItemUseable - No */
     , (450202,  19,      20) /* Value */
     , (450202,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450202,  44,         0) /* Damage */
     , (450202,  45,          2) /* DamageType - Pierce */
     , (450202,  46,          1) /* DefaultCombatStyle - Unarmed */
     , (450202,  47,          1) /* AttackType - Punch */
     , (450202,  48,         45) /* WeaponSkill - LightWeapons */
     , (450202,  49,         20) /* WeaponTime */
     , (450202,  51,          1) /* CombatUse - Melee */
     , (450202,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450202, 151,          2) /* HookType - Wall */
     , (450202, 353,          1) /* WeaponType - Unarmed */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450202,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450202,   5,  -0.033) /* ManaRate */
     , (450202,  12,    0.66) /* Shade */
     , (450202,  21,       1) /* WeaponLength */
     , (450202,  22,   0.205) /* DamageVariance */
     , (450202,  29,    1.18) /* WeaponDefense */
     , (450202,  62,    1.18) /* WeaponOffense */
     , (450202, 155,       1) /* IgnoreArmor */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450202,   1, 'Fist of Three Principles') /* Name */
     , (450202,  16, 'First Principle: Defense implies weakness while attacking implies strength. The Second Principle: Ask not the enemy''s weakness, the enemy will often show it to you freely. The Third Principle: Strike or be struck. ') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450202,   1, 0x02001366) /* Setup */
     , (450202,   3, 0x20000014) /* SoundTable */
     , (450202,   6, 0x04000BEF) /* PaletteBase */
     , (450202,   8, 0x06005BBB) /* Icon */
     , (450202,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450202,  36, 0x0E000012) /* MutateFilter */
     , (450202,  46, 0x38000032) /* TsysMutationFilter */
     , (450202,  52, 0x06005B0C) /* IconUnderlay */;

