DELETE FROM `weenie` WHERE `class_Id` = 450330;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450330, 'ace450330-assaultgreatswordtailor', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450330,   1,          1) /* ItemType - MeleeWeapon */
     , (450330,   3,         14) /* PaletteTemplate - Red */
     , (450330,   5,        0) /* EncumbranceVal */
     , (450330,   9,   33554432) /* ValidLocations - TwoHanded */
     , (450330,  16,          1) /* ItemUseable - No */
     , (450330,  18,          1) /* UiEffects - Magical */
     , (450330,  19,       20) /* Value */
     , (450330,  44,         26) /* Damage */
     , (450330,  45,          1) /* DamageType - Slash */
     , (450330,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (450330,  47,          4) /* AttackType - Slash */
     , (450330,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (450330,  49,         40) /* WeaponTime */
     , (450330,  51,          5) /* CombatUse - TwoHanded */
     , (450330,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450330, 151,          2) /* HookType - Wall */
     , (450330, 292,          2) /* Cleaving */
     , (450330, 353,         11) /* WeaponType - TwoHanded */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450330,  11, True ) /* IgnoreCollisions */
     , (450330,  13, True ) /* Ethereal */
     , (450330,  14, True ) /* GravityStatus */
     , (450330,  15, True ) /* LightsStatus */
     , (450330,  19, True ) /* Attackable */
     , (450330,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450330,   5,  -0.025) /* ManaRate */
     , (450330,  21,       1) /* WeaponLength */
     , (450330,  22,     0.6) /* DamageVariance */
     , (450330,  26,       0) /* MaximumVelocity */
     , (450330,  29,    1.06) /* WeaponDefense */
     , (450330,  39,     1.3) /* DefaultScale */
     , (450330,  62,    1.06) /* WeaponOffense */
     , (450330,  63,       1) /* DamageMod */
     , (450330,  77,       1) /* PhysicsScriptIntensity */
     , (450330, 138,     2.5) /* SlayerDamageBonus */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450330,   1, 'Assault Greatsword') /* Name */
     , (450330,  16, 'A reward for defeating the leaders of the Falcon Clan.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450330,   1, 0x02000EC1) /* Setup */
     , (450330,   3, 0x20000014) /* SoundTable */
     , (450330,   8, 0x06006B99) /* Icon */
     , (450330,  19, 0x00000058) /* ActivationAnimation */
     , (450330,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450330,  30,         88) /* PhysicsScript - Create */;

