DELETE FROM `weenie` WHERE `class_Id` = 450558;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450558, 'ace450558-redrunesilveranmacetailor', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450558,   1,          1) /* ItemType - MeleeWeapon */
     , (450558,   5,        0) /* EncumbranceVal */
     , (450558,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450558,  16,          8) /* ItemUseable - Contained */
     , (450558,  19,      20) /* Value */
     , (450558,  44,         0) /* Damage */
     , (450558,  45,          4) /* DamageType - Bludgeon */
     , (450558,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450558,  47,          4) /* AttackType - Slash */
     , (450558,  48,         45) /* WeaponSkill - LightWeapons */
     , (450558,  49,         40) /* WeaponTime */
     , (450558,  51,          1) /* CombatUse - Melee */
     , (450558,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450558, 151,          2) /* HookType - Wall */
     , (450558, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450558,  19, True ) /* Attackable */
     , (450558,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450558,   5,   -0.05) /* ManaRate */
     , (450558,  22,     0.5) /* DamageVariance */
     , (450558,  29,     1.1) /* WeaponDefense */
     , (450558,  62,    1.15) /* WeaponOffense */
     , (450558, 157,     1.2) /* ResistanceModifier */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450558,   1, 'Red Rune Silveran Mace') /* Name */
     , (450558,  15, 'A fine Mace crafted by Silveran smiths, once commissioned by Varicci on Ispar for the Royal Armory.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450558,   1, 0x02001567) /* Setup */
     , (450558,   3, 0x20000014) /* SoundTable */
     , (450558,   8, 0x0600640A) /* Icon */
     , (450558,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450558,  50, 0x06006413) /* IconOverlay */;

