DELETE FROM `weenie` WHERE `class_Id` = 480567;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480567, 'pickaxerot2pk', 6, '2021-11-07 08:12:46') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480567,   1,          1) /* ItemType - MeleeWeapon */
     , (480567,   5,        0) /* EncumbranceVal */
     , (480567,   8,        350) /* Mass */
     , (480567,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480567,  16,          1) /* ItemUseable - No */
     , (480567,  19,         20) /* Value */
     , (480567,  44,          0) /* Damage */
     , (480567,  45,          2) /* DamageType - Pierce */
     , (480567,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (480567,  47,          4) /* AttackType - Slash */
     , (480567,  48,         45) /* WeaponSkill - LightWeapons */
     , (480567,  49,         60) /* WeaponTime */
     , (480567,  51,          1) /* CombatUse - Melee */
     , (480567,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480567, 150,        103) /* HookPlacement - Hook */
     , (480567, 151,          2) /* HookType - Wall */
     , (480567, 353,          3) /* WeaponType - Axe */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480567,  22, True ) /* Inscribable */
     , (480567,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480567,  21,    0.75) /* WeaponLength */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480567,   1, 'Hiyp the Toad''s pickaxe') /* Name */
     , (480567,  16, 'A pickaxe belonging to Hiyp the Toad') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480567,   1, 0x0200017D) /* Setup */
     , (480567,   3, 0x20000014) /* SoundTable */
     , (480567,   8, 0x06001B43) /* Icon */
     , (480567,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480567,  30,         88) /* PhysicsScript - Create */;
