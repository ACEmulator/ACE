DELETE FROM `weenie` WHERE `class_Id` = 450221;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450221, 'ace450221-decapitatorsbladetailor', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450221,   1,          1) /* ItemType - MeleeWeapon */
     , (450221,   5,        0) /* EncumbranceVal */
     , (450221,   8,         90) /* Mass */
     , (450221,   9,   33554432) /* ValidLocations - TwoHanded */
     , (450221,  16,          1) /* ItemUseable - No */
     , (450221,  19,      20) /* Value */
     , (450221,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450221,  44,         0) /* Damage */
     , (450221,  45,          1) /* DamageType - Slash */
     , (450221,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (450221,  47,          4) /* AttackType - Slash */
     , (450221,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (450221,  49,         50) /* WeaponTime */
     , (450221,  51,          5) /* CombatUse - TwoHanded */
     , (450221,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450221, 151,          2) /* HookType - Wall */
     , (450221, 353,         11) /* WeaponType - TwoHanded */;



INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450221,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450221,   5,  -0.033) /* ManaRate */
     , (450221,  21,       1) /* WeaponLength */
     , (450221,  22,    0.19) /* DamageVariance */
     , (450221,  26,       0) /* MaximumVelocity */
     , (450221,  29,    1.18) /* WeaponDefense */
     , (450221,  39,       1) /* DefaultScale */
     , (450221,  62,    1.18) /* WeaponOffense */
     , (450221,  63,       1) /* DamageMod */
     , (450221, 147,    0.25) /* CriticalFrequency */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450221,   1, 'Decapitator''s Blade') /* Name */
     , (450221,  16, 'A blade once wielded by the royal decapitator. Is is said that this blade has seen the blood of more royals than any other sword in history, recorded or otherwise.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450221,   1, 0x02001A3C) /* Setup */
     , (450221,   3, 0x20000014) /* SoundTable */
     , (450221,   6, 0x04000BEF) /* PaletteBase */
     , (450221,   7, 0x10000860) /* ClothingBase */
     , (450221,   8, 0x06006F35) /* Icon */
     , (450221,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450221,  36, 0x0E000012) /* MutateFilter */
     , (450221,  46, 0x38000032) /* TsysMutationFilter */
     , (450221,  52, 0x06005B0C) /* IconUnderlay */;
