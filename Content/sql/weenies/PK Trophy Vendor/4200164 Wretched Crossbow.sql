DELETE FROM `weenie` WHERE `class_Id` = 4200164;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200164, 'tailor-ace4200164-wretchedcrossbow', 3, '2021-11-17 16:56:08') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200164,   1,        256) /* ItemType - MissileWeapon */
     , (4200164,   5,          1) /* EncumbranceVal */
     , (4200164,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (4200164,  16,          1) /* ItemUseable - No */
     , (4200164,  18,         32) /* UiEffects - Fire */
     , (4200164,  19,         20) /* Value */
     , (4200164,  44,          0) /* Damage */
     , (4200164,  45,          0) /* DamageType - Fire */
     , (4200164,  46,         32) /* DefaultCombatStyle - Crossbow */
     , (4200164,  48,         47) /* WeaponSkill - MissileWeapons */
     , (4200164,  49,        120) /* WeaponTime */
     , (4200164,  50,          2) /* AmmoType - Bolt */
     , (4200164,  51,          2) /* CombatUse - Missile */
     , (4200164,  52,          2) /* ParentLocation - LeftHand */
     , (4200164,  53,          3) /* PlacementPosition - LeftHand */
     , (4200164,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200164, 151,          2) /* HookType - Wall */
     , (4200164, 353,          9) /* WeaponType - Crossbow */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200164,  11, True ) /* IgnoreCollisions */
     , (4200164,  13, True ) /* Ethereal */
     , (4200164,  14, True ) /* GravityStatus */
     , (4200164,  19, True ) /* Attackable */
     , (4200164,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200164,   5,   -0.05) /* ManaRate */
     , (4200164,  21,       0) /* WeaponLength */
     , (4200164,  22,     0.4) /* DamageVariance */
     , (4200164,  26,    27.3) /* MaximumVelocity */
     , (4200164,  39,    1.25) /* DefaultScale */
     , (4200164,  62,       1) /* WeaponOffense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200164,   1, 'Wretched Crossbow') /* Name */
     , (4200164,  16, 'This crossbow appears to be made from the withered flesh of some sort of creature.') /* LongDesc */
     , (4200164,  33, 'WitheredAtollCrossbow0105') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200164,   1, 0x020013F8) /* Setup */
     , (4200164,   3, 0x20000014) /* SoundTable */
     , (4200164,   8, 0x06005FA5) /* Icon */
     , (4200164,  22, 0x3400002B) /* PhysicsEffectTable */;
