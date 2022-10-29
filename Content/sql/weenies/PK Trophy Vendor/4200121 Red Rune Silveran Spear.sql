DELETE FROM `weenie` WHERE `class_Id` = 4200121;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200121, 'ace4200121-redrunesilveranspear2htailor', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200121,   1,          1) /* ItemType - MeleeWeapon */
     , (4200121,   5,          0) /* EncumbranceVal */
     , (4200121,   9,   33554432) /* ValidLocations - MeleeWeapon */
     , (4200121,  16,          1) /* ItemUseable - No */
     , (4200121,  19,         20) /* Value */
     , (4200121,  44,          1) /* Damage */
     , (4200121,  45,          2) /* DamageType - Pierce */
     , (4200121,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (4200121,  47,          2) /* AttackType - Thrust */
     , (4200121,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (4200121,  49,         40) /* WeaponTime */
     , (4200121,  51,          1) /* CombatUse - Melee */
     , (4200121,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200121, 353,         11) /* WeaponType - TwoHanded */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200121,  19, True ) /* Attackable */
     , (4200121,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200121,   5,   -0.05) /* ManaRate */
     , (4200121,  21,       0) /* WeaponLength */
     , (4200121,  22,     0.5) /* DamageVariance */
     , (4200121,  26,       0) /* MaximumVelocity */
     , (4200121,  29,       1) /* WeaponDefense */
     , (4200121,  62,       1) /* WeaponOffense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200121,   1, 'Red Rune Silveran Spear') /* Name */
     , (4200121,  15, 'A spear crafted by Silveran smiths, once commissioned by Varicci on Ispar for the Royal Armory.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200121,   1, 0x0200155D) /* Setup */
     , (4200121,   3, 0x20000014) /* SoundTable */
     , (4200121,   8, 0x06006405) /* Icon */
     , (4200121,  22, 0x3400002B) /* PhysicsEffectTable */
     , (4200121,  50, 0x06006413) /* IconOverlay */;

