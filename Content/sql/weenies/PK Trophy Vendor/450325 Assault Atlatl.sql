DELETE FROM `weenie` WHERE `class_Id` = 450325;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450325, 'atlatltumerokwartailor', 3, '2021-11-01 00:00:00') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450325,   1,        256) /* ItemType - MissileWeapon */
     , (450325,   3,         14) /* PaletteTemplate - Red */
     , (450325,   5,        0) /* EncumbranceVal */
     , (450325,   8,        220) /* Mass */
     , (450325,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (450325,  16,          1) /* ItemUseable - No */
     , (450325,  18,          1) /* UiEffects - Magical */
     , (450325,  19,       20) /* Value */
     , (450325,  44,          0) /* Damage */
     , (450325,  46,       1024) /* DefaultCombatStyle - Atlatl */
     , (450325,  48,         47) /* WeaponSkill - MissileWeapons */
     , (450325,  49,         40) /* WeaponTime */
     , (450325,  50,          4) /* AmmoType - Atlatl */
     , (450325,  51,          2) /* CombatUse - Missile */
     , (450325,  53,        101) /* PlacementPosition - Resting */
     , (450325,  60,        140) /* WeaponRange */
     , (450325,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450325, 150,        103) /* HookPlacement - Hook */
     , (450325, 151,          2) /* HookType - Wall */
     , (450325, 353,         10) /* WeaponType - Thrown */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450325,  11, True ) /* IgnoreCollisions */
     , (450325,  13, True ) /* Ethereal */
     , (450325,  14, True ) /* GravityStatus */
     , (450325,  15, True ) /* LightsStatus */
     , (450325,  19, True ) /* Attackable */
     , (450325,  22, True ) /* Inscribable */
     , (450325,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450325,   5,  -0.025) /* ManaRate */
     , (450325,  21,    0.75) /* WeaponLength */
     , (450325,  26,    24.9) /* MaximumVelocity */
     , (450325,  29,    1.06) /* WeaponDefense */
     , (450325,  39,       1) /* DefaultScale */
     , (450325,  62,       1) /* WeaponOffense */
     , (450325,  63,     2.3) /* DamageMod */
     , (450325,  77,       1) /* PhysicsScriptIntensity */
     , (450325, 138,     2.5) /* SlayerDamageBonus */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450325,   1, 'Assault Atlatl') /* Name */
     , (450325,  16, 'A reward for defeating the leaders of the Mask Clan.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450325,   1, 0x02000EBD) /* Setup */
     , (450325,   3, 0x20000014) /* SoundTable */
     , (450325,   6, 0x04000BEF) /* PaletteBase */
     , (450325,   7, 0x100002E7) /* ClothingBase */
     , (450325,   8, 0x06002A21) /* Icon */
     , (450325,  19, 0x00000058) /* ActivationAnimation */
     , (450325,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450325,  30,         87) /* PhysicsScript - BreatheLightning */;


