DELETE FROM `weenie` WHERE `class_Id` = 450553;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450553, 'ace450553-redrunesilveranstafftailor', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450553,   1,          1) /* ItemType - MeleeWeapon */
     , (450553,   5,        0) /* EncumbranceVal */
     , (450553,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450553,  16,          1) /* ItemUseable - No */
     , (450553,  19,      20) /* Value */
     , (450553,  44,         0) /* Damage */
     , (450553,  45,          4) /* DamageType - Bludgeon */
     , (450553,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450553,  47,          6) /* AttackType - Thrust, Slash */
     , (450553,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (450553,  49,         40) /* WeaponTime */
     , (450553,  51,          1) /* CombatUse - Melee */
     , (450553,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450553, 151,          2) /* HookType - Wall */
     , (450553, 353,          7) /* WeaponType - Staff */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450553,  19, True ) /* Attackable */
     , (450553,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450553,   5,   -0.05) /* ManaRate */
     , (450553,  21,       0) /* WeaponLength */
     , (450553,  22,     0.4) /* DamageVariance */
     , (450553,  26,       0) /* MaximumVelocity */
     , (450553, 157,    1.67) /* ResistanceModifier */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450553,   1, 'Red Rune Silveran Staff') /* Name */
     , (450553,  15, 'A staff crafted by Silveran smiths, once commissioned by Varicci on Ispar for the Royal Armory.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450553,   1, 0x0200156C) /* Setup */
     , (450553,   3, 0x20000014) /* SoundTable */
     , (450553,   8, 0x0600640C) /* Icon */
     , (450553,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450553,  50, 0x06006413) /* IconOverlay */;

