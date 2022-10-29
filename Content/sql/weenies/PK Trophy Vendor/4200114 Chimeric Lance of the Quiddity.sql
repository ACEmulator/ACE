DELETE FROM `weenie` WHERE `class_Id` = 4200114;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200114, 'ace4200114-chimericlanceofthequidditytailor', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200114,   1,          1) /* ItemType - MeleeWeapon */
     , (4200114,   3,         82) /* PaletteTemplate - PinkPurple */
     , (4200114,   5,          0) /* EncumbranceVal */
     , (4200114,   9,   33554432) /* ValidLocations - MeleeWeapon */
     , (4200114,  16,          1) /* ItemUseable - No */
     , (4200114,  18,          1) /* UiEffects - Magical */
     , (4200114,  19,         20) /* Value */
     , (4200114,  44,          1) /* Damage */
     , (4200114,  45,         64) /* DamageType - Light */
     , (4200114,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (4200114,  47,          2) /* AttackType - Thrust */
     , (4200114,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (4200114,  49,         35) /* WeaponTime */
     , (4200114,  51,          1) /* CombatUse - Melee */
     , (4200114,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
	 , (4200114,  52,          1) /* ParentLocation - RightHand */
     , (4200114, 353,         11) /* WeaponType - TwoHanded */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200114,  22, True ) /* Inscribable */
     , (4200114,  69, True) /* IsSellable */
     , (4200114,  99, True) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200114,   5,   -0.05) /* ManaRate */
     , (4200114,  21,     1.5) /* WeaponLength */
     , (4200114,  22,     0.2) /* DamageVariance */
     , (4200114,  29,    1.15) /* WeaponDefense */
     , (4200114,  62,     1.2) /* WeaponOffense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200114,   1, 'Chimeric Lance of the Quiddity') /* Name */
     , (4200114,  16, 'A powerful but unstable weapon made from congealed Portal Energy, pulled from a rift into Portalspace itself.  The origin of these weapons is unknown, and they do not survive exposure to Dereth for more than a few hours.  (This weapon has a 3 hour duration from the time of its creation.)') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200114,   1, 0x02001180) /* Setup */
     , (4200114,   3, 0x20000014) /* SoundTable */
     , (4200114,   6, 0x04000BEF) /* PaletteBase */
     , (4200114,   7, 0x100002E7) /* ClothingBase */
     , (4200114,   8, 0x060035BE) /* Icon */
     , (4200114,  22, 0x3400002B) /* PhysicsEffectTable */
     , (4200114,  52, 0x060065FB) /* IconUnderlay */;

