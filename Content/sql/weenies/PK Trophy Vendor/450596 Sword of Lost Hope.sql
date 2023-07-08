DELETE FROM `weenie` WHERE `class_Id` = 450596;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450596, 'swordlosthopetailor', 6, '2005-02-09 10:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450596,   1,          1) /* ItemType - MeleeWeapon */
     , (450596,   3,         20) /* PaletteTemplate - Silver */
     , (450596,   5,        0) /* EncumbranceVal */
     , (450596,   8,        180) /* Mass */
     , (450596,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450596,  16,          1) /* ItemUseable - No */
     , (450596,  18,          1) /* UiEffects - Magical */
     , (450596,  19,          20) /* Value */
     , (450596,  33,          1) /* Bonded - Bonded */
     , (450596,  44,         0) /* Damage */
     , (450596,  45,         32) /* DamageType - Acid */
     , (450596,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450596,  47,          6) /* AttackType - Thrust, Slash */
     , (450596,  48,         48) /* WeaponSkill - Sword */
	 , (450596, 353,          2) /* WeaponType - Sword */
     , (450596,  49,         30) /* WeaponTime */
     , (450596,  51,          1) /* CombatUse - Melee */
     , (450596,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450596, 150,        103) /* HookPlacement - Hook */
     , (450596, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450596,  15, True ) /* LightsStatus */
     , (450596,  22, True ) /* Inscribable */
     , (450596,  23, True ) /* DestroyOnSell */
     , (450596,  69, False) /* IsSellable */
     , (450596,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450596,   5,    -0.1) /* ManaRate */
     , (450596,  21,    0.95) /* WeaponLength */
     , (450596,  22,     0.5) /* DamageVariance */
     , (450596,  29,       1) /* WeaponDefense */
     , (450596,  39,       1) /* DefaultScale */
     , (450596,  62,       1) /* WeaponOffense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450596,   1, 'Sword of Lost Hope') /* Name */
     , (450596,  16, 'The Sword of Lost Hope.  The weapon seems to coruscate with the power of Ilservian''s blood.  The Light has been forever dimmed, and the weapon is now empowered by the essence of the Hopeslayer.  It will no longer accept infusions of light.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450596,   1, 0x020009D5) /* Setup */
     , (450596,   3, 0x20000014) /* SoundTable */
     , (450596,   6, 0x04000BEF) /* PaletteBase */
     , (450596,   7, 0x1000028D) /* ClothingBase */
     , (450596,   8, 0x06001F5D) /* Icon */
     , (450596,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450596,  37,         11) /* ItemSkillLimit - Sword */;

