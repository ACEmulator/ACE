DELETE FROM `weenie` WHERE `class_Id` = 480525;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480525, 'axeregalpk', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480525,   1,          1) /* ItemType - MeleeWeapon */
     , (480525,   5,        0) /* EncumbranceVal */
     , (480525,   8,        350) /* Mass */
     , (480525,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480525,  16,          1) /* ItemUseable - No */
     , (480525,  18,          1) /* UiEffects - Magical */
     , (480525,  19,       20) /* Value */
     , (480525,  44,         0) /* Damage */
     , (480525,  45,          2) /* DamageType - Pierce */
     , (480525,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (480525,  47,          4) /* AttackType - Slash */
     , (480525,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (480525,  49,         65) /* WeaponTime */
     , (480525,  51,          1) /* CombatUse - Melee */
     , (480525,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480525, 150,        103) /* HookPlacement - Hook */
     , (480525, 151,          2) /* HookType - Wall */
     , (480525, 353,          3) /* WeaponType - Axe */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480525,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480525,   5,  -0.033) /* ManaRate */
     , (480525,  21,    0.75) /* WeaponLength */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480525,   1, 'Regal War Maul') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480525,   1, 0x02001211) /* Setup */
     , (480525,   3, 0x20000014) /* SoundTable */
     , (480525,   8, 0x0600356F) /* Icon */
     , (480525,  22, 0x3400002B) /* PhysicsEffectTable */;

