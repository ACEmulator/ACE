DELETE FROM `weenie` WHERE `class_Id` = 4200166;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200166, 'tailor-ace4200166-bonecrossbow', 3, '2021-11-17 16:56:08') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200166,   1,        256) /* ItemType - MissileWeapon */
     , (4200166,   5,          1) /* EncumbranceVal */
     , (4200166,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (4200166,  16,          1) /* ItemUseable - No */
     , (4200166,  19,         20) /* Value */
     , (4200166,  44,          0) /* Damage */
     , (4200166,  45,          0) /* DamageType - Undef */
     , (4200166,  46,         32) /* DefaultCombatStyle - Crossbow */
     , (4200166,  48,         47) /* WeaponSkill - MissileWeapons */
     , (4200166,  49,        120) /* WeaponTime */
     , (4200166,  50,          2) /* AmmoType - Bolt */
     , (4200166,  51,          2) /* CombatUse - Missile */
     , (4200166,  52,          2) /* ParentLocation - LeftHand */
     , (4200166,  53,          3) /* PlacementPosition - LeftHand */
     , (4200166,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200166, 151,          2) /* HookType - Wall */
     , (4200166, 353,          9) /* WeaponType - Crossbow */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200166,  19, True ) /* Attackable */
     , (4200166,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200166,  21,       0) /* WeaponLength */
     , (4200166,  22,       0) /* DamageVariance */
     , (4200166,  26,    27.3) /* MaximumVelocity */
     , (4200166,  29,       1) /* WeaponDefense */
     , (4200166,  39,    1.25) /* DefaultScale */
     , (4200166,  62,       1) /* WeaponOffense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200166,   1, 'Bone Crossbow') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200166,   1, 0x02001678) /* Setup */
     , (4200166,   3, 0x20000014) /* SoundTable */
     , (4200166,   8, 0x060065A5) /* Icon */
     , (4200166,  22, 0x3400002B) /* PhysicsEffectTable */;
