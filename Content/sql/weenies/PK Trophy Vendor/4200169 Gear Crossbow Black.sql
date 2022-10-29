DELETE FROM `weenie` WHERE `class_Id` = 4200169;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200169, 'tailor-ace4200169-gearcrossbow', 3, '2021-11-17 16:56:08') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200169,   1,        256) /* ItemType - MissileWeapon */
     , (4200169,   5,          1) /* EncumbranceVal */
     , (4200169,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (4200169,  16,          1) /* ItemUseable - No */
     , (4200169,  18,          1) /* UiEffects - Magical */
     , (4200169,  19,         20) /* Value */
     , (4200169,  46,         32) /* DefaultCombatStyle - Crossbow */
     , (4200169,  48,         47) /* WeaponSkill - MissileWeapons */
     , (4200169,  49,         60) /* WeaponTime */
     , (4200169,  50,          2) /* AmmoType - Bolt */
     , (4200169,  51,          2) /* CombatUse - Missile */
     , (4200169,  52,          2) /* ParentLocation - LeftHand */
     , (4200169,  53,          3) /* PlacementPosition - LeftHand */
     , (4200169,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200169, 151,          2) /* HookType - Wall */
     , (4200169, 353,          9) /* WeaponType - Crossbow */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200169,  11, True ) /* IgnoreCollisions */
     , (4200169,  13, True ) /* Ethereal */
     , (4200169,  14, True ) /* GravityStatus */
     , (4200169,  19, True ) /* Attackable */
     , (4200169,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200169,   5,  -0.033) /* ManaRate */
     , (4200169,  22,       0) /* DamageVariance */
     , (4200169,  26,    27.3) /* MaximumVelocity */
     , (4200169,  62,       1) /* WeaponOffense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200169,   1, 'Gear Crossbow') /* Name */
     , (4200169,  16, 'An extremely accurate crossbow of Gearknight make. The action of firing this crossbow builds an electrical charge that occasionally discharges into the surrounding area.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200169,   1, 0x0200195A) /* Setup */
     , (4200169,   3, 0x20000014) /* SoundTable */
     , (4200169,   8, 0x06006BC7) /* Icon */
     , (4200169,  22, 0x3400002B) /* PhysicsEffectTable */;
