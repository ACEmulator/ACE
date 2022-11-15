DELETE FROM `weenie` WHERE `class_Id` = 450193;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450193, 'spearraresquiresglaivetailor', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450193,   1,          1) /* ItemType - MeleeWeapon */
     , (450193,   5,        0) /* EncumbranceVal */
     , (450193,   8,         90) /* Mass */
     , (450193,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450193,  16,          1) /* ItemUseable - No */
     , (450193,  19,      20) /* Value */
     , (450193,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450193,  44,         0) /* Damage */
     , (450193,  45,          2) /* DamageType - Pierce */
     , (450193,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450193,  47,          2) /* AttackType - Thrust */
     , (450193,  48,         45) /* WeaponSkill - LightWeapons */
     , (450193,  49,         35) /* WeaponTime */
     , (450193,  51,          1) /* CombatUse - Melee */
     , (450193,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450193, 151,          2) /* HookType - Wall */
     , (450193, 353,          5) /* WeaponType - Spear */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450193,  11, True ) /* IgnoreCollisions */
     , (450193,  13, True ) /* Ethereal */
     , (450193,  14, True ) /* GravityStatus */
     , (450193,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450193,   5,  -0.033) /* ManaRate */
     , (450193,  21,       0) /* WeaponLength */
     , (450193,  22,   0.184) /* DamageVariance */
     , (450193,  26,       0) /* MaximumVelocity */
     , (450193,  29,    1.18) /* WeaponDefense */
     , (450193,  39,     1.1) /* DefaultScale */
     , (450193,  62,    1.18) /* WeaponOffense */
     , (450193,  63,       1) /* DamageMod */
     , (450193, 138,    1.25) /* SlayerDamageBonus */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450193,   1, 'Squire''s Glaive') /* Name */
     , (450193,  16, 'They say that this glaive once belonged to the squire of a Viamontian knight. One day, as they visited the village in which they had both grown up, the Knight received an order from his lord: he was to sack and destroy the village for failing to pay homage to the King''s nephew. Torn between honor and righteousness, the Knight and squire anguished over the dilemma. In the end, loyalty won over righteousness and they slaughtered every last person of the village. Weapons and armor black with flame and blood, the two were said to have been found standing in the center of the village like grim statues. Apparently they were so grieved by their actions that they could not keep their spirits from departing. No amount of polishing can return this glaive to its original luster.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450193,   1, 0x02001359) /* Setup */
     , (450193,   3, 0x20000014) /* SoundTable */
     , (450193,   6, 0x04000BEF) /* PaletteBase */
     , (450193,   7, 0x10000860) /* ClothingBase */
     , (450193,   8, 0x06005BA1) /* Icon */
     , (450193,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450193,  36, 0x0E000012) /* MutateFilter */
     , (450193,  46, 0x38000032) /* TsysMutationFilter */
     , (450193,  52, 0x06005B0C) /* IconUnderlay */;

