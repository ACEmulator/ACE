DELETE FROM `weenie` WHERE `class_Id` = 490054;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490054, 'ace490054-empoweredswordoflostlightpk', 6, '2023-05-15 03:25:02') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490054,   1,          1) /* ItemType - MeleeWeapon */
     , (490054,   5,        450) /* EncumbranceVal */
     , (490054,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (490054,  16,          1) /* ItemUseable - No */
     , (490054,  18,          1) /* UiEffects - Magical */
     , (490054,  19,      20) /* Value */
     , (490054,  44,         20) /* Damage */
     , (490054,  45,          3) /* DamageType - Slash, Pierce */
     , (490054,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (490054,  47,          6) /* AttackType - Thrust, Slash */
     , (490054,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (490054,  49,         30) /* WeaponTime */
     , (490054,  51,          1) /* CombatUse - Melee */
     , (490054,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (490054, 151,          2) /* HookType - Wall */
     , (490054, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490054,  22, True ) /* Inscribable */
     , (490054,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (490054,   5,  -0.033) /* ManaRate */
     , (490054,  21,       0) /* WeaponLength */
     , (490054,  22,     0.5) /* DamageVariance */
     , (490054,  26,       0) /* MaximumVelocity */
     , (490054,  39,     1.3) /* DefaultScale */
     , (490054,  63,       1) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490054,   1, 'Empowered Sword of Lost Light') /* Name */
     , (490054,  16, 'The Sword of Lost Light, infused with the fire from the volcanoes of Lethe, Esper, and Tenkarrdun, and then empowered by the Radiant Mana drawn from the depths of the Dark Isle.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490054,   1, 0x02000F90) /* Setup */
     , (490054,   3, 0x20000014) /* SoundTable */
     , (490054,   8, 0x06002BD1) /* Icon */
     , (490054,  22, 0x3400002B) /* PhysicsEffectTable */;


