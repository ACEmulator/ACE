DELETE FROM `weenie` WHERE `class_Id` = 480547;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480547, 'bowrenegaderaidsmonsterpk', 3, '2021-11-01 00:00:00') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480547,   1,        256) /* ItemType - MissileWeapon */
     , (480547,   5,        0) /* EncumbranceVal */
     , (480547,   8,        220) /* Mass */
     , (480547,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (480547,  18,          1) /* UiEffects - Magical */
     , (480547,  19,       20) /* Value */
     , (480547,  44,          0) /* Damage */
     , (480547,  46,         16) /* DefaultCombatStyle - Bow */
     , (480547,  48,         47) /* WeaponSkill - MissileWeapons */
     , (480547,  49,         45) /* WeaponTime */
     , (480547,  50,          1) /* AmmoType - Arrow */
     , (480547,  51,          2) /* CombatUse - Missile */
     , (480547,  52,          2) /* ParentLocation - LeftHand */
     , (480547,  53,          3) /* PlacementPosition - LeftHand */
     , (480547,  60,        200) /* WeaponRange */
     , (480547,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (480547, 150,        103) /* HookPlacement - Hook */
     , (480547, 151,          2) /* HookType - Wall */
     , (480547, 353,          8) /* WeaponType - Bow */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480547,  15, True ) /* LightsStatus */
     , (480547,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480547,   5,   -0.05) /* ManaRate */
     , (480547,  21,    0.75) /* WeaponLength */
     , (480547,  26,    26.3) /* MaximumVelocity */
     , (480547,  39,     1.5) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480547,   1, 'Renegade Bow') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480547,   1, 0x02000F68) /* Setup */
     , (480547,   3, 0x20000014) /* SoundTable */
     , (480547,   6, 0x04000BEF) /* PaletteBase */
     , (480547,   8, 0x06002B53) /* Icon */
     , (480547,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480547,  30,         88) /* PhysicsScript - Create */
     , (480547,  37,          2) /* ItemSkillLimit - Bow */;


