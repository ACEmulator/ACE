DELETE FROM `weenie` WHERE `class_Id` = 4200161;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200161, 'tailor-crossbowrarebloodmark', 3, '2021-11-01 00:00:00') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200161,   1,        256) /* ItemType - MissileWeapon */
     , (4200161,   3,          4) /* PaletteTemplate - Brown */
     , (4200161,   5,          1) /* EncumbranceVal */
     , (4200161,   8,         90) /* Mass */
     , (4200161,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (4200161,  16,          1) /* ItemUseable - No */
     , (4200161,  17,        249) /* RareId */
     , (4200161,  19,         20) /* Value */
     , (4200161,  44,          0) /* Damage */
     , (4200161,  45,          0) /* DamageType - Bludgeon */
     , (4200161,  46,         32) /* DefaultCombatStyle - Crossbow */
     , (4200161,  48,         47) /* WeaponSkill - MissileWeapons */
     , (4200161,  49,        100) /* WeaponTime */
     , (4200161,  50,          2) /* AmmoType - Bolt */
     , (4200161,  51,          2) /* CombatUse - Missile */
     , (4200161,  52,          2) /* ParentLocation - LeftHand */
     , (4200161,  53,          3) /* PlacementPosition - LeftHand */
     , (4200161,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200161, 151,          2) /* HookType - Wall */
     , (4200161, 169,  118162702) /* TsysMutationData */
     , (4200161, 353,          9) /* WeaponType - Crossbow */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200161,  11, True ) /* IgnoreCollisions */
     , (4200161,  13, True ) /* Ethereal */
     , (4200161,  14, True ) /* GravityStatus */
     , (4200161,  19, True ) /* Attackable */
     , (4200161,  22, True ) /* Inscribable */
     , (4200161, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200161,   5,  -0.033) /* ManaRate */
     , (4200161,  12,    0.66) /* Shade */
     , (4200161,  21,       0) /* WeaponLength */
     , (4200161,  22,       0) /* DamageVariance */
     , (4200161,  26,    27.3) /* MaximumVelocity */
     , (4200161,  39,     1.2) /* DefaultScale */
     , (4200161, 110,    1.67) /* BulkMod */
     , (4200161, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200161,   1, 'Bloodmark Crossbow') /* Name */
     , (4200161,  16, 'As if craving blood, this crossbow strikes foe with uncanny regularity. It is thought to be the weapon of choice among the Red Hands, a highly secretive network of Sho smugglers and assassins. ') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200161,   1, 0x02001349) /* Setup */
     , (4200161,   3, 0x20000014) /* SoundTable */
     , (4200161,   6, 0x04000BEF) /* PaletteBase */
     , (4200161,   8, 0x06005B81) /* Icon */
     , (4200161,  22, 0x3400002B) /* PhysicsEffectTable */
     , (4200161,  36, 0x0E000012) /* MutateFilter */
     , (4200161,  46, 0x38000032) /* TsysMutationFilter */
     , (4200161,  52, 0x06005B0C) /* IconUnderlay */;