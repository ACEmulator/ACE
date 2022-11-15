DELETE FROM `weenie` WHERE `class_Id` = 450196;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450196, 'staffrarefetteredsoulstsilors', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450196,   1,          1) /* ItemType - MeleeWeapon */
     , (450196,   5,        0) /* EncumbranceVal */
     , (450196,   8,         90) /* Mass */
     , (450196,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450196,  16,          1) /* ItemUseable - No */
     , (450196,  19,      20) /* Value */
     , (450196,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450196,  44,         0) /* Damage */
     , (450196,  45,          4) /* DamageType - Bludgeon */
     , (450196,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450196,  47,          6) /* AttackType - Thrust, Slash */
     , (450196,  48,         45) /* WeaponSkill - LightWeapons */
     , (450196,  49,         35) /* WeaponTime */
     , (450196,  51,          1) /* CombatUse - Melee */
     , (450196,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450196, 151,          2) /* HookType - Wall */
     , (450196, 353,          7) /* WeaponType - Staff */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450196,  11, True ) /* IgnoreCollisions */
     , (450196,  13, True ) /* Ethereal */
     , (450196,  14, True ) /* GravityStatus */
     , (450196,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450196,   5,  -0.033) /* ManaRate */
     , (450196,  21,       0) /* WeaponLength */
     , (450196,  22,     0.2) /* DamageVariance */
     , (450196,  26,       0) /* MaximumVelocity */
     , (450196,  29,    1.18) /* WeaponDefense */
     , (450196,  39,     0.7) /* DefaultScale */
     , (450196,  62,    1.18) /* WeaponOffense */
     , (450196,  63,       1) /* DamageMod */
     , (450196, 138,    1.25) /* SlayerDamageBonus */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450196,   1, 'Staff of Fettered Souls') /* Name */
     , (450196,  16, 'One need only look at this staff to divine its malevolent nature. This staff was fashioned from the haft of the famed Pike of Justice in Viamont, which was used to publicly display the heads of executed traitors. Chains used to bind the tortured prisoners wrap around the full length of the staff. And at ends of the staff are cloudy crystals that seem to invoke the rage and despair of the dead. Faint wails of pain and agony can be heard emanating from the staff.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450196,   1, 0x0200135E) /* Setup */
     , (450196,   3, 0x20000014) /* SoundTable */
     , (450196,   6, 0x04000BEF) /* PaletteBase */
     , (450196,   7, 0x10000860) /* ClothingBase */
     , (450196,   8, 0x06005BAB) /* Icon */
     , (450196,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450196,  36, 0x0E000012) /* MutateFilter */
     , (450196,  46, 0x38000032) /* TsysMutationFilter */
     , (450196,  52, 0x06005B0C) /* IconUnderlay */;

