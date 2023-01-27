DELETE FROM `weenie` WHERE `class_Id` = 450320;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450320, 'axetumerokwartailor', 6, '2005-02-09 10:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450320,   1,          1) /* ItemType - MeleeWeapon */
     , (450320,   3,         14) /* PaletteTemplate - Red */
     , (450320,   5,        0) /* EncumbranceVal */
     , (450320,   8,        320) /* Mass */
     , (450320,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450320,  16,          1) /* ItemUseable - No */
     , (450320,  18,          1) /* UiEffects - Magical */
     , (450320,  19,       20) /* Value */
     , (450320,  44,         0) /* Damage */
     , (450320,  45,          1) /* DamageType - Slash */
     , (450320,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450320,  47,          4) /* AttackType - Slash */
     , (450320,  48,          1) /* WeaponSkill - Axe */
     , (450320,  49,         40) /* WeaponTime */
     , (450320,  51,          1) /* CombatUse - Melee */
     , (450320,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450320, 150,        103) /* HookPlacement - Hook */
     , (450320, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450320,  15, True ) /* LightsStatus */
     , (450320,  22, True ) /* Inscribable */
     , (450320,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450320,   5,  -0.025) /* ManaRate */
     , (450320,  21,    0.75) /* WeaponLength */
     , (450320,  22,    0.95) /* DamageVariance */
     , (450320,  29,    1.06) /* WeaponDefense */
     , (450320,  39,     1.2) /* DefaultScale */
     , (450320,  62,    1.06) /* WeaponOffense */
     , (450320, 136,       3) /* CriticalMultiplier */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450320,   1, 'Assault Axe') /* Name */
     , (450320,  16, 'A axe given as a reward for defeating the leaders of the Reedshark Clan. The blade seems especially sharp.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450320,   1, 0x02000AD0) /* Setup */
     , (450320,   3, 0x20000014) /* SoundTable */
     , (450320,   6, 0x04000BEF) /* PaletteBase */
     , (450320,   7, 0x100002E7) /* ClothingBase */
     , (450320,   8, 0x060020FE) /* Icon */
     , (450320,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450320,  30,         88) /* PhysicsScript - Create */
     , (450320,  37,          1) /* ItemSkillLimit - Axe */;

