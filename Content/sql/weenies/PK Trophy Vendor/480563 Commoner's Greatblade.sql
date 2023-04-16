DELETE FROM `weenie` WHERE `class_Id` = 480563;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480563, 'ace480563-commonersgreatbladepk', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480563,   1,          1) /* ItemType - MeleeWeapon */
     , (480563,   5,        0) /* EncumbranceVal */
     , (480563,   9,   33554432) /* ValidLocations - TwoHanded */
     , (480563,  16,          1) /* ItemUseable - No */
     , (480563,  19,          20) /* Value */
     , (480563,  44,         0) /* Damage */
     , (480563,  45,          1) /* DamageType - Slash */
     , (480563,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (480563,  47,          4) /* AttackType - Slash */
     , (480563,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (480563,  49,          0) /* WeaponTime */
     , (480563,  51,          5) /* CombatUse - TwoHanded */
     , (480563,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480563, 292,          2) /* Cleaving */
     , (480563, 353,         11) /* WeaponType - TwoHanded */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480563,  11, True ) /* IgnoreCollisions */
     , (480563,  13, True ) /* Ethereal */
     , (480563,  14, True ) /* GravityStatus */
     , (480563,  19, True ) /* Attackable */
     , (480563,  22, True ) /* Inscribable */
     , (480563,  69, False) /* IsSellable */
     , (480563,  99, False) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480563,   5,  -0.033) /* ManaRate */
     , (480563,  22,    0.43) /* DamageVariance */
     , (480563,  26,       0) /* MaximumVelocity */
     , (480563,  39,     1.3) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480563,   1, 'Commoner''s Greatblade') /* Name */
     , (480563,  16, 'This sword is designed in the fashion of the sabras that are common among the foot soldiers of Viamont, but its powerful spells and warped black hilt hints at its otherworldly origin. Elegant and deadly as it is, the blade is not stable, and seems to have trouble holding its form in the highly sensitive magic atmosphere of Dereth.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480563,   1, 0x02001319) /* Setup */
     , (480563,   3, 0x20000014) /* SoundTable */
     , (480563,   8, 0x06006B7F) /* Icon */
     , (480563,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480563,  52, 0x060065FB) /* IconUnderlay */;

