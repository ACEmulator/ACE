DELETE FROM `weenie` WHERE `class_Id` = 450302;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450302, 'ace450302-stormwoodaxetailor', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450302,   1,          1) /* ItemType - MeleeWeapon */
     , (450302,   5,        0) /* EncumbranceVal */
     , (450302,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450302,  16,          1) /* ItemUseable - No */
     , (450302,  18,          1) /* UiEffects - Magical */
     , (450302,  19,        20) /* Value */
     , (450302,  44,         0) /* Damage */
     , (450302,  45,         64) /* DamageType - Electric */
     , (450302,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450302,  47,          4) /* AttackType - Slash */
     , (450302,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (450302,  49,         60) /* WeaponTime */
     , (450302,  51,          1) /* CombatUse - Melee */
     , (450302,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450302, 131,         75) /* MaterialType - Oak */
     , (450302, 151,          2) /* HookType - Wall */
     , (450302, 353,          3) /* WeaponType - Axe */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450302,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450302,   5,  -0.025) /* ManaRate */
     , (450302,  21,       0) /* WeaponLength */
     , (450302,  22,    0.93) /* DamageVariance */
     , (450302,  26,       0) /* MaximumVelocity */
     , (450302,  29,    1.0) /* WeaponDefense */
     , (450302,  62,     1.2) /* WeaponOffense */
     , (450302,  63,       1) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450302,   1, 'Stormwood Axe') /* Name */
     , (450302,  14, 'This item may be tinkered and imbued like any loot-generated weapon.') /* Use */
     , (450302,  16, 'An axe imbued with the energies of the Viridian Rise.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450302,   1, 0x02001C3F) /* Setup */
     , (450302,   3, 0x20000014) /* SoundTable */
     , (450302,   8, 0x06007556) /* Icon */
     , (450302,  22, 0x3400002B) /* PhysicsEffectTable */;


