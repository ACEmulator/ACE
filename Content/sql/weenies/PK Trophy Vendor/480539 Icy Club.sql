DELETE FROM `weenie` WHERE `class_Id` = 480539;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480539, 'ace480539-icyclubpk', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480539,   1,          1) /* ItemType - MeleeWeapon */
     , (480539,   3,          5) /* PaletteTemplate - DarkBlue */
     , (480539,   5,        0) /* EncumbranceVal */
     , (480539,   8,        140) /* Mass */
     , (480539,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480539,  16,          1) /* ItemUseable - No */
     , (480539,  19,        20) /* Value */
     , (480539,  44,        0) /* Damage */
     , (480539,  45,          8) /* DamageType - Cold */
     , (480539,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (480539,  47,          4) /* AttackType - Slash */
     , (480539,  48,         45) /* WeaponSkill - LightWeapons */
     , (480539,  49,         40) /* WeaponTime */
     , (480539,  51,          1) /* CombatUse - Melee */
     , (480539,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480539, 151,          2) /* HookType - Wall */
     , (480539, 353,          4) /* WeaponType - Mace */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480539,  11, True ) /* IgnoreCollisions */
     , (480539,  13, True ) /* Ethereal */
     , (480539,  14, True ) /* GravityStatus */
     , (480539,  19, True ) /* Attackable */
     , (480539,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480539,  20,      37) /* CombatSpeed */
     , (480539,  22,     0.4) /* DamageVariance */
     , (480539,  26,       0) /* MaximumVelocity */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480539,   1, 'Icy Club') /* Name */
     , (480539,  16, 'The club glistens with destruction as if mirroring the will of the wielder.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480539,   1, 0x02001343) /* Setup */
     , (480539,   3, 0x20000014) /* SoundTable */
     , (480539,   8, 0x06005AF1) /* Icon */
     , (480539,  22, 0x3400002B) /* PhysicsEffectTable */;
