DELETE FROM `weenie` WHERE `class_Id` = 450554;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450554, 'ace450554-redrunesilveranatlatltailor', 3, '2021-11-17 16:56:08') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450554,   1,        256) /* ItemType - MissileWeapon */
     , (450554,   5,        0) /* EncumbranceVal */
     , (450554,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (450554,  16,          1) /* ItemUseable - No */
     , (450554,  19,      20) /* Value */
     , (450554,  44,         0) /* Damage */
     , (450554,  45,          0) /* DamageType - Undef */
     , (450554,  46,       1024) /* DefaultCombatStyle - Atlatl */
     , (450554,  48,         47) /* WeaponSkill - MissileWeapons */
     , (450554,  49,         50) /* WeaponTime */
     , (450554,  50,          4) /* AmmoType - Atlatl */
     , (450554,  51,          2) /* CombatUse - Missile */
     , (450554,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450554, 151,          2) /* HookType - Wall */
     , (450554, 353,         10) /* WeaponType - Thrown */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450554,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450554,   5,   -0.05) /* ManaRate */
     , (450554,  21,       0) /* WeaponLength */
     , (450554,  22,       0) /* DamageVariance */
     , (450554,  26,    24.9) /* MaximumVelocity */
     , (450554,  29,    1.15) /* WeaponDefense */
     , (450554,  62,       1) /* WeaponOffense */
     , (450554, 157,     1.5) /* ResistanceModifier */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450554,   1, 'Red Rune Silveran Atlatl') /* Name */
     , (450554,  15, 'An atlatl crafted by Silveran smiths, once commissioned by Varicci on Ispar for the Royal Armory.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450554,   1, 0x0200159B) /* Setup */
     , (450554,   3, 0x20000014) /* SoundTable */
     , (450554,   8, 0x06006428) /* Icon */
     , (450554,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450554,  50, 0x06006413) /* IconOverlay */;

