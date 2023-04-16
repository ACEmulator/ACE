DELETE FROM `weenie` WHERE `class_Id` = 480505;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480505, 'ace480505-lordsbladepk', 6, '2022-12-28 05:57:21') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480505,   1,          1) /* ItemType - MeleeWeapon */
     , (480505,   5,        0) /* EncumbranceVal */
     , (480505,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480505,  16,          1) /* ItemUseable - No */
     , (480505,  19,          20) /* Value */
     , (480505,  36,       9999) /* ResistMagic */
     , (480505,  44,         0) /* Damage */
     , (480505,  45,          3) /* DamageType - Slash, Pierce */
     , (480505,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (480505,  47,        166) /* AttackType - Thrust, Slash, DoubleSlash, DoubleThrust */
     , (480505,  48,         45) /* WeaponSkill - LightWeapons */
     , (480505,  49,          0) /* WeaponTime */
     , (480505,  51,          1) /* CombatUse - Melee */
     , (480505,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480505, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480505,  22, True ) /* Inscribable */
     , (480505,  69, False) /* IsSellable */
     , (480505,  99, False) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480505,   5,  -0.033) /* ManaRate */
     , (480505,  26,       0) /* MaximumVelocity */
     , (480505,  39,     1.1) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480505,   1, 'Lord''s Blade') /* Name */
     , (480505,  16, 'This sword is patterned after the double-edged spadas commonly worn as courtly arms by Viamontian aristocracy. Far from being a badge of noble rank, however, this sword possesses many enchantments and powerful, if very rare, properties. It seems to glow with unstable energy, not long for this world.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480505,   1, 0x02001315) /* Setup */
     , (480505,   3, 0x20000014) /* SoundTable */
     , (480505,   6, 0x04001A25) /* PaletteBase */
     , (480505,   8, 0x06005C60) /* Icon */
     , (480505,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480505,  52, 0x060065FB) /* IconUnderlay */;

