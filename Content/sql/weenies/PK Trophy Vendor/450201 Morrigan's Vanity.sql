DELETE FROM `weenie` WHERE `class_Id` = 450201;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450201, 'swordraremorrigansvanity`tailor', 6, '2021-12-21 17:24:33') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450201,   1,          1) /* ItemType - MeleeWeapon */
     , (450201,   5,        0) /* EncumbranceVal */
     , (450201,   8,         90) /* Mass */
     , (450201,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450201,  16,          1) /* ItemUseable - No */
     , (450201,  19,      20) /* Value */
     , (450201,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450201,  45,          3) /* DamageType - Slash, Pierce */
     , (450201,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450201,  47,          6) /* AttackType - Thrust, Slash */
     , (450201,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (450201,  49,         40) /* WeaponTime */
     , (450201,  51,          1) /* CombatUse - Melee */
     , (450201,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450201, 151,          2) /* HookType - Wall */
     , (450201, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450201,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450201,   5,   -0.05) /* ManaRate */
     , (450201,  21,       0) /* WeaponLength */
     , (450201,  22,     0.4) /* DamageVariance */
     , (450201,  26,       0) /* MaximumVelocity */
     , (450201,  29,    1.18) /* WeaponDefense */
     , (450201,  39,     1.1) /* DefaultScale */
     , (450201,  62,    1.18) /* WeaponOffense */
     , (450201,  63,       1) /* DamageMod */
     , (450201, 136,       3) /* CriticalMultiplier */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450201,   1, 'Morrigan''s Vanity') /* Name */
     , (450201,  16, 'In the wild woods of Aluvia, there was a woman named Morrigan, the only female in a gang of highwaymen, bandits, and thieves. After the group looted one particularly rich cache of jewels, Morrigan quickly fenced the jewels for gold coin. One of her friends, a handsome young man who dressed well to seduce or take advantage of nobles, teased her about this. "Not even keeping one jewel for yourself?" he asked. "Where is your womanly vanity?" Morrigan patted the great sword at her side, which she kept gleaming and well-polished, despite its frequent use. "This is my only vanity," was her reply.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450201,   1, 0x02001365) /* Setup */
     , (450201,   3, 0x20000014) /* SoundTable */
     , (450201,   6, 0x04000BEF) /* PaletteBase */
     , (450201,   7, 0x10000860) /* ClothingBase */
     , (450201,   8, 0x06005BB9) /* Icon */
     , (450201,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450201,  36, 0x0E000012) /* MutateFilter */
     , (450201,  46, 0x38000032) /* TsysMutationFilter */
     , (450201,  52, 0x06005B0C) /* IconUnderlay */;

