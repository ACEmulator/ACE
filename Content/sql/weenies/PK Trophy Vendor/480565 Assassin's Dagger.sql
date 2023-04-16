DELETE FROM `weenie` WHERE `class_Id` = 480565;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480565, 'ace480565-assassinsdaggerpk', 6, '2022-12-28 05:57:21') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480565,   1,          1) /* ItemType - MeleeWeapon */
     , (480565,   5,        0) /* EncumbranceVal */
     , (480565,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480565,  16,          1) /* ItemUseable - No */
     , (480565,  19,          20) /* Value */
     , (480565,  44,         0) /* Damage */
     , (480565,  45,          3) /* DamageType - Slash, Pierce */
     , (480565,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (480565,  47,        166) /* AttackType - Thrust, Slash, DoubleSlash, DoubleThrust */
     , (480565,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (480565,  49,          1) /* WeaponTime */
     , (480565,  51,          1) /* CombatUse - Melee */
     , (480565,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480565, 114,          0) /* Attuned - Normal */
     , (480565, 353,          6) /* WeaponType - Dagger */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480565,  11, True ) /* IgnoreCollisions */
     , (480565,  13, True ) /* Ethereal */
     , (480565,  14, True ) /* GravityStatus */
     , (480565,  19, True ) /* Attackable */
     , (480565,  22, True ) /* Inscribable */
     , (480565,  69, False) /* IsSellable */
     , (480565,  99, False) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480565,   5,  -0.033) /* ManaRate */
     , (480565,  22,     0.2) /* DamageVariance */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480565,   1, 'Assassin''s Dagger') /* Name */
     , (480565,  16, 'This single-edged weapon bears a blade design and detailed scroll work similar to ceremonial daggers wielded by the most advanced Rossu Morta assassins of Ispar. However, the wicked and unstable enchantments on the blade mark it as a weapon of otherworldly origin.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480565,   1, 0x02000F35) /* Setup */
     , (480565,   3, 0x20000014) /* SoundTable */
     , (480565,   8, 0x06002AEF) /* Icon */
     , (480565,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480565,  52, 0x060065FB) /* IconUnderlay */;

