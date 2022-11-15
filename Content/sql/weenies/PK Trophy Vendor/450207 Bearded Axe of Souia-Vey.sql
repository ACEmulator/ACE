DELETE FROM `weenie` WHERE `class_Id` = 450207;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450207, 'axerarebeardedaxetailor', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450207,   1,          1) /* ItemType - MeleeWeapon */
     , (450207,   5,        0) /* EncumbranceVal */
     , (450207,   8,         90) /* Mass */
     , (450207,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450207,  16,          1) /* ItemUseable - No */
     , (450207,  19,      20) /* Value */
     , (450207,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450207,  44,         0) /* Damage */
     , (450207,  45,          1) /* DamageType - Slash */
     , (450207,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450207,  47,          4) /* AttackType - Slash */
     , (450207,  48,         45) /* WeaponSkill - LightWeapons */
     , (450207,  49,         50) /* WeaponTime */
     , (450207,  51,          1) /* CombatUse - Melee */
     , (450207,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450207, 151,          2) /* HookType - Wall */
     , (450207, 353,          3) /* WeaponType - Axe */;


INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450207,  11, True ) /* IgnoreCollisions */
     , (450207,  13, True ) /* Ethereal */
     , (450207,  14, True ) /* GravityStatus */
     , (450207,  19, True ) /* Attackable */
     , (450207,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450207,   5,  -0.033) /* ManaRate */
     , (450207,  21,       0) /* WeaponLength */
     , (450207,  22,     0.4) /* DamageVariance */
     , (450207,  26,       0) /* MaximumVelocity */
     , (450207,  29,    1.18) /* WeaponDefense */
     , (450207,  39,     1.1) /* DefaultScale */
     , (450207,  62,    1.18) /* WeaponOffense */
     , (450207,  63,       1) /* DamageMod */
     , (450207, 138,     1.2) /* SlayerDamageBonus */
     , (450207, 151,       1) /* IgnoreShield */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450207,   1, 'Bearded Axe of Souia-Vey') /* Name */
     , (450207,  16, 'At the edge of the known world live the Souia-Vey, fierce warriors from the high steppes. Little is known of these people except that they were fierce and lethal warriors who sold their services as mercenaries and quick-striking raiders to the other nations of the Ironsea. Until recently it was widely believed that the Souia-Vey bartered or stole their weapons as they were thought to only possess rudimentary blacksmithing skills. The Bearded Axes of Souia-Vey, however, have been verified to have been smithed by the warriors of Souia-Vey themselves. Although the axes may appear primitive, they are of surprisingly sturdy construction and can easily penetrate the thickest of armors.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450207,   1, 0x0200136C) /* Setup */
     , (450207,   3, 0x20000014) /* SoundTable */
     , (450207,   6, 0x04000BEF) /* PaletteBase */
     , (450207,   7, 0x10000860) /* ClothingBase */
     , (450207,   8, 0x06005BC7) /* Icon */
     , (450207,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450207,  36, 0x0E000012) /* MutateFilter */
     , (450207,  46, 0x38000032) /* TsysMutationFilter */
     , (450207,  52, 0x06005B0C) /* IconUnderlay */;
