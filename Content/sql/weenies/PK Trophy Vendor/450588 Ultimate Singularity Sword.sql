DELETE FROM `weenie` WHERE `class_Id` = 450588;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450588, 'ace450588-ultimatesingularityswordtailor', 6, '2022-02-10 05:08:07') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450588,   1,          1) /* ItemType - MeleeWeapon */
     , (450588,   3,          8) /* PaletteTemplate - Green */
     , (450588,   5,        0) /* EncumbranceVal */
     , (450588,   8,        180) /* Mass */
     , (450588,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450588,  16,          1) /* ItemUseable - No */
     , (450588,  18,          1) /* UiEffects - Magical */
     , (450588,  19,          20) /* Value */
     , (450588,  33,          1) /* Bonded - Bonded */
     , (450588,  44,         0) /* Damage */
     , (450588,  45,          3) /* DamageType - Slash, Pierce */
     , (450588,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450588,  47,          6) /* AttackType - Thrust, Slash */
     , (450588,  48,         45) /* WeaponSkill - LightWeapons */
     , (450588,  49,         40) /* WeaponTime */
     , (450588,  51,          1) /* CombatUse - Melee */
     , (450588,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450588, 150,        103) /* HookPlacement - Hook */
     , (450588, 151,          2) /* HookType - Wall */
     , (450588, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450588,  22, True ) /* Inscribable */
     , (450588,  23, True ) /* DestroyOnSell */
     , (450588,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450588,   5,  -0.033) /* ManaRate */
     , (450588,  21,    0.95) /* WeaponLength */
     , (450588,  22,    0.16) /* DamageVariance */
     , (450588,  29,    1.15) /* WeaponDefense */
     , (450588,  39,     1.1) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450588,   1, 'Ultimate Singularity Sword') /* Name */
     , (450588,  15, 'A sword imbued with Singularity energy.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450588,   1, 0x02000B47) /* Setup */
     , (450588,   3, 0x20000014) /* SoundTable */
     , (450588,   6, 0x04000BEF) /* PaletteBase */
     , (450588,   7, 0x1000029F) /* ClothingBase */
     , (450588,   8, 0x06002230) /* Icon */
     , (450588,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450588,  36, 0x0E000014) /* MutateFilter */;


