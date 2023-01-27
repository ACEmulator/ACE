DELETE FROM `weenie` WHERE `class_Id` = 450323;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450323, 'stafftumerokwartailor', 6, '2005-02-09 10:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450323,   1,          1) /* ItemType - MeleeWeapon */
     , (450323,   3,         14) /* PaletteTemplate - Red */
     , (450323,   5,        0) /* EncumbranceVal */
     , (450323,   8,         90) /* Mass */
     , (450323,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450323,  16,          1) /* ItemUseable - No */
     , (450323,  18,          1) /* UiEffects - Magical */
     , (450323,  19,       20) /* Value */
     , (450323,  44,         0) /* Damage */
     , (450323,  45,          4) /* DamageType - Bludgeon */
     , (450323,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450323,  47,          6) /* AttackType - Thrust, Slash */
     , (450323,  48,         10) /* WeaponSkill - Staff */
     , (450323,  49,         25) /* WeaponTime */
     , (450323,  51,          1) /* CombatUse - Melee */
     , (450323,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450323, 150,        103) /* HookPlacement - Hook */
     , (450323, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450323,  15, True ) /* LightsStatus */
     , (450323,  22, True ) /* Inscribable */
     , (450323,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450323,   5,  -0.025) /* ManaRate */
     , (450323,  21,    1.33) /* WeaponLength */
     , (450323,  22,    0.94) /* DamageVariance */
     , (450323,  29,    1.06) /* WeaponDefense */
     , (450323,  39,     0.8) /* DefaultScale */
     , (450323,  62,    1.06) /* WeaponOffense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450323,   1, 'Assault Staff') /* Name */
     , (450323,  16, 'A staff given as a reward for defeating the leaders of the Mask Clan.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450323,   1, 0x02000AD6) /* Setup */
     , (450323,   3, 0x20000014) /* SoundTable */
     , (450323,   6, 0x04000BEF) /* PaletteBase */
     , (450323,   7, 0x100002E7) /* ClothingBase */
     , (450323,   8, 0x06002104) /* Icon */
     , (450323,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450323,  37,         10) /* ItemSkillLimit - Staff */;


