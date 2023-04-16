DELETE FROM `weenie` WHERE `class_Id` = 480510;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480510, 'ace480510-swordofsorokupk', 6, '2021-12-21 17:24:33') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480510,   1,          1) /* ItemType - MeleeWeapon */
     , (480510,   5,        0) /* EncumbranceVal */
     , (480510,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480510,  16,          1) /* ItemUseable - No */
     , (480510,  18,          1) /* UiEffects - Magical */
     , (480510,  19,          20) /* Value */
     , (480510,  44,         0) /* Damage */
     , (480510,  45,          3) /* DamageType - Slash, Pierce */
     , (480510,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (480510,  47,          6) /* AttackType - Thrust, Slash */
     , (480510,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (480510,  49,         30) /* WeaponTime */
     , (480510,  51,          1) /* CombatUse - Melee */
     , (480510,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480510, 151,          2) /* HookType - Wall */
     , (480510, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480510,  22, True ) /* Inscribable */
     , (480510,  23, True ) /* DestroyOnSell */
     , (480510,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480510,   5,  -0.033) /* ManaRate */
     , (480510,  21,       0) /* WeaponLength */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480510,   1, 'Sword of Soroku') /* Name */
     , (480510,  16, 'This sword once belonged to the champion of the Tanada Battle Burrows, Tanada Soroku.') /* LongDesc */
     , (480510,  33, 'SwordofSorokuPickupTimer') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480510,   1, 0x02001754) /* Setup */
     , (480510,   3, 0x20000014) /* SoundTable */
     , (480510,   8, 0x06006717) /* Icon */
     , (480510,  22, 0x3400002B) /* PhysicsEffectTable */;


