DELETE FROM `weenie` WHERE `class_Id` = 450191;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450191, 'daggerraremoriharuskitchenknifetailor', 6, '2021-12-21 17:24:33') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450191,   1,          1) /* ItemType - MeleeWeapon */
     , (450191,   5,        0) /* EncumbranceVal */
     , (450191,   8,         90) /* Mass */
     , (450191,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450191,  16,          1) /* ItemUseable - No */
     , (450191,  19,      20) /* Value */
     , (450191,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450191,  44,         0) /* Damage */
     , (450191,  45,          3) /* DamageType - Slash, Pierce */
     , (450191,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450191,  47,          6) /* AttackType - Thrust, Slash */
     , (450191,  48,         45) /* WeaponSkill - LightWeapons */
     , (450191,  49,         20) /* WeaponTime */
     , (450191,  51,          1) /* CombatUse - Melee */
     , (450191,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450191, 109,          0) /* ItemDifficulty */
     , (450191, 151,          2) /* HookType - Wall */
     , (450191, 353,          6) /* WeaponType - Dagger */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450191,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450191,   5,  -0.033) /* ManaRate */
     , (450191,  21,       0) /* WeaponLength */
     , (450191,  22,   0.205) /* DamageVariance */
     , (450191,  26,       0) /* MaximumVelocity */
     , (450191,  29,    1.18) /* WeaponDefense */
     , (450191,  39,     1.1) /* DefaultScale */
     , (450191,  62,    1.18) /* WeaponOffense */
     , (450191,  63,       1) /* DamageMod */
     , (450191, 136,       3) /* CriticalMultiplier */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450191,   1, 'Moriharu''s Kitchen Knife') /* Name */
     , (450191,  16, 'Moriharu was a highly accomplished Sho cook who traveled extensively, and settled in the Viamontian capital city of Corcosa. He found a patron, a Viamontian noble who was fond of hunting and enjoyed novelty. Moriharu worked out an unusual arrangement with his patron: once a month, the chef would go out by himself and slay a dangerous beast of the noble''s own choosing, and prepare a dish featuring that beast for the lord''s dinner. This is the dagger that Moriharu brought on each of his hunts.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450191,   1, 0x02001355) /* Setup */
     , (450191,   3, 0x20000014) /* SoundTable */
     , (450191,   6, 0x04000BEF) /* PaletteBase */
     , (450191,   7, 0x10000860) /* ClothingBase */
     , (450191,   8, 0x06005B99) /* Icon */
     , (450191,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450191,  36, 0x0E000012) /* MutateFilter */
     , (450191,  46, 0x38000032) /* TsysMutationFilter */
     , (450191,  52, 0x06005B0C) /* IconUnderlay */;
