DELETE FROM `weenie` WHERE `class_Id` = 450205;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450205, 'uarareskullpunchertailor', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450205,   1,          1) /* ItemType - MeleeWeapon */
     , (450205,   5,        0) /* EncumbranceVal */
     , (450205,   8,         90) /* Mass */
     , (450205,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450205,  16,          1) /* ItemUseable - No */
     , (450205,  19,      20) /* Value */
     , (450205,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450205,  44,         0) /* Damage */
     , (450205,  45,          2) /* DamageType - Pierce */
     , (450205,  46,          1) /* DefaultCombatStyle - Unarmed */
     , (450205,  47,          1) /* AttackType - Punch */
     , (450205,  48,         45) /* WeaponSkill - LightWeapons */
     , (450205,  49,         20) /* WeaponTime */
     , (450205,  51,          1) /* CombatUse - Melee */
     , (450205,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450205, 151,          2) /* HookType - Wall */
     , (450205, 353,          1) /* WeaponType - Unarmed */;


INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450205,  11, True ) /* IgnoreCollisions */
     , (450205,  13, True ) /* Ethereal */
     , (450205,  14, True ) /* GravityStatus */
     , (450205,  19, True ) /* Attackable */
     , (450205,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450205,   5,  -0.033) /* ManaRate */
     , (450205,  21,       0) /* WeaponLength */
     , (450205,  22,   0.205) /* DamageVariance */
     , (450205,  26,       0) /* MaximumVelocity */
     , (450205,  29,    1.18) /* WeaponDefense */
     , (450205,  39,     0.9) /* DefaultScale */
     , (450205,  62,    1.18) /* WeaponOffense */
     , (450205,  63,       1) /* DamageMod */
     , (450205, 138,     1.2) /* SlayerDamageBonus */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450205,   1, 'Skullpuncher') /* Name */
     , (450205,  16, 'For reasons unknown, those who use this weapon feel compelled to strike their opponent in the head, earning it the name Skullpuncher. Many who have used the weapon swear they feel almost a narcotic euphoria when their enemies die in this manner.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450205,   1, 0x02001369) /* Setup */
     , (450205,   3, 0x20000014) /* SoundTable */
     , (450205,   6, 0x04000BEF) /* PaletteBase */
     , (450205,   7, 0x10000860) /* ClothingBase */
     , (450205,   8, 0x06005BC1) /* Icon */
     , (450205,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450205,  36, 0x0E000012) /* MutateFilter */
     , (450205,  46, 0x38000032) /* TsysMutationFilter */
     , (450205,  52, 0x06005B0C) /* IconUnderlay */;

