DELETE FROM `weenie` WHERE `class_Id` = 450315;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450315, 'lomacequidditytailor', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450315,   1,          1) /* ItemType - MeleeWeapon */
     , (450315,   5,        0) /* EncumbranceVal */
     , (450315,   8,        360) /* Mass */
     , (450315,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450315,  16,          1) /* ItemUseable - No */
     , (450315,  18,          1) /* UiEffects - Magical */
     , (450315,  19,       20) /* Value */
     , (450315,  44,         0) /* Damage */
     , (450315,  45,          4) /* DamageType - Bludgeon */
     , (450315,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450315,  47,          4) /* AttackType - Slash */
     , (450315,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (450315,  49,         40) /* WeaponTime */
     , (450315,  51,          1) /* CombatUse - Melee */
     , (450315,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450315, 150,        103) /* HookPlacement - Hook */
     , (450315, 151,          2) /* HookType - Wall */
     , (450315, 353,          4) /* WeaponType - Mace */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450315,  11, True ) /* IgnoreCollisions */
     , (450315,  13, True ) /* Ethereal */
     , (450315,  14, True ) /* GravityStatus */
     , (450315,  15, True ) /* LightsStatus */
     , (450315,  19, True ) /* Attackable */
     , (450315,  22, True ) /* Inscribable */
     , (450315,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450315,   5,  -0.025) /* ManaRate */
     , (450315,  21,    0.62) /* WeaponLength */
     , (450315,  22,     0.5) /* DamageVariance */
     , (450315,  29,    1.08) /* WeaponDefense */
     , (450315,  62,    1.04) /* WeaponOffense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450315,   1, 'Mace of the Quiddity') /* Name */
     , (450315,  15, 'A weapon made of a strange pulsating energy.') /* ShortDesc */
     , (450315,  16, 'A weapon made of a strange pulsating energy.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450315,   1, 0x02000A76) /* Setup */
     , (450315,   3, 0x20000014) /* SoundTable */
     , (450315,   8, 0x060020D1) /* Icon */
     , (450315,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450315,  36, 0x0E000014) /* MutateFilter */;

