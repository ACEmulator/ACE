DELETE FROM `weenie` WHERE `class_Id` = 450552;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450552, 'ace450552-redrunesilveranspeartailor', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450552,   1,          1) /* ItemType - MeleeWeapon */
     , (450552,   5,        0) /* EncumbranceVal */
     , (450552,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450552,  16,          1) /* ItemUseable - No */
     , (450552,  19,      20) /* Value */
     , (450552,  44,         0) /* Damage */
     , (450552,  45,          2) /* DamageType - Pierce */
     , (450552,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450552,  47,          2) /* AttackType - Thrust */
     , (450552,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (450552,  49,         40) /* WeaponTime */
     , (450552,  51,          1) /* CombatUse - Melee */
     , (450552,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450552, 151,          2) /* HookType - Wall */
     , (450552, 353,          5) /* WeaponType - Spear */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450552,  19, True ) /* Attackable */
     , (450552,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450552,   5,   -0.05) /* ManaRate */
     , (450552,  21,       0) /* WeaponLength */
     , (450552,  22,     0.5) /* DamageVariance */
     , (450552,  26,       0) /* MaximumVelocity */
     , (450552,  29,     1.1) /* WeaponDefense */
     , (450552,  62,    1.15) /* WeaponOffense */
     , (450552,  63,       1) /* DamageMod */
     , (450552, 157,    1.65) /* ResistanceModifier */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450552,   1, 'Red Rune Silveran Spear') /* Name */
     , (450552,  15, 'A spear crafted by Silveran smiths, once commissioned by Varicci on Ispar for the Royal Armory.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450552,   1, 0x0200155D) /* Setup */
     , (450552,   3, 0x20000014) /* SoundTable */
     , (450552,   8, 0x06006405) /* Icon */
     , (450552,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450552,  50, 0x06006413) /* IconOverlay */;

