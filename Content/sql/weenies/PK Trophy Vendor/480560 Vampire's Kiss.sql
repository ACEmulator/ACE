DELETE FROM `weenie` WHERE `class_Id` = 480560;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480560, 'ace480560-vampireskisspk', 6, '2022-12-28 05:57:21') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480560,   1,          1) /* ItemType - MeleeWeapon */
     , (480560,   5,        0) /* EncumbranceVal */
     , (480560,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480560,  16,          1) /* ItemUseable - No */
     , (480560,  19,          20) /* Value */
     , (480560,  44,         0) /* Damage */
     , (480560,  45,          1) /* DamageType - Slash */
     , (480560,  46,          1) /* DefaultCombatStyle - Unarmed */
     , (480560,  47,          1) /* AttackType - Punch */
     , (480560,  48,         45) /* WeaponSkill - LightWeapons */
     , (480560,  49,          1) /* WeaponTime */
     , (480560,  51,          1) /* CombatUse - Melee */
     , (480560,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480560, 353,          1) /* WeaponType - Unarmed */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480560,  22, True ) /* Inscribable */
     , (480560,  69, False) /* IsSellable */
     , (480560,  99, False) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480560,   5,  -0.033) /* ManaRate */
     , (480560,  22,     0.5) /* DamageVariance */
     , (480560,  26,       0) /* MaximumVelocity */
     , (480560,  39,    1.25) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480560,   1, 'Vampire''s Kiss') /* Name */
     , (480560,  16, 'This weapon seems like a simple katar, available from any smith in the Gharu''n lands, but the strange spells and black smoke that flow from the blade mark it as a creation of a malevolent, alien intelligence. It seems to suck in the life-force of those it wounds, and its blade shivers with unstable power.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480560,   1, 0x02000735) /* Setup */
     , (480560,   3, 0x20000014) /* SoundTable */
     , (480560,   6, 0x04000BEF) /* PaletteBase */
     , (480560,   8, 0x060015FF) /* Icon */
     , (480560,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480560,  52, 0x060065FB) /* IconUnderlay */;


