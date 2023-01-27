DELETE FROM `weenie` WHERE `class_Id` = 450551;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450551, 'ace450551-redrunesilveranswordtailor', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450551,   1,          1) /* ItemType - MeleeWeapon */
     , (450551,   5,        0) /* EncumbranceVal */
     , (450551,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450551,  16,          1) /* ItemUseable - No */
     , (450551,  19,      20) /* Value */
     , (450551,  44,         0) /* Damage */
     , (450551,  45,          3) /* DamageType - Slash, Pierce */
     , (450551,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450551,  47,          6) /* AttackType - Thrust, Slash */
     , (450551,  48,         45) /* WeaponSkill - LightWeapons */
     , (450551,  49,         35) /* WeaponTime */
     , (450551,  51,          1) /* CombatUse - Melee */
     , (450551,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450551, 151,          2) /* HookType - Wall */
     , (450551, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450551,  19, True ) /* Attackable */
     , (450551,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450551,   5,   -0.05) /* ManaRate */
     , (450551,  21,       0) /* WeaponLength */
     , (450551,  22,     0.5) /* DamageVariance */
     , (450551,  26,       0) /* MaximumVelocity */
     , (450551,  29,     1.1) /* WeaponDefense */
     , (450551,  62,    1.15) /* WeaponOffense */
     , (450551,  63,       1) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450551,   1, 'Red Rune Silveran Sword') /* Name */
     , (450551,  15, 'A fine sword crafted by Silveran smiths, once commissioned by Varicci on Ispar for the Royal Armory.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450551,   1, 0x02001553) /* Setup */
     , (450551,   3, 0x20000014) /* SoundTable */
     , (450551,   8, 0x060063EC) /* Icon */
     , (450551,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450551,  50, 0x06006413) /* IconOverlay */;


