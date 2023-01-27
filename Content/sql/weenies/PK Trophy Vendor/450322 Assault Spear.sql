DELETE FROM `weenie` WHERE `class_Id` = 450322;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450322, 'speartumerokwartailor2', 6, '2005-02-09 10:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450322,   1,          1) /* ItemType - MeleeWeapon */
     , (450322,   3,         14) /* PaletteTemplate - Red */
     , (450322,   5,        0) /* EncumbranceVal */
     , (450322,   8,        140) /* Mass */
     , (450322,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450322,  16,          1) /* ItemUseable - No */
     , (450322,  18,          1) /* UiEffects - Magical */
     , (450322,  19,       20) /* Value */
     , (450322,  44,         20) /* Damage */
     , (450322,  45,          2) /* DamageType - Pierce */
     , (450322,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450322,  47,          2) /* AttackType - Thrust */
     , (450322,  48,          9) /* WeaponSkill - Spear */
     , (450322,  49,         25) /* WeaponTime */
     , (450322,  51,          1) /* CombatUse - Melee */
     , (450322,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450322, 150,        103) /* HookPlacement - Hook */
     , (450322, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450322,  15, True ) /* LightsStatus */
     , (450322,  22, True ) /* Inscribable */
     , (450322,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450322,   5,  -0.025) /* ManaRate */
     , (450322,  21,     1.5) /* WeaponLength */
     , (450322,  22,    0.95) /* DamageVariance */
     , (450322,  29,    1.06) /* WeaponDefense */
     , (450322,  39,     1.2) /* DefaultScale */
     , (450322,  62,    1.06) /* WeaponOffense */
     , (450322, 151,       1) /* IgnoreShield */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450322,   1, 'Assault Spear') /* Name */
     , (450322,  16, 'A weapon made of a strange pulsating energy.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450322,   1, 0x02000AD5) /* Setup */
     , (450322,   3, 0x20000014) /* SoundTable */
     , (450322,   6, 0x04000BEF) /* PaletteBase */
     , (450322,   7, 0x100002E7) /* ClothingBase */
     , (450322,   8, 0x06002103) /* Icon */
     , (450322,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450322,  37,          9) /* ItemSkillLimit - Spear */;


