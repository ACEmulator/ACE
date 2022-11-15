DELETE FROM `weenie` WHERE `class_Id` = 450204;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450204, 'uararemalachiteslashertailor', 6, '2021-12-21 17:24:33') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450204,   1,          1) /* ItemType - MeleeWeapon */
     , (450204,   5,        0) /* EncumbranceVal */
     , (450204,   8,         90) /* Mass */
     , (450204,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450204,  16,          1) /* ItemUseable - No */
     , (450204,  19,      20) /* Value */
     , (450204,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450204,  44,         0) /* Damage */
     , (450204,  45,          1) /* DamageType - Slash */
     , (450204,  46,          1) /* DefaultCombatStyle - Unarmed */
     , (450204,  47,          1) /* AttackType - Punch */
     , (450204,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (450204,  49,         20) /* WeaponTime */
     , (450204,  51,          1) /* CombatUse - Melee */
     , (450204,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450204, 109,          0) /* ItemDifficulty */
     , (450204, 151,          2) /* HookType - Wall */
     , (450204, 353,          1) /* WeaponType - Unarmed */;


INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450204,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450204,   5,  -0.033) /* ManaRate */
     , (450204,  21,       0) /* WeaponLength */
     , (450204,  22,     0.5) /* DamageVariance */
     , (450204,  26,       0) /* MaximumVelocity */
     , (450204,  29,    1.18) /* WeaponDefense */
     , (450204,  39,     0.9) /* DefaultScale */
     , (450204,  62,    1.18) /* WeaponOffense */
     , (450204,  63,       1) /* DamageMod */
     , (450204, 136,       3) /* CriticalMultiplier */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450204,   1, 'Malachite Slasher') /* Name */
     , (450204,  16, 'The handle of this blade is made from pure malachite, while the blade is forged from steel. Mounted on the blade and handle are three pieces of jade that glow with inner power. This is the favorite weapons of the Malachite Claws, an order of female assassins who were active in the courts of Roulea long ago.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450204,   1, 0x02001368) /* Setup */
     , (450204,   3, 0x20000014) /* SoundTable */
     , (450204,   6, 0x04000BEF) /* PaletteBase */
     , (450204,   7, 0x10000860) /* ClothingBase */
     , (450204,   8, 0x06005BBF) /* Icon */
     , (450204,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450204,  36, 0x0E000012) /* MutateFilter */
     , (450204,  46, 0x38000032) /* TsysMutationFilter */
     , (450204,  52, 0x06005B0C) /* IconUnderlay */;

