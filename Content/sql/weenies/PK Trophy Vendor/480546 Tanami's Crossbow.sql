DELETE FROM `weenie` WHERE `class_Id` = 480546;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480546, 'crossbowishaqslostkeypk', 3, '2022-01-08 18:29:57') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480546,   1,        256) /* ItemType - MissileWeapon */
     , (480546,   5,        0) /* EncumbranceVal */
     , (480546,   8,        640) /* Mass */
     , (480546,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (480546,  16,          1) /* ItemUseable - No */
     , (480546,  18,          1) /* UiEffects - Magical */
     , (480546,  19,       20) /* Value */
     , (480546,  44,          0) /* Damage */
     , (480546,  46,         32) /* DefaultCombatStyle - Crossbow */
     , (480546,  48,         47) /* WeaponSkill - MissileWeapons */
     , (480546,  49,         60) /* WeaponTime */
     , (480546,  50,          2) /* AmmoType - Bolt */
     , (480546,  51,          2) /* CombatUse - Missile */
     , (480546,  52,          2) /* ParentLocation - LeftHand */
     , (480546,  53,          3) /* PlacementPosition - LeftHand */
     , (480546,  60,        192) /* WeaponRange */
     , (480546,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480546, 150,        103) /* HookPlacement - Hook */
     , (480546, 151,          2) /* HookType - Wall */
     , (480546, 353,          9) /* WeaponType - Crossbow */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480546,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480546,   5,   -0.05) /* ManaRate */
     , (480546,  26,    27.3) /* MaximumVelocity */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480546,   1, 'Tanami''s Crossbow') /* Name */
     , (480546,  16, 'This crossbow was a gift from Tanami Kei of Ayan Baqur.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480546,   1, 0x0200124F) /* Setup */
     , (480546,   3, 0x20000014) /* SoundTable */
     , (480546,   8, 0x060036F4) /* Icon */
     , (480546,  22, 0x3400002B) /* PhysicsEffectTable */;
