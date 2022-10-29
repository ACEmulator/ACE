DELETE FROM `weenie` WHERE `class_Id` = 4200165;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200165, 'tailor-crossbowrarefeatheredrazor', 3, '2021-11-01 00:00:00') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200165,   1,        256) /* ItemType - MissileWeapon */
     , (4200165,   3,          4) /* PaletteTemplate - Brown */
     , (4200165,   5,          1) /* EncumbranceVal */
     , (4200165,   8,         90) /* Mass */
     , (4200165,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (4200165,  16,          1) /* ItemUseable - No */
     , (4200165,  17,        198) /* RareId */
     , (4200165,  18,         32) /* UiEffects - Fire */
     , (4200165,  19,         20) /* Value */
     , (4200165,  44,          0) /* Damage */
     , (4200165,  45,          0) /* DamageType - Fire */
     , (4200165,  46,         32) /* DefaultCombatStyle - Crossbow */
     , (4200165,  48,         47) /* WeaponSkill - MissileWeapons */
     , (4200165,  49,        100) /* WeaponTime */
     , (4200165,  50,          2) /* AmmoType - Bolt */
     , (4200165,  51,          2) /* CombatUse - Missile */
     , (4200165,  52,          2) /* ParentLocation - LeftHand */
     , (4200165,  53,          3) /* PlacementPosition - LeftHand */
     , (4200165,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200165, 151,          2) /* HookType - Wall */
     , (4200165, 169,  118162702) /* TsysMutationData */
     , (4200165, 353,          9) /* WeaponType - Crossbow */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200165,  11, True ) /* IgnoreCollisions */
     , (4200165,  13, True ) /* Ethereal */
     , (4200165,  14, True ) /* GravityStatus */
     , (4200165,  19, True ) /* Attackable */
     , (4200165,  22, True ) /* Inscribable */
     , (4200165, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200165,   5,  -0.033) /* ManaRate */
     , (4200165,  12,    0.66) /* Shade */
     , (4200165,  21,       0) /* WeaponLength */
     , (4200165,  22,       0) /* DamageVariance */
     , (4200165,  26,    27.3) /* MaximumVelocity */
     , (4200165,  39,     1.2) /* DefaultScale */
     , (4200165, 110,    1.67) /* BulkMod */
     , (4200165, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200165,   1, 'Feathered Razor') /* Name */
     , (4200165,  16, 'An exquisitely crafted crossbow adorned with a metal relief of the Firebird of Splendor. Originally this crossbow was crafted by a follower of the Washui Iiwah Jou Gai as a piece of art. The intent of the artist is open for great discussion and debate, but it is widely believed that the artist meant to express his disdain for war by depicting a weapon of destruction as something beautiful to behold. After all, how can something so beautiful be used to kill? Regretfully, due to the artist''s perfectionism and great attention to detail, he ended up creating a superb weapon. Use of this weapon is an affront to the Washui Iiwah Jou Gai.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200165,   1, 0x0200134A) /* Setup */
     , (4200165,   3, 0x20000014) /* SoundTable */
     , (4200165,   6, 0x04000BEF) /* PaletteBase */
     , (4200165,   8, 0x06005B83) /* Icon */
     , (4200165,  22, 0x3400002B) /* PhysicsEffectTable */
     , (4200165,  36, 0x0E000012) /* MutateFilter */
     , (4200165,  46, 0x38000032) /* TsysMutationFilter */
     , (4200165,  52, 0x06005B0C) /* IconUnderlay */;
