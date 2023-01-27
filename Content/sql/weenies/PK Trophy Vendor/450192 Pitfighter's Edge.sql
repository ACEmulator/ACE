DELETE FROM `weenie` WHERE `class_Id` = 450192;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450192, 'daggerrarepitfightersedgetailor', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450192,   1,          1) /* ItemType - MeleeWeapon */
     , (450192,   5,        0) /* EncumbranceVal */
     , (450192,   8,         90) /* Mass */
     , (450192,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450192,  16,          1) /* ItemUseable - No */
     , (450192,  19,      20) /* Value */
     , (450192,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450192,  44,         0) /* Damage */
     , (450192,  45,          3) /* DamageType - Slash, Pierce */
     , (450192,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450192,  47,        160) /* AttackType - DoubleSlash, DoubleThrust */
     , (450192,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (450192,  49,         20) /* WeaponTime */
     , (450192,  51,          1) /* CombatUse - Melee */
     , (450192,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450192, 151,          2) /* HookType - Wall */
     , (450192, 353,          6) /* WeaponType - Dagger */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450192,  11, True ) /* IgnoreCollisions */
     , (450192,  13, True ) /* Ethereal */
     , (450192,  14, True ) /* GravityStatus */
     , (450192,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450192,   5,  -0.033) /* ManaRate */
     , (450192,  21,       0) /* WeaponLength */
     , (450192,  22,     0.3) /* DamageVariance */
     , (450192,  26,       0) /* MaximumVelocity */
     , (450192,  29,    1.18) /* WeaponDefense */
     , (450192,  39,     1.1) /* DefaultScale */
     , (450192,  62,    1.18) /* WeaponOffense */
     , (450192,  63,       1) /* DamageMod */
     , (450192, 138,    1.18) /* SlayerDamageBonus */
     , (450192, 147,    0.33) /* CriticalFrequency */
     , (450192, 151,       1) /* IgnoreShield */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450192,   1, 'Pitfighter''s Edge') /* Name */
     , (450192,  16, 'One of the most popular entertainments in the port cities of the Ironsea was the sport of pitfighting. The rules were simple: two fighters, unarmored and armed only with daggers, would fight to the death in a circular pit with wooden walls. The most successful pitfighter of them all was Enza "The Jugular" Speltari of Corcosa. She survived fifty pitfights, relying on uncanny quickness and blinding hand-speed. She went so far as to embark on a tour of all the great pitfighting cities of the Ironsea. Sadly, she was washed overboard in a storm off the coast of Tirethas, halfway through her tour. Her knife was left stuck into the railing of the ship.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450192,   1, 0x02001356) /* Setup */
     , (450192,   3, 0x20000014) /* SoundTable */
     , (450192,   6, 0x04000BEF) /* PaletteBase */
     , (450192,   7, 0x10000860) /* ClothingBase */
     , (450192,   8, 0x06005B9B) /* Icon */
     , (450192,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450192,  36, 0x0E000012) /* MutateFilter */
     , (450192,  46, 0x38000032) /* TsysMutationFilter */
     , (450192,  52, 0x06005B0C) /* IconUnderlay */;
