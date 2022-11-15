DELETE FROM `weenie` WHERE `class_Id` = 450209;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450209, 'axeraretuskedaxeayabaqurtailor', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450209,   1,          1) /* ItemType - MeleeWeapon */
     , (450209,   5,        0) /* EncumbranceVal */
     , (450209,   8,         90) /* Mass */
     , (450209,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450209,  16,          1) /* ItemUseable - No */
     , (450209,  19,      20) /* Value */
     , (450209,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450209,  44,         0) /* Damage */
     , (450209,  45,          1) /* DamageType - Slash */
     , (450209,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450209,  47,          4) /* AttackType - Slash */
     , (450209,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (450209,  49,         60) /* WeaponTime */
     , (450209,  51,          1) /* CombatUse - Melee */
     , (450209,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450209, 151,          2) /* HookType - Wall */
     , (450209, 353,          3) /* WeaponType - Axe */;


INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450209,  11, True ) /* IgnoreCollisions */
     , (450209,  13, True ) /* Ethereal */
     , (450209,  14, True ) /* GravityStatus */
     , (450209,  19, True ) /* Attackable */
     , (450209,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450209,   5,  -0.033) /* ManaRate */
     , (450209,  21,       0) /* WeaponLength */
     , (450209,  22,   0.205) /* DamageVariance */
     , (450209,  26,       0) /* MaximumVelocity */
     , (450209,  29,    1.18) /* WeaponDefense */
     , (450209,  39,     1.1) /* DefaultScale */
     , (450209,  62,    1.18) /* WeaponOffense */
     , (450209,  63,       1) /* DamageMod */
     , (450209, 138,     1.2) /* SlayerDamageBonus */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450209,   1, 'Tusked Axe of Ayan Baqur') /* Name */
     , (450209,  16, 'The haft of this axe is made of ivory and is wrapped in animal skins. Craft markings located on the haft of the axe would seem to indicate that this particular axe was crafted in Ayan Baqur. A picture of a Banderling has been carefully embossed in the head of the axe, a sure sign of its intended prey.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450209,   1, 0x0200136F) /* Setup */
     , (450209,   3, 0x20000014) /* SoundTable */
     , (450209,   6, 0x04000BEF) /* PaletteBase */
     , (450209,   7, 0x10000860) /* ClothingBase */
     , (450209,   8, 0x06005BCE) /* Icon */
     , (450209,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450209,  36, 0x0E000012) /* MutateFilter */
     , (450209,  46, 0x38000032) /* TsysMutationFilter */
     , (450209,  52, 0x06005B0C) /* IconUnderlay */;

