DELETE FROM `weenie` WHERE `class_Id` = 450197;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450197, 'staffrarespiritshiftingstafftailor', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450197,   1,          1) /* ItemType - MeleeWeapon */
     , (450197,   5,        0) /* EncumbranceVal */
     , (450197,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450197,  16,          1) /* ItemUseable - No */
     , (450197,  19,      20) /* Value */
     , (450197,  44,         0) /* Damage */
     , (450197,  45,          4) /* DamageType - Bludgeon */
     , (450197,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450197,  47,          6) /* AttackType - Thrust, Slash */
     , (450197,  48,         45) /* WeaponSkill - LightWeapons */
     , (450197,  49,         35) /* WeaponTime */
     , (450197,  51,          1) /* CombatUse - Melee */
     , (450197,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450197, 151,          2) /* HookType - Wall */
     , (450197, 353,          7) /* WeaponType - Staff */;


INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450197,  11, True ) /* IgnoreCollisions */
     , (450197,  13, True ) /* Ethereal */
     , (450197,  14, True ) /* GravityStatus */
     , (450197,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450197,   5,  -0.033) /* ManaRate */
     , (450197,  21,       0) /* WeaponLength */
     , (450197,  22,    0.25) /* DamageVariance */
     , (450197,  26,       0) /* MaximumVelocity */
     , (450197,  29,    1.18) /* WeaponDefense */
     , (450197,  39,       1) /* DefaultScale */
     , (450197,  62,    1.18) /* WeaponOffense */
     , (450197,  63,       1) /* DamageMod */
     , (450197, 138,     1.2) /* SlayerDamageBonus */
     , (450197, 151,       1) /* IgnoreShield */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450197,   1, 'Spirit Shifting Staff') /* Name */
     , (450197,  16, 'This staff has been carved from the bones of some great magical beast. Apparently the soul of the beast still lives on in this staff, because the weapon''s wielder seems to draw upon some bestial strength. Other times, however, the wielder will have his mind clouded with hallucinations of running free and wild across a vast, unfamiliar plain.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450197,   1, 0x0200135F) /* Setup */
     , (450197,   3, 0x20000014) /* SoundTable */
     , (450197,   6, 0x04000BEF) /* PaletteBase */
     , (450197,   8, 0x06005BAD) /* Icon */
     , (450197,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450197,  52, 0x06005B0C) /* IconUnderlay */;

