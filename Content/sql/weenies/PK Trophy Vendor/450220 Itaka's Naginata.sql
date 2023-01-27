DELETE FROM `weenie` WHERE `class_Id` = 450220;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450220, 'ace450220-itakasnaginatatailor', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450220,   1,          1) /* ItemType - MeleeWeapon */
     , (450220,   5,        0) /* EncumbranceVal */
     , (450220,   8,         90) /* Mass */
     , (450220,   9,   33554432) /* ValidLocations - TwoHanded */
     , (450220,  16,          1) /* ItemUseable - No */
     , (450220,  19,      20) /* Value */
     , (450220,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450220,  44,         0) /* Damage */
     , (450220,  45,          2) /* DamageType - Pierce */
     , (450220,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (450220,  47,          2) /* AttackType - Thrust */
     , (450220,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (450220,  49,         50) /* WeaponTime */
     , (450220,  51,          5) /* CombatUse - TwoHanded */
     , (450220,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450220, 151,          2) /* HookType - Wall */
     , (450220, 353,         11) /* WeaponType - TwoHanded */;


INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450220,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450220,   5,  -0.033) /* ManaRate */
     , (450220,  21,       1) /* WeaponLength */
     , (450220,  22,     0.6) /* DamageVariance */
     , (450220,  26,       0) /* MaximumVelocity */
     , (450220,  29,    1.18) /* WeaponDefense */
     , (450220,  39,       1) /* DefaultScale */
     , (450220,  62,    1.18) /* WeaponOffense */
     , (450220,  63,       1) /* DamageMod */
     , (450220, 138,    1.25) /* SlayerDamageBonus */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450220,   1, 'Itaka''s Naginata') /* Name */
     , (450220,  16, 'Itaka, a sho woman, is believed to have held this naginata in defense of her village. In the dull light of late afternoon the bodies of the village''s men lay defeated in the fields as marauders approached Itaka, who stood alone. Itaka''s long graceful motions kept the marauders out of sword''s reach and shed the blood of those who stepped too close. Dozens of marauder''s fell but as the sun set, arms quivering from exertion, Itaka died at the hands of the marauder''s leader.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450220,   1, 0x02001A39) /* Setup */
     , (450220,   3, 0x20000014) /* SoundTable */
     , (450220,   6, 0x04000BEF) /* PaletteBase */
     , (450220,   7, 0x10000860) /* ClothingBase */
     , (450220,   8, 0x06006F31) /* Icon */
     , (450220,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450220,  36, 0x0E000012) /* MutateFilter */
     , (450220,  46, 0x38000032) /* TsysMutationFilter */
     , (450220,  52, 0x06005B0C) /* IconUnderlay */;

