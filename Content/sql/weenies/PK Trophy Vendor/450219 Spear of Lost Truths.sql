DELETE FROM `weenie` WHERE `class_Id` = 450219;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450219, 'ace450219-spearoflosttruthstailor', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450219,   1,          1) /* ItemType - MeleeWeapon */
     , (450219,   5,        0) /* EncumbranceVal */
     , (450219,   8,         90) /* Mass */
     , (450219,   9,   33554432) /* ValidLocations - TwoHanded */
     , (450219,  16,          1) /* ItemUseable - No */
     , (450219,  19,      20) /* Value */
     , (450219,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450219,  44,         0) /* Damage */
     , (450219,  45,          2) /* DamageType - Pierce */
     , (450219,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (450219,  47,          2) /* AttackType - Thrust */
     , (450219,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (450219,  49,         35) /* WeaponTime */
     , (450219,  51,          5) /* CombatUse - TwoHanded */
     , (450219,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450219, 151,          2) /* HookType - Wall */
     , (450219, 353,         11) /* WeaponType - TwoHanded */;


INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450219,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450219,   5,  -0.033) /* ManaRate */
     , (450219,  21,       1) /* WeaponLength */
     , (450219,  22,     0.6) /* DamageVariance */
     , (450219,  26,       0) /* MaximumVelocity */
     , (450219,  29,    1.18) /* WeaponDefense */
     , (450219,  39,       1) /* DefaultScale */
     , (450219,  62,    1.18) /* WeaponOffense */
     , (450219,  63,       1) /* DamageMod */
     , (450219, 136,       2) /* CriticalMultiplier */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450219,   1, 'Spear of Lost Truths') /* Name */
     , (450219,  16, 'This spear burns with the "light of truth;" a flame that is believed to burn brighter when held before someone telling a lie. Ancient manuscripts indicate that those bearing spears of lost truth would travel the lands bringing law to the people and settling disputes.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450219,   1, 0x02001A3A) /* Setup */
     , (450219,   3, 0x20000014) /* SoundTable */
     , (450219,   6, 0x04000BEF) /* PaletteBase */
     , (450219,   7, 0x10000860) /* ClothingBase */
     , (450219,   8, 0x06006F30) /* Icon */
     , (450219,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450219,  36, 0x0E000012) /* MutateFilter */
     , (450219,  46, 0x38000032) /* TsysMutationFilter */
     , (450219,  52, 0x06005B0C) /* IconUnderlay */;

