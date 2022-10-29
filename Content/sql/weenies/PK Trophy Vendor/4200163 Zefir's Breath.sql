DELETE FROM `weenie` WHERE `class_Id` = 4200163;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200163, 'tailor-crossbowrarezefirsbreath', 3, '2021-11-01 00:00:00') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200163,   1,        256) /* ItemType - MissileWeapon */
     , (4200163,   3,          4) /* PaletteTemplate - Brown */
     , (4200163,   5,          1) /* EncumbranceVal */
     , (4200163,   8,         90) /* Mass */
     , (4200163,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (4200163,  16,          1) /* ItemUseable - No */
     , (4200163,  17,        197) /* RareId */
     , (4200163,  18,         64) /* UiEffects - Lightning */
     , (4200163,  19,         20) /* Value */
     , (4200163,  44,          0) /* Damage */
     , (4200163,  45,          0) /* DamageType - Electric */
     , (4200163,  46,         32) /* DefaultCombatStyle - Crossbow */
     , (4200163,  48,         47) /* WeaponSkill - MissileWeapons */
     , (4200163,  49,        100) /* WeaponTime */
     , (4200163,  50,          2) /* AmmoType - Bolt */
     , (4200163,  51,          2) /* CombatUse - Missile */
     , (4200163,  52,          2) /* ParentLocation - LeftHand */
     , (4200163,  53,          3) /* PlacementPosition - LeftHand */
     , (4200163,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200163, 151,          2) /* HookType - Wall */
     , (4200163, 169,  118162702) /* TsysMutationData */
     , (4200163, 353,          9) /* WeaponType - Crossbow */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200163,  11, True ) /* IgnoreCollisions */
     , (4200163,  13, True ) /* Ethereal */
     , (4200163,  14, True ) /* GravityStatus */
     , (4200163,  19, True ) /* Attackable */
     , (4200163,  22, True ) /* Inscribable */
     , (4200163, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200163,   5,  -0.033) /* ManaRate */
     , (4200163,  12,    0.66) /* Shade */
     , (4200163,  21,       0) /* WeaponLength */
     , (4200163,  26,    27.3) /* MaximumVelocity */
     , (4200163,  39,     1.2) /* DefaultScale */
     , (4200163, 110,    1.67) /* BulkMod */
     , (4200163, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200163,   1, 'Zefir''s Breath') /* Name */
     , (4200163,  16, 'This crossbow was commissioned by the great hunter Josya Sunin and made by Master Bowyer Scildith Dyrson. Originally this crossbow was to be used to hunt pesky Zefirs, but due to an oversight by Scildith, the spells to enchant the weapon against Zefirs was reversed. Subsequently any damage dealt to a Zefir was reduced instead of increased. Josya Sunin took the opportunity to complain to the Bowyers'' guild and reduce the price of the crossbow to a fraction of its original cost. The word ''Master'' was stripped from Scildith''s title. Josya did not bother to point out that the crossbow worked amazingly well against any other creature...') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200163,   1, 0x0200134C) /* Setup */
     , (4200163,   3, 0x20000014) /* SoundTable */
     , (4200163,   6, 0x04000BEF) /* PaletteBase */
     , (4200163,   8, 0x06005B87) /* Icon */
     , (4200163,  22, 0x3400002B) /* PhysicsEffectTable */
     , (4200163,  36, 0x0E000012) /* MutateFilter */
     , (4200163,  46, 0x38000032) /* TsysMutationFilter */
     , (4200163,  52, 0x06005B0C) /* IconUnderlay */;
