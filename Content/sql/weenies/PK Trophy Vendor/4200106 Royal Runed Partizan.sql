DELETE FROM `weenie` WHERE `class_Id` = 4200106;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200106, 'ace4200106-royalrunedpartizantailor', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200106,   1,          1) /* ItemType - MeleeWeapon */
     , (4200106,   5,          0) /* EncumbranceVal */
     , (4200106,   9,   33554432) /* ValidLocations - MeleeWeapon */
     , (4200106,  16,          1) /* ItemUseable - No */
     , (4200106,  19,         20) /* Value */
     , (4200106,  44,          1) /* Damage */
     , (4200106,  45,          2) /* DamageType - Pierce */
     , (4200106,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (4200106,  47,          2) /* AttackType - Thrust */
     , (4200106,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (4200106,  49,         20) /* WeaponTime */
     , (4200106,  51,          1) /* CombatUse - Melee */
	 , (4200106,  52,          1) /* ParentLocation - RightHand */
     , (4200106,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200106, 151,          2) /* HookType - Wall */
     , (4200106, 353,         11) /* WeaponType - TwoHanded */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200106,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200106,   5,   -0.05) /* ManaRate */
     , (4200106,  21,       0) /* WeaponLength */
     , (4200106,  22,     0.5) /* DamageVariance */
     , (4200106,  26,       0) /* MaximumVelocity */
     , (4200106,  29,    1.15) /* WeaponDefense */
     , (4200106,  62,     1.1) /* WeaponOffense */
     , (4200106,  63,       1) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200106,   1, 'Royal Runed Partizan') /* Name */
     , (4200106,  15, 'A partizan crafted by Silveran smiths, once commissioned by Varicci on Ispar for the Royal Armory.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200106,   1, 0x0200157F) /* Setup */
     , (4200106,   3, 0x20000014) /* SoundTable */
     , (4200106,   6, 0x04001A28) /* PaletteBase */
     , (4200106,   8, 0x06005C89) /* Icon */
     , (4200106,  22, 0x3400002B) /* PhysicsEffectTable */
     , (4200106,  50, 0x06006412) /* IconOverlay */
     , (4200106,  55,       2074) /* ProcSpell - Gossamer Flesh */;
