DELETE FROM `weenie` WHERE `class_Id` = 450595;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450595, 'swordlostlighttailor', 6, '2005-02-09 10:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450595,   1,          1) /* ItemType - MeleeWeapon */
     , (450595,   5,        0) /* EncumbranceVal */
     , (450595,   8,        180) /* Mass */
     , (450595,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450595,  16,          1) /* ItemUseable - No */
     , (450595,  18,          1) /* UiEffects - Magical */
     , (450595,  19,       20) /* Value */
     , (450595,  33,          1) /* Bonded - Bonded */
     , (450595,  44,         0) /* Damage */
     , (450595,  45,          3) /* DamageType - Slash, Pierce */
     , (450595,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450595,  47,          6) /* AttackType - Thrust, Slash */
     , (450595,  48,         48) /* WeaponSkill - Sword */
	 , (450595, 353,          2) /* WeaponType - Sword */
     , (450595,  49,         30) /* WeaponTime */
     , (450595,  51,          1) /* CombatUse - Melee */
     , (450595,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450595, 150,        103) /* HookPlacement - Hook */
     , (450595, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450595,  22, True ) /* Inscribable */
     , (450595,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450595,   5,    -0.1) /* ManaRate */
     , (450595,  21,    0.95) /* WeaponLength */
     , (450595,  22,     0.5) /* DamageVariance */
     , (450595,  29,       1) /* WeaponDefense */
     , (450595,  39,       1) /* DefaultScale */
     , (450595,  62,       1) /* WeaponOffense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450595,   1, 'Sword of Lost Light') /* Name */
     , (450595,  16, 'The Sword of Lost Light.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450595,   1, 0x020005D7) /* Setup */
     , (450595,   3, 0x20000014) /* SoundTable */
     , (450595,   8, 0x0600194C) /* Icon */
     , (450595,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450595,  37,         11) /* ItemSkillLimit - Sword */;

