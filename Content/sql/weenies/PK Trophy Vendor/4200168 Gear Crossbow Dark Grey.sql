DELETE FROM `weenie` WHERE `class_Id` = 4200168;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200168, 'tailor-ace4200168-gearcrossbow', 3, '2021-11-17 16:56:08') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200168,   1,        256) /* ItemType - MissileWeapon */
     , (4200168,   5,          1) /* EncumbranceVal */
     , (4200168,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (4200168,  16,          1) /* ItemUseable - No */
     , (4200168,  18,          1) /* UiEffects - Magical */
     , (4200168,  19,         20) /* Value */
     , (4200168,  46,         32) /* DefaultCombatStyle - Crossbow */
     , (4200168,  48,         47) /* WeaponSkill - MissileWeapons */
     , (4200168,  49,         60) /* WeaponTime */
     , (4200168,  50,          2) /* AmmoType - Bolt */
     , (4200168,  51,          2) /* CombatUse - Missile */
     , (4200168,  52,          2) /* ParentLocation - LeftHand */
     , (4200168,  53,          3) /* PlacementPosition - LeftHand */
     , (4200168,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200168, 151,          2) /* HookType - Wall */
     , (4200168, 353,          9) /* WeaponType - Crossbow */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200168,  11, True ) /* IgnoreCollisions */
     , (4200168,  13, True ) /* Ethereal */
     , (4200168,  14, True ) /* GravityStatus */
     , (4200168,  19, True ) /* Attackable */
     , (4200168,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200168,   5,  -0.033) /* ManaRate */
     , (4200168,  22,       0) /* DamageVariance */
     , (4200168,  26,    27.3) /* MaximumVelocity */
     , (4200168,  62,       1) /* WeaponOffense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200168,   1, 'Gear Crossbow') /* Name */
     , (4200168,  16, 'An extremely accurate crossbow of Gearknight make. The action of firing this crossbow builds an electrical charge that occasionally discharges into the surrounding area.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200168,   1, 0x02001959) /* Setup */
     , (4200168,   3, 0x20000014) /* SoundTable */
     , (4200168,   8, 0x06006BC7) /* Icon */
     , (4200168,  22, 0x3400002B) /* PhysicsEffectTable */;
