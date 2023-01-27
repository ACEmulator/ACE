DELETE FROM `weenie` WHERE `class_Id` = 450208;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450208, 'axerarecanfieldcleavertailor', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450208,   1,          1) /* ItemType - MeleeWeapon */
     , (450208,   5,        0) /* EncumbranceVal */
     , (450208,   8,         90) /* Mass */
     , (450208,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450208,  16,          1) /* ItemUseable - No */
     , (450208,  19,      20) /* Value */
     , (450208,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450208,  44,         0) /* Damage */
     , (450208,  45,          1) /* DamageType - Slash */
     , (450208,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450208,  47,          4) /* AttackType - Slash */
     , (450208,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (450208,  49,         65) /* WeaponTime */
     , (450208,  51,          1) /* CombatUse - Melee */
     , (450208,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450208, 151,          2) /* HookType - Wall */
     , (450208, 353,          3) /* WeaponType - Axe */;


INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450208,  11, True ) /* IgnoreCollisions */
     , (450208,  13, True ) /* Ethereal */
     , (450208,  14, True ) /* GravityStatus */
     , (450208,  19, True ) /* Attackable */
     , (450208,  22, True ) /* Inscribable */
     , (450208,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450208,   5,  -0.033) /* ManaRate */
     , (450208,  21,       0) /* WeaponLength */
     , (450208,  22,   0.205) /* DamageVariance */
     , (450208,  26,       0) /* MaximumVelocity */
     , (450208,  29,    1.18) /* WeaponDefense */
     , (450208,  39,     1.1) /* DefaultScale */
     , (450208,  62,    1.18) /* WeaponOffense */
     , (450208,  63,       1) /* DamageMod */
     , (450208, 155,       1) /* IgnoreArmor */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450208,   1, 'Canfield Cleaver') /* Name */
     , (450208,  16, 'Along the verdant shores of the Canfield River lie the great forests of Aluvia. These forests provide Ispar with the vast majority of its building lumber. Unfortunately, the creatures that live within the forests heartily objected to the harvesting of their homes and sometimes struck out against the woodsmen. In response, the Aluvian woodcutters modified their axes to be useful in cutting down trees and enemies alike. These axes became known lovingly as the Canfield Cleavers. ') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450208,   1, 0x0200136D) /* Setup */
     , (450208,   3, 0x20000014) /* SoundTable */
     , (450208,   6, 0x04000BEF) /* PaletteBase */
     , (450208,   7, 0x10000860) /* ClothingBase */
     , (450208,   8, 0x06005BC9) /* Icon */
     , (450208,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450208,  36, 0x0E000012) /* MutateFilter */
     , (450208,  46, 0x38000032) /* TsysMutationFilter */
     , (450208,  52, 0x06005B0C) /* IconUnderlay */;

