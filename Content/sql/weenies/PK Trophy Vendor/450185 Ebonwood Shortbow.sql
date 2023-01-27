DELETE FROM `weenie` WHERE `class_Id` = 450185;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450185, 'bowrareebonwoodshortbowtailor', 3, '2021-11-17 16:56:08') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450185,   1,        256) /* ItemType - MissileWeapon */
     , (450185,   3,          4) /* PaletteTemplate - Brown */
     , (450185,   5,        0) /* EncumbranceVal */
     , (450185,   8,         90) /* Mass */
     , (450185,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (450185,  16,          1) /* ItemUseable - No */
     , (450185,  19,      20) /* Value */
     , (450185,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450185,  44,          0) /* Damage */
     , (450185,  45,          2) /* DamageType - Pierce */
     , (450185,  46,         16) /* DefaultCombatStyle - Bow */
     , (450185,  48,         47) /* WeaponSkill - MissileWeapons */
     , (450185,  49,         70) /* WeaponTime */
     , (450185,  50,          1) /* AmmoType - Arrow */
     , (450185,  51,          2) /* CombatUse - Missile */
     , (450185,  52,          2) /* ParentLocation - LeftHand */
     , (450185,  53,          3) /* PlacementPosition - LeftHand */
     , (450185,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450185, 151,          2) /* HookType - Wall */
     , (450185, 169,  118162702) /* TsysMutationData */
     , (450185, 353,          8) /* WeaponType - Bow */;


INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450185,  11, True ) /* IgnoreCollisions */
     , (450185,  13, True ) /* Ethereal */
     , (450185,  14, True ) /* GravityStatus */
     , (450185,  19, True ) /* Attackable */
     , (450185,  22, True ) /* Inscribable */
     , (450185, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450185,   5,  -0.033) /* ManaRate */
     , (450185,  12,    0.66) /* Shade */
     , (450185,  21,       0) /* WeaponLength */
     , (450185,  22,       0) /* DamageVariance */
     , (450185,  26,    27.3) /* MaximumVelocity */
     , (450185,  29,    1.18) /* WeaponDefense */
     , (450185,  39,     1.3) /* DefaultScale */
     , (450185,  62,       1) /* WeaponOffense */
     , (450185, 110,    1.67) /* BulkMod */
     , (450185, 111,       1) /* SizeMod */
     , (450185, 155,       1) /* IgnoreArmor */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450185,   1, 'Ebonwood Shortbow') /* Name */
     , (450185,  16, 'At first glance this would be a fairly unremarkable shortbow, except that it is made of ebonwood. This wood hails from the perilous Forest of Shades on Ispar and is prized by bowyers for its exceptional qualities.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450185,   1, 0x02001346) /* Setup */
     , (450185,   3, 0x20000014) /* SoundTable */
     , (450185,   6, 0x04000BEF) /* PaletteBase */
     , (450185,   8, 0x06005B7B) /* Icon */
     , (450185,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450185,  36, 0x0E000012) /* MutateFilter */
     , (450185,  46, 0x38000032) /* TsysMutationFilter */
     , (450185,  52, 0x06005B0C) /* IconUnderlay */;


