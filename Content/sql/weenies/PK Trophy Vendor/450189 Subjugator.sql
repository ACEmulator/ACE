DELETE FROM `weenie` WHERE `class_Id` = 450189;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450189, 'maceraresubjugatortailor', 6, '2021-12-21 17:24:33') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450189,   1,          1) /* ItemType - MeleeWeapon */
     , (450189,   5,        0) /* EncumbranceVal */
     , (450189,   8,         90) /* Mass */
     , (450189,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450189,  16,          1) /* ItemUseable - No */
     , (450189,  19,      20) /* Value */
     , (450189,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450189,  44,         0) /* Damage */
     , (450189,  45,          4) /* DamageType - Bludgeon */
     , (450189,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450189,  47,          4) /* AttackType - Slash */
     , (450189,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (450189,  49,         50) /* WeaponTime */
     , (450189,  51,          1) /* CombatUse - Melee */
     , (450189,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450189, 109,          0) /* ItemDifficulty */
     , (450189, 151,          2) /* HookType - Wall */
     , (450189, 353,          4) /* WeaponType - Mace */;


INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450189,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450189,   5,  -0.033) /* ManaRate */
     , (450189,  21,       0) /* WeaponLength */
     , (450189,  22,     0.3) /* DamageVariance */
     , (450189,  26,       0) /* MaximumVelocity */
     , (450189,  29,    1.18) /* WeaponDefense */
     , (450189,  39,     1.1) /* DefaultScale */
     , (450189,  62,    1.18) /* WeaponOffense */
     , (450189,  63,       1) /* DamageMod */
     , (450189, 136,       3) /* CriticalMultiplier */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450189,   1, 'Subjugator') /* Name */
     , (450189,  16, 'The Subjugator is a weapon that has made its mark upon history. The founder of the Roulean Empire, a warlord named Maleksoros, wielded this mace as his personal battle-weapon. With the Subjugator, he personally defeated the leaders of every neighboring tribe, forming the seed of the Empire that would spread out to conquer almost all the known world. Since then, the mace came to represent royal authority in all of the lands conquered by Maleksoros and his successors.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450189,   1, 0x02001353) /* Setup */
     , (450189,   3, 0x20000014) /* SoundTable */
     , (450189,   6, 0x04000BEF) /* PaletteBase */
     , (450189,   7, 0x10000860) /* ClothingBase */
     , (450189,   8, 0x06005B95) /* Icon */
     , (450189,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450189,  36, 0x0E000012) /* MutateFilter */
     , (450189,  46, 0x38000032) /* TsysMutationFilter */
     , (450189,  52, 0x06005B0C) /* IconUnderlay */;


